using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PCPlayerHit : MonoBehaviour
{
    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody>() == null) { return; }

        float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        print(f_objVelocity);
        if (f_objVelocity >= 10f && currentTime>=invincibilityTime)
        {
            print("PC È÷Æ®");
            this.GetComponent<PC_Player_Move>().Hit_PCPlayer(1);
            currentTime = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }
}
