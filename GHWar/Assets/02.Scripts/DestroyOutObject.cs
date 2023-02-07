using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutObject : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle")
            || other.gameObject.CompareTag("Arrow")
            || other.gameObject.CompareTag("CannonBall")
            || other.gameObject.CompareTag("Rock"))
        {
            Destroy(other.gameObject);
        }

        //if (other.gameObject.CompareTag("PC_Player"))
        //{
        //    // 싱글턴을 통한 원래 스폰 위치로 복귀
        //    other.transform.position = ConnManager.Conn.PC_Spawn;

        //    // 반동으로 떨어졌을때 힘 억제
        //    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //    // 일정시간 동안 무적, 약 2초
        //    other.GetComponent<PCPlayerHit>().currentTime = 0;


        //    // 일정시간 동안 무적, 약 2초
        //    other.GetComponent<PCPlayerHit>().HP -= 1;
        //}
    }
}
