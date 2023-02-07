using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resource 에 있는 대포알에 넣는다.
public class CannonBall : MonoBehaviour
{
    bool isHit = false;

    // 소리부분 //
    AudioSource as_arrow;
    //[SerializeField] AudioClip ac_shotArrow;
    [SerializeField] AudioClip ac_cannonHit;
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
        as_arrow = GetComponent<AudioSource>();
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        // VR플레이어가 피격될 시
        if (collision.gameObject.CompareTag("VRPlayerHead"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // 소리부분 //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // 소리부분 //

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        // 대포알이 대포에 부딪힘방지, 주석풀면 플레이어가 자살 불가능
        else if (
            //!collision.gameObject.CompareTag("PC_Player") && 
            !(collision.gameObject.layer == LayerMask.NameToLayer("Turret")))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // 소리부분 //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // 소리부분 // 

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        if (collision.gameObject.CompareTag("Obstacle")|| collision.gameObject.CompareTag("Rock"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // 소리부분 //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // 소리부분 //

            PhotonNetwork.Destroy(collision.gameObject);
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
