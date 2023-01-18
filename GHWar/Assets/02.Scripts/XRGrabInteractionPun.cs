using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractionPun : XRGrabInteractable//, IPunOwnershipCallbacks//, IPunObservable // �� �׸��� �ֱ⿡ �ν�����â�� �׷����� �׸��� ����
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

    // ������ ��� ������ ��û
    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        //sc_PCPlayerMove.st_PC = PC_Player_Move.PC_Player_State.IsGrab;
        base.OnSelectEntered(interactor);
    }

    [System.Obsolete]
    // ��� �ִ� �߿��� ������ ��û
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

    // ����Ʈ���� �� ������ �� ��� IsMine�� ��Ʈ�ѷ����� �����ٷ��� ������ ����(���Ĵ� TransferInteractionMaster ��ũ��Ʈ ����)
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
