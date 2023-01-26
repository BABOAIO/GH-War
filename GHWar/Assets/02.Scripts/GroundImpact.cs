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
    [SerializeField] XRController controller;
    [Header("¶¥À» Ä¥¶§ ³ª¿À´Â ÀÌÆåÆ®")]
    [SerializeField] ParticleSystem ps_hitGround;
    [Header("¶¥À» Ä¥¶§ ³ª¿À´Â Å¥ºê")]
    [SerializeField] GameObject o_stone;
    [Header("¶¥À» Ä¡´Â ÀÌÆåÆ® µô·¹ÀÌ")]
    [SerializeField] float delayTime = 1.0f;
    float currentTime = 0;

    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();

        currentTime = delayTime;
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Col");
        if (collision.gameObject.CompareTag("Ground"))
        {
            print("Tag");
            if ((currentTime >= delayTime) &&
                (controller.inputDevice.TryGetFeatureValue(CommonUsages.gripButton,out bool isGrab)))
            {
                if (isGrab)
                {
                    print("Btn");
                    pv.RPC("Hit_Ground_withEffect", RpcTarget.All, collision.contacts[0].point, Quaternion.Euler(collision.contacts[0].normal));
                    currentTime = 0;
                }
            }
        }
    }

    [PunRPC]
    public void Hit_Ground_withEffect(Vector3 position, Quaternion rotation)
    {
        GameObject o_ps = PhotonNetwork.Instantiate("HitEffect",position,rotation);
        GameObject o_tmp = PhotonNetwork.Instantiate("Rock", position, rotation);

        StartCoroutine(Delayed_Destroy(0.5f, o_ps));
    }

    IEnumerator Delayed_Destroy(float t, GameObject obj)
    {
        yield return new WaitForSeconds(t);
        if(obj != null)
        {
            PhotonNetwork.Destroy(obj);
        }
    }
}
