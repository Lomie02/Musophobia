using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

enum ConfigMode
{
    Auto = 0,
    Manually
}
enum DoorMode
{
    Collision = 0,
    Swing,
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshObstacle))]
public class DoorModule : MonoBehaviour
{
    [Header("General")]
    [SerializeField] ConfigMode m_Config = ConfigMode.Auto;
    [SerializeField] DoorMode m_Mode = DoorMode.Collision;

    [SerializeField] int m_DoorId;
    [SerializeField] bool m_LockedStart = true;

    [Header("Events")]
    [SerializeField, Space] UnityEvent m_OnDoorUnlocked;
    [SerializeField, Space] UnityEvent m_OnIncorrectKey;

    bool m_IsLocked = true;
    HingeJoint m_DoorJoint;

    //=============================================

    public GameObject m_PlayerView;

    private float m_PickUpDistance = 3f;
    private float m_Distance = 3f;
    private float m_MaxDistanceGrab = 4f;

    private GameObject m_TargetObject;
    float m_ThrowStrength = 50f;
    private bool mIsHolding;
    private bool m_PickingUp;

    //======================================

    float m_DoorPickupRange = 2f;
    float m_DoorThrow = 5f;
    float m_DoorDistance = 1f;
    float m_DoorMaxGrab = 2f;

    Vector3 m_PreviousPos;
    [SerializeField] AudioSource m_DoorMovingSound = null;
    InputManager m_InputManager;

    //================= 
    public bool RequestDoorOpen(KeyIdentifier _Id)
    {
        if (_Id == null) { return false; }

        if (m_IsLocked && _Id.GetKeyID() == m_DoorId)
        {
            m_IsLocked = false;

            if (m_OnDoorUnlocked != null)
            {
                m_OnDoorUnlocked.Invoke();
            }

            if (m_Config == ConfigMode.Auto)
            {
                m_Mode = DoorMode.Swing;
                GetComponent<NavMeshObstacle>().carving = false;
                GetComponent<NavMeshObstacle>().enabled = false;
            }

            return true;
        }
        else
        {

            if (m_OnIncorrectKey != null)
            {
                m_OnIncorrectKey.Invoke();
            }
            return false;
        }
    }

    public bool GetLockState()
    {
        return m_IsLocked;
    }

    public void SetLockState(bool _state)
    {
        m_IsLocked = _state;
    }
    void Start()
    {
        m_DoorJoint = GetComponent<HingeJoint>();
        m_InputManager = FindObjectOfType<InputManager>();
        if (m_LockedStart)
        {
            SetLockState(m_LockedStart);
        }
        else
        {
            SetLockState(m_LockedStart);
        }

        if (!m_DoorJoint)
        {
            Debug.LogError("Door: " + gameObject.name + " has no hinge & will not use hinge joints.");
        }

        mIsHolding = false;
        m_PickingUp = false;
        m_TargetObject = null;

        if (m_DoorMovingSound)
        {
            m_DoorMovingSound.loop = true;
        }

        if (m_Config == ConfigMode.Auto)
        {
            if (m_LockedStart)
            {
                m_Mode = DoorMode.Collision;
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<HingeJoint>().useLimits = true;
                GetComponent<NavMeshObstacle>().carving = true;
            }
            else
            {
                m_Mode = DoorMode.Swing;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<HingeJoint>().useLimits = true;
                GetComponent<NavMeshObstacle>().carving = false;
                GetComponent<NavMeshObstacle>().enabled = false;
            }
        }

        GetComponent<NavMeshObstacle>().center = GetComponent<BoxCollider>().center;
        GetComponent<NavMeshObstacle>().size = GetComponent<BoxCollider>().size;



        if (m_Mode == DoorMode.Collision && m_Config == ConfigMode.Manually)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<HingeJoint>().useLimits = true;
            GetComponent<NavMeshObstacle>().carving = true;
            GetComponent<NavMeshObstacle>().enabled = true;
        }
        else if(m_Mode == DoorMode.Swing && m_Config == ConfigMode.Manually)
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<HingeJoint>().useLimits = false;
            SetLockState(false);
            GetComponent<NavMeshObstacle>().carving = false;
            GetComponent<NavMeshObstacle>().enabled = false;
        }

        m_PlayerView = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void FixedUpdate()
    {
        if (m_IsLocked == false)
        {
            if (Input.GetKey(m_InputManager.GetDoorBind()) && !m_IsLocked)
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


        if (gameObject.GetComponent<Rigidbody>().rotation.y != m_PreviousPos.y)
        {
            if (m_DoorMovingSound)
            {
                m_DoorMovingSound.UnPause();
            }

            m_PreviousPos.y = gameObject.GetComponent<Rigidbody>().rotation.y;
        }
        else
        {
            if (m_DoorMovingSound)
            {
                m_DoorMovingSound.Pause();
            }
        }
    }

    private void tryPickObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_PlayerView.transform.position, m_PlayerView.transform.forward, out hit, m_PickUpDistance))
        {
            m_TargetObject = hit.collider.gameObject;
            if (hit.collider.GetComponent<DoorModule>() && m_PickingUp && hit.collider.GetComponent<DoorModule>().m_IsLocked == false)
            {
                mIsHolding = true;

                m_TargetObject.GetComponent<Rigidbody>().isKinematic = false;
                m_TargetObject.GetComponent<Rigidbody>().useGravity = true;
                m_TargetObject.GetComponent<Rigidbody>().freezeRotation = false;
                /**/
                m_PickUpDistance = m_DoorPickupRange;
                m_ThrowStrength = m_DoorThrow;
                m_Distance = m_DoorDistance;
                m_MaxDistanceGrab = m_DoorMaxGrab;
            }
            else
            {
                return;
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

    private void DropObject()
    {
        mIsHolding = false;
        m_PickingUp = false;

        if (m_Mode == DoorMode.Collision)
        {
            m_TargetObject.GetComponent<Rigidbody>().useGravity = false;
            m_TargetObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (m_Mode == DoorMode.Swing)
        {
            m_TargetObject.GetComponent<Rigidbody>().useGravity = true;
            m_TargetObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        m_TargetObject.GetComponent<Rigidbody>().freezeRotation = false;
        m_TargetObject = null;
    }
}