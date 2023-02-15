using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UIElements;
using UniRx;

[RequireComponent(typeof(PhotonView))]
public class GroundImpact : MonoBehaviourPunCallbacks
{
    AudioSource as_underHand;
    [SerializeField] AudioClip ac_HitGround;

    [SerializeField] XRController controller;
    [Header("¶¥À» Ä¡´Â ÀÌÆåÆ® µô·¹ÀÌ")]
    [SerializeField] float delayTime = 1.0f;
    float currentTime = 0;

    PhotonView pv;

    private void Start()
    {
        as_underHand = GetComponent<AudioSource>();
        pv = GetComponent<PhotonView>();

        currentTime = delayTime;
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) return;
        if (collision.gameObject.CompareTag("Ground"))
        {
            if ((currentTime >= delayTime) &&
                (controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton,out bool isGrab)))
            {
                if (isGrab)
                {
                    pv.RPC("Hit_Ground_withEffect", RpcTarget.All, collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));
                    currentTime = 0;
                }
            }
        }
    }

    [PunRPC]
    public void Hit_Ground_withEffect(Vector3 position, Quaternion rotation)
    {
        GameObject o_ps = PhotonNetwork.Instantiate("Effect_Smoke", position, rotation);
        as_underHand.PlayOneShot(ac_HitGround);
        int tmp_randomValue = Random.Range(1, 4);
        Vector3 tmp_pos;

        if (pv.IsMine)
        {
            switch (tmp_randomValue)
            {
                case 1:
                    tmp_pos = position + new Vector3(Random.Range(2, 3), 1.5f, Random.Range(2, 3));
                    position = tmp_pos;
                    break;
                case 2:
                    tmp_pos = position + new Vector3(Random.Range(-3, -2), 1.5f, Random.Range(2, 3));
                    position = tmp_pos;
                    break;
                case 3:
                    tmp_pos = position + new Vector3(Random.Range(2, 3), 1.5f, Random.Range(-3, -2));
                    position = tmp_pos;
                    break;
                case 4:
                    tmp_pos = position + new Vector3(Random.Range(-3, -2), 1.5f, Random.Range(-3, -2));
                    position = tmp_pos;
                    break;
            }
            GameObject o_tmp = PhotonNetwork.Instantiate("Rock", position, rotation);

        }
        
        StartCoroutine(Delayed_Destroy(0.5f, o_ps));

    }

    IEnumerator Delayed_Destroy(float t, GameObject obj)
    {
        yield return new WaitForSeconds(t);
        if (obj != null)
        {
            PhotonNetwork.Destroy(obj);
        }
        else print("Non Destroy Target...");
    }
}
