using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

// VR의 양손 중 withPhysics 최상단에 넣는다.
// VR 손이 대포에 맞았을 경우, 마비되고 진동이 울리는 스크립트
// 지금은 쓰는 곳 없음
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
