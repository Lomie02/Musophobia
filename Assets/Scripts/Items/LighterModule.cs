using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterModule : MonoBehaviour
{
    [SerializeField] Camera m_Camera;

    bool m_IsOn = false;
    public void TurnOn()
    {
        if (!m_IsOn)
        {
            Debug.Log("On");
            m_IsOn = true;
        }
    }

    public void TurnOff()
    {
        if (m_IsOn)
        {
            Debug.Log("Off");
            m_IsOn = false;
        }
    }

    public void SearchForCandle()
    {
        if (!m_IsOn)
        {
            return;
        }

        RaycastHit cast;

        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out cast, 4))
        {
            if (cast.collider.GetComponent<CandleIdentifier>() != null)
            {
                CandleIdentifier temp = cast.collider.gameObject.GetComponent<CandleIdentifier>();
                temp.LightCandle();
            }
        }
    }
}