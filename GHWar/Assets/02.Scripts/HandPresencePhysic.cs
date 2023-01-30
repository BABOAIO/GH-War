using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPresencePhysic : MonoBehaviour
{
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
    //[Header("땅을 칠때 나오는 이펙트")]
    //[SerializeField] ParticleSystem ps_hitGround;
    //[Header("땅을 칠때 나오는 큐브")]
    //[SerializeField] GameObject o_stone;

    void Start()
    {
        rb_this = GetComponent<Rigidbody>();
        col_hands = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
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

    // XR 이벤트 함수, 선택(트리거 버튼)하면 콜라이더 없어짐
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

    //public void OnHoverEnter_HitGround(HoverEnterEventArgs args)
    //{
    //    print("호버 이벤트 : 충돌 인식 성공");
    //    if (rb_this.velocity.magnitude >= 0)
    //    {
    //        print("호버 이벤트 : 태그 인식 성공");
    //    }
    //}
    //public void OnHoverExit_HitGround(HoverExitEventArgs args)
    //{
    //    print("호버 익시트 이벤트 : 충돌 인식 성공");
    //    if (rb_this.velocity.magnitude >= 0)
    //    {
    //        print("호버 익시트 이벤트 : 태그 인식 성공");
    //    }
    //}



    void FixedUpdate()
    {
        rb_this.velocity = (t_target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion q_rotationDifference = t_target.rotation*Quaternion.Inverse(transform.rotation);
        q_rotationDifference.ToAngleAxis(out float f_angleInDegree, out Vector3 v3_rotationAxis);

        // 이 코드가 없으면 특정 각도에서 손이 180도를 넘어가서 한바퀴 돌아감 > 아직 돌아가지만 손을 비틀정도만 아니면 됨..., 왼손, 오른손 비교도 필수 불가결, 일단 코드 해석 먼저 해야함
        f_angleInDegree = Mathf.Clamp(f_angleInDegree, v2_rangeOfHandRotation.x, v2_rangeOfHandRotation.y);

        Vector3 v3_rotationDifferenceInDegree = f_angleInDegree * v3_rotationAxis;

        rb_this.angularVelocity = (v3_rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
