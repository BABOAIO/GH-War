using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ArrowProperty : MonoBehaviour
{
    [SerializeField] float shotSpeed = 20.0f;
    Rigidbody rb_this;
    Transform tr_this;

    void Awake()
    {
        //rb_this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        tr_this = GetComponent<Transform>();
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
    }

    void FixedUpdate()
    {
        tr_this.Translate(Vector3.forward * shotSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("VR_Player"))
        {
            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            Destroy(this.gameObject);
            Destroy(o_ps, 0.5f);
        }

        else
        {
            GameObject o_ps = 
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            Destroy(this.gameObject);
            Destroy(o_ps, 0.5f);
        }
    }
}
