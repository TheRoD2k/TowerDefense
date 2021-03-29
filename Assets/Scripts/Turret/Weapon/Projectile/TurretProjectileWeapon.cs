using Enemy;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile
{
    public class TurretProjectileWeapon : ITurretWeapon
    {
        private TurretProjectileWeaponAsset m_Asset;
        private TurretView m_View;
        private float m_TimeBetweenShots;
        private float m_MaxDistance;

        private float m_LastShotTime = 0f;
        public TurretProjectileWeapon(TurretProjectileWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_TimeBetweenShots = 1f / m_Asset.RateOfFire;
            m_MaxDistance = m_Asset.MaxDistance;
            m_View = view;
        }
        
        public void TickShoot()
        {
            float passedTime = Time.time - m_LastShotTime;
            if (passedTime < m_TimeBetweenShots)
            {
                return;
            }

            EnemyData closestEnemyData =
                Game.Player.EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance);

            if (closestEnemyData == null)
            {
                return;
            }
            
            Shoot(closestEnemyData);
            m_LastShotTime = Time.time;
        }

        private void Shoot(EnemyData enemyData)
        {
            m_Asset.ProjectileAsset.CreateProjectile( m_View.ProjectileOrigin.position, m_View.ProjectileOrigin.forward, enemyData);
            
        }
    }
}