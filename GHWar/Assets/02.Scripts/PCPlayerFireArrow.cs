using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UniRx;
using Photon.Realtime;
using DG.Tweening;

// 플레이어 최상단에 넣는다.
[RequireComponent(typeof(AudioSource))]
public class PCPlayerFireArrow : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    Animator a_playerInFire;
    PC_Player_Move _Move;

    [Header("플레이어 죽음 표시")]
    public bool isDie;

    [Header("활 쏘는 주기")]
    [SerializeField] float delayTime = 2.0f;
    float currentTime = 0f;
    //[SerializeField] ParticleSystem ps_ReadyToPowerShot;
    [SerializeField] ParticleSystem[] ps_ReadyToPowerShot;
    bool b_ReadyToPowerShot;

    float shotPow = 0.5f;

    float shotPowInGame;

    [SerializeField] Transform firePos;
    [SerializeField] Transform firePosEnd;

    Transform tr_this;

    // 소리 부분 //
    AudioSource as_fireArrow;
    [SerializeField] AudioClip ac_shotInit;
    [SerializeField] AudioClip ac_shot;
    [SerializeField] AudioClip ac_Ult;
    // 소리 부분 //

    [Header("활쏠 준비 완")]
    public bool B_isReadyToShot = false;

    #region 궁극기
    float currentUlt = 0.0f;
    [SerializeField]
    float maxUlt = 5.0f;
    float perUlt = 1.0f;

    [SerializeField] Image img_Skill;
    [SerializeField] Image img_CoolDown;
    #endregion

    void Start()
    {
        pv = GetComponent<PhotonView>();
        tr_this = GetComponent<Transform>();

        // 소리 부분 //
        as_fireArrow = firePos.GetComponent<AudioSource>();
        // 소리 부분 //

        isDie = false;
        _Move = GetComponent<PC_Player_Move>();
        a_playerInFire = this.gameObject.GetComponent<PC_Player_Move>().a_player;

        shotPowInGame = shotPow;

        currentTime = delayTime;
        B_isReadyToShot = false;

        b_ReadyToPowerShot = false;
        for (int i = 0; i < ps_ReadyToPowerShot.Length; i++)
        {
            ps_ReadyToPowerShot[i].Stop();
        }

        // 파워 샷 부분
        CoolTimeUI();
    }

    void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        currentUlt += Time.fixedDeltaTime;

        if (pv.IsMine)
        {
            if (!isDie)
            {
                // 서버에 접속할 경우에만 작동
                Shot();
                //UltGauage();
                PowerShot();
                //pv.RPC("Shot", RpcTarget.AllBuffered);
                // 서버 접속하지 않을 경우 확인용
                //if (PhotonNetwork.CountOfPlayers >= 2)
                //{
                //    pv.RPC("Shot", RpcTarget.All);
                //}
                //elsew
                //{
                //    Shot();
                //}
            }
            // 이후 참가자들에게 안보이게 하기 위한 장치
            //PhotonNetwork.SendAllOutgoingCommands();
        }
    }

    void PowerShot()
    {
        if (Input.GetKeyDown(KeyCode.Q)
            && (currentUlt > maxUlt)
            && !b_ReadyToPowerShot
            )
        {
            for (int i = 0; i < ps_ReadyToPowerShot.Length; i++)
            {
                ps_ReadyToPowerShot[i].Play();
            }
            b_ReadyToPowerShot = true;
            GaugeFullChage();
            currentUlt = maxUlt;
        }
        if (ps_ReadyToPowerShot[0].isStopped
            && b_ReadyToPowerShot
            //&& ps_ReadyToPowerShot[1].isStopped
            //&& ps_ReadyToPowerShot[2].isStopped
            //&& ps_ReadyToPowerShot[3].isStopped
            )
        {
            b_ReadyToPowerShot = false;
            currentUlt = 0.0f;
            print("동작");
        }

        if (currentUlt <= maxUlt)
        {
            currentUlt = Mathf.Min(currentUlt, maxUlt);
            float fillValue = currentUlt / maxUlt;
            img_Skill.fillAmount = fillValue;
        }
    }

    [PunRPC]
    GameObject ParticleGenerate()
    {
        GameObject returnObj = 
        PhotonNetwork.Instantiate("PowerShotMagicCircle", transform.position, transform.rotation);
        return returnObj;
    }

    [PunRPC]
    void ParticleDegenerate(GameObject _ps)
    {
        PhotonNetwork.Destroy(_ps);
    }

    [PunRPC]

    private void Shot()
    {
        if (Input.GetMouseButtonDown(0)
            && (currentTime >= delayTime)
            && !_Move.isJump
            )
        {
            shotPowInGame = shotPow;
            StartCoroutine(ShotPowerUp());
            a_playerInFire.SetBool("ReadyToShot", true);
            B_isReadyToShot = true;
        }
        // 당겨진 상태에서 마우스를 놓거나, 활을 최대로 당기면 발사
        if ((Input.GetMouseButtonUp(0)
            && B_isReadyToShot
            && currentTime >= delayTime)
            || shotPowInGame >= 10
            )
        {
            AnimOperator();
            if (b_ReadyToPowerShot)
            {
                for (int i = 0; i < ps_ReadyToPowerShot.Length; i++)
                {
                    ps_ReadyToPowerShot[i].Stop();
                }
                GameObject obj_tmp = PhotonNetwork.Instantiate("UltArrow", firePos.position, firePos.rotation);
                b_ReadyToPowerShot = false;
                currentUlt = 0.0f;
                img_Skill.fillAmount = 0f;
            }
            else
            {
                GameObject obj_tmp = PhotonNetwork.Instantiate("Arrow", firePos.position, firePos.rotation);
            }

            //obj_tmp.GetComponent<Rigidbody>().AddForce(shotPowInGame * (firePosEnd.position - firePos.position), ForceMode.Impulse);
        }
        if (!B_isReadyToShot)
        {
            StopAllCoroutines();
            shotPowInGame = shotPow;
        }
    }

    void AnimOperator()
    {
        //StopCoroutine(ShotPowerUp());
        StopAllCoroutines();
        a_playerInFire.SetBool("ReadyToShot", false);

        // 소리 부분 //
        as_fireArrow.PlayOneShot(ac_shot, 0.5f);
        // 소리 부분 //

        currentTime = 0;

        a_playerInFire.SetBool("Shot", true);
        Observable.NextFrame().Subscribe(_ => a_playerInFire.SetBool("Shot", false));
        Invoke("DelayedActive", 0.4f);

        shotPowInGame = shotPow;
    }

    IEnumerator ShotPowerUp()
    {
        as_fireArrow.PlayOneShot(ac_shotInit, 0.5f);
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 3;
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 5;
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 7;
        yield return new WaitForSeconds(1.0f);
        shotPowInGame = 10;
    }

    void GaugeFullChage()
    {
        // q누르면 바운스하는 효과
        img_Skill.rectTransform.DOScale(img_Skill.rectTransform.localScale * 1.3f, 0.3f).
            OnStart(() => { img_CoolDown.enabled = false; }).
            OnComplete(() => { img_Skill.rectTransform.DOScale(Vector3.one, 0.3f)
                .OnComplete(() => { img_CoolDown.enabled = true; });   });
    }

    void DelayedActive()
    {
        B_isReadyToShot = false;
    }

    // 파워 샷 부분
    void CoolTimeUI()
    {
        currentUlt = 0f;
        img_Skill.fillAmount = 0f;
        img_Skill.type = Image.Type.Filled;
        img_Skill.fillMethod = Image.FillMethod.Radial360;
        img_Skill.fillOrigin = (int)Image.Origin360.Top;
        img_Skill.fillClockwise = true;
    }

    [PunRPC]
    void Ultimate()
    {
        if (Input.GetKeyDown(KeyCode.Q)
            && (currentTime >= delayTime)
            && currentUlt >= maxUlt
            )
        {
            shotPowInGame = shotPow;
            StartCoroutine(ShotPowerUp());
            a_playerInFire.SetBool("ReadyToShot", true);
            B_isReadyToShot = true;
        }
        // 당겨진 상태에서 마우스를 놓거나, 활을 최대로 당기면 발사
        if ((Input.GetKeyUp(KeyCode.Q)
            && B_isReadyToShot
            && currentTime >= delayTime)
            || shotPowInGame >= 10
            )
        {
            AnimOperator();
            if (b_ReadyToPowerShot)
            {
                for (int i = 0; i < ps_ReadyToPowerShot.Length; i++)
                {
                    ps_ReadyToPowerShot[i].Stop();
                }
                as_fireArrow.PlayOneShot(ac_Ult);
                GameObject obj_tmp = PhotonNetwork.Instantiate("UltArrow", firePos.position, firePos.rotation);
                b_ReadyToPowerShot = false;
            }
            else
            {
                GameObject obj_tmp = PhotonNetwork.Instantiate("UltArrow", firePos.position, firePos.rotation);
            }
        }
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    a_playerInFire.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
    //    //a_playerInFire.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
    //    a_playerInFire.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
    //    //a_playerInFire.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

    //    a_playerInFire.SetIKPosition(AvatarIKGoal.LeftHand, firePos.position);
    //    //a_playerInFire.SetIKPosition(AvatarIKGoal.RightHand, firePosEnd.position);
    //    a_playerInFire.SetIKRotation(AvatarIKGoal.LeftHand, firePos.rotation);
    //    //a_playerInFire.SetIKRotation(AvatarIKGoal.RightHand, firePosEnd.rotation);

    //}
}