using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

// VR 플레이어의 손(콜라이더가 있는)에 넣어준다.
// 손이 바닥에 닿았을 경우, 통과하지 못하게 방지
public class HandPresencePhysic : MonoBehaviourPun
{
    public AudioSource as_parent;

    [Header("같이 이동할 컨트롤러 위치")]
    [SerializeField] Transform t_target;
    [Header("움직임 제한 초과 시 나올 투명 매터리얼")]
    [SerializeField] Renderer rd_nonPhysicsHand;
    [Header("볼 수 있는 거리(?)")]
    [SerializeField] float f_showDistance = 0.05f;

    [SerializeField] Vector2 v2_rangeOfHandRotation;

    // 손에 있는 모든 콜라이더
    Collider[] col_hands;
    // 움직임에 제약을 주기 위한 리지드바디 컨트롤
    Rigidbody rb_this;

    // 대포와 부딪힘
    public bool IsHit;

    void Start()
    {
        rb_this = GetComponent<Rigidbody>();
        col_hands = GetComponentsInChildren<Collider>();

        IsHit = false;
        //foreach(var col in col_hands)
        //{
        //    col.AddComponent<VRPlayerHandHit>();
        //    col.GetComponent<VRPlayerHandHit>().ParentHand = this;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            PhotonNetwork.Destroy(collision.gameObject);
            as_parent.Stop();
            as_parent.Play();
            StartCoroutine(StopHandScript());
        }
    }

    IEnumerator StopHandScript()
    {
        IsHit = true;

        // 진동을 주기위한 DoTween 함수, OnStart는 진동 시작 시 발생할 함수
        transform.DOShakeRotation(0.5f, 30.0f).OnStart(() =>
        {
            transform.DOShakePosition(0.5f, 0.05f);
        }
        );

        // 3초동안 손 마비됨
        yield return new WaitForSeconds(3.0f);
        IsHit = false;
    }

    private void Update()
    {
        if (IsHit) return;
        float f_distance = Vector3.Distance(transform.position, t_target.position);

        if (f_distance > f_showDistance)
        {
            rd_nonPhysicsHand.enabled = true;
        }
        else
        {
            rd_nonPhysicsHand.enabled = false;
        }
    }

    // 아래 Invoke 함수 참고
    public void OnSelect_EnableHandCollider()
    {
        foreach(var item in col_hands)
        {
            item.enabled = true;
        }
    }

    // XR 이벤트 함수, 선택(트리거 버튼) 해제하면 콜라이더 발생, 이것을 0.5초 이후 발생
    public void OnSelect_EnableHandCollider_Delay(float delay)
    {
        Invoke("OnSelect_EnableHandCollider", delay);
    }

    // XR 이벤트 함수, 선택(트리거 버튼)하면 콜라이더 없어짐, 충돌 시 떨리는 것을 방지하기 위함
    public void OnSelect_DisableHandCollider()
    {
        foreach (var item in col_hands)
        {
            item.enabled = false;
        }
    }
    public void OnSelect_DisableHandCollider_Delay(float delay)
    {
        Invoke("OnSelect_DisableHandCollider", delay);
    }

    void FixedUpdate()
    {
        // 쿼터니언의 단점 : 180도를 초과하는 회전을 나타낼수 없음
        // 오일러각의 단점 : 짐벌락, 즉 같은 각도로 표현하더라도 오일러의 세 다른 축에 따라 완전히 다른 각으로 해석됨.

        if (IsHit) return;
        if (photonView.IsMine)
        {
            // 해결책 1 : 손의 회전각을 그대로 따라감
            // 이 부분이면 한번에 해결되지만, 땅과 닿는 부분에서 어색함이 느껴짐
            rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);
            transform.rotation = t_target.rotation;
        }
        // 손에 위치를 따라오게 하는 부분(방향벡터)
        rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);

        // 해결책 2 : 손의 움직임에서 역산하여 적용함
        // 장점 : 포톤뷰가 없이 자연스러운 움직임 발생, 땅에 닿아도 어색한 부분이 없음(다만 어느정도 질량이 받혀줘야함)
        // 단점 : 쿼터니언의 단점을 그대로 가져온다, 즉 주어진 축에서 180도를 넘어가면 손이 돌아가버림

        //// 쿼터니언은 3x3행렬을 변환시킨 것 >> 각도의 연산을 하려면, 먼저 계산할 부분이 뒤에 있어야함.
        //// ex) BAv != ABv, A만큼 회전한 뒤 B만큼 회전한다. != B만큼 회전한 뒤 A만큼 회전한다. 기본적으로 회전할 축이 달라지기 때문
        //// 연산은 기본적으로 이후 방향 + (- 지난 방향) (-1은 쿼터니언의 경우 역행렬)
        //// 원점으로 옮긴 후 원하는 방향으로 회전한다는 의미
        //// 벡터도 마찬가지로 원하는 벡터를 구하기 위해 원점에서 시작하는 두 벡터의 차를 이용한다. 이와 유사함
        //Quaternion q_rotationDifference = t_target.rotation * Quaternion.Inverse(transform.rotation);
        //// 쿼터니언을 회전축과 회전각으로 변환시킴, identity는 축 1,0,0과 각 0도로 분류됨
        //q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        //// 쿼터니언을 그대로 채용할 것을 오일러로 다시 변환 >> 위의 계산에 의미가 없어짐
        //// 다만, 각속도를 구하기 위해서는 축이 움직이는 양을 판별하기 위해서 쓰임
        //// 같은 각도라도 시계바늘이 길면 더 많은 움직임이, 짧으면 더 적은 움직임이 발생한다고 생각하면 된다.
        //Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        //// 오일러로 변환한 값을 라디안으로 수정하여 단위시간에 따른 각속도로 변환
        //rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
