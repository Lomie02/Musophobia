using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField, Tooltip("Object to act as a ligher")] GameObject m_LighterObject;

    [Header("Lighter Events")]
    [SerializeField] UnityEvent m_LighterOn;
    [SerializeField] UnityEvent m_LighterOff;

    ItemManager m_ItemManager;
    RaycastHit m_Ray;
    [SerializeField] Transform m_ItemBox;

    bool m_HoldingItem;
    PlayerCamera m_PlayerView;
    GameManger m_GameManger;

    bool m_CandleOn = true;

    private void Start()
    {
        m_ItemManager = FindObjectOfType<ItemManager>();
        m_GameManger = FindObjectOfType<GameManger>();

        m_PlayerView = FindObjectOfType<PlayerCamera>();
        m_ItemManager.SetItemBox(m_ItemBox);
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

            if (!m_HoldingItem)
            {
                Searchitem();
            }
            else
            {
                SearchKeyDoor();
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && m_HoldingItem)
        {
            CycleMode();
        }

        if (m_HoldingItem)
        {
            m_Ray.collider.gameObject.transform.position = m_ItemBox.position;
            m_Ray.collider.gameObject.transform.parent = m_ItemBox;
        }
    }

    public void Searchitem()
    {
        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out m_Ray, 5))
        {
            if (m_Ray.collider.tag == "Item")
            {
                m_HoldingItem = true;
                m_Ray.collider.transform.forward = m_ItemBox.forward;
                m_Ray.collider.gameObject.transform.position = m_ItemBox.position;


                m_ItemManager.SetObject(m_Ray.collider.gameObject);
            }
        }
    }
     void SearchForDoor()
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
     void SearchKeyDoor()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 3))
        {
            if (cast.collider.gameObject.GetComponent<DoorModule>())
            {
                DoorModule temp;
                temp = cast.collider.gameObject.GetComponent<DoorModule>();

                if (temp.RequestDoorOpen(m_ItemManager.GetKey()))
                {
                    ClearKey();
                }
            }
        }
    }

    void CycleCandle()
    {
        if (m_CandleOn)
        {
            m_LighterOff.Invoke();
            m_CandleOn = false;
        }
        else
        {
            m_LighterOn.Invoke();
            m_CandleOn = true;
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

    private void CycleMode()
    {
        if (m_HoldingItem)
        {
            m_ItemManager.SetNullObject();

            m_Ray.collider.gameObject.transform.parent = null;
            m_ItemManager.ClearVectors();
            m_HoldingItem = false;
        }
        else
        {
            m_HoldingItem = true;
        }
    }

    private void ClearKey()
    {
        if (m_HoldingItem)
        {
            m_ItemManager.DeleteItem();

            m_Ray.collider.gameObject.transform.parent = null;
            m_ItemManager.ClearVectors();
            m_HoldingItem = false;
        }
        else
        {
            m_HoldingItem = true;
        }
    }
}