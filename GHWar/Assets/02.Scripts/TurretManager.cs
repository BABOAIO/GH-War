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

    // ��ư �νĿ� ���� ���� ����
    [SerializeField] float f_errorRange = 0.025f;
    [SerializeField] float delayTime = 2f;
    float currentTime = 2f;

    Transform tr_this;
    ConfigurableJoint _joint;

    bool b_isPress;
    Vector3 v3_originPos;

    // ����Ƽ �̺�Ʈ�� ���� �־��ָ� �� ��ũ��Ʈ ��Ĵ�� �۵�, ������Ʈ�� ���� ���ذ� ��.
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

        //print("����1" + (tr_this.localPosition.y - v3_originPos.y));
        //print("����2" + (tr_this.localPosition.y - _joint.linearLimit.limit));

        if (currentTime > delayTime)
        {
            txt_countDown.text = "O.K.";
            GetPressOrRelease();
        }
    }

    [PunRPC]
    void GetPressOrRelease()
    {
        // ������ ����
        if (!b_isPress && !anim_turretOpen.isPlaying)
        {
            if(Mathf.Abs(tr_this.localPosition.y - v3_originPos.y) < f_errorRange)
            {
                Debug.Log("Turret �ȴ���..");
                b_isPress = true;
                //OnPressButton_Turret.Invoke();

            }
        }
        // ������ ����
        if (b_isPress)
        {
            if (Mathf.Abs(tr_this.localPosition.y - _joint.linearLimit.limit) < f_errorRange)
            {
                Debug.Log("Turret ����!");
                b_isPress = false;
                //OnReleaseButton_Turret.Invoke();

                anim_turretOpen.Play();
                currentTime = 0f;
            }
        }
    }
}
