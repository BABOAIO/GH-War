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

// VR�÷��̾��� ��忡 �ִ´�. ���� �ö��̴��� ������, �±׸� VRPlayerHead�� �ٲ۴�.
// ������� �޴� ������Ʈ�� enter�� ����. ������� �ִ� ������Ʈ�� stay�� ����.(exit�� ���, contacts�� Ȱ���� �� ���� ������ �ִ�.)
public class VRPlayerHit : MonoBehaviourPunCallbacks
{
    AudioSource as_VRPlayerHit;
    public AudioSource as_parent;

    [Header("Max HP")]
    public float MaxHP = 5.0f;
    [Header("HP")]
    public float HP = 5.0f;

    [Header("HP �����̴�")]
    [SerializeField] Slider hpBar;
    [SerializeField] Slider hpBar2;

    [SerializeField] XRBaseController[] xRBaseControllers = new XRBaseController[2];

    [SerializeField]
    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    // �ǰ� �� �����ϴ� �κ�, �ص� VR������ ������ �Ⱥ���
    //[Header("������ ī�޶�")]
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

    //isTrigger�� �̿��� ���̱� ������ ���� �ʴ´�.
    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (currentTime >= invincibilityTime
                && (collision.gameObject.layer == LayerMask.NameToLayer("Arrow"))
            //&& (other.gameObject.CompareTag("Arrow") || other.gameObject.CompareTag("CannonBall"))
            )
            {
                print("VR ��Ʈ");

                // �ε����� ����, ȸ�翡 ���صǼ� �ּ�
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
                print("VR ��Ʈ");

                // �ε����� ����, ȸ�翡 ���صǼ� �ּ�
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

        // �ڱ� �� ������ ���� �Ⱥ��̹Ƿ� �÷��̾��� ���, �����´�.(�ö��̴��� ��������Ƿ� �ǰ��� �ȴ�.)
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
            print("VR ����");
            //gameObject.GetComponent<VRPlayerMove1>().enabled= false;
        }
    }
}
