using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundImpact : MonoBehaviour
{
    [Header("땅을 칠때 나오는 이펙트")]
    [SerializeField] ParticleSystem ps_hitGround;
    [Header("땅을 칠때 나오는 큐브")]
    [SerializeField] GameObject o_stone;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HitZone"))
        {
            print("히트 존 충돌인식");
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 0)
            {

                print("히트 존 속도 감지");
            }
        }
    }
}
