using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetector : MonoBehaviour
{
    [SerializeField] HingeJoint m_Joint;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AI")
        {
            m_Joint.useLimits = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AI")
        {
            m_Joint.useLimits = true;
        }
    }
}
