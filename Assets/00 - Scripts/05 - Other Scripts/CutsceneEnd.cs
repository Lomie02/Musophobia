using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    [SerializeField]
    private GameObject TurnOn1;
    [SerializeField]
    private GameObject TurnOff1;
    [SerializeField]
    private GameObject TurnOff2;



    private void OnEnable()
    {
        TurnOn1.SetActive(true);
        TurnOff1.SetActive(false);
        TurnOff2.SetActive(false);
    }
}
