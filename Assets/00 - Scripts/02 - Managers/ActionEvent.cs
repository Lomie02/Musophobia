using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


enum AnimationMode
{
    AnimationLegacy = 0,
    Animator,
}
public class ActionEvent : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField, Tooltip("An object that can trigger the event")] string m_Tag = "Player";
    [Space]
    [SerializeField] bool m_UseBothTags = false;
    [SerializeField, Tooltip("An object that can trigger the event")] string m_Tag2 = "Other";

    [Space]

    [Header("AI Settings")]
    [SerializeField, Tooltip("The AI object")] GameObject m_TargetObject = null;
    [SerializeField, Range(0, 15), Tooltip("How low does the distance need to be to register an Event.")] float m_ActivateDistance = 1f;

    [Space]

    [Header("Animations")]

    [SerializeField] AnimationMode m_Version = AnimationMode.AnimationLegacy;
    [SerializeField, Tooltip("For animation driven events. Must match the version that is selected")] GameObject m_TargetAnimation;

    [Space]
    [SerializeField, Tooltip("Called when the tagged object enters the trigger.")] UnityEvent m_OnEnter;


    //==========================================

    AnimationClip m_AnimationClip;
    AnimatorClipInfo[] m_AnimatorClipArray;
    AnimationClip m_AnimatorClips;

    Animation m_ANimationComponent;

    Animator m_Animator;

    bool m_DistanceCheck = false;

    bool m_PlayAnimation = false;
    float m_Timer;

    //==========================================
    private void Start()
    {
        if (m_Version == AnimationMode.AnimationLegacy && m_TargetAnimation)
        {
            m_TargetAnimation.SetActive(false);
            m_ANimationComponent = m_TargetAnimation.GetComponent<Animation>();

            m_AnimationClip = m_ANimationComponent.clip;

            m_ANimationComponent.playAutomatically = false;
            m_AnimationClip.wrapMode = WrapMode.Default;

            m_Timer = m_AnimationClip.length;
        }

        if (m_Version == AnimationMode.Animator && m_TargetAnimation)
        {
            m_TargetAnimation.SetActive(false);
            m_Animator = m_TargetAnimation.GetComponent<Animator>();

            m_AnimatorClipArray = m_Animator.GetCurrentAnimatorClipInfo(0);

            m_AnimatorClips = m_AnimatorClipArray[0].clip;

            m_AnimationClip.wrapMode = WrapMode.Default;
            m_Timer = m_AnimationClip.length;
        }


        if (m_TargetObject)
        {
            m_DistanceCheck = true;
        }

        if (!m_TargetAnimation)
        {
            m_OnEnter.AddListener(Delete);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_Tag || other.gameObject.tag == m_Tag2 && m_UseBothTags)
        {
            if (m_OnEnter != null)
            {
                m_OnEnter.Invoke();
            }

            if (m_Version == AnimationMode.AnimationLegacy && m_TargetAnimation)
            {
                if (!m_TargetAnimation.activeSelf)
                {
                    m_TargetAnimation.SetActive(true);
                }

                m_ANimationComponent.Play();
                m_PlayAnimation = true;
            }

            if (m_Version == AnimationMode.Animator && m_TargetAnimation)
            {
                if (!m_TargetAnimation.activeSelf)
                {
                    m_TargetAnimation.SetActive(true);
                }

                m_PlayAnimation = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_PlayAnimation)
        {
            if (m_Version == AnimationMode.Animator)
            {
                UpdateAnimator();
            }
            else
            {
                UpdateAnimation();
            }
        }
        if (m_DistanceCheck)
        {
            float distance = Vector3.Distance(transform.position, m_TargetObject.transform.position);

            if (distance <= m_ActivateDistance)
            {
                if (m_OnEnter != null)
                {
                    m_OnEnter.Invoke();
                    m_DistanceCheck = false;
                }
            }
        }
    }

    void Delete()
    {
        gameObject.SetActive(false);
    }
    void UpdateAnimator()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            m_TargetAnimation.gameObject.SetActive(false);
            m_Timer = m_AnimationClip.length;
            Delete();
        }
    }
    void UpdateAnimation()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            m_TargetAnimation.gameObject.SetActive(false);
            m_Timer = m_AnimationClip.length;
            Delete();
        }
    }
}
