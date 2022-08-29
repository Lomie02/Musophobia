using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemIdentifier : MonoBehaviour
{
    [SerializeField] string m_ItemName = "empty";
    [SerializeField] UnityEvent m_TurnOn;
    [SerializeField] UnityEvent m_TurnOff;
    [SerializeField] UnityEvent m_OnUse;

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
}