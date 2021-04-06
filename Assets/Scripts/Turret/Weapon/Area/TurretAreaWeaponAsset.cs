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
            AreaView instantiatedArea = Instantiate(AreaView, view.transform);
            instantiatedArea.transform.localScale = Vector3.one * (MaxDistance*2*10f);
            return new TurretAreaWeapon(this, view, instantiatedArea);
        }
    }
}