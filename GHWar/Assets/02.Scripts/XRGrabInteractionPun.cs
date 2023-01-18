using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractionPun : XRGrabInteractable//, IPunOwnershipCallbacks//, IPunObservable // 이 항목이 있기에 인스펙터창에 그랩관련 항목이 생성
{
    Player player_origin;
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        player_origin = pv.Controller;
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

    // 쥐고 있는 중에도 소유권 요청
    [System.Obsolete]
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        //sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntering(interactor);
    }

    // 쥐고 있는 플레이어를 놓았을 경우, 그 플레이어의 소유권을 돌려줌
    [System.Obsolete]
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        pv.TransferOwnership(player_origin);
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
