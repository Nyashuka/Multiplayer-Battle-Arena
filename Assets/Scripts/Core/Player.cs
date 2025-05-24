using Core.PlayerComponents;
using Core.PlayerComponents.MainWeapons.Abstract;
using Data;
using Fusion;
using Fusion.Addons.SimpleKCC;
using ScriptableObjects;
using UnityEngine;

namespace Core
{
	[DefaultExecutionOrder(-5)]
	public sealed class Player : NetworkBehaviour
	{
		[Header("Player Components")] 
		[SerializeField] private NetworkHealth networkHealth;
		[SerializeField] private PlayerLives playerLives;
		[SerializeField] private PlayerInput input;
		[SerializeField] private SimpleKCC kcc;
		[SerializeField] private PlayerLifecycle playerLifecycle;
		[SerializeField] private Transform primaryWeaponHolder;
		[SerializeField] private Transform cameraHandle;

		[Header("Movement Settings")] [SerializeField]
		private PlayerMovementSettings playerMovementSettings;

		private MovementConfig _movementConfig;
		private float _jumpImpulse;
		private Transform _cameraTransform;

		[Networked] private Vector3 MoveVelocity { get; set; }
		[Networked] private WeaponBase CurrentWeapon { get; set; }
		
		public PlayerLives PlayerLives => playerLives;
		
		[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
		private void Rpc_SetupPrimaryGunVisual()
		{
			CurrentWeapon.transform.SetParent(primaryWeaponHolder);
			CurrentWeapon.transform.localPosition = new Vector3(0, 0, 0.5f);
		}

		public void SetWeapon(WeaponBase newWeapon)
		{
			if (!HasStateAuthority) return;
    
			CurrentWeapon = newWeapon;
			Rpc_SetupPrimaryGunVisual();
		}
	
		private float GetCurrentAcceleration(Vector3 desiredMoveVelocity)
		{
			if (desiredMoveVelocity == Vector3.zero)
			{
				// No desired move velocity - we are stopping.
				return kcc.IsGrounded ? _movementConfig.GroundDeceleration : _movementConfig.AirDeceleration;
			}

			return kcc.IsGrounded ? _movementConfig.GroundAcceleration : _movementConfig.AirAcceleration;
		}

		private void HandleLookRotation()
		{
			// Apply look rotation delta. This propagates to Transform component immediately.
			kcc.AddLookRotation(input.CurrentInput.LookRotationDelta);
		}
		
		private void HandleJumpInput()
		{
			_jumpImpulse = 0f;

			// Comparing current input to previous input - this prevents glitches when input is lost.
			if (input.CurrentInput.Actions.WasPressed(input.PreviousInput.Actions, GameplayInput.JUMP_BUTTON))
			{
				if (kcc.IsGrounded)
				{
					_jumpImpulse = _movementConfig.JumpImpulse;
				}
			}
		}

		private void HandleShootingInput()
		{
			if (input.CurrentInput.Actions.WasPressed(input.PreviousInput.Actions, GameplayInput.FIRE_BUTTON))
			{
				if (CurrentWeapon != null)
				{
					GetCameraStartAndDirection(out var cameraStart, out var cameraDirection);
					CurrentWeapon.Fire(cameraStart, cameraDirection);
				}
			}
		}

		private void ApplyGravity()
		{
			// It feels better when the player falls quicker.
			float gravity = kcc.RealVelocity.y >= 0.0f ? _movementConfig.UpGravity : _movementConfig.DownGravity;
			kcc.SetGravity(gravity);
		}
		
		private void HandleMovement()
		{
			// Set default world space input direction and jump impulse.
			Vector3 inputDirection = kcc.TransformRotation * new Vector3(
				input.CurrentInput.MoveDirection.x,
				0.0f,
				input.CurrentInput.MoveDirection.y
			);

			Vector3 desiredMoveVelocity = inputDirection * _movementConfig.MoveSpeed;

			if (kcc.ProjectOnGround(desiredMoveVelocity, out Vector3 projectedDesiredMoveVelocity))
			{
				desiredMoveVelocity = Vector3.Normalize(projectedDesiredMoveVelocity) * _movementConfig.MoveSpeed;
			}

			float acceleration = GetCurrentAcceleration(desiredMoveVelocity);
			MoveVelocity = Vector3.Lerp(MoveVelocity, desiredMoveVelocity, acceleration * Runner.DeltaTime);

			kcc.Move(MoveVelocity, _jumpImpulse);
		}
		
		private void AimGun()
		{
			Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
			Vector3 lookTarget;
        
			if (Physics.Raycast(ray, out RaycastHit hit, 100f))
				lookTarget = hit.point;
			else
				lookTarget = ray.GetPoint(100f);
        
			Vector3 direction = (lookTarget - primaryWeaponHolder.position).normalized;
			primaryWeaponHolder.rotation = Quaternion.LookRotation(direction);
		}
		
		private void GetCameraStartAndDirection(out Vector3 cameraStart, out Vector3 cameraDirection)
		{
			Camera cam = Camera.main;

			Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
			cameraDirection = ray.direction;
			cameraStart = ray.origin;	
		}

		private void OnDeath(DeathData deathData)
		{
			if(!HasStateAuthority) return;
			
			Rpc_DeathPlayer();
		}
		
		[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
		private void Rpc_DeathPlayer()
		{
			if (!networkHealth.IsAlive && playerLifecycle.IsEnabled)
			{
				playerLifecycle.Die();
			}
		}

		public void Respawn(Vector3 respawnPosition)
		{
			if(!HasStateAuthority) return;
			
			kcc.SetPosition(respawnPosition);
			Rpc_RespawnPlayer();
			networkHealth.Reset();
		}
		
		[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
		private void Rpc_RespawnPlayer()
		{
			playerLifecycle.Respawn();
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
			return primaryWeaponHolder;
		}
		
		public override void Spawned()
		{
			if (HasStateAuthority)
			{
				networkHealth.Owner = Object.InputAuthority;
				networkHealth.DeathEvent += OnDeath;
			}
			
			_movementConfig = playerMovementSettings.GetConfig();
			InitializeCamera();
		}
		
		public override void FixedUpdateNetwork()
		{
			if(!networkHealth.IsAlive) return; 
			
			HandleLookRotation();
			HandleJumpInput();
			HandleShootingInput();
			ApplyGravity();
			HandleMovement();
		}

		private void LateUpdate()
		{
			// Only InputAuthority needs to update camera.
			if (HasInputAuthority == false)
				return;

			// Update camera pivot and transfer properties from camera handle to Main Camera.
			// Render() is executed before KCC because of [OrderBefore(typeof(KCC))].
			// So we have to do it from LateUpdate() - which is called after Render().

			Vector2 pitchRotation = kcc.GetLookRotation(true, false);
			cameraHandle.localRotation = Quaternion.Euler(pitchRotation);

			_cameraTransform.SetPositionAndRotation(cameraHandle.position, cameraHandle.rotation);
			
			AimGun();
		}
	}
}