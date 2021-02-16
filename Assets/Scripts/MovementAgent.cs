using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The class is needed to make the object move towards the target
public class MovementAgent : MonoBehaviour
{
    // Default parameters are set to be non connected to the grid as
    // this class doesn't now anything about it
    [SerializeField]
    private float m_Speed = 0.1f;
    [SerializeField]
    private Vector3 m_BodyStartPosition = new Vector3(0f, 0f, 0f);

    private const float Tolerance = 0.001f;

    private Vector3 m_CurrentTarget;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = m_BodyStartPosition;
        m_CurrentTarget = m_BodyStartPosition;
    }

    // Sets the new target
    public void SetTarget(Vector3 target)
    {
        m_CurrentTarget = target;
    }
    
    void FixedUpdate()
    {
        Vector3 distance = m_CurrentTarget - transform.position;
        Vector3 delta = new Vector3(0f, 0f, 0f);
        // Move while the distance is bigger than Tolerance
        if (distance.magnitude > Tolerance)
        {
            delta = distance.normalized * (m_Speed * Time.deltaTime);
        }
        // Seems like it doesn't matter whether to use the position or
        // the Translate as far as we don't rotate
        transform.position += delta;
    }
}
