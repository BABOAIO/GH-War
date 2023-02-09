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

    // �Ҹ��κ� //
    AudioSource as_rock;
    [SerializeField] AudioClip ac_throwing;
    [SerializeField] AudioClip ac_throwHit;
    // �Ҹ��κ� //


    void Start()
    {
        pv= GetComponent<PhotonView>();
        rb_this= GetComponent<Rigidbody>();
        // �Ҹ��κ� //
        as_rock = GetComponent<AudioSource>();
        // �Ҹ��κ� //
    }

    void Update()
    {
        var velocityValue = rb_this.velocity.magnitude;

        //if(velocityValue < 10) { return; }

        //// �Ҹ��κ� //
        //if (!as_rock.isPlaying)
        //{
        //    as_rock.PlayOneShot(ac_throwing);
        //}
        //// �Ҹ��κ� //
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PC_Player"))
        {
            GameObject o_effect = PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            //as_rock.Stop();
            //as_rock.PlayOneShot(ac_throwHit);

            PhotonNetwork.Destroy(this.gameObject);
            //PhotonNetwork.Destroy(this.gameObject);
        }

        if (!collision.gameObject.CompareTag("PC_Player")
            && !(collision.gameObject.layer == LayerMask.NameToLayer("LeftHandPhysics") || collision.gameObject.layer == LayerMask.NameToLayer("RightHandPhysics")))
        {
            GameObject o_effect = PhotonNetwork.Instantiate("HitEffect", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            //as_rock.Stop();
            //as_rock.PlayOneShot(ac_throwHit);

            if (collision.gameObject.GetComponent<Rigidbody>().mass >= GetComponent<Rigidbody>().mass
                && GetComponent<Rigidbody>().velocity.magnitude > 20f)
            {
                print(collision.gameObject.layer);
                GetComponent<MeshCollider>().enabled = false;
                PhotonNetwork.Destroy(this.gameObject);

                for(int i = 0; i < 5; i++)
                {
                    GameObject tmp = PhotonNetwork.Instantiate("SmallRock", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));
                    tmp.transform.localScale /= 3;
                }
            }
        }
    }

    IEnumerator DestroyuDelayed(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            PhotonNetwork.Destroy(obj);
        }
    }
}
