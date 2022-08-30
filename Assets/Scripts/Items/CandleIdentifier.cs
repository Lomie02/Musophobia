using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CandleIdentifier : MonoBehaviour
{
    [SerializeField] float m_Duration = 15f;
    [SerializeField] Light m_CandleLight = null;

    [SerializeField] UnityEvent m_OnLit;
    [SerializeField] UnityEvent m_OnBlow;

    float m_Timer = 0;
    bool m_IsLit = false;

    private void Start()
    {
        m_CandleLight.gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        if (m_IsLit)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > m_Duration)
            {
                if (m_CandleLight)
                {
                    m_CandleLight.gameObject.SetActive(false);
                }

                if (m_OnBlow != null)
                {
                    m_OnBlow.Invoke();
                }
                m_Timer = 0;
            }
        }
    }

    public void LightCandle()
    {
        if (!m_IsLit)
        {
            m_CandleLight.gameObject.SetActive(true);
            m_OnLit.Invoke();
            m_IsLit = true;
        }
    }
}
