using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEngine.Experimental.AI;
using static UnityEngine.GraphicsBuffer;

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
        if (IsHit) return;
        // 손에 위치를 따라오게 하는 부분(방향벡터)
        rb_this.velocity = (t_target.position - transform.position) / (Time.fixedDeltaTime);

        // 쿼터니언은 3x3행렬을 변환시킨 것 >> 각도의 연산을 하려면, 역산할 부분이 앞에 곱해짐(결합법칙이 성립하지 않음)
        Quaternion q_rotationDifference = Quaternion.Inverse(transform.rotation) * t_target.rotation;
        // 쿼터니언을 회전축과 회전각으로 변환시킴
        q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        //// 축을 모르는 상황에서 상하한을 주는 것은 의미가 없음 >> 실험으로 0보다 작은 값을 주면 안되는 이유가 여기있음
        //f_angleInDegree = Mathf.Clamp(f_angleInDegree, v2_rangeOfHandRotation.x, v2_rangeOfHandRotation.y);

        //// 쿼터니언을 그대로 채용할 것을 오일러로 다시 변환 >> 의미가 없음
        //Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        //rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);

        
    }
}
