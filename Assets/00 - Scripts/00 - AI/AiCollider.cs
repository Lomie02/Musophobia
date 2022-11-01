using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Modes
{
    Yes = 0,
    No,
}

[DisallowMultipleComponent]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class AiCollider : MonoBehaviour
{
    [SerializeField] Modes m_AutomaticSetup = Modes.Yes;

    CapsuleCollider m_CapsuleCollider;
    Vector3 m_CentreIndex = new Vector3(-0.03302122f, -0.269552f, -0.01960879f);
    float m_Radius = 0.3587099f;
    float m_Height = 1.445533f;
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_CapsuleCollider.isTrigger = true;

        if (m_AutomaticSetup == Modes.Yes)
        {
            m_CapsuleCollider.center = m_CentreIndex;
            m_CapsuleCollider.height = m_Height;
            m_CapsuleCollider.radius = m_Radius;
        }
    }
}
