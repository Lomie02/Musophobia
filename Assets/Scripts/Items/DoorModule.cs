using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorModule : MonoBehaviour
{
    [Header("General")]

    [SerializeField] Collider m_Barrier;
    [SerializeField] int m_DoorId;

    [Header("Destruction")]
    [SerializeField, Tooltip("When enabled, the door can be ripped off its hinges with enough force.")] bool m_Destructable = false;
    [SerializeField, Tooltip("The amount of force needed to break a door.")] float m_DestructionForce = 20f;

    [Header("Events")]
    [SerializeField, Space] UnityEvent m_OnDoorUnlocked;
    [SerializeField, Space] UnityEvent m_OnIncorrectKey;
    [SerializeField, Space] UnityEvent m_OnDoorBreak;

    bool m_IsLocked = true;
    HingeJoint m_DoorJoint;

    public void RequestDoorOpen(KeyIdentifier _Id)
    {
        if (_Id == null) { return; }

        if (m_IsLocked && _Id.GetKeyID() == m_DoorId)
        {
            m_IsLocked = false;
            m_Barrier.gameObject.SetActive(m_IsLocked);

            if (m_OnDoorUnlocked != null)
            {
                m_OnDoorUnlocked.Invoke();
            }
        }
        else
        {
            if (m_OnIncorrectKey != null)
            {
                m_OnIncorrectKey.Invoke();
            }
            return;
        }
    }

    public bool GetLockState()
    {
        return m_IsLocked;
    }

    public void SetLockState(bool _state)
    {
        m_IsLocked = _state;
        m_Barrier.gameObject.SetActive(m_IsLocked);
    }
    void Start()
    {
        m_DoorJoint = GetComponent<HingeJoint>();
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), m_Barrier);

        if (m_Destructable)
        {
            m_DoorJoint.breakForce = m_DestructionForce;
        }
        else
        {
            m_DoorJoint.breakForce = float.PositiveInfinity;
        }

        if (!m_DoorJoint)
        {
            Debug.LogError("Failed to assign door!");
        }
    }

    private void OnJointBreak(float breakForce)
    {
        if (m_OnDoorBreak != null)
        {
            m_OnDoorBreak.Invoke();
        }
    }
}