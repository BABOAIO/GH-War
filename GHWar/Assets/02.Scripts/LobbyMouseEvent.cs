using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

// ĳ���� ������ �� �߻���ų �̺�Ʈ
public class LobbyMouseEvent : MonoBehaviour
{
    // ĳ���͸� ������ �� �ٲ����� �κ��� ������ ������Ʈ
    [SerializeField] GameObject O_EventSystem;

    [SerializeField]
    AudioClip ac_Enter;
    [SerializeField]
    AudioClip ac_Select;

    AudioSource as_This;

    // �ڽ��� �ִϸ�����
    Animator a_Player;
    // �ٸ� ĳ������ �ִϸ�����
    [SerializeField]
    Animator a_Other;

    // ���¸� �з�, enum�� ���� ���ºз��� �������� �߻���Ű�Ƿ� �����ϸ� �Ⱦ��°� ���ٰ� ��
    public enum SelectPlayerState
    {
        Idle,
        Selecting,
        CheckSelect,
        Rejected,
    }
    // �ν�����â������ ����, �⺻ ���¸� Idle�� ��Ÿ��
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

    // ���콺 �̺�Ʈ... �ȵȴٸ� �ö��̴��� �پ��ִ��� Ȯ������
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
