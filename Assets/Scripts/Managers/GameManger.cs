using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    enum AutoFixMouse
    {
        ENABLED = 0,
        DISABLED,
    }

    //======================================================

    [SerializeField] bool m_UseSaving = true;
    [SerializeField, Tooltip("When enabled the user can press the Esc key to quit the game.")] bool m_UseQuit = false;
    [SerializeField, Tooltip("Called when game quits.")] UnityEvent m_OnQuit = null;

    [SerializeField, Tooltip("When enabled the mouse cursor will get fixed at the start of the frame.")] AutoFixMouse m_AutoFixMouse = AutoFixMouse.DISABLED;

    [Header("Scene Changes")]

    //======================================================

    [SerializeField] string m_MainMenuScreen = "MainMenu";
    [SerializeField] string m_WinScreen = "WinScreen";
    [SerializeField] string m_GameOverScreen = "GameOver";

    //======================================================

    PauseManager m_PauseManager = null;
    DataSystem m_DataSystem = null;

    PlayerCamera m_PlayerCamera = null;
    private void Start()
    {
        m_PauseManager = GetComponent<PauseManager>();
        m_DataSystem = GetComponent<DataSystem>();
        m_PlayerCamera = FindObjectOfType<PlayerCamera>();

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
        if (Input.GetKeyDown(KeyCode.Escape) && m_UseQuit)
        {
            if (m_OnQuit != null) 
            {
                m_OnQuit.Invoke();
            }
            QuitGame();
        }
        if (m_PauseManager)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                m_PauseManager.CyclePause();
            }
        }
    }

    public void ChangeScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void QuitGame()
    {
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