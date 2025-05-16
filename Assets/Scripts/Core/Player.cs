using System.Collections;
using System.Linq;
using Core.PlayerComponents;
using Core.PlayerComponents.MainWeapons.Abstract;
using Core.Projectiles;
using Data;
using Fusion;
using Fusion.Addons.SimpleKCC;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
	[DefaultExecutionOrder(-5)]
	public sealed class Player : NetworkBehaviour
	{
		[Header("Weapons")] 
		[SerializeField] private Transform primaryWeaponPosition;
		[SerializeField] private NetworkPrefabRef primaryWeaponPrefab;
		[SerializeField] private WeaponBase primaryWeapon;
		[SerializeField] private ProjectilesLauncher projectilesLauncher;

		[Header("Movement Components")] public SimpleKCC KCC;
		public PlayerInput Input;
		public Transform CameraPivot;
		public Transform CameraHandle;

		[Header("Movement Settings")] public float MoveSpeed = 10.0f;
		public float JumpImpulse = 10.0f;
		public float UpGravity = -25.0f;
		public float DownGravity = -40.0f;
		public float GroundAcceleration = 55.0f;
		public float GroundDeceleration = 25.0f;
		public float AirAcceleration = 25.0f;
		public float AirDeceleration = 1.3f;

		[Networked] private Vector3 _moveVelocity { get; set; }

		private Transform _cameraTransform;

		public override void Spawned()
		{
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
					jumpImpulse = JumpImpulse;
				}
			}
			if (Input.CurrentInput.Actions.WasPressed(Input.PreviousInput.Actions, GameplayInput.FIRE_BUTTON) == true)
			{
				if (primaryWeapon != null)
				{
					// projectilesLauncher.Launch(primaryWeapon.transform.position + primaryWeapon.transform.forward, 
					// 		KCC.LookDirection, Quaternion.identity);
					// primaryWeapon.Fire(KCC.LookDirection, Runner, Object.InputAuthority);
					primaryWeapon.Fire();
				}
			}

			// It feels better when the player falls quicker.
			KCC.SetGravity(KCC.RealVelocity.y >= 0.0f ? UpGravity : DownGravity);

			Vector3 desiredMoveVelocity = inputDirection * MoveSpeed;

			if (KCC.ProjectOnGround(desiredMoveVelocity, out Vector3 projectedDesiredMoveVelocity) == true)
			{
				desiredMoveVelocity = Vector3.Normalize(projectedDesiredMoveVelocity) * MoveSpeed;
			}

			float acceleration;
			if (desiredMoveVelocity == Vector3.zero)
			{
				// No desired move velocity - we are stopping.
				acceleration = KCC.IsGrounded == true ? GroundDeceleration : AirDeceleration;
			}
			else
			{
				acceleration = KCC.IsGrounded == true ? GroundAcceleration : AirAcceleration;
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
			
			Debug.Log("Weapon ready!");
		}

		private void InitializeCamera()
		{
			var mainCamera = Camera.main;
			if (mainCamera != null)
			{
				_cameraTransform = mainCamera.transform;
				_cameraTransform.SetParent(CameraHandle);
				_cameraTransform.localPosition = Vector3.zero;
				_cameraTransform.localRotation = Quaternion.identity;
			}
		}

		public Transform GetPrimaryWeaponTransform()
		{
			return primaryWeaponPosition;
		}
	}
}