using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

public class VRPlayerHandHit : MonoBehaviourPun
{
    public HandPresencePhysic ParentHand;
    Transform tr_parent;

    private void Start()
    {
        tr_parent = ParentHand.GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            StartCoroutine(StopHandScript());
        }
    }

    IEnumerator StopHandScript()
    {
        tr_parent.DOShakeRotation(2.0f, 0.05f).OnStart(() =>
            {
                ParentHand.gameObject.GetComponent<HandPresencePhysic>().enabled = false;
                ParentHand.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                ParentHand.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }).OnComplete(() =>
            {
                ParentHand.gameObject.GetComponent<HandPresencePhysic>().enabled = true;
                ParentHand.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                ParentHand.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            });

        yield return null;
    }
}
