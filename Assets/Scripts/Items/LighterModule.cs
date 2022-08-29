using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterModule : MonoBehaviour
{
    Camera m_Camera;
    void Start()
    {
        m_Camera = Camera.main;
    }

    public void SearchForCandle()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_Camera.transform.position, m_Camera.transform.forward, out cast, 3))
        {
            if (cast.collider.gameObject.GetComponent<CandleIdentifier>())
            {
                CandleIdentifier temp;
                temp = cast.collider.gameObject.GetComponent<CandleIdentifier>();

                temp.LightCandle();
            }
        }
    }
}
