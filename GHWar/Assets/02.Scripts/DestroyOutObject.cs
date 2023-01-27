using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Arrow"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("PC_Player"))
        {
            // 싱글턴을 통한 원래 스폰 위치로 복귀
            other.transform.position = ConnManager.Conn.PC_Spawn;

            // 일정시간 동안 무적, 약 2초
            other.GetComponent<PCPlayerHit>().currentTime = 0;

            other.GetComponent<PCPlayerHit>().HP  -= 1;
            // 게임을 시작했을 경우에만 목숨 카운트 소모
            if (GameManager.instance.B_GameStart)
            {
                //GameManager.instance.i_PCDeathCount--;
            }
        }
    }
}
