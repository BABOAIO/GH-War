using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// VR 플레이어의 손(콜라이더가 있는)에 넣어준다.
// 주먹을 쥐지 않는 한 멀리 날라감을 방지
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
