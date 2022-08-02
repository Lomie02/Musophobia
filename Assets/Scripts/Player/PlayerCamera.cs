using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [Header("General")]

    Transform m_Camera = null;
    [SerializeField] Transform m_Body = null;
    [SerializeField] float m_RotationLimits = 90;

    [Space]

    [SerializeField] float m_StoppingDistance = 2;

    [Header("Mouse"), Space]

    [SerializeField] bool m_State = true;
    [SerializeField] float m_Sensitivity = 200f;

    [Header("Crouching"), Space]
    [SerializeField] Vector3 m_RestPosition;
    [SerializeField] Vector3 m_CroushPositon;

    [Space]

    [SerializeField] CapsuleCollider m_ColliderBody;

    //======================================================
    public float m_Xpos;
    public float m_Ypos;

    bool m_IsCrouching = false;
    bool m_IsCursorShowing = false;

    void Start()
    {
        m_Camera = GetComponent<Transform>();
        m_Ypos = 0;
    }

    void Update()
    {
        if (m_State)
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
        m_State = _state;
    }

    public bool GetPlayerState()
    {
        return m_State;
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
}
