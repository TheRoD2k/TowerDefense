using UnityEngine;

namespace Enemy
{

    [CreateAssetMenu(menuName = "Assets/Enemy Asset", fileName = "Enemy Asset")]

    public class EnemyAsset : ScriptableObject
    {
        public bool IsFlyingEnemy;
        public float StartHealth;
        public float Speed;
        public EnemyView ViewPrefab;
    }
}