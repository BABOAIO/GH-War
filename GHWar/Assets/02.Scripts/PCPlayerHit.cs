using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PCPlayerHit : MonoBehaviour
{
    float invincibilityTime = 2.0f;
    float currentTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        print(f_objVelocity);
        if (f_objVelocity >= 10f && currentTime>=invincibilityTime)
        {
            print("PC È÷Æ®");
            this.GetComponent<PC_Player_Move>().Hit_PCPlayer(1);
            currentTime = 0.0f;
        }
    }
}
