using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public struct Connection
    {
        private Vector2Int coordinate;
        private float weight;

        public Connection(Vector2Int coordinate, float weight)
        {
            this.coordinate = coordinate;
            this.weight = weight;
        }

        public Vector2Int Coordinate => coordinate;

        public float Weight => weight;
    }

    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;

        public FlowFieldPathfinding(Grid mGrid, Vector2Int mStart, Vector2Int mTarget)
        {
            m_Grid = mGrid;
            m_Target = mTarget;
            m_Start = mStart;
        }

        public void UpdateField()
        {
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
                if (!node.IsOccupied)
                {
                    node.OccupationAvailability = OccupationAvailability.CanOccupy;
                }
                else
                {
                    node.OccupationAvailability = OccupationAvailability.CanNotOccupy;
                }
            }

            Node startNode = m_Grid.GetNode(m_Start);
            Node targetNode = m_Grid.GetNode(m_Target);

            startNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
            targetNode.OccupationAvailability = OccupationAvailability.CanNotOccupy;
            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).PathWeight = 0f;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);
                float weightToTarget = m_Grid.GetNode(current).PathWeight;
                foreach (Connection neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);
                    if (weightToTarget + neighbour.Weight < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = weightToTarget + neighbour.Weight;
                        queue.Enqueue(neighbour.Coordinate);
                    }
                }
            }

            Node wayNode = startNode.NextNode;
            while (wayNode != targetNode)
            {
                wayNode.OccupationAvailability = OccupationAvailability.Undefined;
                wayNode = wayNode.NextNode;
            }
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {

            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;

            Vector2Int upRightCoordinate = coordinate + Vector2Int.up + Vector2Int.right;
            Vector2Int upLeftCoordinate = coordinate + Vector2Int.up + Vector2Int.left;
            Vector2Int downRightCoordinate = coordinate + Vector2Int.down + Vector2Int.right;
            Vector2Int downLeftCoordinate = coordinate + Vector2Int.down + Vector2Int.left;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied;

            bool hasUpRightNode = rightCoordinate.x < m_Grid.Width && upCoordinate.y < m_Grid.Height
                                                                   && !m_Grid.GetNode(upRightCoordinate).IsOccupied;
            bool hasUpLeftNode = leftCoordinate.x >= 0 && upCoordinate.y < m_Grid.Height
                                                       && !m_Grid.GetNode(upLeftCoordinate).IsOccupied;
            bool hasDownRightNode = rightCoordinate.x < m_Grid.Width && downCoordinate.y >= 0
                                                                     && !m_Grid.GetNode(downRightCoordinate).IsOccupied;
            bool hasDownLeftNode = leftCoordinate.x >= 0 && downCoordinate.y >= 0
                                                         && !m_Grid.GetNode(downLeftCoordinate).IsOccupied;
 
            if (hasRightNode)
            {
                yield return new Connection(rightCoordinate, 1f);
            }

            if (hasLeftNode)
            {
                yield return new Connection(leftCoordinate, 1f);
            }

            if (hasUpNode)
            {
                yield return new Connection(upCoordinate, 1f);
            }

            if (hasDownNode)
            {
                yield return new Connection(downCoordinate, 1f);
            }

            float sqrt2 = 1.414213f;
            // Check "main" nodes there again (we would have to do it anyway)
            // or we can check diagonal ways in previous if ops (a bit faster, much more confusing)
            if (hasUpRightNode && hasRightNode && hasUpNode)
            {
                yield return new Connection(upRightCoordinate, sqrt2);
            }

            if (hasUpLeftNode && hasLeftNode && hasUpNode)
            {
                yield return new Connection(upLeftCoordinate, sqrt2);
            }

            if (hasDownRightNode && hasRightNode && hasDownNode)
            {
                yield return new Connection(downRightCoordinate, sqrt2);
            }

            if (hasDownLeftNode && hasLeftNode && hasDownNode)
            {
                yield return new Connection(downLeftCoordinate, sqrt2);
            }
        }

        // Check if occupying won't block the last possible way
        // Idea: recalculate weights without changing other fields if undefined
        public bool CanOccupy(Vector2Int coordinate)
        {
            Node node = m_Grid.GetNode(coordinate);

            if (node.OccupationAvailability == OccupationAvailability.CanOccupy)
                return true;
            if (node.OccupationAvailability == OccupationAvailability.CanNotOccupy)
                return false;

            foreach (Node tempNode in m_Grid.EnumerateAllNodes())
            {
                tempNode.ResetWeight();
            }

            Node startNode = m_Grid.GetNode(m_Start);
            Node targetNode = m_Grid.GetNode(m_Target);
            
            // Basically the copy of the code above with minor changes
            targetNode.PathWeight = 0f;

            Queue<Vector2Int> queue = new Queue<Vector2Int>();

            queue.Enqueue(m_Target);
            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                float weightToTarget = m_Grid.GetNode(current).PathWeight;
                foreach (Connection neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour.Coordinate);

                    if (neighbourNode == node)
                    {
                        continue;
                    }

                    if (weightToTarget + neighbour.Weight < neighbourNode.PathWeight)
                    {
                        // Possible to get to start without our node => not needed, return
                        if (neighbourNode == startNode)
                            return true;
                        neighbourNode.PathWeight = weightToTarget + neighbour.Weight;
                        queue.Enqueue(neighbour.Coordinate);
                    }
                }
            }
            return false;
        }
    }

}