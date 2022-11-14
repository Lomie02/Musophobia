using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSub : AudioManager
{
    [SerializeField] string m_ClipName = "example: Rat Step";
    AudioManager m_AudioManager;
    void Start()
    {
        m_AudioManager = FindObjectOfType<AudioManager>();
    }

    public void Step()
    {
        m_AudioManager.PlaySound(m_ClipName);
    }
}
