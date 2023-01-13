using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

// 자연스런 그랩 무브먼트를 위한 스크립트
public class XRGrabInteractionPun : XRGrabInteractable
{
    PhotonView pv;
    void Start()
    {    
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        base.OnSelectEntered(interactor);
    }

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        pv.RequestOwnership();
        base.OnSelectEntering(interactor);
    }
}
