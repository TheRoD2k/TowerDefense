using Field;
using UnityEngine;
using Grid = Field.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;

        private Transform m_Transform;

        private const float TOLERANCE = 0.01f;
        private EnemyData m_EnemyData;

        private Grid m_Grid;
        private Node m_CurrentNode;
        
        public FlyingMovementAgent(float mSpeed, Transform mTransform, Grid grid, EnemyData enemyData)
        {
            m_Speed = mSpeed;
            m_Transform = mTransform;
            m_EnemyData = enemyData;
            m_Grid = grid;
            SetTargetNode(grid.GetTargetNode());
            m_CurrentNode = grid.GetStartNode();
        }

        private Node m_TargetNode;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            Vector3 position = m_Transform.position; // Needed for calculating position on the grid
            target.y = position.y;
            
            Node currentNode = m_Grid.GetNodeAtPoint(position);
            float distance = (target - position).sqrMagnitude;
            if (currentNode != m_CurrentNode)
            {
                m_CurrentNode.EnemyDatas.Remove(m_EnemyData);
                m_CurrentNode = currentNode;
                m_CurrentNode.EnemyDatas.Add(m_EnemyData);
            }
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Vector3 dir = (target - position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}