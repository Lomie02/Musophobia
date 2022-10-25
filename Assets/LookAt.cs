using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class LookAt : MonoBehaviour
{
    [SerializeField] Transform m_Head = null;
    public Vector3 m_LookAtPosition;
    [SerializeField] float m_LookAtCoolTime = 0.2f;
    [SerializeField] float m_LookAtHeadTime = 0.2f;
    [SerializeField] bool m_Looking = true;

    Vector3 lookAtPosition;
    Animator animator;
    float lookAtWeight = 0.0f;

    void Start()
    {
        if (!m_Head)
        {
            enabled = false;
            return;
        }
        animator = GetComponent<Animator>();
        m_LookAtPosition = m_Head.position + transform.forward;
        lookAtPosition = m_LookAtPosition;
    }

    void OnAnimatorIK()
    {
        m_LookAtPosition.y = m_Head.position.y;
        float lookAtTargetWeight = m_Looking ? 1.0f : 0.0f;

        Vector3 curDir = lookAtPosition - m_Head.position;
        Vector3 futDir = m_LookAtPosition - m_Head.position;

        curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
        lookAtPosition = m_Head.position + curDir;

        float blendTime = lookAtTargetWeight > lookAtWeight ? m_LookAtHeadTime : m_LookAtCoolTime;
        lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
        animator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        animator.SetLookAtPosition(lookAtPosition);
    }
}