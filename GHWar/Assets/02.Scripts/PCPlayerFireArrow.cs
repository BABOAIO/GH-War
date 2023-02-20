using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;
using System.ComponentModel;
using SimpleMan.VisualRaycast;
using UnityEditor;
using Unity.VisualScripting;
using CartoonFX;
using JetBrains.Annotations;

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
    float currentTime = 0f;
    float PowerShotDelay = 2.0f;
    //[SerializeField] ParticleSystem ps_ReadyToPowerShot;
    [SerializeField] ParticleSystem[] ps_ReadyToPowerShot;
    bool b_ReadyToPowerShot;

    float shotPow = 0.5f;

    float shotPowInGame;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;

    // 소리 부분 //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    [SerializeField] AudioClip ac_shot;
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

        shotPowInGame = shotPow;

        currentTime = delayTime;
        B_isReadyToShot = false;

        b_ReadyToPowerShot = false;
        for(int i = 0; i < ps_ReadyToPowerShot.Length; i++)
        {
            ps_ReadyToPowerShot[i].Stop();
        }
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if (pv.IsMine)
        {
            if (!isDie)
            {
                // 서버에 접속할 경우에만 작동
                Shot();
                PowerShot();
                //pv.RPC("Shot", RpcTarget.AllBuffered);
                // 서버 접속하지 않을 경우 확인용
                //if (PhotonNetwork.CountOfPlayers >= 2)
                //{
                //    pv.RPC("Shot", RpcTarget.All);
                //}
                //else
                //{
                //    Shot();
                //}
            }
            // 이후 참가자들에게 안보이게 하기 위한 장치
            //PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    void PowerShot()
    {
        if (Input.GetKeyDown(KeyCode.Q)
            && (currentTime >= PowerShotDelay)
            && !_Move.isJump
            && !b_ReadyToPowerShot
            )
        {
            for (int i = 0; i < ps_ReadyToPowerShot.Length; i++)
            {
                ps_ReadyToPowerShot[i].Play();
            }
            b_ReadyToPowerShot = true;
        }
        if (ps_ReadyToPowerShot[0].isStopped
            && ps_ReadyToPowerShot[1].isStopped
            && ps_ReadyToPowerShot[2].isStopped
            && ps_ReadyToPowerShot[3].isStopped
            )
        {
            b_ReadyToPowerShot = false;
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
            shotPowInGame = shotPow;
            StartCoroutine(ShotPowerUp());
            a_playerInFire.SetBool("ReadyToShot", true);
            B_isReadyToShot = true;
        }
        // 당겨진 상태에서 마우스를 놓거나, 활을 최대로 당기면 발사
        if ((Input.GetMouseButtonUp(0)
            && B_isReadyToShot
            && currentTime >= delayTime)
            || shotPowInGame >= 10
            )
        {
            AnimOperator();
            if (b_ReadyToPowerShot)
            {
                for (int i = 0; i < ps_ReadyToPowerShot.Length; i++)
                {
                    ps_ReadyToPowerShot[i].Stop();
                }
                GameObject obj_tmp = PhotonNetwork.Instantiate("UltArrow", firePos.position, firePos.rotation);
                b_ReadyToPowerShot = false;
                PowerShotDelay = 0.0f;
            }
            else
            {
                GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
            }

            //obj_tmp.GetComponent<Rigidbody>().AddForce(shotPowInGame * (firePosEnd.position - firePos.position), ForceMode.Impulse);
        }
        if (!B_isReadyToShot)
        {
            StopAllCoroutines();
            shotPowInGame = shotPow;
        }
    }

    void AnimOperator()
    {
        //StopCoroutine(ShotPowerUp());
        StopAllCoroutines();
        a_playerInFire.SetBool("ReadyToShot", false);

        // 소리 부분 //
        as_fireArrow.PlayOneShot(ac_shot, 0.5f);
        // 소리 부분 //

        currentTime = 0;

        a_playerInFire.SetBool("Shot", true);
        Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
        Invoke("DelayedActive", 0.4f);

        shotPowInGame = shotPow;
    }

    IEnumerator ShotPowerUp()
    {
        as_fireArrow.PlayOneShot(ac_shotInit, 0.5f);
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 3;
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 5;
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 7;
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 10;
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
