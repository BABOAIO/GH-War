using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractionPun : XRGrabInteractable//, IPunOwnershipCallbacks//, IPunObservable // 이 항목이 있기에 인스펙터창에 그랩관련 항목이 생성
{
    Player player_this;
    //Transform t_this;
    //Vector3 v3_grabbed;
    PhotonView pv;

    //PC_Player_Move sc_PCPlayerMove = new PC_Player_Move();

    void Start()
    {
        //t_this = GetComponent<Transform>();
        //player_this = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
        player_this = pv.Controller;
    }

    void Update()
    {

    }

    // 집었을 경우 소유권 요청
    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        //sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntered(interactor);
    }

    [System.Obsolete]
    // 쥐고 있는 중에도 소유권 요청
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        //sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntering(interactor);
    }

    [System.Obsolete]
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        pv.TransferOwnership(player_this);
        base.OnSelectExited(interactor);
    }

    // 셀렉트에서 손 놓았을 때 당시 IsMine인 컨트롤러에게 돌려줄려고 했지만 실패(이후는 TransferInteractionMaster 스크립트 참고)
    //protected override void OnSelectExiting(XRBaseInteractor interactor)
    //{
    //    pv.TransferOwnership(player_this);
    //    base.OnSelectExiting(interactor);
    //}

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsReading)
    //    {
    //        v3_grabbed = (Vector3)stream.ReceiveNext();
    //    }
    //    else if(stream.IsWriting)
    //    {
    //        stream.SendNext(t_this.position);
    //    }
    //}

}
