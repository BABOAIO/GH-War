using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using DG.Tweening;

// VR 플레이어의 손(콜라이더가 있는)에 넣어준다.
// 손이 바닥에 닿았을 경우, 통과하지 못하게 방지
public class HandPresencePhysic : MonoBehaviour
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
            as_parent.Stop();
            as_parent.Play();
            StartCoroutine(StopHandScript());
        }
    }

    IEnumerator StopHandScript()
    {
        IsHit = true;

        transform.DOShakeRotation(0.5f, 30.0f).OnStart(() =>
        {
            transform.DOShakePosition(0.5f, 0.05f);
        }
        );

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
        // 손에 위치를 따라오게 하는 부분(방향벡터)
        rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);

        //// 쿼터니언은 3x3행렬을 변환시킨 것 >> 각도의 연산을 하려면, 역산할 부분이 앞에 곱해짐(결합법칙이 성립하지 않음)
        //// 연산은 기본적으로 이후 방향 + (- 지난 방향) (-1은 쿼터니언의 경우 역행렬)
        //Quaternion q_rotationDifference = t_target.rotation * Quaternion.Inverse(transform.rotation);
        //// 쿼터니언을 회전축과 회전각으로 변환시킴
        //q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        //// 축을 모르는 상황에서 상하한을 주는 것은 의미가 없음 >> 실험에서 0보다 작은 값을 주면 안되는 이유가 여기있음
        //f_angleInDegree = Mathf.Clamp(f_angleInDegree, v2_rangeOfHandRotation.x, v2_rangeOfHandRotation.y);

        //// 쿼터니언을 그대로 채용할 것을 오일러로 다시 변환 >> 위의 계산에 의미가 없어짐
        //Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        //// 오일러로 변환한 값을 라디안으로 수정하여 단위시간에 따른 각속도로 변환
        //rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);

        //해결책 1 : 손의 회전각을 그대로 따라감
        // 이부분이면 한번에 해결되지만, 땅과 닿는 부분에서 어색함이 느껴짐
        transform.rotation = t_target.rotation;

        ////해결책 2 : 각속도 = 두 축 사이의 각 / 단위시간
        //transform.rotation.ToAngleAxis(out float angle_this, out Vector3 v3_thisAxis);
        //t_target.rotation.ToAngleAxis(out float angle_target, out Vector3 v3_targetAxis);

    }
}
