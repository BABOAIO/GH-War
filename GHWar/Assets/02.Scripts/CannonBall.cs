using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resource 에 있는 대포알에 넣는다.
// 대미지를 받는 오브젝트는 enter를 쓴다. 대미지를 주는 오브젝트는 stay를 쓴다.(exit의 경우, contacts를 활용할 수 없는 단점이 있다.)
public class CannonBall : MonoBehaviourPun
{
    //bool isHit = false;

    private void Start()
    {
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
        //photonView.TransferOwnership(PhotonNetwork.MasterClient);
    }

    private void OnCollisionStay(Collision collision)
    {
        // VR플레이어가 피격될 시
        if (collision.gameObject.CompareTag("VRPlayerHead"))
        {
            //isHit = true;
            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        // 대포알이 대포에 부딪힘방지
        if (!(collision.gameObject.layer == LayerMask.NameToLayer("Turret")))
        {
            //isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Rock"))
            {
                // 소리부분 //
                //as_arrow.Stop();
                //as_arrow.PlayOneShot(ac_shotHit);
                // 소리부분 //

                if (photonView.IsMine)
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
                //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
            }

            // 소리부분 //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // 소리부분 // 

            print(collision.gameObject.name);
            //PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f))
            
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
