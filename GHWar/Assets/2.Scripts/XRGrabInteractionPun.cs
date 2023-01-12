using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

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
