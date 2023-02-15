using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 맵을 둘러싸놓은 투명 벽에 넣는다.(트리거를 체크해야 동작)
// 밖으로 빠져나간 오브젝트에 대한 연산을 줄여주기 위한 벽
public class DestroyOutObject : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle")
            || other.gameObject.CompareTag("Arrow")
            || other.gameObject.CompareTag("CannonBall")
            || other.gameObject.CompareTag("Rock")
            || other.gameObject.CompareTag("Ground")
            || other.gameObject.CompareTag("SmallRock"))
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
