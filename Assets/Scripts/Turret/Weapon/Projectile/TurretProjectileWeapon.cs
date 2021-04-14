using System.Collections.Generic;
using System.Data;
using Enemy;
using Field;
using JetBrains.Annotations;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile
{
    public class TurretProjectileWeapon : ITurretWeapon
    {
        private TurretProjectileWeaponAsset m_Asset;
        private TurretView m_View;
        [CanBeNull]
        private EnemyData m_ClosestEnemyData;

        private List<IProjectile> m_Projectiles = new List<IProjectile>();
        private float m_TimeBetweenShots;
        private float m_MaxDistance;

        private float m_LastShotTime = 0f;

        private List<Node> m_NodeCircle;
        public TurretProjectileWeapon(TurretProjectileWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_TimeBetweenShots = 1f / m_Asset.RateOfFire;
            m_MaxDistance = m_Asset.MaxDistance;
            m_View = view;
            m_NodeCircle = Game.Player.Grid.GetNodesInCircle(view.transform.position, m_MaxDistance);
        }
        
        public void TickShoot()
        {
            TickWeapon();
            TickTower();
            TickProjectiles();
        }

        private void TickWeapon()
        {
            float passedTime = Time.time - m_LastShotTime;
            if (passedTime < m_TimeBetweenShots)
            {
                return;
            }

            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_NodeCircle);

            if (m_ClosestEnemyData == null)
            {
                Debug.Log("Closest enemy null!");
                return;
            }
            
            TickTower();
            
            Shoot(m_ClosestEnemyData);
            m_LastShotTime = Time.time;
        }

        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                Debug.Log("Enemy not null!");
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }
        private void TickProjectiles()
        {
            for (int i = 0; i < m_Projectiles.Count; i++)
            {
                var projectile = m_Projectiles[i];
                projectile.TickApproaching();
                if (projectile.DidHit())
                {
                    projectile.DestroyProjectile();
                    m_Projectiles[i] = null;
                }
            }

            m_Projectiles.RemoveAll(projectile => projectile == null);
        }

        private void Shoot(EnemyData enemyData)
        {
            m_Projectiles.Add(m_Asset.ProjectileAsset.CreateProjectile( m_View.ProjectileOrigin.position, m_View.ProjectileOrigin.forward, enemyData));
            m_View.AnimateShot();
        }
    }
}