using System;
using UnityEngine;

namespace Field
{
    public class MovementCursor : MonoBehaviour
    {
        [SerializeField] 
        private int m_GridWidth = 20;
        [SerializeField] 
        private int m_GridHeight = 20;

        [SerializeField] 
        private float m_NodeSize = 0.1f;

        // The class we use as the movable character
        [SerializeField] 
        private MovementAgent m_MovementAgent;
        
        // The cursor object (obviously)
        [SerializeField]
        private GameObject m_Cursor;
        

        private Camera m_Camera;

        // Coordinates of the left bottom corner
        private Vector3 m_Offset;
        

        private void Start()
        {
            m_Camera = Camera.main; // Initialize camera to raytrace
        }

        private void Update()
        {
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit))   // Raycast camera -> cursor
            {
                // Did not hit the plane -> do nothing
                // Not really okay to simply return imho, but fine so far
                if (hit.transform != transform)
                {
                    return;
                }
                
                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int y = (int) (difference.z / m_NodeSize);
                Vector3 hitNodeCenter = m_Offset + new Vector3(
                    m_NodeSize * x + m_NodeSize * 0.5f,
                    0f, 
                    m_NodeSize * y + m_NodeSize * 0.5f);
                
                // Set a new target for the object if the lmb was pressed
                if (Input.GetMouseButton(0) && m_MovementAgent != null)
                {
                    m_MovementAgent.SetTarget(hitNodeCenter);
                }

                if (m_Cursor != null)   // Move the cursor
                {
                    m_Cursor.SetActive(true);
                    m_Cursor.transform.position = hitNodeCenter;
                    //Debug.Log("Cursor enabled and set at " + hitNodeCenter);
                }

                
                // Print the grid coordinates in console
                Debug.Log(x + " " + y);
            }
            else
            {
                // We assume that the only time we need to disable cursor
                // is when it doesn't point to the grid or object
                if (m_Cursor != null)
                {
                    //Debug.Log("CURSOR DISABLED");
                    m_Cursor.SetActive(false);
                }
            }
        }

        
        // Recalculate m_Offset to make gizmos work
        private void OnValidate()
        {
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            m_Offset = transform.position - (new Vector3(width, 0f, height) * 0.5f);
            m_Cursor.SetActive(false);  // The cursor itself has no logic => it's ok to disable
        }


        // Draw gizmo grid
        /*private void OnDrawGizmos()
        {
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
        }*/
    }
}