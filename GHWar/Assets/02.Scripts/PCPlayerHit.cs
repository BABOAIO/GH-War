using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PCPlayerHit : MonoBehaviourPunCallbacks
{
    float invincibilityTime = 1.0f;
    float currentTime = 1.0f;

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if (f_objVelocity > 10 && currentTime >= invincibilityTime)
        {
            //this.GetComponent<PC_Player_Move>().Hit_PCPlayer(f_objVelocity);
            currentTime = 0.0f;
        }
    }
}
