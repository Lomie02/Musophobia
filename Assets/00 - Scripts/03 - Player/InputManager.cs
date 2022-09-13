using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [Header("Lighter Events")]
    [SerializeField] UnityEvent m_LighterOn;
    [SerializeField] UnityEvent m_LighterOff;

    ItemManager m_ItemManager;
    [SerializeField] Transform m_ItemBox;

    bool m_HoldingItem;
    PlayerCamera m_PlayerView;
    GameManger m_GameManger;

    bool m_CandleOn = false;
    bool m_IsInspecting = false;

    [SerializeField] GameObject m_MessageDrawer = null;

    private void Start()
    {
        m_ItemManager = FindObjectOfType<ItemManager>();
        m_GameManger = FindObjectOfType<GameManger>();

        m_PlayerView = FindObjectOfType<PlayerCamera>();
        m_ItemManager.SetItemBox(m_ItemBox);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CycleCandle();

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (m_ItemManager.GetCurrentSlot())
            {
                m_ItemManager.CyclePower();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && m_ItemManager.GetCurrentSlot())
        {
            CycleInspect();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SearchForDoor();
            SearchDrawer();

            SearchKeyDoor();
            SearchForNote();

            m_ItemManager.Use();

            if (!m_ItemManager.GetCurrentSlot())
            {
                Searchitem();
            }

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_ItemManager.CycleSlots();
        }

        if (Input.GetKeyDown(KeyCode.G) && m_ItemManager.GetCurrentSlot())
        {
            if (m_IsInspecting)
            {
                m_IsInspecting = false;
                m_PlayerView.SetPlayerState(true);
                m_ItemManager.SetUsingState(false);
            }

            m_ItemManager.SetNullObject();

            m_ItemManager.ClearVectors();
        }

        ScanForDrawer();
    }

    public void Searchitem()
    {
        RaycastHit cast;
        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 5))
        {
            if (cast.collider.tag == "Item")
            {
                cast.collider.gameObject.transform.parent = null;
                cast.collider.transform.forward = m_ItemBox.forward;

                cast.collider.gameObject.transform.position = m_ItemBox.position;
                cast.collider.gameObject.transform.parent = m_ItemBox;

                m_ItemManager.SetObject(cast.collider.gameObject);
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
            if (cast.collider.gameObject.GetComponent<DoorModule>() != null)
            {
                DoorModule temp;
                temp = cast.collider.gameObject.GetComponent<DoorModule>();

                if (temp.RequestDoorOpen(m_ItemManager.GetKey()))
                {
                    m_ItemManager.DeleteItem();
                    m_ItemManager.ClearVectors();
                }
            }
        }
    }

    void SearchForCandle()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 3))
        {
            if (cast.collider.gameObject.GetComponent<CandleIdentifier>())
            {
                CandleIdentifier temp;
                temp = cast.collider.gameObject.GetComponent<CandleIdentifier>();

                temp.LightCandle();
            }
        }
    }

    void SearchForNote()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 3))
        {
            if (cast.collider.gameObject.GetComponent<NoteIdentifier>())
            {
                NoteIdentifier temp;
                temp = cast.collider.gameObject.GetComponent<NoteIdentifier>();

                temp.ReadNote();
            }
        }
    }

    void SearchDrawer()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 3))
        {
            if (cast.collider.gameObject.GetComponent<DrawerModule>())
            {
                DrawerModule temp;
                temp = cast.collider.gameObject.GetComponent<DrawerModule>();
                temp.CycleState();
            }
        }
    }

    void ScanForDrawer()
    {
        RaycastHit cast;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out cast, 3))
        {
            if (cast.collider.gameObject.tag == "Interact")
            {
                if (m_MessageDrawer)
                {
                    m_MessageDrawer.SetActive(true);
                }
            }
            else
            {
                if (m_MessageDrawer)
                {
                    m_MessageDrawer.SetActive(false);
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

    public bool GetCandleState()
    {
        return m_CandleOn;
    }

    private void ClearKey()
    {
        if (m_HoldingItem)
        {
        }
        else
        {
            m_HoldingItem = true;
        }
    }

    void CycleInspect()
    {
        if (m_IsInspecting)
        {
            m_PlayerView.SetPlayerState(true);
            m_ItemManager.SetUsingState(false);
            m_IsInspecting = false;
        }
        else
        {
            m_PlayerView.SetPlayerState(false);
            m_ItemManager.SetUsingState(true);
            m_IsInspecting = true;
        }
    }
}