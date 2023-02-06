using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;
using UniRx;

public class TurretManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cannonBall;
    [SerializeField] Transform tr_firePos;
    [SerializeField] float f_shotPower = 150f;

    [SerializeField] Text txt_countDown;
    //[SerializeField] Animation anim_turretOpen;
    [SerializeField] Animator a_turret;

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
        if (!a_turret.GetBool("Open"))
        {
            currentTime += Time.fixedDeltaTime;

            int i_currTime = (int)currentTime;

            txt_countDown.text = i_currTime.ToString();
        }

        //print("오차1" + (tr_this.localPosition.y - v3_originPos.y));
        //print("오차2" + (tr_this.localPosition.y - _joint.linearLimit.limit));

        if (currentTime > delayTime)
        {
            txt_countDown.text = "O.K.";
            TurretButtonPress();
            if (photonView.IsMine)
            {
                //GetPressOrRelease();
            }
        }
    }

    IEnumerator TurretActive()
    {
        txt_countDown.text = "shot!!";

        a_turret.SetBool("Open", true);
        yield return new WaitForSeconds(1.0f);
        a_turret.SetBool("Fire", true);

        tr_firePos.LookAt(GameObject.FindGameObjectWithTag("VRPlayerHead").transform.position);
        GameObject ball = Instantiate(cannonBall, tr_firePos.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(tr_firePos.forward * f_shotPower, ForceMode.Impulse);
        yield return new WaitForSeconds(1.0f);

        Observable.NextFrame().Subscribe(_ => a_turret.SetBool("Fire", false));
        yield return new WaitForSeconds(0.5f);
        a_turret.SetBool("Open", false);

        yield return null;
    }

    void TurretButtonPress()
    {
        if (b_isPress)
        {
            if (Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
            {
                Debug.Log("Turret 안눌림..");
                b_isPress = true;
                //OnPressButton_Turret.Invoke();

            }
        }
        else if (!b_isPress)
        {
            if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
            {
                Debug.Log("Turret 눌림!");
                b_isPress = false;
                //OnReleaseButton_Turret.Invoke();

                //StopCoroutine(TurretActive());
                currentTime = 0f;

                StartCoroutine(TurretActive());
            }

        }
    }

    //[PunRPC]
    //void GetPressOrRelease()
    //{
    //    // 누르기 시작
    //    if (!b_isPress && !anim_turretOpen.isPlaying)
    //    {
    //        if(Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
    //        {
    //            Debug.Log("Turret 안눌림..");
    //            b_isPress = true;
    //            //OnPressButton_Turret.Invoke();
    //        }
    //    }
    //    // 누르기 해제
    //    if (b_isPress)
    //    {
    //        if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
    //        {
    //            Debug.Log("Turret 눌림!");
    //            b_isPress = false;
    //            //OnReleaseButton_Turret.Invoke();

    //            anim_turretOpen.Play();
    //            currentTime = 0f;
    //        }
    //    }
    //}
}
