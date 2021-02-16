using System;
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
        private float m_NodeSize = 0.1f;
        
        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;
        
        private void Awake()
        {
            m_Grid = new Grid(m_GridWidth, m_GridHeight);
            m_Camera = Camera.main;
        }

        
        // Identical to the one we wrote on the lesson
        private void Update()
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
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int y = (int) (difference.z / m_NodeSize);
                
                Debug.Log(x + " " + y);
            }
        }

        // Using OnValidate to be able to see the current grid
        // outside the game mode
        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f, 
                height * 0.1f);

            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
        }

        // Places a sphere at the left bottom corner of the grid
        /*private void OnDrawGizmos()
        {
            Debug.Log(m_Offset);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_Offset, 1f);
        }*/
    }
}