using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class PCPlayerFireArrow : MonoBehaviourPunCallbacks
{
    PhotonView pv;
    
    Animator a_playerInFire;

    [SerializeField] float delayTime = 3.0f;
    float currentTime = 0;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;
    [SerializeField] Transform tr_lookSide;

    // 家府 何盒 //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    // 家府 何盒 //

    void Start()
    {
        pv = GetComponent<PhotonView>();
        tr_this = GetComponent<Transform>();

        // 家府 何盒 //
        as_fireArrow = GetComponent<AudioSource>();
        // 家府 何盒 //

        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

        currentTime = delayTime;
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if ((pv.IsMine) && (Input.GetMouseButtonDown(0))
            //&& (currentTime >= delayTime)
            )
        {
            a_playerInFire.SetBool("ReadyToShot", true);
            Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("ReadyToShot", false));
            //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
        }
        if ((pv.IsMine) && (Input.GetMouseButtonUp(0))
            && (currentTime >= delayTime)
            )
        {
            // 家府 何盒 //
            as_fireArrow.PlayOneShot(ac_shotInit);
            // 家府 何盒 //

            currentTime = 0;
            a_playerInFire.SetBool("ReadyToShot", false);
            a_playerInFire.SetBool("Shot", true);
            Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
            GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePosEnd.rotation);
            //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
        }
    }

    [PunRPC]
    public void InitShootingArrow()
    {
        
    }
}
