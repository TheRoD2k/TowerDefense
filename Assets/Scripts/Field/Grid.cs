﻿using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class Grid
    {
        private Node[,] m_Nodes;

        private int m_Width;
        private int m_Height;

        private FloatFieldPathfinding m_Pathfinding;
        
        public int Width => m_Width;

        public int Height => m_Height;

        public Grid(int width, int height, Vector3 offset, float nodeSize, Vector2Int target, Vector2Int start)
        {
            m_Width = width;
            m_Height = height;

            m_Nodes = new Node[m_Width, m_Height];

            for (int i = 0; i < m_Nodes.GetLength(0); i++)
            {
                for (int j = 0; j < m_Nodes.GetLength(1); j++)
                {
                    m_Nodes[i,j] = new Node(offset + new Vector3(i + .5f, 0, j + .5f) * nodeSize);
                }
            }
            
            m_Pathfinding = new FloatFieldPathfinding(this, target, start);
            m_Pathfinding.UpdateField();
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