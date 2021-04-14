using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private Vector2Int m_StartCoordinate;
        private Vector2Int m_TargetCoordinate;

        private Node m_SelectedNode = null;
        
        private FlowFieldPathfinding m_Pathfinding;

        public FlowFieldPathfinding Pathfinding => m_Pathfinding;

        public int Width => m_Width;

        public int Height => m_Height;

        private float m_NodeSize;
        private Vector3 m_Offset;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int start, Vector2Int target)
        {
            m_Width = width;
            m_Height = height;
            
            m_NodeSize = nodeSize;
            m_Offset = offset;

            m_StartCoordinate = start;
            m_TargetCoordinate = target;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i,j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }
            
            m_Pathfinding = new FlowFieldPathfinding(this, start, target);
            m_Pathfinding.UpdateField();
        }

        public Node GetStartNode()
        {
            return GetNode(m_StartCoordinate);
        }

        public Node GetTargetNode()
        {
            return GetNode(m_TargetCoordinate);
        }

        public void SelectCoordinate(Vector2Int coordinate)
        {
            m_SelectedNode = GetNode(coordinate);
        }
        
        public void UnselectNode()
        {
            m_SelectedNode = null;
        }

        public bool HasSelectedNode()
        {
            return m_SelectedNode != null;
        }

        public Node GetSelectedNode()
        {
            return m_SelectedNode;
        }

        public Node GetNode(Vector2Int coordinate)
        {
            return GetNode(coordinate.x, coordinate.y);
        }

        public Node GetNode(int i, int j)
        {
            if (i < 0 || i >= m_Width)
            {
                return null;
            }

            if (j < 0 || j >= m_Height)
            {
                return null;
            }
            
            return m_Nodes[i, j];
        }

        public Node GetNodeAtPoint(Vector3 point)
        {
            float invertNodeSize = 1f / m_NodeSize;
            int i = Mathf.RoundToInt((point.x - m_Offset.x) * invertNodeSize);
            int j = Mathf.RoundToInt((point.z - m_Offset.z) * invertNodeSize);
            return GetNode(i, j);
        }

        public List<Node> GetNodesInCircle(Vector3 point, float radius)
        {
            float wider_radius = radius + m_NodeSize * 0.70710675f;
            float sqrRadius = wider_radius * wider_radius;
            List<Node> nodesCircle = new List<Node>();
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    Node node = GetNode(i, j);
                    float sqrDistance = (node.Position - point).sqrMagnitude;
                    if (sqrDistance <= sqrRadius)
                    {
                        nodesCircle.Add(node);
                    }
                }
            }
            return nodesCircle;
        }

        public IEnumerable<Node> EnumerateAllNodes()
        {
            for (int i = 0; i < m_Width; i++)
            {
                for (int j = 0; j < m_Height; j++)
                {
                    yield return GetNode(i, j);
                }
            }
        }

        public void UpdatePathfinding()
        {
            m_Pathfinding.UpdateField();
        }

        public void TryOccupyNode(Vector2Int coordinate, bool occupy)
        {
            Node node = GetNode(coordinate);
            if (occupy == false)
            {
                node.IsOccupied = false;
                UpdatePathfinding();
                return;
            }

            if (m_Pathfinding.CanOccupy(coordinate))
            {
                node.IsOccupied = true;
                UpdatePathfinding();
            }
        }

    }
    

}