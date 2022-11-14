using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemIdentifier : MonoBehaviour
{
    [Header("General")]
    [SerializeField] string m_ItemName = "empty";
    [SerializeField] UnityEvent m_TurnOn;
    [SerializeField] UnityEvent m_TurnOff;
    [SerializeField] UnityEvent m_OnUse;

    [Header("Audio")]

    [SerializeField] UnityEvent m_OnPickedUp;
    [SerializeField] UnityEvent m_OnDroppedItem;

    [Header("Animation")]
    [SerializeField] bool m_IsLighter = false;
    [SerializeField] Animator m_Animator;

    AnimatorClipInfo[] m_Clips;
    float m_AnimationTimer = 0;
    bool m_AnimationPlaying = false;

    bool m_OnState = false;


    private void Start()
    {
        if (m_Animator)
        {
            m_Clips = m_Animator.GetCurrentAnimatorClipInfo(0);
        }
    }
    public void UseItem()
    {
        if (m_OnUse != null && m_OnState)
        {
            m_OnUse.Invoke();
        }

        if (m_IsLighter && m_Animator)
        {
            if (!m_AnimationPlaying)
            {
                m_AnimationPlaying = true;
                m_Animator.SetTrigger("Use");
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_AnimationPlaying)
        {
            m_AnimationTimer += Time.deltaTime;
            if (m_AnimationTimer >= 3)
            {
                m_AnimationTimer = 0;
                m_AnimationPlaying = false;
            }
        }
    }

    public void TurnOn()
    {
        if (m_TurnOn != null)
        {
            m_TurnOn.Invoke();
        }
    }
    public string GetName()
    {
        return m_ItemName;
    }

    public void TurnOff()
    {
        if (m_TurnOff != null)
        {
            m_TurnOff.Invoke();
        }
    }

    public void CycleState()
    {
        if (m_OnState)
        {
            TurnOff();
            m_OnState = false;
        }
        else
        {
            TurnOn();
            m_OnState = true;
        }
    }

    public void PickUpSound()
    {
        m_OnPickedUp.Invoke();
    }

    public void DroppedSound()
    {
        m_OnDroppedItem.Invoke();
    }
}