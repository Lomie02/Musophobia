using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionEvent : MonoBehaviour
{
    [SerializeField, Tooltip("The object that can trigger the event")] string m_Tag = "Player";
    [SerializeField, Tooltip("Called when the tagged object enters the trigger.")] UnityEvent m_OnEnter;

    [Header("Animations")]
    [SerializeField, Tooltip("Used for animation based events only.")] GameObject m_Target;

    //==========================================

    AnimationClip m_AnimationClip;
    Animation m_ANimationComponent;

    public bool m_PlayAnimation = false;
    public float m_Timer;

    //==========================================
    private void Start()
    {
        m_Target.SetActive(false);
        m_ANimationComponent = m_Target.GetComponent<Animation>();
        m_AnimationClip = m_ANimationComponent.clip;

        m_ANimationComponent.playAutomatically = false;
        m_AnimationClip.wrapMode = WrapMode.Default;

        m_Timer = m_AnimationClip.length;

        if (!m_Target)
        {
            m_OnEnter.AddListener(Delete);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_Tag)
        {
            if (m_OnEnter != null)
            {
                m_OnEnter.Invoke();
            }

            if (m_Target)
            {
                if (!m_Target.activeSelf)
                {
                    m_Target.SetActive(true);
                }

                m_ANimationComponent.Play();
                m_PlayAnimation = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_PlayAnimation)
        {
            UpdateAnimation();
        }
    }

    void Delete()
    {
        gameObject.SetActive(false);
    }

    void UpdateAnimation()
    {

        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            m_Target.gameObject.SetActive(false);
            m_Timer = m_AnimationClip.length;
            Delete();
        }
    }
}
