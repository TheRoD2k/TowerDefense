using UnityEngine;

namespace Turret.Weapon.Area
{
    [CreateAssetMenu(menuName = "Assets/Turret Area Weapon Asset", fileName = "Turret Area Weapon Asset")]
    public class TurretAreaWeaponAsset : TurretWeaponAssetBase
    {
        [SerializeField] 
        private AreaView AreaView;
        
        public float MaxDistance;
        public float Damage;
        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretAreaWeapon(this, view, AreaView);
        }
    }
}