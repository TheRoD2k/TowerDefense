using System.Collections.Generic;
using Enemy;
using Field;
using JetBrains.Annotations;
using UnityEngine;

namespace Turret.Weapon
{
    public static class EnemySearch
    {
        [CanBeNull]
        public static EnemyData GetClosestEnemy(Vector3 center, float maxDistance, List<Node> nodes)
        {
            float maxSqrDistance = maxDistance * maxDistance;
            EnemyData closestEnemyData = null;
            float closestDistance = float.MaxValue;
            foreach (Node node in nodes)
            {
                foreach (EnemyData enemyData in node.EnemyDatas)
                {
                    Vector3 enemyPosition = enemyData.View.transform.position;
                    float distance = (enemyPosition - center).sqrMagnitude;

                    if (distance < maxDistance && distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemyData = enemyData;
                    }
                }
            }

            return closestEnemyData;
        }
    }
}