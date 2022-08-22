using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionEvent : MonoBehaviour
{
    [SerializeField] string m_Tag = "Player";
    [SerializeField] UnityEvent m_OnEnter;

    [Header("Animations")]
    [SerializeField] GameObject m_Target;
    AnimationClip m_AnimationClip;

    GameObject m_gameObject;

    private void Start()
    {
        m_Target.SetActive(false);

        m_gameObject = gameObject.transform.parent.gameObject.GetComponent<GameObject>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == m_Tag)
        {
            if (m_OnEnter != null)
            {
                m_OnEnter.Invoke();
                Destroy(m_gameObject);
            }
        }
    }
}
