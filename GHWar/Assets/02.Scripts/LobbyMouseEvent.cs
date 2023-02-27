using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

// 캐릭터 선택할 시 발생시킬 이벤트
public class LobbyMouseEvent : MonoBehaviour
{
    // 캐릭터를 선택할 시 바뀌어야할 부분을 참고할 오브젝트
    [SerializeField] GameObject O_EventSystem;

    [SerializeField]
    AudioClip ac_Enter;
    [SerializeField]
    AudioClip ac_Select;

    AudioSource as_This;

    // 자신의 애니메이터
    Animator a_Player;
    // 다른 캐릭터의 애니메이터
    [SerializeField]
    Animator a_Other;

    // 상태를 분류, enum에 의한 상태분류는 가비지를 발생시키므로 가능하면 안쓰는게 좋다고 함
    public enum SelectPlayerState
    {
        Idle,
        Selecting,
        CheckSelect,
        Rejected,
    }
    // 인스펙터창에서는 숨김, 기본 상태를 Idle로 나타냄
    [HideInInspector]
    public SelectPlayerState _selectPlayerState = SelectPlayerState.Idle;

    void Start()
    {
        as_This = GetComponent<AudioSource>();
        a_Player = GetComponent<Animator>();

        _selectPlayerState = SelectPlayerState.Idle;
        as_This.Stop();
    }

    private void Update()
    {
        switch(_selectPlayerState)
        {
            case SelectPlayerState.Idle:
                if (a_Player != null)
                {
                    a_Player.SetBool("OnSelect", false);
                }
                break;

            case SelectPlayerState.Selecting:
                if (a_Player != null)
                {
                    a_Player.SetBool("OnSelect", true);
                }
                break;

            case SelectPlayerState.CheckSelect:
                if (a_Player != null)
                {
                    a_Player.SetBool("CheckSelect", true);
                }
                break;

            case SelectPlayerState.Rejected:
                if (a_Player != null)
                {
                    a_Player.SetBool("Rejected", true);
                    Observable.NextFrame().Subscribe(_ => a_Player.SetBool("Rejected", false));
                }
                break;
        }
    }

    // 마우스 이벤트... 안된다면 컬라이더가 붙어있는지 확인하자
    private void OnMouseEnter()
    {
        as_This.PlayOneShot(ac_Enter);
        _selectPlayerState = SelectPlayerState.Selecting;
        // Start anim...
    }

    private void OnMouseExit()
    {
        _selectPlayerState = SelectPlayerState.Idle;
        // Stop Anim...
    }

    private void OnMouseDown()
    {
        as_This.PlayOneShot(ac_Select);
        _selectPlayerState = SelectPlayerState.CheckSelect;
        a_Other.GetComponent<LobbyMouseEvent>()._selectPlayerState = SelectPlayerState.Rejected;
        O_EventSystem.GetComponent<UIButtonManagement>().connManager.GetComponent<ConnManager>().array_PlayerType[1] = gameObject.name;
        O_EventSystem.GetComponent<UIButtonManagement>().OnClickButton_GameStart();
    }
}
