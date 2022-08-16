using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [Header("General")]

    [SerializeField, Tooltip("The players body object.")] Transform m_Body = null;
    [SerializeField, Tooltip("Adds a limit when looking up & down.")] float m_RotationLimits = 90;

    [Space]

    [Header("Mouse"), Space]
    [SerializeField, Range(5, 999), Tooltip("How fast the camera moves with the mouse")] float m_Sensitivity = 200f;
    bool m_EnableMouse = true;

    [Header("Crouching"), Space]
    [SerializeField, Tooltip("The players collider for crouching adjustments.")] CapsuleCollider m_ColliderBody;

    [Space]

    [SerializeField, Tooltip("Where the players camera will sit when not crouching.")] Vector3 m_RestPosition;
    [SerializeField, Tooltip("Where the players camera will sit when crouching.")] Vector3 m_CroushPositon;

    //======================================================
    
    float m_Xpos;
    float m_Ypos;

    bool m_IsCrouching = false;
    bool m_IsCursorShowing = false;

    Transform m_Camera = null;
    float m_FieldOfView = 80f;

    void Start()
    {
        m_Camera = GetComponent<Transform>();
        m_Ypos = 0;
    }

    void Update()
    {
        if (m_EnableMouse)
        {
            float Xpos = Input.GetAxis("Mouse X") * (m_Sensitivity * 2) * Time.deltaTime;
            float Ypos = Input.GetAxis("Mouse Y") * (m_Sensitivity * 2) * Time.deltaTime;

            if (m_Body)
            {
                m_Body.Rotate(0, Xpos, 0);
            }

            m_Xpos += Xpos;
            m_Ypos -= Ypos;

            m_Ypos = Mathf.Clamp(m_Ypos, -m_RotationLimits, m_RotationLimits);

            m_Camera.eulerAngles = new Vector3(m_Ypos, m_Xpos, 0);

            if (m_IsCrouching)
            {
                m_ColliderBody.center = new Vector3(0, -0.29f, 0);
                m_ColliderBody.height = 1.35f;
                transform.localPosition = m_CroushPositon;
            }
            else
            {
                m_ColliderBody.center = Vector3.zero;
                m_ColliderBody.height = 2f;
                transform.localPosition = m_RestPosition;
            }
        }
    }

    public void CursorState(bool _state)
    {
        if (_state)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SetPlayerState(bool _state)
    {
        m_EnableMouse = _state;
    }

    public bool GetPlayerState()
    {
        return m_EnableMouse;
    }

    bool CheckCanCrouch()
    {
        RaycastHit cast;
        if (Physics.Raycast(transform.position, m_Body.transform.up, out cast, 1.5f))
        {
            Debug.Log(cast.collider.name);
            return false;
        }
        else
        {
            return true;
        }
    }
    public void SetCrouchState(bool _state)
    {
        if (CheckCanCrouch())
        {
            m_IsCrouching = _state;
        }
    }

    public bool GetCrouchState()
    {
        return m_IsCrouching;
    }

    public void CycleCursor()
    {
        if (m_IsCursorShowing)
        {
            m_IsCursorShowing = false;
            SetCrouchState(m_IsCursorShowing);
        }
        else
        {
            m_IsCursorShowing = true;
            SetCrouchState(m_IsCursorShowing);
        }
    }

    public void SetFieldOfView(float _amount)
    {
        m_FieldOfView = _amount;
        Camera.main.fieldOfView = m_FieldOfView;
    }

    public float GetFieldOfView()
    {
        return m_FieldOfView;
    }

    public void SetMouseSens(float _amount)
    {
        m_Sensitivity = _amount;
    }

    public float GetMouseSens()
    {
        return m_Sensitivity;
    }
}
