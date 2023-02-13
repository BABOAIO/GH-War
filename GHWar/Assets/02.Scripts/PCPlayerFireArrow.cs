using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;
using System.ComponentModel;
using SimpleMan.VisualRaycast;
using UnityEditor;

// 플레이어 최상단에 넣는다.
[RequireComponent(typeof(AudioSource))]
public class PCPlayerFireArrow : MonoBehaviourPunCallbacks
{
    PhotonView pv;
 
    Animator a_playerInFire;
    PC_Player_Move _Move;

    [Header("플레이어 죽음 표시")]
    public bool isDie;

    [Header("활 쏘는 주기")]
    [SerializeField] float delayTime = 2.0f;
    float currentTime = 0;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;

    // 소리 부분 //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    // 소리 부분 //

    [Header("활쏠 준비 완")]
    public bool B_isReadyToShot = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        tr_this = GetComponent<Transform>();

        // 소리 부분 //
        as_fireArrow = firePos.GetComponent<AudioSource>();
        // 소리 부분 //

        isDie = false;
        _Move = GetComponent<PC_Player_Move>();
        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

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
                if (PhotonNetwork.CountOfPlayers >= 2)
                {
                    pv.RPC("Shot", RpcTarget.All);
                }
                else
                {
                    Shot();
                }
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
            a_playerInFire.SetBool("ReadyToShot", true);
            a_playerInFire.SetBool("RunToAimWalk", true);
            B_isReadyToShot = true;
            //Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ReadyToShot", false));
            //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
        }
        if (Input.GetMouseButtonUp(0)
            && B_isReadyToShot
            && currentTime != 0
            )
        {
            a_playerInFire.SetBool("ReadyToShot", false);
            a_playerInFire.SetBool("RunToAimWalk", false);

            // 소리 부분 //
            as_fireArrow.PlayOneShot(ac_shotInit, 0.5f);
            // 소리 부분 //

            currentTime = 0;

            a_playerInFire.SetBool("Shot", true);
            Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
            Invoke("DelayedActive", 0.4f);

            //Vector2 v2_tmp = (firePosEnd.position - firePos.position);
            GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
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
