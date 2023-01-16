using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractionPun : XRGrabInteractable//, IPunObservable // 이 항목이 있기에 인스펙터창에 그랩관련 항목이 생성
{
    Transform t_this;
    Vector3 v3_grabbed;
    PhotonView pv;

    PC_Player_Move sc_PCPlayerMove = new PC_Player_Move();

    void Start()
    {
        t_this = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        
    }

    // 집었을 경우 소유권 요청
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntered(interactor);
    }

    // 쥐고 있는 중에도 소유권 요청
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntering(interactor);
    }

    protected override void OnSelectCanceling(XRBaseInteractor interactor)
    {
        sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.Normal;
        base.OnSelectCanceling(interactor);
    }

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
