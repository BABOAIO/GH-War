using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class VRPlayerHit : MonoBehaviour
{
    float invincibilityTime = 2.0f;
    float currentTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && currentTime>=invincibilityTime)
        {
            print("VR È÷Æ®");
            this.GetComponent<VRPlayerMove1>().Hit_VRPlayer(1);
            currentTime = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }
}
