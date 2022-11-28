using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody m_PlayerMovement = null;

    [Header("General")]
    [SerializeField, Tooltip("How fast the player walks.")] float m_WalkSpeed = 2f;
    [SerializeField, Tooltip("How fast the player runs.")] float m_SprintSpeed = 5f;

    float m_MovementSpeed;
    PlayerCamera m_PlayerCamera = null;
    bool m_CrouchState = false;
    bool m_FlyMode = false;


    void Start()
    {

        m_PlayerCamera = FindObjectOfType<PlayerCamera>();
        m_PlayerMovement = GetComponent<Rigidbody>();

        m_PlayerCamera.CursorState(false);
        m_PlayerMovement.freezeRotation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            CycleCrouch();
            m_PlayerCamera.SetCrouchState(m_CrouchState);
        }
    }

    void FixedUpdate()
    {
        if (m_PlayerCamera.GetPlayerState())
        {
            float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
            float z = Input.GetAxisRaw("Vertical") * Time.deltaTime;

            Vector3 MoveV = transform.right * x + transform.forward * z;

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !m_CrouchState)
            {
                if (!m_FlyMode)
                {
                    m_MovementSpeed = m_SprintSpeed;
                    m_PlayerCamera.SetSprint(true);
                }
                else
                {
                    m_MovementSpeed = 7f;
                }
            }
            else
            {
                m_MovementSpeed = m_WalkSpeed;
                m_PlayerCamera.SetSprint(false);

            }

            m_PlayerMovement.MovePosition(transform.position + MoveV.normalized * m_MovementSpeed * Time.fixedDeltaTime);
        }
    }

    public void FlyMode(bool _state)
    {
        m_FlyMode = _state;
    }

    public void CycleCrouch()
    {
        if (m_CrouchState)
        {
            m_CrouchState = false;
        }
        else
        {
            m_CrouchState = true;
        }
    }
}