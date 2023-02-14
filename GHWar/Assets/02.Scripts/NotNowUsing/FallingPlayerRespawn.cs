using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FallingPlayerRespawn : MonoBehaviourPun
{
    [SerializeField] List<GameObject> list_LocalArea = new List<GameObject>();
    List<Vector3> list_v3_localPosition = new List<Vector3>();
    List<Quaternion> list_q_localRotation = new List<Quaternion>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PC_Player"))
        {
            for (int i = 0; i < list_LocalArea.Count; i++)
            {
                GameObject la = list_LocalArea[i];
                list_v3_localPosition.Add((la.transform.position - other.transform.position));
                list_q_localRotation.Add(la.transform.rotation);

                GameObject tmp = Instantiate(la, list_v3_localPosition[i], list_q_localRotation[i]);
            }
        }
    }


}
