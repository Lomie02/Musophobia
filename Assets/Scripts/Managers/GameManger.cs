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

    [SerializeField, Tooltip("When enabled the user can press the Esc key to quit the game.")] bool m_UseQuit = false;
    [SerializeField, Tooltip("Called when game quits.")] UnityEvent m_OnQuit = null;

    [SerializeField, Tooltip("When enabled the mouse cursor will get fixed at the start of the frame.")] AutoFixMouse m_AutoFixMouse = AutoFixMouse.DISABLED;

    PauseManager m_PauseManager = null;


    private void Start()
    {
        m_PauseManager = GetComponent<PauseManager>();

        if (m_AutoFixMouse == AutoFixMouse.ENABLED)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
}