using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Tooltip("Object to act as a ligher")] GameObject m_LighterObject;

    [Header("General")]



    PlayerCamera m_PlayerView;
    GameManger m_GameManger;
    bool m_CandleOn = true;

    private void Start()
    {
        m_GameManger = FindObjectOfType<GameManger>();  
        m_PlayerView = FindObjectOfType<PlayerCamera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CycleCandle();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SearchForDoor();
        }

    }

    public void SearchForDoor()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 3))
        {
            if (cast.collider.tag == "ExitDoor")
            {
                m_GameManger.ChangeScene("Escaped");
            }
        }
    }

    void CycleCandle()
    {
        if (m_LighterObject.activeSelf)
        {
            SetCandle(false);
        }
        else
        {
            SetCandle(true);
        }
    }

    //TODO items that can be picked up.

    void SetCandle(bool _state)
    {
        m_CandleOn = _state;
        m_LighterObject.gameObject.SetActive(_state);
    }

    public bool GetCandleState()
    {
        return m_CandleOn;
    }

}
