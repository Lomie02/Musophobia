using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayer : MonoBehaviour
{
    GameObject m_Player;
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), m_Player.GetComponent<Collider>());
    }
}
