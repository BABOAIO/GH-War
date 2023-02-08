using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

// VR�÷��̾��� ��忡 �ִ´�. ���� �ö��̴��� ������, �±׸� VRPlayerHead�� �ٲ۴�.
public class VRPlayerHit : MonoBehaviourPunCallbacks
{
    [Header("������ ī�޶�")]
    [SerializeField] Camera _camera;

    [Header("Max HP")]
    public float MaxHP = 5.0f;
    [Header("HP")]
    public float HP = 5.0f;

    [Header("HP �����̴�")]
    [SerializeField] Slider hpBar;
    [SerializeField] Slider hpBar2;

    [SerializeField] XRBaseController[] xRBaseControllers = new XRBaseController[2];

    float invincibilityTime = 2.0f;
    public float currentTime = 2.0f;

    [SerializeField] float f_hapticTime = 0.5f;
    [SerializeField] float f_hapticStrength = 0.8f;

    private void Start()
    {
        if (!gameObject.CompareTag("VRPlayerHead"))
        {
            gameObject.tag = "VRPlayerHead";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (currentTime >= invincibilityTime
                && (collision.gameObject.CompareTag("Arrow") || collision.gameObject.CompareTag("CannonBall"))
                ) 
            {
                print("VR ��Ʈ");

                xRBaseControllers[0].SendHapticImpulse(0.5f, 0.3f);
                xRBaseControllers[1].SendHapticImpulse(0.5f, 0.3f);

                photonView.RPC("Hit_VRPlayer", RpcTarget.AllBuffered, 1);
                currentTime = 0.0f;
            }
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
        _camera.DOShakePosition(f_hapticTime, f_hapticStrength);
        HP -= damage;
        Debug.Log($"VR Player {photonView.Controller} is Damaged : Dmg {damage}");

        if (HP <= 0)
        {
            print("VR ����");
            //gameObject.GetComponent<VRPlayerMove1>().enabled= false;
        }
    }
}
