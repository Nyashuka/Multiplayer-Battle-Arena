using Core.MainWeapons;
using Core.MainWeapons.Abstract;
using Core.PlayerComponents.HealthComponent;
using Core.UtilityItems;
using Data;
using Fusion;
using Fusion.Addons.SimpleKCC;
using Networking;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using UnityEngine;

namespace Core.PlayerComponents
{
	[DefaultExecutionOrder(-5)]
	public sealed class Player : NetworkBehaviour
	{
		[Header("Player Modules")] 
		[SerializeField] private PlayerMovement playerMovement;
		[SerializeField] private PlayerCamera playerCamera;
		[SerializeField] private PlayerCombat playerCombat;
		[SerializeField] private PlayerLifecycle playerLifecycle;
		[SerializeField] private MainWeaponHandler mainWeaponHandler;
		[SerializeField] private UtilityItemHandler utilityItemHandler;
		[SerializeField] private NetworkHealth networkHealth;
		[SerializeField] private PlayerInput input;
		[Header("Player Components")] 
		[SerializeField] private SimpleKCC kcc;
		[SerializeField] private Transform mainWeaponTransform;
		[SerializeField] private GameObject[] playerVisualParts;
		[SerializeField] private LayerMask playerVisualLayer;
		
		public NetworkHealth NetworkHealth => networkHealth;
		public Transform MainWeaponTransform => mainWeaponTransform;
		
		public override void Spawned()
		{
			networkHealth.ResetHealth();
			if (HasStateAuthority)
			{
				networkHealth.Owner = Object.InputAuthority;
				networkHealth.DeathEvent += OnDeath;
			}
			
			playerCombat.Init(input, playerCamera, this);
			playerMovement.Init(Runner, input);
			playerCamera.Init(input);

			LocalPlayerSetup();
		}

		private void LocalPlayerSetup()
		{
			if (Runner.LocalPlayer == Object.InputAuthority)
			{
				Debug.Log(Object.InputAuthority);
				GameEventBus.Instance.RaiseEvent(GameEventDefinitions.PlayerInitialSpawned, 
					new PlayerSpawnedEventArgs(this), 
					true);
				
				int layerIndex = Mathf.RoundToInt(Mathf.Log(playerVisualLayer.value, 2));
				foreach (GameObject obj in playerVisualParts)
				{
					obj.layer = layerIndex;
				}
				
				kcc.Collider.gameObject.layer = LayerMask.NameToLayer("Player");
			}
		}
		
		public override void FixedUpdateNetwork()
		{
			if(!networkHealth.IsAlive) return;

			playerMovement.Tick();
			playerCombat.Tick();
		}

		private void LateUpdate()
		{
			if (HasInputAuthority)	
				playerCamera.Tick();			
		}
		
		public void SetWeapon(WeaponBase newWeapon)
		{
			if (!HasStateAuthority) return;
    
			mainWeaponHandler.EquipWeapon(newWeapon);	
		}
		
		public void SetUtilityItem(string id)
		{
			if(!HasStateAuthority) return;
			
			utilityItemHandler.SetItem(id);
		}
	
		private void OnDeath(DeathData deathData)
		{
			if(!HasStateAuthority) return;
			
			playerLifecycle.Die();
		}
		
		public void Respawn(Transform spawnPoint)
		{
			if(!HasStateAuthority) return;
			
			playerLifecycle.Respawn(spawnPoint);
		}
	}
}