using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resource �� �ִ� �����˿� �ִ´�.
public class CannonBall : MonoBehaviourPun
{
    //bool isHit = false;

    private void Start()
    {
        //rb_this.AddForce(new Vector3(0, 0, shotPower));
        //photonView.TransferOwnership(PhotonNetwork.MasterClient);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // VR�÷��̾ �ǰݵ� ��
        if (collision.gameObject.CompareTag("VRPlayerHead"))
        {
            //isHit = true;
            collision.gameObject.GetComponent<VRPlayerHit>().HitVRPlayer_PhotonView(2);

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));

            PhotonNetwork.Destroy(this.gameObject);
            //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
        }

        // �������� ������ �ε�������, �ּ�Ǯ�� �÷��̾ �ڻ� �Ұ���
        if (
            //!collision.gameObject.CompareTag("PC_Player") && 
            !(collision.gameObject.layer == LayerMask.NameToLayer("Turret")))
        {
            //isHit = true;

            GameObject o_ps =
            PhotonNetwork.Instantiate("HitEffectWithEXP", collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].point, collision.contacts[0].normal));

            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Rock"))
            {
                // �Ҹ��κ� //
                //as_arrow.Stop();
                //as_arrow.PlayOneShot(ac_shotHit);
                // �Ҹ��κ� //

                PhotonNetwork.Destroy(collision.gameObject);
                //StartCoroutine(DestroyDelayed(gameObject, 0.1f));
            }

            // �Ҹ��κ� //
            //as_arrow.Stop();
            //as_arrow.PlayOneShot(ac_shotHit);
            // �Ҹ��κ� // 

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
