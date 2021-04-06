using System;
using System.Collections.Generic;
using Enemy;
using Field;
using Runtime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile :  MonoBehaviour, IProjectile
    {
        private float m_Speed = 2f;
        private float m_Damage = 10.0f;
        private float m_Radius = 0.1f;
        private bool m_DidHit = false;
        private EnemyData m_HitEnemy;
        private EnemyData m_TargetEnemy;
        public void TickApproaching()
        {
            Vector3 direction = (m_TargetEnemy.View.transform.position - transform.position).normalized;
            
            transform.Translate(direction * (m_Speed * Time.deltaTime), Space.World);
            
        }

        public void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_HitEnemy = enemyView.Data;
                }
            }
        }

        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            if (m_HitEnemy != null)
            {
                List<Node> affectedNodes = Game.Player.Grid.GetNodesInCircle(m_HitEnemy.View.transform.position, m_Radius);
                foreach (Node node in affectedNodes)
                {
                    foreach (EnemyData enemyData in node.EnemyDatas)
                    {
                        Debug.Log("Hit rocket!");
                        enemyData.GetDamage(m_Damage);
                    }
                }

                
            }

            Destroy(gameObject);
        }

        public void SetTarget(EnemyData target)
        {
            m_TargetEnemy = target;
        }
    }
}