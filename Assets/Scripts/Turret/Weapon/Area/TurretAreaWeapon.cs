using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Area
{
    public class TurretAreaWeapon : ITurretWeapon
    {
        private TurretView m_View;
        private AreaView m_AreaView;    // Might be needed later, left it for now
        private TurretAreaWeaponAsset m_Asset;
        private List<Node> m_NodeCircle = new List<Node>();
        private EnemyData m_ClosestEnemy;
        private float m_MaxDistance;
        private float m_Damage;

        public TurretAreaWeapon(TurretAreaWeaponAsset asset, TurretView view, AreaView areaView)
        {
            m_Asset = asset;
            m_View = view;
            m_AreaView = areaView;
            m_MaxDistance = m_Asset.MaxDistance;
            m_Damage = m_Asset.Damage;
            Vector3 position = m_View.transform.position;
            m_NodeCircle = Game.Player.Grid.GetNodesInCircle(position, m_MaxDistance);
            AreaView instantiatedArea = AreaView.Instantiate(m_AreaView, view.transform);
            instantiatedArea.transform.localScale = Vector3.one * (m_MaxDistance*2*10f);
        }

        public void TickShoot()
        {
            TickWeapon();
        }

        private void TickWeapon()
        {
            // Idea: make it check for the closest enemy to trigger animations more nicely
            foreach (Node node in m_NodeCircle)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    Debug.Log("Hit Area!");
                    enemyData.GetDamage(m_Damage * Time.deltaTime);
                }
            }
        }
    }
}