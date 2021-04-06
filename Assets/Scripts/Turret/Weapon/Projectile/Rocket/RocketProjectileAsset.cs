using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    
    [CreateAssetMenu]
    
    public class RocketProjectileAsset : ProjectileAssetBase
    {
        
        [SerializeField] 
        private RocketProjectile m_RocketPrefab;
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            var instantiatedPrefab = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            instantiatedPrefab.SetTarget(enemyData);
            return instantiatedPrefab;
        }
    }
}