using System.Collections.Generic;
using Enemy;
using JetBrains.Annotations;
using UnityEngine;

namespace Turret.Weapon
{
    public class EnemySearch
    {
        private IReadOnlyList<EnemyData> m_EnemyDatas;

        public EnemySearch(IReadOnlyList<EnemyData> enemyDatas)
        {
            m_EnemyDatas = enemyDatas;
        }

        [CanBeNull]
        public EnemyData GetClosestEnemy(Vector3 center, float maxDistance)
        {
            float maxSqrDistance = maxDistance * maxDistance;
            EnemyData closestEnemyData = null;
            float closestDistance = float.MaxValue;
            foreach (EnemyData enemyData in m_EnemyDatas)
            {
                Vector3 enemyPosition = enemyData.View.transform.position;
                float distance = (enemyPosition - center).sqrMagnitude;

                if (distance < maxDistance && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemyData = enemyData;
                }
            }

            return closestEnemyData;
        }
    }
}