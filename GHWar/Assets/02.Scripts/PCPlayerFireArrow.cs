using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PCPlayerFireArrow : MonoBehaviour
{
    PhotonView pv;

    [SerializeField] float delayTime = 3.0f;
    float currentTime = 0;

    [SerializeField] Transform firePos;
    [SerializeField] GameObject arrow;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        currentTime = delayTime;
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        if ((pv.IsMine) && (Input.GetMouseButtonUp(0)) && (currentTime >= delayTime))
        {
            currentTime = 0;
            PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
        }
    }
}
