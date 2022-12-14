using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CandleIdentifier : MonoBehaviour
{
    [SerializeField] float m_Duration = 15f;

    [SerializeField] UnityEvent m_OnLit;
    [SerializeField] UnityEvent m_OnBlow;

    float m_Timer = 0;
    bool m_IsLit = false;

    void FixedUpdate()
    {
        if (m_IsLit)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > m_Duration)
            {
                if (m_OnBlow != null)
                {
                    m_OnBlow.Invoke();
                }
                m_IsLit = false;
                m_Timer = 0;
            }
        }
    }

    public void LightCandle()
    {
        if (!m_IsLit)
        {
            m_OnLit.Invoke();
            m_IsLit = true;
        }
    }
}
