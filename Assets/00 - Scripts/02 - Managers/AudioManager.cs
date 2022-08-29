using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClipMode
{
    DEFAULT = 0,
    RANDOM,
}

public enum Rolloff
{
    LogarithmicRolloff = 0,
    LinearRolloff,
}
[System.Serializable]
public struct SoundList
{
    [Header("General")]
    public string m_Name;
    public AudioSource m_Source;
    public bool m_UseMixer;

    [Header("Mixer"), Space]

    public bool m_Mute;
    public bool m_BypassEffects;
    public bool m_BypassListenerEffects;
    public bool m_BypassReverbZones;
    [Space]

    public bool m_PlayOnAwake;
    public bool m_Loop;

    [Space]

    [Range(0, 1)] public float m_Volume;
    [Range(-3, 3)] public float m_Pitch;
    [Range(-1, 1)] public float m_StereoPan;
    [Range(0, 1)] public float m_SpatialBlend;
    [Range(0, 1.1f)] public float m_ReverbZoneMix;

    [Header("3D")]
    [Range(0, 5)] public float m_DopplerLevel;
    [Range(0, 360)] public float m_Spread;
    public Rolloff m_VolumeRollff;
    public float m_MinDistance;
    public float m_MaxDistance;

    [Header("Clips"), Space]
    public ClipMode m_Mode;
    public AudioClip[] m_Clips;
}

public class AudioManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] SoundList[] m_Sounds;

    private void Start()
    {
        BeginSetUp();
    }

    void BeginSetUp()
    {
        if (m_Sounds.Length < 1)
        {
            return;
        }

        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].m_UseMixer)
            {
                m_Sounds[i].m_Source.clip = m_Sounds[i].m_Clips[0];

                m_Sounds[i].m_Source.mute = m_Sounds[i].m_Mute;
                m_Sounds[i].m_Source.bypassEffects = m_Sounds[i].m_BypassEffects;
                m_Sounds[i].m_Source.bypassListenerEffects = m_Sounds[i].m_BypassListenerEffects;
                m_Sounds[i].m_Source.bypassReverbZones = m_Sounds[i].m_BypassReverbZones;

                m_Sounds[i].m_Source.playOnAwake = m_Sounds[i].m_PlayOnAwake;
                m_Sounds[i].m_Source.loop = m_Sounds[i].m_Loop;
                m_Sounds[i].m_Source.volume = m_Sounds[i].m_Volume;

                m_Sounds[i].m_Source.pitch = m_Sounds[i].m_Pitch;
                m_Sounds[i].m_Source.panStereo = m_Sounds[i].m_StereoPan;
                m_Sounds[i].m_Source.spatialBlend = m_Sounds[i].m_SpatialBlend;

                m_Sounds[i].m_Source.reverbZoneMix = m_Sounds[i].m_ReverbZoneMix;
                m_Sounds[i].m_Source.dopplerLevel = m_Sounds[i].m_DopplerLevel;
                m_Sounds[i].m_Source.spread = m_Sounds[i].m_Spread;

                if (m_Sounds[i].m_VolumeRollff == Rolloff.LogarithmicRolloff)
                {
                    m_Sounds[i].m_Source.rolloffMode = AudioRolloffMode.Logarithmic;
                }
                else
                {
                    m_Sounds[i].m_Source.rolloffMode = AudioRolloffMode.Linear;
                }

                m_Sounds[i].m_Source.minDistance = m_Sounds[i].m_MinDistance;
                m_Sounds[i].m_Source.maxDistance = m_Sounds[i].m_MaxDistance;
            }
            else
            {
                continue;
            }
        }
    }
    public void PlaySound(string _name)
    {
        if (m_Sounds.Length < 1)
        {
            return;
        }

        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].m_Name == _name)
            {
                if (m_Sounds[i].m_Mode == ClipMode.RANDOM)
                {
                    m_Sounds[i].m_Source.clip = m_Sounds[i].m_Clips[Random.Range(0, m_Sounds[i].m_Clips.Length)];
                }
                else
                {
                    m_Sounds[i].m_Source.Play();
                }
            }
        }
    }

    public void PauseSound(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].m_Name == _name)
            {
                m_Sounds[i].m_Source.Pause();
            }
        }
    }

    public void UnPauseSound(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].m_Name == _name)
            {
                m_Sounds[i].m_Source.UnPause();
            }
        }
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < m_Sounds.Length; i++)
        {
            if (m_Sounds[i].m_Name == _name)
            {
                m_Sounds[i].m_Source.Stop();
            }
        }
    }
}
