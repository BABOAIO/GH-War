using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

// 부서지는 지형에 넣는다
// 리지드바드 콘스트레인스트 전부 체크 + 메쉬컬라이더 콘벡스+이즈트리거 체크 필수
// 이즈 트리거 없으면 플레이어가 땅을 밟을 수 없다.
public class FractureTest : MonoBehaviourPunCallbacks
{
    public Collider[] colliders;    // 충돌체를 배열로 가져옴

    //[SerializeField] float f_quakeStrength = 1.0f;

    // player가 인식하는 무너지는 시간, 초기화는 100으로 고정한다.
    public int i_destroyTime = 100;

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>(); // 해당 게임오브젝트의 자식 오브젝트의 컴포넌트를 불러옴
        foreach (Collider c in colliders)
        {
            if (c.name.Contains("RandShatter")) continue;  // 부모 예외처리
            c.gameObject.GetComponent<Renderer>().enabled = false;  // 렌더러 false
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();  // 각 자식오브젝트의 리지드바디 컴포넌트 변수 할당
            rb.constraints = (RigidbodyConstraints)126; // 컨스트레인 전부 체크
        }
    }

    private void Update()
    {
        //if(Input.GetKeyUp(KeyCode.Alpha2)) { this.transform.DOShakePosition(0.5f, f_quakeStrength); }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shatter")
        {
            GetComponent<Renderer>().enabled = false;
            foreach (Collider c in colliders)
            {
                if (c.name == "RandShatterTest") continue;
                c.gameObject.GetComponent<Renderer>().enabled = true;
                Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
                rb.constraints = (RigidbodyConstraints)0;   // 컨스트레인 전부 체크해제
            }
        }
    }

    public void DestructionThisArea()
    {
        GetComponent<Renderer>().enabled = false;
        foreach (Collider c in colliders)
        {
            if (c.name.Contains("RandShatter")) continue;  // 부모 예외처리
            c.gameObject.GetComponent<Renderer>().enabled = true;
            Rigidbody rb = c.gameObject.GetComponent<Rigidbody>();
            rb.constraints = (RigidbodyConstraints)0;   // 컨스트레인 전부 체크해제
            rb.AddForce(new Vector3(Random.Range(-10f, 10f), Random.Range(-1f, -0.1f), Random.Range(-10f, 10f)),ForceMode.Impulse); // 부서질때 다양한 방식으로 무너지게함
        }
    }
}