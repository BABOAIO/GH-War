using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 플레이어 프리팹 내부 UI에 넣는다.
// 다른 사람은 보지 못하게 하려는 스크립트
public class NotPhotonviewisNull : MonoBehaviour
{
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (!pv.IsMine)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(this.gameObject.activeSelf) { return; }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
