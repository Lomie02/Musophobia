using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerModule : MonoBehaviour
{
    [SerializeField] Animator m_Animator;
    [SerializeField] bool m_OpenState = false;
    [SerializeField] GameObject m_Icon;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void CycleState()
    {
        if (m_OpenState)
        {
            m_OpenState = false;
            UpdateAnimator();
        }
        else
        {
            m_OpenState = true;
            UpdateAnimator();
        }
    }

    private void UpdateAnimator()
    {
        m_Animator.SetBool("State", m_OpenState);
    }
}
