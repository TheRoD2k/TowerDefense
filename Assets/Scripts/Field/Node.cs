using UnityEngine;

namespace Field
{
    public enum OccupationAvailability
    {
        CanOccupy,
        CanNotOccupy,
        Undefined
    }
    public class Node
    {
        public Vector3 Position;

        public Node NextNode;
        public bool IsOccupied = false;
        
        public float PathWeight;
        public OccupationAvailability OccupationAvailability = OccupationAvailability.Undefined;
        public Node(Vector3 position)
        {
            Position = position;
        }
        
        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}