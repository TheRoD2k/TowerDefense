using System;
using System.Threading;
using UnityEngine;

namespace Field
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField] 
        private int m_GridWidth = 20;
        [SerializeField] 
        private int m_GridHeight = 20;

        [SerializeField] 
        private Vector2Int m_StartCoordinate;
        [SerializeField]
        private Vector2Int m_TargetCoordinate;

        
        
        
        [SerializeField] 
        private float m_NodeSize = 0.1f;
        
        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;
        
        private void Awake()
        {
           
        }

        public Vector2Int StartCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;

        public void CreateGrid()
        {
            m_Camera = Camera.main;
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f, 
                height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_StartCoordinate, m_TargetCoordinate);

        }
        
        // Identical to the one we wrote on the lesson
        public void RaycastInGrid()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    m_Grid.UnselectNode();
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int y = (int) (difference.z / m_NodeSize);
                
                //Debug.Log(x + " " + y);
                m_Grid.SelectCoordinate(new Vector2Int(x, y));
                /*if (Input.GetMouseButtonDown(0))
                {
                    Node node = m_Grid.GetNode(x, y);
                    m_Grid.TryOccupyNode(new Vector2Int(x, y), !node.IsOccupied);
                }*/
            }
            else
            {
                m_Grid.UnselectNode();
            }
        }

        // Using OnValidate to be able to see the current grid
        // outside the game mode
        private void OnValidate()
        {
            m_Camera = Camera.main;
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f, 
                height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
        }
        
        private void OnDrawGizmos()
        {
            
            // Draw gizmo grid
           
                Debug.Log(m_Offset);
                Gizmos.color = Color.black;
                float height = m_GridHeight * m_NodeSize;
                float width = m_GridWidth * m_NodeSize;
                for (int i = 0; i <= m_GridWidth; i++)
                {
                    Vector3 from = m_Offset + new Vector3(i * m_NodeSize, 0f, 0f);
                    Vector3 to = m_Offset + new Vector3(i * m_NodeSize, 0f, height);
                    Gizmos.DrawLine(from, to);
                }
                for (int i = 0; i <= m_GridHeight; i++)
                {
                    Vector3 from = m_Offset + new Vector3(0f, 0f, i*m_NodeSize);
                    Vector3 to = m_Offset + new Vector3(width, 0f, i*m_NodeSize);
                    Gizmos.DrawLine(from, to);
                }
                
                //Gizmos.DrawSphere(m_Offset, 1f);
            
            
            
            if (m_Grid == null)
            {
                return;
            }

            foreach (var node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }

                if (node.IsOccupied)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(node.Position, 0.05f);
                    continue;
                }
                Gizmos.color = Color.red;
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 dir = end - start;

                start -= dir * 0.25f;
                end -= dir * 0.75f;

                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.01f);
            }
        }
    }
}