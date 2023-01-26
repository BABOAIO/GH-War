using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class HitEffectRock : MonoBehaviour
{
    PhotonView pv;

    Rigidbody rb_this;

    // 家府何盒 //
    AudioSource as_rock;
    [SerializeField] AudioClip ac_throwing;
    [SerializeField] AudioClip ac_throwHit;
    // 家府何盒 //


    void Start()
    {
        pv= GetComponent<PhotonView>();
        rb_this= GetComponent<Rigidbody>();
        // 家府何盒 //
        as_rock = GetComponent<AudioSource>();
        // 家府何盒 //
    }

    void Update()
    {
        var velocityValue = rb_this.velocity.magnitude;

        if(velocityValue < 10) { return; }

        // 家府何盒 //
        if (!as_rock.isPlaying)
        {
            as_rock.PlayOneShot(ac_throwing);
        }
        // 家府何盒 //
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PC_Player"))
        {
            PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            as_rock.Stop();
            as_rock.PlayOneShot(ac_throwHit);

            Destroy(this.gameObject);
        }
    }
}
