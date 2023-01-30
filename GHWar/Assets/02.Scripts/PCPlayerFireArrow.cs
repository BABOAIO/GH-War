using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;
using System.ComponentModel;
using SimpleMan.VisualRaycast;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class PCPlayerFireArrow : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    
    Animator a_playerInFire;

    public bool isDie;

    [SerializeField] float delayTime = 3.0f;
    float currentTime = 0;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;

    // �Ҹ� �κ� //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    // �Ҹ� �κ� //

    void Start()
    {
        pv = GetComponent<PhotonView>();
        tr_this = GetComponent<Transform>();

        // �Ҹ� �κ� //
        as_fireArrow = GetComponent<AudioSource>();
        // �Ҹ� �κ� //

        isDie = false;
        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

        currentTime = delayTime;
    }

    //void FixedUpdate()
    //{
    //    currentTime += Time.fixedDeltaTime;

    //    if ((pv.IsMine) && (Input.GetMouseButtonDown(0))
    //        //&& (currentTime >= delayTime)
    //        )
    //    {
    //        a_playerInFire.SetBool("ReadyToShot", true);
    //        Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ReadyToShot", false));
    //        //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
    //    }
    //    if ((pv.IsMine) && (Input.GetMouseButtonUp(0))
    //        && (currentTime >= delayTime)
    //        )
    //    {
    //        // �Ҹ� �κ� //
    //        as_fireArrow.PlayOneShot(ac_shotInit);
    //        // �Ҹ� �κ� //

    //        currentTime = 0;
    //        a_playerInFire.SetBool("ReadyToShot", false);
    //        a_playerInFire.SetBool("Shot", true);
    //        Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
    //        GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePosEnd.rotation);
    //        //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
    //    }
    //}
    void FixedUpdate()
    {
        //RaycastHit hitinfo;
        Debug.DrawRay(firePos.transform.position, (firePosEnd.transform.position - firePos.transform.position) * 10f, Color.red);
        //if(Physics.Raycast(firePos.transform.position, (firePosEnd.transform.position - firePos.transform.position),out hitinfo, Mathf.Infinity))
        //{
        //    if(hitinfo.collider.CompareTag("VR_Player"))
        //    {
        //        Debug.DrawRay(firePos.transform.position, firePosEnd.transform.position, Color.red);
        //    }
        //    else
        //    {
        //        Debug.DrawRay(firePos.transform.position, firePosEnd.transform.position, Color.green);
        //    }
        //}

        currentTime += Time.fixedDeltaTime;

        if (isDie == false)
        {
            if ((pv.IsMine) && (Input.GetMouseButtonDown(0))
                && (currentTime >= delayTime - Time.fixedDeltaTime * 3)
                )
            {
                a_playerInFire.SetBool("ReadyToShot", true);
                //Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ReadyToShot", false));
                //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
            }
            if ((pv.IsMine) && (Input.GetMouseButtonUp(0))
                )
            {
                if (currentTime >= delayTime)
                {
                    // �Ҹ� �κ� //
                    as_fireArrow.PlayOneShot(ac_shotInit);
                    // �Ҹ� �κ� //

                    currentTime = 0;
                    a_playerInFire.SetBool("ReadyToShot", false);
                    a_playerInFire.SetBool("Shot", true);
                    Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));

                    Vector2 v2_tmp = (firePosEnd.position - firePos.position);
                    GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
                    //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
                }
                else
                {
                    a_playerInFire.SetBool("ToIdle", true);
                    Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ToIdle", false));
                }
            }


        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        a_playerInFire.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        //a_playerInFire.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        a_playerInFire.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        //a_playerInFire.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        a_playerInFire.SetIKPosition(AvatarIKGoal.LeftHand, firePos.position);
        //a_playerInFire.SetIKPosition(AvatarIKGoal.RightHand, firePosEnd.position);
        a_playerInFire.SetIKRotation(AvatarIKGoal.LeftHand, firePos.rotation);
        //a_playerInFire.SetIKRotation(AvatarIKGoal.RightHand, firePosEnd.rotation);

    }

    [PunRPC]
    public void InitShootingArrow()
    {
        
    }
}
