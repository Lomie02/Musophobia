using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NoteIdentifier : MonoBehaviour
{
    [SerializeField, Tooltip("The Targeted Note UI")] GameObject m_Canvas;

    //=================================================

    [SerializeField] UnityEvent m_OnRead;
    [SerializeField] UnityEvent m_OnStopReading;

    PlayerCamera m_Player;
    PauseManager m_PauseManager;
    bool m_IsReading = false;

    //=================================================

    int m_CurrentPage = 0;
    bool m_HasPages = false;

    private void Start()
    {
        m_PauseManager = FindObjectOfType<PauseManager>();
        m_Player = FindObjectOfType<PlayerCamera>();
    }

    public void CycleNote()
    {
        if (m_IsReading)
        {
            m_PauseManager.UnFreeze();

            m_Player.CursorState(false);
            m_Player.SetPlayerState(true);

            m_Canvas.SetActive(false);
            m_IsReading = false;
        }
        else
        {
            m_PauseManager.FreezeWorld();

            m_Player.CursorState(true);
            m_Player.SetPlayerState(false);

            m_Canvas.SetActive(true);
            m_IsReading = true;
        }
    }

    public void ReadNote()
    {
        if (m_OnRead != null)
        {
            m_OnRead.Invoke();
        }

        m_Canvas.gameObject.SetActive(true);
        m_IsReading = true;
    }

    public void CloseNote()
    {
        if (m_OnStopReading != null)
        {
            m_OnStopReading.Invoke();
        }
        m_Canvas.gameObject.SetActive(false);
        m_PauseManager.UnFreeze();

        m_Player.CursorState(false);
        m_Player.SetPlayerState(true);
        m_IsReading = false;
    }
}
