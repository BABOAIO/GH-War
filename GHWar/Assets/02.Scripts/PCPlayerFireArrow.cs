using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UniRx;
using Photon.Realtime;

public class PCPlayerFireArrow : MonoBehaviour
{
    PhotonView pv;
    
    Animator a_playerInFire;

    [SerializeField] float delayTime = 3.0f;
    float currentTime = 0;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

        currentTime = delayTime;
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        
        if ((pv.IsMine) && (Input.GetMouseButtonUp(0)) && (currentTime >= delayTime))
        {
            currentTime = 0;
            a_playerInFire.SetBool("Shot", true);
            Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
            GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePosEnd.rotation);
            //obj_tmp.transform.LookAt(firePosEnd.position - firePos.position);
        }
    }
}
