using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTimer : MonoBehaviour
{

    float Timer = 0;

    public UnityEvent onFinish;

    public float Duration = 5;

    bool isTiming = false;

    bool m_PlayTimerStart = false;

    // Start is called before the first frame update
    void Start()
    {
        Timer = Duration;

        if (m_PlayTimerStart)
        {
            startTimer();
        }
    }

    public void startTimer()
    {
        if (!isTiming)
        {
            isTiming = true;
            m_PlayTimerStart = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (isTiming)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                Timer = Duration;
                isTiming = false;
                onFinish.Invoke();
            }
        }
    }
}
