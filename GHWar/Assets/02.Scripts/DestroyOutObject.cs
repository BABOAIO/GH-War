using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("CannonBall"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("PC_Player"))
        {
            // �̱����� ���� ���� ���� ��ġ�� ����
            other.transform.position = ConnManager.Conn.PC_Spawn;

            // �ݵ����� ���������� �� ����
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // �����ð� ���� ����, �� 2��
            other.GetComponent<PCPlayerHit>().currentTime = 0;
        }
    }
}
