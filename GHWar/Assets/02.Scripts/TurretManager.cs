using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;

public class TurretManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text txt_countDown;
    [SerializeField] Animation anim_turretOpen;

    // 버튼 인식에 대한 오차 범위
    [SerializeField] float f_errorRange = 0.025f;
    [SerializeField] float delayTime = 2f;
    float currentTime = 2f;

    Transform tr_this;
    ConfigurableJoint _joint;

    bool b_isPress;
    Vector3 v3_originPos;

    // 유니티 이벤트를 직접 넣어주면 그 스크립트 방식대로 작동, 컴포넌트를 보면 이해가 됨.
    //public UnityEvent OnPressButton_Turret, OnReleaseButton_Turret;

    void Start()
    {
        b_isPress = false;
        tr_this = GetComponent<Transform>();
        v3_originPos= tr_this.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;

        int i_currTime = (int)currentTime;

        txt_countDown.text = i_currTime.ToString();

        //print("오차1" + (tr_this.localPosition.y - v3_originPos.y));
        //print("오차2" + (tr_this.localPosition.y - _joint.linearLimit.limit));

        if (currentTime > delayTime)
        {
            txt_countDown.text = "O.K.";
            GetPressOrRelease();
        }
    }

    [PunRPC]
    void GetPressOrRelease()
    {
        // 누르기 시작
        if (!b_isPress && !anim_turretOpen.isPlaying)
        {
            if(Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
            {
                Debug.Log("Turret 안눌림..");
                b_isPress = true;
                //OnPressButton_Turret.Invoke();

            }
        }
        // 누르기 해제
        if (b_isPress)
        {
            if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
            {
                Debug.Log("Turret 눌림!");
                b_isPress = false;
                //OnReleaseButton_Turret.Invoke();

                anim_turretOpen.Play();
                currentTime = 0f;
            }
        }
    }
}
