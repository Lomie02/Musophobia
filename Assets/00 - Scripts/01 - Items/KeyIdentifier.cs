using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyIdentifier : MonoBehaviour
{
    [Header("General")]
    [SerializeField,Tooltip("The ID must match the door ID")] int m_KeyId = 0;

    public int GetKeyID()
    {
        return m_KeyId;
    }
}
