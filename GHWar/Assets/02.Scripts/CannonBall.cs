using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    Rigidbody rb_this;
    Transform tr_this;

    bool isHit = false;

    // �Ҹ��κ� //
    AudioSource as_arrow;
    //[SerializeField] AudioClip ac_shotArrow;
    [SerializeField] AudioClip ac_cannonHit;
    // �Ҹ��κ� //

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
        as_arrow = GetComponent<AudioSource>();
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("VR_Player"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // �Ҹ��κ� //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // �Ҹ��κ� //

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        else if (!collision.gameObject.CompareTag("PC_Player"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // �Ҹ��κ� //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // �Ҹ��κ� // 

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
