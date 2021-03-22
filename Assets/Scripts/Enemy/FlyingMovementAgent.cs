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

        public FlyingMovementAgent(float mSpeed, Transform mTransform, Grid grid)
        {
            m_Speed = mSpeed;
            m_Transform = mTransform;
            
            SetTargetNode(grid.GetTargetNode());
        }

        private Node m_TargetNode;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            target.y = m_Transform.position.y;
            
            float distance = (target - m_Transform.position).magnitude;
            if (distance < TOLERANCE)
            {
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Vector3 dir = (target - m_Transform.position).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}