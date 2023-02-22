using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

// VR ������ ������ϴ� ��� ������Ʈ�� �ִ´�.
// �� ������Ʈ�� ����䰡 �־���ϸ�, ����並 Take Over(���� �ѱ�� ����)���� �ٲ��־�Ѵ�.
// XRInteractionManager�� ���� �ʴ´�. ������ ���� ������Ʈ������ ��ȣ�ۿ���(�տ��� ����ֱ� ����)
public class XRGrabInteractionPun : XRGrabInteractable // �� �׸��� �ֱ⿡ �ν�����â�� �׷����� �ν�����â�� ����
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

    // ������ ��� ������ ��û
    [System.Obsolete]
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        pv.RPC("IsGrabReverse", RpcTarget.All);
        base.OnSelectEntered(interactor);
    }

    [System.Obsolete]
    // ��� �ִ� �߿��� ������ ��û
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

    // �׷��� ������ ���, �ö��̴��� �����ָ鼭 ȭ�鿡�� ������ ����� �����ش�.
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
