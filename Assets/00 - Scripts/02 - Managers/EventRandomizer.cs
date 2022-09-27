using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRandomizer : MonoBehaviour
{
    [SerializeField] bool m_HideAtStart = false;
    [SerializeField] bool m_StartRandom = false;

    [SerializeField] GameObject[] m_Events = null;

    int m_Interations = 1;


    void Start()
    {
        if (m_HideAtStart && m_Events != null)
        {
            for (int i = 0; i < m_Events.Length; i++)
            {
                m_Events[i].SetActive(false);
            }
        }

        if (m_StartRandom)
        {
            Randomize();
        }
    }
    public void HideAll()
    {
        if (m_Events != null)
        {
            for (int i = 0; i < m_Events.Length; i++)
            {
                m_Events[i].SetActive(false);
            }
        }
    }

    public void Randomize()
    {
        HideAll();

        for (int i = 0; i < m_Interations; i++)
        {
            for (int j = 0; j < m_Events.Length; j++)
            {
                m_Events[Random.Range(0, m_Events.Length)].SetActive(true);
            }
        }
    }
}
