using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

// VR 손으로 잡고자하는 모든 오브젝트에 넣는다.
// 이 오브젝트는 포톤뷰가 있어야하며, 포톤뷰를 Take Over(권한 넘기기 가능)으로 바꿔주어여한다.
// XRInteractionManager는 넣지 않는다. 넣으면 넣은 오브젝트끼리만 상호작용함(손에도 들어있기 때문)
public class XRGrabInteractionPun : XRGrabInteractable // 이 항목이 있기에 인스펙터창에 그랩관련 인스펙터창이 생성
{
    [SerializeField] AudioClip ac_grab;

    Player player_this;
    PhotonView pv;

    public bool isGrab = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        player_this = pv.Controller;
    }

    // 집었을 경우 소유권 요청
    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        pv.RPC("IsGrabReverse", RpcTarget.All);
        base.OnSelectEntered(interactor);
    }

    [System.Obsolete]
    // 쥐고 있는 중에도 소유권 요청
    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        base.OnSelectEntering(interactor);
    }

    [System.Obsolete]
    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        pv.TransferOwnership(player_this);
        pv.RPC("IsGrabReverse", RpcTarget.All);
        base.OnSelectExited(interactor);
    }

    // 그랩이 되있을 경우, 컬라이더를 없애주면서 화면에서 떨리는 모습을 막아준다.
    [PunRPC]
    void IsGrabReverse()
    {
        isGrab = !isGrab;
    }

    [PunRPC]
    void GrabSound()
    {
        if(gameObject.GetComponent<AudioSource>() != null)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(ac_grab);
        }
    }

    IEnumerator DelayedTransferOwnership()
    {
        yield return new WaitForSeconds(0.5f);
        pv.TransferOwnership(player_this);
    }
}
