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
        //    // �̱����� ���� ���� ���� ��ġ�� ����
        //    other.transform.position = ConnManager.Conn.PC_Spawn;

        //    // �ݵ����� ���������� �� ����
        //    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //    // �����ð� ���� ����, �� 2��
        //    other.GetComponent<PCPlayerHit>().currentTime = 0;


        //    // �����ð� ���� ����, �� 2��
        //    other.GetComponent<PCPlayerHit>().HP -= 1;
        //}
    }
}
