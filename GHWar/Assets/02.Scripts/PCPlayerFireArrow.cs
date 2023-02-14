using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;
using System.ComponentModel;
using SimpleMan.VisualRaycast;
using UnityEditor;

// �÷��̾� �ֻ�ܿ� �ִ´�.
[RequireComponent(typeof(AudioSource))]
public class PCPlayerFireArrow : MonoBehaviourPunCallbacks
{
    PhotonView pv;
 
    Animator a_playerInFire;
    PC_Player_Move _Move;

    [Header("�÷��̾� ���� ǥ��")]
    public bool isDie;

    [Header("Ȱ ��� �ֱ�")]
    [SerializeField] float delayTime = 2.0f;
    float currentTime = 0f;

    [SerializeField] float shotPow = 1f;
    float shotPowerOrigin;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;

    // �Ҹ� �κ� //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    // �Ҹ� �κ� //

    [Header("Ȱ�� �غ� ��")]
    public bool B_isReadyToShot = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        tr_this = GetComponent<Transform>();

        // �Ҹ� �κ� //
        as_fireArrow = firePos.GetComponent<AudioSource>();
        // �Ҹ� �κ� //

        isDie = false;
        _Move = GetComponent<PC_Player_Move>();
        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

        shotPowerOrigin = shotPow;

        currentTime = delayTime;
        B_isReadyToShot = false;
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if (pv.IsMine)
        {
            if (isDie == false)
            {
                // ������ ������ ��쿡�� �۵�
                pv.RPC("Shot", RpcTarget.All);

                // ���� �������� ���� ��� Ȯ�ο�
                //if (PhotonNetwork.CountOfPlayers >= 2)
                //{
                //    pv.RPC("Shot", RpcTarget.All);
                //}
                //else
                //{
                //    Shot();
                //}
            }
        }
    }

    [PunRPC]

    private void Shot()
    {
        if (Input.GetMouseButtonDown(0)
            && (currentTime >= delayTime)
            && !_Move.isJump
            )
        {
            shotPow = 1;
            StartCoroutine(ShotPowerUp());
            a_playerInFire.SetBool("ReadyToShot", true);
            a_playerInFire.SetBool("RunToAimWalk", true);
            B_isReadyToShot = true;
            //Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ReadyToShot", false));
            //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
        }
        if ((Input.GetMouseButtonUp(0)
            && B_isReadyToShot
            && currentTime >= delayTime)
            || shotPow >= 5
            )
        {
            //StopCoroutine(ShotPowerUp());
            StopAllCoroutines();
            print(shotPow);
            a_playerInFire.SetBool("ReadyToShot", false);
            a_playerInFire.SetBool("RunToAimWalk", false);

            // �Ҹ� �κ� //
            as_fireArrow.PlayOneShot(ac_shotInit, 0.5f);
            // �Ҹ� �κ� //

            currentTime = 0;

            a_playerInFire.SetBool("Shot", true);
            Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
            Invoke("DelayedActive", 0.4f);

            //Vector2 v2_tmp = (firePosEnd.position - firePos.position);
            GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
            obj_tmp.GetComponent<Rigidbody>().AddForce(shotPow * (firePosEnd.position - firePos.position), ForceMode.Impulse);

            shotPow = 1;
            //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
            //else
            //{
            //    a_playerInFire.SetBool("ToIdle", true);
            //    Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ToIdle", false));
            //}
        }
        if(!B_isReadyToShot)
        {
            a_playerInFire.SetBool("ReadyToShot", false);
            a_playerInFire.SetBool("RunToAimWalk", false);
            a_playerInFire.SetBool("Shot", false);
        }
    }

    IEnumerator ShotPowerUp()
    {
        yield return new WaitForSeconds(1.0f);
        shotPow = 2;
        yield return new WaitForSeconds(1.0f);
        shotPow = 3;
        yield return new WaitForSeconds(1.0f);
        shotPow = 4;
        yield return new WaitForSeconds(1.0f);
        shotPow = 5;
    }

    void DelayedActive()
    {
        B_isReadyToShot = false;
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    a_playerInFire.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
    //    //a_playerInFire.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
    //    a_playerInFire.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
    //    //a_playerInFire.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

    //    a_playerInFire.SetIKPosition(AvatarIKGoal.LeftHand, firePos.position);
    //    //a_playerInFire.SetIKPosition(AvatarIKGoal.RightHand, firePosEnd.position);
    //    a_playerInFire.SetIKRotation(AvatarIKGoal.LeftHand, firePos.rotation);
    //    //a_playerInFire.SetIKRotation(AvatarIKGoal.RightHand, firePosEnd.rotation);

    //}
}
