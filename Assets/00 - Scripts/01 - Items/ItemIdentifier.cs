using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemIdentifier : MonoBehaviour
{
    [SerializeField] string m_ItemName = "empty";
    [SerializeField] UnityEvent m_OnUse;

    public void UseItem()
    {
        if (m_OnUse != null)
        {
            m_OnUse.Invoke();
        }
    }
}