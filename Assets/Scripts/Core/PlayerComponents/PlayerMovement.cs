using System.Collections.Generic;
using Data;
using Fusion;
using Fusion.Addons.SimpleKCC;
using ScriptableObjects;
using UnityEngine;

namespace Core.PlayerComponents
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SimpleKCC kcc;
        [SerializeField] private PlayerMovementSettings movementSettings;

        private PlayerInput _input;
        private MovementConfig _config;
        private float _jumpImpulse;
        private NetworkRunner _runner;
        private TickTimer _dashCooldownTimer;

        [Networked] private Vector3 MoveVelocity { get; set; }
        
        public void Init(NetworkRunner runner, PlayerInput input)
        {
            _input = input;
            _runner = runner;
            _config = movementSettings.GetConfig();
        }

        public void Tick()
        {
            CheckDash();
            ApplyGravity();
            Rotate();
            ProcessJump();
            ProcessMove();
        }

        public void RespawnAt(Transform point)
        {
            kcc.SetPosition(point.position);
            kcc.SetLookRotation(point.rotation);
        }

        private void ApplyGravity()
        {
            float gravity = kcc.RealVelocity.y >= 0 ? _config.UpGravity : _config.DownGravity;
            kcc.SetGravity(gravity);
        }

        private void Rotate() =>
            kcc.AddLookRotation(_input.CurrentInput.LookRotationDelta);

        private void ProcessJump()
        {
            _jumpImpulse = 0f;
            if (_input.CurrentInput.Actions.WasPressed(_input.PreviousInput.Actions, GameplayInput.JUMP_BUTTON)
                && kcc.IsGrounded)
            {
                _jumpImpulse = _config.JumpImpulse;
            }
        }

        private void ProcessMove()
        {
            Vector3 dir = kcc.TransformRotation * new Vector3(_input.CurrentInput.MoveDirection.x, 0, _input.CurrentInput.MoveDirection.y);
            Vector3 desired = dir * _config.MoveSpeed;

            if (kcc.ProjectOnGround(desired, out var projected))
                desired = projected.normalized * _config.MoveSpeed;

            float accel = GetAcceleration(desired);
            MoveVelocity = Vector3.Lerp(MoveVelocity, desired, accel * _runner.DeltaTime);
            kcc.Move(MoveVelocity, _jumpImpulse);
        }

        private float GetAcceleration(Vector3 desired) =>
            desired == Vector3.zero
                ? (kcc.IsGrounded ? _config.GroundDeceleration : _config.AirDeceleration)
                : (kcc.IsGrounded ? _config.GroundAcceleration : _config.AirAcceleration);
        
        
        private readonly Dictionary<Vector2, float> _lastKeyPressTime = new();
        private const float DoubleTapThreshold = 0.3f;

        private void Dash(Vector3 direction)
        {
            kcc.SetPosition(kcc.Position + direction * movementSettings.DashDistance);
        }        
        
        private void CheckDash()
        {
            var directions = new[]
            {
                (Vector2.up,    kcc.TransformRotation * Vector3.forward),
                (Vector2.down,  kcc.TransformRotation * Vector3.back),
                (Vector2.left,  kcc.TransformRotation * Vector3.left),
                (Vector2.right, kcc.TransformRotation * Vector3.right),
            };

            foreach (var (inputDir, worldDir) in directions)
            {
                if (_input.CurrentInput.MoveDirection == inputDir &&
                    _input.PreviousInput.MoveDirection != inputDir)
                {
                    float currentTime = Time.time;

                    if (_lastKeyPressTime.TryGetValue(inputDir, out float lastTime))
                    {
                        if (currentTime - lastTime <= DoubleTapThreshold && _dashCooldownTimer.ExpiredOrNotRunning(_runner))
                        {
                            Dash(worldDir.normalized);
                            _dashCooldownTimer = TickTimer.CreateFromSeconds(_runner, movementSettings.DashCooldown);
                        }
                    }

                    _lastKeyPressTime[inputDir] = currentTime;
                }
            }
        }    
    }
}