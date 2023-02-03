using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArrowLineRendering : MonoBehaviourPun
{
    LineRenderer lr;
    Transform start;
    [SerializeField] Transform end;

    private void OnEnable()
    {
        if (!photonView.IsMine) { GetComponent<LineRenderer>().enabled = false; }
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        start = GetComponent<Transform>();

        lr.startWidth = 0.3f;
        lr.endWidth = 0.3f;
    }

    void Update()
    {
        lr.SetPosition(0, start.position);
        lr.SetPosition(1, end.position);
    }

       
}
