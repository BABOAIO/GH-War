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

    // �Ҹ��κ� //
    AudioSource as_rock;
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
        //if(velocityValue < 10) { return; }

        //// �Ҹ��κ� //
        //if (!as_rock.isPlaying)
        //{
        //    as_rock.PlayOneShot(ac_throwing);
        //}
        //// �Ҹ��κ� //
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

                // ������ �ķ� ������ �� �μ����� �ϴ� �κ�
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

                // ���ſ� ��ü(���̳� ��)�� �ε����� ������
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
