using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    enum AutoFixMouse
    {
        ENABLED = 0,
        DISABLED,
    }

    enum Resolution
    {
        Res1 = 0,   // 1280 x 720
        Res2,       // 1920 x 1080
        Res3,       // 2560 x 1440
        Res4,       // 3840 x 2160
    }

    //======================================================

    [SerializeField, Tooltip("Enable only in main game.")] bool m_UseSaving = true;
    [SerializeField, Tooltip("When enabled the user can press the Esc key to quit the game.")] bool m_UseQuit = false;
    [SerializeField, Tooltip("Called when game quits.")] UnityEvent m_OnQuit = null;

    [SerializeField, Tooltip("When enabled the mouse cursor will get fixed at the start of the frame.")] AutoFixMouse m_AutoFixMouse = AutoFixMouse.DISABLED;

    [Header("Scene Changes")]

    //======================================================

    [SerializeField, Tooltip("Main menu scene")] string m_MainMenuScreen = "MainMenu";
    [SerializeField, Tooltip("Win Screen")] string m_WinScreen = "WinScreen";
    [SerializeField, Tooltip("Game Over scene")] string m_GameOverScreen = "GameOver";

    [Space]
    [Header("Key Binds")]
    [SerializeField] KeyCode m_ExitGame = KeyCode.Escape;
    [SerializeField] KeyCode m_Pause = KeyCode.P;

    //======================================================
    [SerializeField] Slider m_MusicSlider = null;
    [SerializeField] Slider m_AudioSlider = null;


    //======================================================
    PauseManager m_PauseManager = null;
    DataSystem m_DataSystem = null;

    PlayerCamera m_PlayerCamera = null;
    Resolution m_Resolution = Resolution.Res2;

    [SerializeField] Dropdown m_Dropdown = null;
    /*
        1280 x 720
        1920 x 1080 - default
        2560 x 1440
        3840 x 2160
     */

    private void Start()
    {
        m_PauseManager = GetComponent<PauseManager>();
        m_DataSystem = GetComponent<DataSystem>();
        m_PlayerCamera = FindObjectOfType<PlayerCamera>();

        if (m_DataSystem)
        {
            int resolution = m_DataSystem.GetResolution();
            AdjustResolution(resolution);
        }

        if (!m_PlayerCamera)
        {
            m_UseSaving = false;
        }

        if (m_AutoFixMouse == AutoFixMouse.ENABLED)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (m_DataSystem != null && m_UseQuit)
        {
            SetupValues();
        }
    }

    public void ChangeResolution()
    {
        if (m_Dropdown.value == 0)
        {
            m_Resolution = Resolution.Res1;
        }
        else if (m_Dropdown.value == 1)
        {
            m_Resolution = Resolution.Res2;
        }
        else if (m_Dropdown.value == 2)
        {
            m_Resolution = Resolution.Res3;
        }
        else
        {
            m_Resolution = Resolution.Res4;
        }
        UpdateResolution();
    }

    void AdjustResolution(int res)
    {
        if (res == 0)
        {
            m_Resolution = Resolution.Res1;
        }
        else if (res == 1)
        {
            m_Resolution = Resolution.Res2;
        }
        else if (res == 2)
        {
            m_Resolution = Resolution.Res3;
        }
        else
        {
            m_Resolution = Resolution.Res4;
        }
        UpdateResolution();
    }

    void UpdateResolution()
    {
        switch (m_Resolution)
        {
            case Resolution.Res1:

                Screen.SetResolution(1280, 720, true, 60);
                break;

            case Resolution.Res2:
                
                Screen.SetResolution(1920, 1080, true, 60);
                break;

            case Resolution.Res3:
                Screen.SetResolution(2560, 1440, true, 60);
                break;

            case Resolution.Res4:
                Screen.SetResolution(3840, 2160, true, 60);
                break;
        }
        m_DataSystem.SetResolution((int)m_Resolution);
    }

    public void SetupValues()
    {
        if (m_PlayerCamera)
        {
            if (m_DataSystem.GetFieldOfView() != 0)
            {
                m_PlayerCamera.SetFieldOfView(m_DataSystem.GetFieldOfView());
            }
            if (m_DataSystem.GetMouseSens() != 0)
            {
                m_PlayerCamera.SetMouseSens(m_DataSystem.GetMouseSens());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_ExitGame) && m_UseQuit)
        {
            if (m_OnQuit != null)
            {
                m_OnQuit.Invoke();
            }
            QuitGame();
        }
        if (m_PauseManager)
        {
            if (Input.GetKeyDown(m_Pause))
            {
                m_PauseManager.CyclePause();
            }
        }
    }

    public void ChangeAudioMusic()
    {
        m_DataSystem.SetMusicLevel(m_MusicSlider.value);
    }

    public void ChangeAudioRegular()
    {
        m_DataSystem.SetAudioLevel(m_AudioSlider.value);
    }

    public void ChangeScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void QuitGame()
    {
        if (m_OnQuit != null)
        {
            m_OnQuit.Invoke();
        }

        Application.Quit();
    }

    public string GetGameOverName()
    {
        return m_GameOverScreen;
    }

    public string GetMainMenuScreen()
    {
        return m_MainMenuScreen;
    }

    public string GetWinScreen()
    {
        return m_WinScreen;
    }
}