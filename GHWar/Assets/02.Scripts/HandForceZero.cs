using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HandForceZero : MonoBehaviourPun
{
    HandPresence handPresence;

    void Start()
    {
        handPresence= GetComponent<HandPresence>();
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PC_Player"))
        {
            if (handPresence.gripValue < 0.3f && handPresence.triggerValue < 0.3f)
            {
                collision.rigidbody.velocity = Vector3.zero;
            }
        }
    }
}
