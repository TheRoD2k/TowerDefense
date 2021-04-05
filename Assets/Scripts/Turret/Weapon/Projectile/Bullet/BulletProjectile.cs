using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Bullet
{
    public class BulletProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed = 1f;
        private int m_Damage = 5;
        private bool m_DidHit = false;
        private EnemyData m_HitEnemy = null;
        public void TickApproaching()
        {
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("On trigger enter");
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
                m_HitEnemy.GetDamage(m_Damage);
                Debug.Log("hit!");
                
            }

            Destroy(gameObject);
        }
    }
}