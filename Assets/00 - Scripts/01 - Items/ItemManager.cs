using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int m_ItemSlots = 3;
    [Space]
    bool m_UsingRotate = false;

    public GameObject[] m_PhysicalObject;
    int m_CurrentSlot = 0;
    Rigidbody[] m_Rig = null;

    [Header("Rotation")]
    public float m_RotateSpeed = 12;
    float XVal = 0;
    float YVal = 0;

    [Header("Item Box")]
    public Transform m_ItemBox = null;
    ItemIdentifier m_ItemIdentifier = null;

    public Vector3 m_DefaultPosition = Vector3.zero;
    public Vector3 m_ItemBoxPreview = Vector3.zero;

    [Header("Scroll Wheel")]
    public float m_MouseWheelValue = 0;
    public float m_ScrollSpeed = 1.5f;

    public float m_maxMouseValue = 12;
    public float m_minMouseValue = 9;

    KeyIdentifier m_Key = null;
    bool m_HoldingItem;
    bool m_FixRotation = true;

    //============================ Objective Vars

    void Start()
    {
        m_PhysicalObject = new GameObject[m_ItemSlots];
        m_Rig = new Rigidbody[m_PhysicalObject.Length];

        m_ItemBox.localPosition = m_DefaultPosition;
    }

    public bool IsKeySlot()
    {
        if (m_PhysicalObject[m_CurrentSlot].GetComponent<KeyIdentifier>() && m_PhysicalObject[m_CurrentSlot] != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetRotationState()
    {
        return m_UsingRotate;
    }

    void Update()
    {
        if (m_PhysicalObject[m_CurrentSlot] && m_UsingRotate)
        {
            float MX = Input.GetAxis("Mouse X") * m_RotateSpeed * Time.deltaTime;
            float MY = Input.GetAxis("Mouse Y") * m_RotateSpeed * Time.deltaTime;

            XVal = MY;
            YVal = MX;

            m_PhysicalObject[m_CurrentSlot].transform.Rotate(XVal, YVal, 0);
            float XM = Input.mouseScrollDelta.y * m_ScrollSpeed * Time.deltaTime;

            m_MouseWheelValue += XM;
            m_MouseWheelValue = Mathf.Clamp(m_MouseWheelValue, m_minMouseValue, m_maxMouseValue);

            m_ItemBoxPreview.z = m_MouseWheelValue;
            m_ItemBox.localPosition = m_ItemBoxPreview;
            m_FixRotation = false;
        }
        else
        {
            if (!m_FixRotation)
            {
                m_Rig[m_CurrentSlot].transform.localRotation = m_ItemBox.localRotation;
                m_FixRotation = true;
            }

            m_ItemBox.localPosition = m_DefaultPosition;
        }
    }

    public void SetUsingState(bool _State)
    {
        m_UsingRotate = _State;
    }

    public bool GetUsingState()
    {
        return m_UsingRotate;
    }

    public void SetObject(GameObject _Object)
    {
        m_PhysicalObject[m_CurrentSlot] = _Object;

        m_PhysicalObject[m_CurrentSlot].GetComponent<Collider>().enabled = false;

        m_ItemIdentifier = _Object.GetComponent<ItemIdentifier>();
        m_ItemIdentifier.PickUpSound();

        if (m_PhysicalObject[m_CurrentSlot].gameObject.GetComponent<Rigidbody>() != null)
        {
            m_Rig[m_CurrentSlot] = m_PhysicalObject[m_CurrentSlot].gameObject.GetComponent<Rigidbody>();
            m_Rig[m_CurrentSlot].isKinematic = true;
            m_Rig[m_CurrentSlot].useGravity = false;
        }

        if (!m_PhysicalObject[m_CurrentSlot].gameObject.activeSelf)
        {
            m_PhysicalObject[m_CurrentSlot].gameObject.SetActive(true);
        }

        //=====================================
        m_PhysicalObject[m_CurrentSlot].layer = 3;
        for (int i = 0; i < m_PhysicalObject[m_CurrentSlot].transform.childCount; i++)
        {
            m_PhysicalObject[m_CurrentSlot].transform.GetChild(i).gameObject.layer = 3;
        }

        //=====================================


        if (m_PhysicalObject[m_CurrentSlot].gameObject.GetComponent<KeyIdentifier>() != null)
        {
            m_Key = m_PhysicalObject[m_CurrentSlot].gameObject.GetComponent<KeyIdentifier>();
        }


    }

    public string GetCurrentItemName()
    {
        return m_ItemIdentifier.GetName();
    }

    public void CycleSlots()
    {
        if (m_PhysicalObject[m_CurrentSlot] != null)
        {
            m_PhysicalObject[m_CurrentSlot].gameObject.SetActive(false);
        }

        m_CurrentSlot++;

        if (m_CurrentSlot >= m_PhysicalObject.Length)
        {
            m_CurrentSlot = 0;
        }

        if (m_PhysicalObject[m_CurrentSlot] != null)
        {
            if (m_PhysicalObject[m_CurrentSlot].GetComponent<ItemIdentifier>() != null)
            {
                m_ItemIdentifier = m_PhysicalObject[m_CurrentSlot].GetComponent<ItemIdentifier>();
            }

            m_PhysicalObject[m_CurrentSlot].gameObject.SetActive(true);
        }
        else
        {
            m_ItemIdentifier = null;
        }
    }

    public bool GetCurrentSlot()
    {
        if (m_PhysicalObject[m_CurrentSlot] != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public KeyIdentifier GetKey()
    {
        return m_Key;
    }

    public void SetNullObject()
    {
        YVal = 0;
        XVal = 0;

        //=====================================
        m_PhysicalObject[m_CurrentSlot].layer = 0;
        for (int i = 0; i < m_PhysicalObject[m_CurrentSlot].transform.childCount; i++)
        {
            m_PhysicalObject[m_CurrentSlot].transform.GetChild(i).gameObject.layer = 0;
        }

        //=====================================

        if (!m_PhysicalObject[m_CurrentSlot].gameObject.activeSelf)
        {
            m_PhysicalObject[m_CurrentSlot].gameObject.SetActive(true);
        }


        if (m_Key)
        {
            m_Key = null;
        }

        m_ItemIdentifier.DroppedSound();
        m_ItemIdentifier = null;

        m_Rig[m_CurrentSlot].isKinematic = false;
        m_Rig[m_CurrentSlot].useGravity = true;

        m_PhysicalObject[m_CurrentSlot].GetComponent<Collider>().enabled = true;

        m_PhysicalObject[m_CurrentSlot].transform.parent = null;

        m_Rig[m_CurrentSlot] = null;
        m_PhysicalObject[m_CurrentSlot] = null;
    }

    public void DeleteItem()
    {
        m_Key = null;
        m_Rig[m_CurrentSlot] = null;

        Destroy(m_PhysicalObject[m_CurrentSlot]);
        m_PhysicalObject[m_CurrentSlot] = null;
    }

    public void ClearVectors()
    {
        m_MouseWheelValue = 0;
        m_ItemBoxPreview = m_DefaultPosition;
        m_ItemBox.localPosition = m_DefaultPosition;
    }

    public bool GetTransformStat()
    {
        if (m_PhysicalObject == null)
        {
            return false;
        }

        return true;
    }

    public void SetItemBox(Transform _ItemBox)
    {
        m_ItemBox = _ItemBox;
    }

    public void CyclePower()
    {
        if (m_ItemIdentifier)
        {
            m_ItemIdentifier.CycleState();
        }
    }

    public void Use()
    {
        if (m_ItemIdentifier)
        {
            m_ItemIdentifier.UseItem();
        }
    }
}