using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonViewAutoCheck : MonoBehaviourPun
{
    PhotonTransformViewClassic photonTransform;
    PhotonRigidbodyView photonRigidbody;

    void Awake()
    {
        photonTransform = GetComponent<PhotonTransformViewClassic>();
        photonRigidbody = GetComponent<PhotonRigidbodyView>();

        photonTransform.m_PositionModel.InterpolateOption = PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues;
        photonTransform.m_PositionModel.ExtrapolateOption = PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues;
        photonRigidbody.m_SynchronizeVelocity = true;
        photonRigidbody.m_SynchronizeAngularVelocity = true;
    }
}
