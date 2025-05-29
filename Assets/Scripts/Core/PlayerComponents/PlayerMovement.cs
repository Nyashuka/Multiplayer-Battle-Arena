using System.Net.NetworkInformation;
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

        [Networked] private Vector3 MoveVelocity { get; set; }
        
        public void Init(NetworkRunner runner, PlayerInput input)
        {
            _input = input;
            _runner = runner;
            _config = movementSettings.GetConfig();
        }

        public void Tick()
        {
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
    }
}