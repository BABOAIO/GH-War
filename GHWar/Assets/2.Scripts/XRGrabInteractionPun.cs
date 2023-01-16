using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractionPun : XRGrabInteractable//, IPunObservable // �� �׸��� �ֱ⿡ �ν�����â�� �׷����� �׸��� ����
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

    // ������ ��� ������ ��û
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntered(interactor);
    }

    // ��� �ִ� �߿��� ������ ��û
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
