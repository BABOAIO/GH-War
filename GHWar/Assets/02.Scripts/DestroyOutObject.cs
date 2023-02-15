using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ѷ��γ��� ���� ���� �ִ´�.(Ʈ���Ÿ� üũ�ؾ� ����)
// ������ �������� ������Ʈ�� ���� ������ �ٿ��ֱ� ���� ��
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
