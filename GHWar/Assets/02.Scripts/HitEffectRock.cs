using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class HitEffectRock : MonoBehaviourPun
{
    PhotonView pv;

    Rigidbody rb_this;

    // 소리부분 //
    AudioSource as_rock;
    [SerializeField] AudioClip ac_throwHit;
    // 소리부분 //


    void Start()
    {
        pv= GetComponent<PhotonView>();
        rb_this= GetComponent<Rigidbody>();
        // 소리부분 //
        as_rock = GetComponent<AudioSource>();
        // 소리부분 //
    }

    void Update()
    {
        //if(velocityValue < 10) { return; }

        //// 소리부분 //
        //if (!as_rock.isPlaying)
        //{
        //    as_rock.PlayOneShot(ac_throwing);
        //}
        //// 소리부분 //
    }

    private void OnCollisionStay(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.CompareTag("PC_Player"))
            {
                as_rock.PlayOneShot(ac_throwHit);
                PhotonNetwork.Destroy(this.gameObject);
            }

            if (!collision.gameObject.CompareTag("PC_Player"))
            {

                // 손으로 후려 쳤을때 돌 부서지게 하는 부분
                if (collision.gameObject.layer == LayerMask.NameToLayer("LeftHandPhysics") || collision.gameObject.layer == LayerMask.NameToLayer("RightHandPhysics"))
                {
                    as_rock.PlayOneShot(ac_throwHit);
                    if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 100f)
                    {
                        GetComponent<MeshCollider>().enabled = false;
                        PhotonNetwork.Destroy(this.gameObject);

                        for (int i = 0; i < 5; i++)
                        {
                            GameObject tmp = PhotonNetwork.Instantiate("SmallRock", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));
                        }
                    }
                }

                // 무거운 물체(땅이나 돌)에 부딪히면 뭉게짐
                else if (collision.gameObject.GetComponent<Rigidbody>().mass >= GetComponent<Rigidbody>().mass)
                {
                    as_rock.PlayOneShot(ac_throwHit);
                    if (GetComponent<Rigidbody>().velocity.magnitude > 10f || collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 20f)
                    {
                        GameObject o_effect = PhotonNetwork.Instantiate("HitEffect2", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));
                        GetComponent<MeshCollider>().enabled = false;
                        PhotonNetwork.Destroy(this.gameObject);

                        for (int i = 0; i < 5; i++)
                        {
                            GameObject tmp = PhotonNetwork.Instantiate("SmallRock", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));
                        }
                    }
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
