using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLineRendering : MonoBehaviour
{
    LineRenderer lr;
    Transform start;
    [SerializeField] Transform end;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        start = GetComponent<Transform>();

        lr.startWidth = 1.0f;
        lr.endWidth = 0.5f;
    }

    void Update()
    {
        //RaycastHit hitinfo;
        //Debug.DrawRay(start.position, (end.position - start.position) * 10f, Color.red);
        //if (Physics.Raycast(start.position, (end.position - start.position), out hitinfo, Mathf.Infinity))
        //{
        //    end = hitinfo.transform;
        //}
        lr.SetPosition(0, start.position);
        lr.SetPosition(1, end.position);
    }

       
}
