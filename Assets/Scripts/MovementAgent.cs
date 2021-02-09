using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_Speed = new Vector3(0f, 0f, 0f);

    [SerializeField] 
    private Vector3 m_Center = new Vector3(0f, 0f, 0f);
    [SerializeField]
    private float m_Center_Mass = 10f;
    
    private const float TOLERANCE = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        //m_Speed = 0f;
        //m_Target = new Vector3(10f, 0f, 0f);
    }

    private const float k_Grav_Const = 0.0001f;
    
    void FixedUpdate()
    {
        Application.targetFrameRate = 144;

        float distance = (m_Center - transform.position).magnitude;
        //if (distance < TOLERANCE)
        //{
        //    return;
       // }
        Vector3 acceleration_direction = m_Center - transform.position;
        Vector3 acceleration = k_Grav_Const * m_Center_Mass * acceleration_direction.normalized /
            acceleration_direction.magnitude / acceleration_direction.magnitude;

        if (distance < 5*TOLERANCE)
        {
            acceleration = acceleration_direction * 0f;
        }
        
        m_Speed = m_Speed + (acceleration * Time.deltaTime/2);
        Vector3 delta = m_Speed * Time.deltaTime;    

        
        transform.Translate(delta);
    }
}
