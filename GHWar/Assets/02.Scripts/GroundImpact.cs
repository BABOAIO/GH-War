using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundImpact : MonoBehaviour
{
    [Header("���� ĥ�� ������ ����Ʈ")]
    [SerializeField] ParticleSystem ps_hitGround;
    [Header("���� ĥ�� ������ ť��")]
    [SerializeField] GameObject o_stone;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HitZone"))
        {
            print("��Ʈ �� �浹�ν�");
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 0)
            {

                print("��Ʈ �� �ӵ� ����");
            }
        }
    }
}
