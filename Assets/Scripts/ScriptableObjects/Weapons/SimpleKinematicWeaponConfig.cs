using UnityEngine;

namespace ScriptableObjects.Weapons
{
    [CreateAssetMenu(menuName = "Weapons/Kinematic Weapon Config")] 
    public class KinematicWeaponConfig : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private float damage;
        [SerializeField] private float bulletSpeed;
    }
}