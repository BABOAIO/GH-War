using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PCPlayerHit : MonoBehaviourPunCallbacks
{

    private void OnCollisionEnter(Collision collision)
    {
        float f_objVelocity = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        if(f_objVelocity > 0 )
        {
            this.GetComponent<PC_Player_Move>().Hit_PCPlayer(50);
        }
    }
}
