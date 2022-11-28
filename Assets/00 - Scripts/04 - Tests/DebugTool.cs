using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class DebugTool : MonoBehaviour
{
    [SerializeField] KeyCode m_ToggleClip = KeyCode.Backspace;
    bool m_FlyMode = false;
    PlayerCamera m_PlayerCamera;
    PlayerController m_Controller;

    CapsuleCollider m_CapsuleCollider;
    Rigidbody m_PlayerBody;

    void Start()
    {
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_PlayerCamera = GetComponentInChildren<PlayerCamera>();
        m_Controller = GetComponent<PlayerController>();
        m_PlayerBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(m_ToggleClip) && Input.GetKey(KeyCode.LeftAlt))
        {
            CycleFlyMode();
        }

        if (m_FlyMode)
        {
            if (Input.GetKey(KeyCode.Z)) // down
            {
                transform.position = transform.position + Vector3.up * 5 * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.X)) // up
            {
                transform.position = transform.position + Vector3.down * 5 * Time.deltaTime;
            }
        }
    }

    void CycleFlyMode()
    {
        if (m_FlyMode)
        {
            ExitFly();
            m_FlyMode = false;
            Debug.Log("DebugTool: Flymode = Off");
        }
        else
        {
            EnterFly();
            Debug.Log("DebugTool: Flymode = On");
            m_FlyMode = true;
        }
    }

    void EnterFly()
    {
        m_CapsuleCollider.enabled = false;

        m_PlayerBody.isKinematic = true;
        m_PlayerBody.useGravity = false;

        m_Controller.FlyMode(true);
    }

    void ExitFly()
    {
        m_CapsuleCollider.enabled = true;

        m_PlayerBody.useGravity = true;
        m_PlayerBody.isKinematic = false;
        m_Controller.FlyMode(false);
    }
}
#endif