using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class ArrowProperty : MonoBehaviour
{
    [SerializeField] float shotSpeed = 20.0f;
    Rigidbody rb_this;
    Transform tr_this;

    bool isHit = false;

    // 家府何盒 //
    AudioSource as_arrow;
    //[SerializeField] AudioClip ac_shotArrow;
    [SerializeField] AudioClip ac_shotHit;
    // 家府何盒 //

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
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
        // 家府何盒 //
        //if (!as_arrow.isPlaying)
        //{
        //    as_arrow.PlayOneShot(ac_shotArrow);
        //}
        // 家府何盒 //

        if(!isHit)
        {
            tr_this.Translate(Vector3.forward * shotSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("VR_Player"))
        {
            isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // 家府何盒 //
            as_arrow.Stop();
            as_arrow.PlayOneShot(ac_shotHit);
            // 家府何盒 //

            //PhotonNetwork.Destroy(this.gameObject);            
            StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        else if(!collision.gameObject.CompareTag("PC_Player"))
        {
            isHit = true;

            GameObject o_ps = 
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            // 家府何盒 //
            as_arrow.Stop();
            as_arrow.PlayOneShot(ac_shotHit);
            // 家府何盒 // 

            //PhotonNetwork.Destroy(this.gameObject);
            StartCoroutine(DestroyDelayed(gameObject, 0.1f));
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
