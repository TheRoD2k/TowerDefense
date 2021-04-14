using System.Collections;
using Assets;
using Runtime;
using UnityEngine;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        private float m_Health;
        private bool m_IsDead = false;
        
        public EnemyView View => m_View;

        private EnemyAsset m_Asset;

        public EnemyAsset Asset => m_Asset;
        public EnemyData(EnemyAsset mAsset)
        {
            m_Asset = mAsset;
            m_Health = mAsset.StartHealth;
        }

        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }

        public void GetDamage(float damage)
        {
            Debug.Log("Damage taken!");
            m_Health -= damage;
            if (m_Health <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            if (m_IsDead == true)
                return;
            m_IsDead = true;
            Debug.Log("Death func launched");
            Game.Player.EnemyDied(this);
            m_View.MovementAgent.Die();
            m_View.AnimateDeath();
            GameObject.Destroy(m_View.gameObject, 10f);
            Debug.Log("Die");
        }
    }
}