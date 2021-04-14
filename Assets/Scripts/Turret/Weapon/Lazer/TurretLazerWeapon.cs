using System;
using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Turret.Weapon.Lazer
{
    public class TurretLazerWeapon : ITurretWeapon
    {
        private LineRenderer m_LineRenderer;
        private TurretLazerWeaponAsset m_Asset;
        private TurretView m_View;
        private List<Node> m_NodeCircle = new List<Node>();
        private float m_MaxDistance;
        private float m_Damage;
        private EnemyData m_ClosestEnemyData; 
        public TurretLazerWeapon(TurretLazerWeaponAsset asset, TurretView view)
        {
            m_Asset = asset;
            m_MaxDistance = m_Asset.MaxDistance;
            m_Damage = m_Asset.Damage;
            m_View = view;
            m_NodeCircle = Game.Player.Grid.GetNodesInCircle(view.transform.position, m_MaxDistance);
            m_LineRenderer = Object.Instantiate(asset.LineRendererPrefab, m_View.ProjectileOrigin.transform, true);
            m_LineRenderer.positionCount = 2;
        }

        public void TickShoot()
        {
            TickWeapon();
            TickTower();
        }

        private void TickWeapon()
        {
            m_ClosestEnemyData = EnemySearch.GetClosestEnemy(m_View.transform.position, m_MaxDistance, m_NodeCircle);

            if (m_ClosestEnemyData == null)
            {
                m_LineRenderer.gameObject.SetActive(false);
                Debug.Log("Closest enemy null!");
                return;
            }
            else
            {
                var closestEnemyPosition = m_ClosestEnemyData.View.transform.position;
                m_View.TowerLookAt(closestEnemyPosition);
                Vector3 projectileOriginPosition = m_View.ProjectileOrigin.position;
                m_LineRenderer.SetPosition(0, projectileOriginPosition);
                m_LineRenderer.SetPosition(1, closestEnemyPosition);
                m_LineRenderer.gameObject.SetActive(true);
            }
            
            TickTower();
            
            Debug.Log("Hit Lazer!");
            m_ClosestEnemyData.GetDamage(m_Damage * Time.deltaTime);
        }

        private void TickTower()
        {
            if (m_ClosestEnemyData != null)
            {
                Debug.Log("Enemy not null!");
                m_View.TowerLookAt(m_ClosestEnemyData.View.transform.position);
            }
        }
    }
}