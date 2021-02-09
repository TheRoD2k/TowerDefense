using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject m_Position_Object;
    [SerializeField]
    private GameObject m_Lookat_Object;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Application.targetFrameRate = 144;
        transform.position = m_Position_Object.transform.position;
        transform.LookAt(m_Lookat_Object.transform.position);
    }
}
