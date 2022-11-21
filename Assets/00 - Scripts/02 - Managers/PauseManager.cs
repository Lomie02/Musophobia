using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : GameManger
{
    [Header("General")]
    [SerializeField, Tooltip("The panel that will be used as the pause menu.")] Image m_PausePanel = null;

    [SerializeField] Slider m_MouseSlider;
    [SerializeField] Slider m_FieldOfViewSlider;

    PlayerCamera m_Player;
    AudioSource[] m_PausedAudioSource = null;
    
    bool m_IsPaused = false;
    DataSystem m_DataSystemZZZ;
    bool m_Frozen;
    void Start()
    {
        m_PausedAudioSource = FindObjectsOfType<AudioSource>();
        m_Player = FindObjectOfType<PlayerCamera>();

        m_DataSystemZZZ = GetComponent<DataSystem>();
        SetPausePanel(m_IsPaused);
        SetAudioSources(m_IsPaused);

        if (m_FieldOfViewSlider)
        {
            m_FieldOfViewSlider.value = m_Player.GetFieldOfView();
        }
        if (m_MouseSlider)
        {
            m_MouseSlider.value = m_Player.GetMouseSens();
        }
    }

    public void SetPauseState(bool _state)
    {
        if (m_Frozen) { return; }
        m_IsPaused = _state;
        SetPausePanel(m_IsPaused);
        SetAudioSources(m_IsPaused);
    }

    public void FreezeWorld()
    {
        Time.timeScale = 0;
    }

    public void UnFreeze()
    {
        Time.timeScale = 1;
    }

    public void ChangeFov()
    {
        m_Player.SetFieldOfView(m_FieldOfViewSlider.value);
    }

    public void ChangeMouse()
    {
        m_Player.SetMouseSens(m_MouseSlider.value);
    }

    public void SaveSettings()
    {
        m_DataSystemZZZ.SetMouseSens(m_MouseSlider.value);
        m_DataSystemZZZ.SetFieldOfView(m_FieldOfViewSlider.value);
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
        if (m_Frozen) { return; }
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