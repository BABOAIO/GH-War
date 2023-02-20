using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// Resource 폴더에 있는 Arrow에 넣는다.
[RequireComponent(typeof(AudioSource))]
public class ArrowProperty_PowerShot : MonoBehaviour
{
    // 생성 후 날라가는 속도
    [SerializeField] float shotSpeed = 20.0f;
    Rigidbody rb_this;
    Transform tr_this;

    [SerializeField] Transform tr_COM;

    // 부딪혔을때 계속 날라감을 방지
    bool isHit = false;

    // 소리부분 //
    AudioSource as_arrow;
    //[SerializeField] AudioClip ac_shotArrow;
    [SerializeField] AudioClip ac_shotHit;
    // 소리부분 //

    void Awake()
    {
        //rb_this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        isHit = false;
    }

    private void Start()
    {
        tr_this = GetComponent<Transform>();
        as_arrow= GetComponent<AudioSource>();
        rb_this = GetComponent<Rigidbody>();
        rb_this.centerOfMass = tr_COM.position;
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
        // 부딪히지 않았을 경우, 앞으로 날라간다.(중력의 영향을 어느정도 받는것으로 보인다.)
        // 같은 화살끼리 부딪히지 않게 주의(레이어 피직스로 조정가능)
        if (!isHit)
        {
            tr_this.Translate(Vector3.forward * shotSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("LeftHandPhysics") || collision.gameObject.layer == LayerMask.NameToLayer("RightHandPhysics"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("VRPlayerHead"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        // 땅의 메쉬가 안보여서 이렇게 안하면 땅에 박힘
        else if (!collision.gameObject.CompareTag("Ground"))
        {
        }

        else if(!collision.gameObject.CompareTag("PC_Player"))
        {
            isHit = true;

            GameObject o_ps = 
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }
    }

    IEnumerator DestroyDelayed(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null) 
        {
            PhotonNetwork.Destroy(obj);
        }
    }
}
