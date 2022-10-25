using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemIdentifier : MonoBehaviour
{
    [Header("General")]
    [SerializeField] string m_ItemName = "empty";
    [SerializeField] UnityEvent m_TurnOn;
    [SerializeField] UnityEvent m_TurnOff;
    [SerializeField] UnityEvent m_OnUse;

    [Header("Audio")]

    [SerializeField] UnityEvent m_OnPickedUp;
    [SerializeField] UnityEvent m_OnDroppedItem;

    bool m_OnState = false;

    public void UseItem()
    {
        if (m_OnUse != null && m_OnState)
        {
            m_OnUse.Invoke();
        }
    }

    public void TurnOn()
    {
        if (m_TurnOn != null)
        {
            m_TurnOn.Invoke();
        }
    }
    public string GetName()
    {
        return m_ItemName;
    }

    public void TurnOff()
    {
        if (m_TurnOff != null)
        {
            m_TurnOff.Invoke();
        }
    }

    public void CycleState()
    {
        if (m_OnState)
        {
            TurnOff();
            m_OnState = false;
        }
        else
        {
            TurnOn();
            m_OnState = true;
        }
    }

    public void PickUpSound()
    {
        m_OnPickedUp.Invoke();
    }

    public void DroppedSound()
    {
        m_OnDroppedItem.Invoke();
    }
}