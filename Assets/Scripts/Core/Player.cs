using System.Collections;
using System.Linq;
using Core.PlayerComponents;
using Core.PlayerComponents.MainWeapons.Abstract;
using Core.Projectiles;
using Data;
using Fusion;
using Fusion.Addons.SimpleKCC;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
	[DefaultExecutionOrder(-5)]
	public sealed class Player : NetworkBehaviour
	{
		[Header("Health")]
		[SerializeField] private NetworkHealth networkHealth;
		
		[Header("Weapons")] 
		[SerializeField] private Transform primaryWeaponPosition;
		[SerializeField] private NetworkPrefabRef primaryWeaponPrefab;
		[SerializeField] private WeaponBase primaryWeapon;
		[SerializeField] private ProjectilesLauncher projectilesLauncher;

		[Header("Movement Components")] public SimpleKCC KCC;
		public PlayerInput Input;
		public Transform CameraPivot;
		public Transform CameraHandle;

		[FormerlySerializedAs("movementSettings")]
		[Header("Movement Settings")] 
		[SerializeField] private PlayerMovementSettings playerMovementSettings;
		private MovementConfig _movementConfig;
		
		[Networked] private Vector3 _moveVelocity { get; set; }

		private Transform _cameraTransform;

		public override void Spawned()
		{
			_movementConfig = playerMovementSettings.GetConfig();
			InitializeCamera();
			
			if (Object.HasInputAuthority)
			{
				RPC_RequestWeapon();
			}
			
			StartCoroutine(WaitForWeapon());
		}

		public override void FixedUpdateNetwork()
		{
			// Apply look rotation delta. This propagates to Transform component immediately.
			KCC.AddLookRotation(Input.CurrentInput.LookRotationDelta);

			// Set default world space input direction and jump impulse.
			Vector3 inputDirection = KCC.TransformRotation * new Vector3(Input.CurrentInput.MoveDirection.x, 0.0f,
				Input.CurrentInput.MoveDirection.y);
			float jumpImpulse = default;

			// Comparing current input to previous input - this prevents glitches when input is lost.
			if (Input.CurrentInput.Actions.WasPressed(Input.PreviousInput.Actions, GameplayInput.JUMP_BUTTON) == true)
			{
				if (KCC.IsGrounded == true)
				{
					// Set world space jump vector.
					jumpImpulse = _movementConfig.JumpImpulse;
				}
			}
			if (Input.CurrentInput.Actions.WasPressed(Input.PreviousInput.Actions, GameplayInput.FIRE_BUTTON) == true)
			{
				if (primaryWeapon != null)
				{
					GetCameraStartAndDirection(out var cameraStart, out var cameraDirection);
					primaryWeapon.Fire(cameraStart, cameraDirection);
				}
			}
			
			// It feels better when the player falls quicker.
			KCC.SetGravity(KCC.RealVelocity.y >= 0.0f ? _movementConfig.UpGravity : _movementConfig.DownGravity);

			Vector3 desiredMoveVelocity = inputDirection * _movementConfig.MoveSpeed;

			if (KCC.ProjectOnGround(desiredMoveVelocity, out Vector3 projectedDesiredMoveVelocity) == true)
			{
				desiredMoveVelocity = Vector3.Normalize(projectedDesiredMoveVelocity) * _movementConfig.MoveSpeed;
			}

			float acceleration;
			if (desiredMoveVelocity == Vector3.zero)
			{
				// No desired move velocity - we are stopping.
				acceleration = KCC.IsGrounded == true ? _movementConfig.GroundDeceleration : _movementConfig.AirDeceleration;
			}
			else
			{
				acceleration = KCC.IsGrounded == true ? _movementConfig.GroundAcceleration : _movementConfig.AirAcceleration;
			}

			_moveVelocity = Vector3.Lerp(_moveVelocity, desiredMoveVelocity, acceleration * Runner.DeltaTime);

			KCC.Move(_moveVelocity, jumpImpulse);
		}

		private void LateUpdate()
		{
			// Only InputAuthority needs to update camera.
			if (HasInputAuthority == false)
				return;

			// Update camera pivot and transfer properties from camera handle to Main Camera.
			// Render() is executed before KCC because of [OrderBefore(typeof(KCC))].
			// So we have to do it from LateUpdate() - which is called after Render().

			Vector2 pitchRotation = KCC.GetLookRotation(true, false);
			CameraPivot.localRotation = Quaternion.Euler(pitchRotation);

			_cameraTransform.SetPositionAndRotation(CameraHandle.position, CameraHandle.rotation);
			
			if (!HasInputAuthority) return;
			
			AimGun();
		}

		private void AimGun()
		{
			Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
			Vector3 lookTarget;

			if (Physics.Raycast(ray, out RaycastHit hit, 100f))
				lookTarget = hit.point;
			else
				lookTarget = ray.GetPoint(100f);

			Vector3 direction = (lookTarget - primaryWeaponPosition.position).normalized;
			primaryWeaponPosition.rotation = Quaternion.LookRotation(direction);
		}

		[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
		private void RPC_RequestWeapon()
		{
			var weapon = Runner.Spawn(primaryWeaponPrefab,
				primaryWeaponPosition.position, Quaternion.identity, Object.InputAuthority,
				(runner, o) => { o.GetComponent<WeaponBase>().Owner = Object; });

			primaryWeapon = weapon.GetComponent<WeaponBase>();
		}
		
		private IEnumerator WaitForWeapon()
		{
			while (!primaryWeapon)
			{
				var allWeapons = FindObjectsOfType<WeaponBase>();
				primaryWeapon = allWeapons.FirstOrDefault(w => w.Owner == Object);
				yield return null;
			}

			// primaryWeapon.transform.position = primaryWeaponPosition.position;
			primaryWeapon.transform.position = new Vector3(primaryWeaponPosition.position.x,
				primaryWeaponPosition.position.y,
				primaryWeaponPosition.position.z + primaryWeapon.transform.localScale.z / 2);
			primaryWeapon.transform.rotation = primaryWeaponPosition.rotation;
			
			Debug.Log("Weapon ready!");
		}

		private void GetCameraStartAndDirection(out Vector3 cameraStart, out Vector3 cameraDirection)
		{
			Camera cam = Camera.main;

			Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			cameraDirection = ray.direction;
			cameraStart = ray.origin;	
		}
		private void InitializeCamera()
		{
			var mainCamera = Camera.main;
			if (mainCamera != null)
			{
				_cameraTransform = mainCamera.transform;
			}
		}

		public Transform GetPrimaryWeaponTransform()
		{
			return primaryWeaponPosition;
		}
	}
}