using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// PC 플레이어의 활 안에 있는 FirePos에 넣는다.
public class ArrowLineRendering : MonoBehaviourPun
{
    LineRenderer lr;
    Transform start;
    [SerializeField] Transform end;

    // 이 렌더러는 PC 플레이어 중 자신만 볼 수 있다.
    private void OnEnable()
    {
        if (!photonView.IsMine) { GetComponent<LineRenderer>().enabled = false; }
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        start = GetComponent<Transform>();

        // 라인 렌더러의 굵기(1인 경우 최대)
        lr.startWidth = 0.3f;
        lr.endWidth = 0.3f;
    }

    void Update()
    {
        lr.SetPosition(0, start.position);
        lr.SetPosition(1, end.position);
    }

       
}
