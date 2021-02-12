using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // This script manages the camera movement depending on the objects' position
    [SerializeField]
    private GameObject mPositionObject;
    [SerializeField]
    private GameObject mLookatObject;
    [SerializeField] 
    private bool mFirstPersonView = false; // determines whether the camera is placed on the object

    [SerializeField] private Vector3 mCameraStartPosition = new Vector3(5f, 5f, 5f);

    private const float MaxDistance = 25;
    void Start()
    {
        transform.position = mCameraStartPosition;
    }
    
    void FixedUpdate()
    {
        transform.position = mPositionObject.transform.position; // place the camera near (on) the object

        if (!mFirstPersonView)
        {
            var distance = mPositionObject.transform.position - mLookatObject.transform.position;
            if (distance.magnitude > MaxDistance) // limit the maximum distance to avoid extreme distancing
            {
                distance = distance.normalized * MaxDistance;
            }
            transform.position += (distance + mCameraStartPosition); // place the camera on 
        }

        transform.LookAt(mLookatObject.transform.position); // look at the LookAt object
    }
}
