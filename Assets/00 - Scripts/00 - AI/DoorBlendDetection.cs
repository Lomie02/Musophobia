using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlendDetection : MonoBehaviour
{
    AIModule m_AiAgent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ai" || other.gameObject.tag == "AI")
        {
            m_AiAgent = other.gameObject.GetComponentInParent<AIModule>();
            m_AiAgent.CompressBlendshape();
            Debug.Log("CompressBlend");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ai" || other.gameObject.tag == "AI")
        {
            m_AiAgent.CancelCompression();
            m_AiAgent = null;
            Debug.Log("Stopped Compressing");
        }
    }
}