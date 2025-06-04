using Core.MainWeapons;
using Core.UtilityItems;
using Data;
using UnityEngine;

namespace Core.PlayerComponents
{
    public sealed class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private MainWeaponHandler weaponHandler;
        [SerializeField] private UtilityItemHandler utilityHandler;
        [SerializeField] private Transform gunOrigin;

        private PlayerInput _input;
        private PlayerCamera _playerCamera;
        private Player _player;

        public void Init(PlayerInput input, PlayerCamera playerCamera, Player player)
        {
            _input = input;
            _playerCamera = playerCamera;
            _player = player;
        }

        public void Tick()
        {
            if (_input == null)
                return;

            if (_input.CurrentInput.Actions.WasPressed(_input.PreviousInput.Actions, GameplayInput.FIRE_BUTTON))
            {
                _playerCamera.GetAim(out var origin, out var dir);
                weaponHandler.Fire(origin, dir);
            }

            if (_input.CurrentInput.Actions.WasPressed(_input.PreviousInput.Actions, GameplayInput.USE_UTILITY_BUTTON))
            {
                _playerCamera.GetAim(out var origin, out var dir);
                var context = new UtilityItemUseContext
                {
                    AimDirection = dir,
                    ThrowFrom = origin + dir.normalized,
                };
                utilityHandler.UseItem(context);
            }
        }
    }
}