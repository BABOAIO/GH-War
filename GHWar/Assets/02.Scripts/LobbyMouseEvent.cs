using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class LobbyMouseEvent : MonoBehaviour
{
    [SerializeField] GameObject O_EventSystem;

    AudioSource as_This;

    Animator a_Player;
    [SerializeField]
    Animator a_Other;

    public enum SelectPlayerState
    {
        Idle,
        Selecting,
        CheckSelect,
        Rejected,
    }
    [HideInInspector]
    public SelectPlayerState _selectPlayerState = SelectPlayerState.Idle;

    void Start()
    {
        as_This = GetComponent<AudioSource>();
        a_Player = GetComponent<Animator>();

        _selectPlayerState = SelectPlayerState.Idle;
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

    private void OnMouseEnter()
    {
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
        _selectPlayerState = SelectPlayerState.CheckSelect;
        a_Other.GetComponent<LobbyMouseEvent>()._selectPlayerState = SelectPlayerState.Rejected;
        O_EventSystem.GetComponent<UIButtonManagement>().connManager.GetComponent<ConnManager>().array_PlayerType[1] = gameObject.name;
        O_EventSystem.GetComponent<UIButtonManagement>().OnClickButton_GameStart();
    }
}
