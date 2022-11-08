using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSystem : MonoBehaviour
{
    //==================================================== Mouse Sensitivity 
    public void SetMouseSens(float _amount)
    {
        PlayerPrefs.SetFloat("player_mouse", _amount);
    }

    public float GetMouseSens()
    {
        if (PlayerPrefs.HasKey("player_mouse"))
        {
            return PlayerPrefs.GetFloat("player_mouse");
        }
        else
        {
            return 300f;
        }
    }

    //==================================================== Audio 

    public void SetAudioLevel(float _amount)
    {
        PlayerPrefs.SetFloat("player_audio", _amount);
    }

    public float GetAudioLevel()
    {
        if (PlayerPrefs.HasKey("player_audio"))
        {
            return PlayerPrefs.GetFloat("player_audio");
        }
        else
        {
            return 40f;
        }
    }

    //==================================================== Field of view
    public void SetFieldOfView(float _amount)
    {
        PlayerPrefs.SetFloat("player_fov", _amount);
    }

    public float GetFieldOfView()
    {
        return PlayerPrefs.GetFloat("player_fov");
    }

    public void SetMusicLevel(float _amount)
    {
        PlayerPrefs.SetFloat("music_audio", _amount);
    }

    public float GetMusicLevel()
    {
        if (PlayerPrefs.HasKey("music_audio"))
        {
            return PlayerPrefs.GetFloat("music_audio");
        }
        else
        {
            return 40f;
        }
    }

    //====================================

    public void SetResolution(int _index)
    {
        PlayerPrefs.SetInt("resolution", _index);
    }

    public int GetResolution()
    {
        if (PlayerPrefs.HasKey("resolution"))
        {
            return PlayerPrefs.GetInt("resolution");
        }
        else
        {
            return 1;
        }
    }
}