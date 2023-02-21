using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using Oculus.Interaction;

// VR플레이어의 헤드에 넣는다. 헤드는 컬라이더를 가지고, 태그를 VRPlayerHead로 바꾼다.
// 대미지를 받는 오브젝트는 enter를 쓴다. 대미지를 주는 오브젝트는 stay를 쓴다.(exit의 경우, contacts를 활용할 수 없는 단점이 있다.)
public class VRPlayerHit : MonoBehaviourPunCallbacks
{
    AudioSource as_VRPlayerHit;
    public AudioSource as_parent;

    [Header("Max HP")]
    public float MaxHP = 5.0f;
    [Header("HP")]
    public float HP = 5.0f;

    [Header("HP 슬라이더")]
    [SerializeField] Slider hpBar;
    [SerializeField] Slider hpBar2;

    [SerializeField] XRBaseController[] xRBaseControllers = new XRBaseController[2];

    [SerializeField]
    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    // 피격 시 진동하는 부분, 해도 VR에서는 진동이 안보임
    //[Header("진동할 카메라")]
    //[SerializeField] Camera _camera;
    //[SerializeField] float f_hapticTime = 0.5f;
    //[SerializeField] float f_hapticStrength = 0.8f;

    private void Start()
    {
        as_VRPlayerHit = GetComponent<AudioSource>();
        if (!gameObject.CompareTag("VRPlayerHead"))
        {
            gameObject.tag = "VRPlayerHead";
        }
    }

    //isTrigger를 이용할 것이기 때문에 쓰지 않는다.
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (currentTime >= invincibilityTime
                && (collision.gameObject.layer == LayerMask.NameToLayer("Arrow"))
            //&& (other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("CannonBall"))
            )
            {
                print("VR 히트");

                // 부딪히면 진동, 회사에 방해되서 주석
                //xRBaseControllers[0].SendHapticImpulse(0.5f, 0.3f);
                //xRBaseControllers[1].SendHapticImpulse(0.5f, 0.3f);

                //photonView.RPC("DestroyPhotonObject", RpcTarget.All, other.gameObject);

                if ((collision.gameObject.GetComponent<Rigidbody>().mass >= 10))
                {
                    HitVRPlayer_PhotonView(2);
                    as_parent.Play();
                }
                else
                {
                    HitVRPlayer_PhotonView(1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (currentTime >= invincibilityTime
                && (other.gameObject.layer == LayerMask.NameToLayer("Arrow"))
            //&& (other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("CannonBall"))
            )
            {
                print("VR 히트");

                // 부딪히면 진동, 회사에 방해되서 주석
                //xRBaseControllers[0].SendHapticImpulse(0.5f, 0.3f);
                //xRBaseControllers[1].SendHapticImpulse(0.5f, 0.3f);

                //photonView.RPC("DestroyPhotonObject", RpcTarget.All, other.gameObject);

                if ((other.gameObject.GetComponent<Rigidbody>().mass >= 10))
                {
                    HitVRPlayer_PhotonView(2);
                    as_parent.Play();
                }
                else
                {
                    HitVRPlayer_PhotonView(1);
                }
            }
        }
    }


    public void HitVRPlayer_PhotonView(int damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("Hit_VRPlayer", RpcTarget.AllBuffered, damage);
        }
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        hpBar.value = HP / MaxHP;
        hpBar2.value = hpBar.value;

        // 자기 얼굴 때문에 앞이 안보이므로 플레이어의 경우, 꺼놓는다.(컬라이더는 살아있으므로 피격이 된다.)
        if (photonView.IsMine)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    [PunRPC]
    public void Hit_VRPlayer(int damage)
    {
        //_camera.DOShakePosition(f_hapticTime, f_hapticStrength);
        as_VRPlayerHit.Stop();
        as_VRPlayerHit.Play();
        HP -= damage;
        Debug.Log($"VR Player {photonView.Controller} is Damaged : Dmg {damage}");

        currentTime = 0.0f;

        if (HP <= 0)
        {
            print("VR 죽음");
            //gameObject.GetComponent<VRPlayerMove1>().enabled= false;
        }
    }
}
