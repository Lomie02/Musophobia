using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("General")]
    public bool m_UsingRotate = false;
    public GameObject m_PhysicalObject = null;
    public Rigidbody m_Rig = null;

    [Header("Rotation")]
    public float m_RotateSpeed = 12;
    public float XVal = 0;
    public float YVal = 0;

    Transform m_ItemBox = null;
    [Header("Item Box")]
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

    void Update()
    {
        if (m_PhysicalObject && m_UsingRotate)
        {
            float MX = Input.GetAxis("Mouse X") * m_RotateSpeed * Time.deltaTime;
            float MY = Input.GetAxis("Mouse Y") * m_RotateSpeed * Time.deltaTime;

            XVal += MX;
            YVal -= MY;

            m_PhysicalObject.transform.Rotate(XVal, YVal, 0);
            float XM = Input.mouseScrollDelta.y * m_ScrollSpeed * Time.deltaTime;

            m_MouseWheelValue += XM;
            m_MouseWheelValue = Mathf.Clamp(m_MouseWheelValue, m_minMouseValue, m_maxMouseValue);

            m_ItemBoxPreview.z = m_MouseWheelValue;
            m_ItemBox.localPosition = m_ItemBoxPreview;
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
        m_PhysicalObject = _Object;

        m_PhysicalObject.GetComponent<Collider>().enabled = false;

        if (m_PhysicalObject.gameObject.GetComponent<KeyIdentifier>() != null)
        {
            m_Key = m_PhysicalObject.gameObject.GetComponent<KeyIdentifier>();
            m_PhysicalObject.gameObject.GetComponent<Renderer>().enabled = false;
        }

        if (m_PhysicalObject.gameObject.GetComponent<Rigidbody>() != null)
        {
            m_Rig = m_PhysicalObject.gameObject.GetComponent<Rigidbody>();
            m_Rig.isKinematic = true;
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

        if (m_Key)
        {
            m_PhysicalObject.gameObject.GetComponent<Renderer>().enabled = true;
            m_Key = null;
        }

        m_Rig.isKinematic = false;
        m_PhysicalObject.GetComponent<Collider>().enabled = true;

        m_Rig = null;
        m_PhysicalObject = null;
    }

    public void DeleteItem()
    {
        Destroy(m_PhysicalObject);
        m_Key = null;
        m_Rig = null;
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
}
