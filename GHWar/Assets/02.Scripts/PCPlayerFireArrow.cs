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
    PC_Player_Move _Move;

    public bool isDie;

    [SerializeField] float delayTime = 2.0f;
    float currentTime = 0;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;

    // �Ҹ� �κ� //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    // �Ҹ� �κ� //

    public bool B_isReadyToShot = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        tr_this = GetComponent<Transform>();

        // �Ҹ� �κ� //
        as_fireArrow = GetComponent<AudioSource>();
        // �Ҹ� �κ� //

        isDie = false;
        _Move = GetComponent<PC_Player_Move>();
        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

        currentTime = delayTime;
        B_isReadyToShot = false;
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

    private RaycastHit hit;

    void FixedUpdate()
    {
        //if (photonView.IsMine)
        //{

        //    //Debug.Log(pv.Synchronization);

        //    //���� ī�޶� ���콺 Ŀ���� ��ġ�� ĳ���õǴ� ���̸� ����
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    //������ Ray�� Scene �信 ��� �������� ǥ��
        //    Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);
        //    //8��° ���̾�(TERRAIN)�� ���̰� �ε����ٸ�
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //    {
        //        //Ray�� ���� ��ġ�� ������ǥ�� ��ȯ
        //        Vector3 relative = tr_this.InverseTransformPoint(hit.point);
        //        //��ź��Ʈ �Լ��� �� �� �� ������ �Ի�
        //        float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
        //    }
        //}


        currentTime += Time.fixedDeltaTime;

        if (isDie == false)
        {
            if ((pv.IsMine) && (Input.GetMouseButtonDown(0))
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
            if ((pv.IsMine) && (Input.GetMouseButtonUp(0))
                && B_isReadyToShot
                )
            {
                // �Ҹ� �κ� //
                as_fireArrow.PlayOneShot(ac_shotInit);
                // �Ҹ� �κ� //

                currentTime = 0;

                a_playerInFire.SetBool("ReadyToShot", false);
                a_playerInFire.SetBool("RunToAimWalk", false);
                a_playerInFire.SetBool("Shot", true);
                Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
                Invoke("DelayedActive", 0.8f);

                //Vector2 v2_tmp = (firePosEnd.position - firePos.position);
                GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
                //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
                //else
                //{
                //    a_playerInFire.SetBool("ToIdle", true);
                //    Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ToIdle", false));
                //}
            }
            else
            {
                // �ʱ�ȭ
            }
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

    [PunRPC]
    public void InitShootingArrow()
    {
        
    }
}
