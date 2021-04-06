using UnityEngine;

namespace Turret.Weapon.Lazer
{
    [CreateAssetMenu(menuName = "Assets/Turret Lazer Weapon Asset", fileName = "Turret Lazer Weapon Asset")]
    public class TurretLazerWeaponAsset : TurretWeaponAssetBase
    {
        public float MaxDistance;
        public float Damage;
        
        public LineRenderer LineRendererPrefab;

        public override ITurretWeapon GetWeapon(TurretView view)
        {
            return new TurretLazerWeapon(this, view);
        }
    }
}