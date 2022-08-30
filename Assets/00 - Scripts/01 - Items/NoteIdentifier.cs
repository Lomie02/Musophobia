using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NoteIdentifier : MonoBehaviour
{
    [SerializeField, Tooltip("The Targeted Note UI")] GameObject m_Canvas;
    [SerializeField, Tooltip("The Note Image")] Text m_TextPages;

    [SerializeField, Tooltip("Different Pages")] string[] m_PageDescriptions;
    //=================================================

    [SerializeField] UnityEvent m_OnRead;
    [SerializeField] UnityEvent m_OnStopReading;

    PlayerCamera m_Player;
    PauseManager m_PauseManager;
    bool m_IsReading = false;

    //=================================================

    int m_CurrentPage = 0;
    bool m_HasPages = false;

    [SerializeField] GameObject m_LeftButton;
    [SerializeField] GameObject m_RightButton;

    private void Start()
    {
        m_PauseManager = FindObjectOfType<PauseManager>();
        m_Player = FindObjectOfType<PlayerCamera>();
        m_HasPages = CheckForPages();
    }

    bool CheckForPages()
    {
        if (m_PageDescriptions.Length > 1)
        {
            if (m_LeftButton)
            {
                m_LeftButton.SetActive(false);
            }
            m_RightButton.SetActive(true);
            
            CheckPlacement();
            return true;
        }
        else
        {
            if (m_LeftButton)
            {
                m_LeftButton.SetActive(false);
            }
            m_RightButton.SetActive(false);
            CheckPlacement();
            return false;
        }
    }

    public void PressLeft()
    {
        if (m_HasPages)
        {
            m_CurrentPage--;
            m_CurrentPage = Mathf.Clamp(m_CurrentPage, 0, m_PageDescriptions.Length);
            CheckPlacement();
        }
    }

    void ChangeText()
    {
        m_TextPages.text = m_PageDescriptions[m_CurrentPage];
    }

    public void PressRight()
    {
        if (m_HasPages)
        {
            m_CurrentPage++;
            m_CurrentPage = Mathf.Clamp(m_CurrentPage, 0, m_PageDescriptions.Length);
            CheckPlacement();
        }
    }

    void CheckPlacement()
    {
        if (m_HasPages)
        {
            if (m_CurrentPage >= m_PageDescriptions.Length - 1)
            {
                m_RightButton.SetActive(false);
            }
            else
            {
                m_RightButton.SetActive(true);
            }

            if (m_CurrentPage < 1)
            {
                m_LeftButton.SetActive(false);
            }
            else
            {
                m_LeftButton.SetActive(true);
            }
            ChangeText();
        }
    }

    public void ReadNote()
    {
        if (m_OnRead != null)
        {
            m_OnRead.Invoke();
        }

        m_Canvas.gameObject.SetActive(true);
        m_PauseManager.FreezeWorld();

        m_Player.CursorState(true);
        m_Player.SetPlayerState(false);
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
