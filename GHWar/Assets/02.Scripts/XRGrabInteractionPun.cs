using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractionPun : XRGrabInteractable // �� �׸��� �ֱ⿡ �ν�����â�� �׷����� �׸��� ����
{
    Player player_this;
    //Transform t_this;
    //Vector3 v3_grabbed;
    PhotonView pv;

    public bool isGrab = false;

    //PC_Player_Move sc_PCPlayerMove = new PC_Player_Move();

    void Start()
    {
        //t_this = GetComponent<Transform>();
        //player_this = GetComponent<Player>();
        pv = GetComponent<PhotonView>();
        player_this = pv.Controller;
    }

    // ������ ��� ������ ��û
    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        //pv.RPC("IsGrabReverse", RpcTarget.All);
        //isGrab = true;
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
        //StartCoroutine(DelayedTransferOwnership());
        pv.TransferOwnership(player_this);
        //pv.RPC("IsGrabReverse", RpcTarget.All);
        //isGrab = false;
        base.OnSelectExited(interactor);
    }

    [PunRPC]
    void IsGrabReverse()
    {
        isGrab = !isGrab;
    }

    IEnumerator DelayedTransferOwnership()
    {
        yield return new WaitForSeconds(0.5f);
        pv.TransferOwnership(player_this);
    }
}
