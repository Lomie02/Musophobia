using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class SlidingDoor : MonoBehaviour
{
    Transform m_Location;

    [SerializeField] float m_Min;
    [SerializeField] float m_Max;
    
    [Space]
    
    public GameObject m_PlayerView;
    Vector3 m_NewPos = Vector3.zero;

    float m_ThrowStrength = 50f;
    private bool mIsHolding;
    private bool m_PickingUp;

    //======================================

    private float m_PickUpDistance = 3f;
    private float m_Distance = 3f;
    private float m_MaxDistanceGrab = 4f;

    private GameObject m_TargetObject;
    float m_DoorPickupRange = 5f;
    float m_DoorThrow = 10f;
    float m_DoorDistance = 2f;
    float m_DoorMaxGrab = 5f;

    void Start()
    {
        m_PlayerView = GameObject.FindGameObjectWithTag("MainCamera");
        m_Location = GetComponent<Transform>();

        GetComponent<Rigidbody>().freezeRotation = true;
    }

    void FixedUpdate()
    {
        m_NewPos.x = m_Location.position.x;
        m_NewPos.y = m_Location.position.y;
        m_NewPos.z = m_Location.position.z;

        m_NewPos.z = Mathf.Clamp(m_NewPos.z, m_Min, m_Max);

        m_Location.position = m_NewPos;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!mIsHolding)
            {
                tryPickObject();
                m_PickingUp = true;
            }
            else
            {
                holdObject();
            }
        }
        else if (mIsHolding)
        {
            DropObject();
        }
    }

    private void DropObject()
    {
        mIsHolding = false;
        m_PickingUp = false;
        m_TargetObject.GetComponent<Rigidbody>().useGravity = true;
        m_TargetObject = null;
    }

    private void tryPickObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out hit, 5))
        {
            m_TargetObject = hit.collider.gameObject;
            if (hit.collider.gameObject.GetComponent<SlidingDoor>() && m_PickingUp)
            {
                mIsHolding = true;
                m_TargetObject.GetComponent<Rigidbody>().useGravity = true;
                /**/
                m_PickUpDistance = m_DoorPickupRange;
                m_ThrowStrength = m_DoorThrow;
                m_Distance = m_DoorDistance;
                m_MaxDistanceGrab = m_DoorMaxGrab;
            }
        }
    }

    private void holdObject()
    {
        Ray playerAim = m_PlayerView.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Vector3 nextPos = m_PlayerView.transform.position + playerAim.direction * m_Distance;
        Vector3 currPos = m_TargetObject.transform.position;

        m_TargetObject.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

        if (Vector3.Distance(m_TargetObject.transform.position, m_PlayerView.transform.position) > m_MaxDistanceGrab)
        {
            DropObject();
        }
    }
}
