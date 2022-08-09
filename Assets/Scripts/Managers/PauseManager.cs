using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Tooltip("The panel that will be used as the pause menu.")] Image m_PausePanel = null;
    PlayerCamera m_Player;
    AudioSource[] m_PausedAudioSource = null;

    bool m_IsPaused = false;
    void Start()
    {
        m_PausedAudioSource = FindObjectsOfType<AudioSource>();
        m_Player = FindObjectOfType<PlayerCamera>();

        SetPausePanel(m_IsPaused);
        SetAudioSources(m_IsPaused);
    }

    public void SetPauseState(bool _state)
    {
        m_IsPaused = _state;
        SetPausePanel(m_IsPaused);
        SetAudioSources(m_IsPaused);
    }

    void SetAudioSources(bool _state)
    {
        if (m_PausedAudioSource != null) return;

        for (int i = 0; i < m_PausedAudioSource.Length; i++)
        {
            if (!_state)
            {
                m_PausedAudioSource[i].Pause();
            }
            else
            {
                m_PausedAudioSource[i].UnPause();
            }
        }
    }

    public void CyclePause()
    {
        if (m_IsPaused)
        {
            m_IsPaused = false;
            Time.timeScale = 1;
            m_Player.CursorState(m_IsPaused);

            SetPausePanel(m_IsPaused);
            SetAudioSources(m_IsPaused);
        }
        else
        {
            m_IsPaused = true;
            Time.timeScale = 0;
            SetPausePanel(m_IsPaused);

            m_Player.CursorState(m_IsPaused);
            SetAudioSources(m_IsPaused);
        }
    }
    protected void SetPausePanel(bool _state)
    {
        if (m_PausePanel)
        {
            m_PausePanel.gameObject.SetActive(m_IsPaused);
        }
    }
}