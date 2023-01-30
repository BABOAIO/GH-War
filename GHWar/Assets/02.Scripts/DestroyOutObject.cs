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
            // �̱����� ���� ���� ���� ��ġ�� ����
            other.transform.position = ConnManager.Conn.PC_Spawn;

            // �����ð� ���� ����, �� 2��
            other.GetComponent<PCPlayerHit>().currentTime = 0;

            // ������ �������� ��쿡�� ��� ī��Ʈ �Ҹ�
            if (GameManager.instance.B_GameStart)
            {
                other.GetComponent<PCPlayerHit>().HP -= 1;
                //GameManager.instance.i_PCDeathCount--;
            }
        }
    }
}
