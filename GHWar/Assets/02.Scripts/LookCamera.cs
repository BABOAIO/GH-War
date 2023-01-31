using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    Camera cameraToLookAt;

    void Start()
    {
        cameraToLookAt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    // Update is called once per frame

    void Update()
    {
        if (!cameraToLookAt)
        {
            cameraToLookAt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        else
        {
            transform.LookAt(cameraToLookAt.transform.position);
            //transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.back, cameraToLookAt.transform.rotation * Vector3.down);
        }
    }
}
