using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LookAt))]
public class AIModule : MonoBehaviour
{
    enum EnemyStates
    {
        ROAM = 0,
        CHASE,
    }

    enum PlayerSearch
    {
        MANUAL = 0,
        TAG,
    }

    //===========================================

    [Header("Locomotion")]
    [SerializeField] bool m_UseLocomation = false;
    [SerializeField] Vector3 worldDeltaPosition;

    [Space]

    [SerializeField, Tooltip("MANUAL: Player can be assigned by user | TAG: System will automatically search for object with search tag.")] PlayerSearch m_PlayerSearch = PlayerSearch.TAG;
    [SerializeField, Tooltip("Tag to search for. (Requires TAG setting.)")] string m_SearchTag = "Player";
    [SerializeField, Tooltip("The AI will treat this object as the player.")] GameObject m_Player;

    [Header("Detection")]
    [SerializeField, Tooltip("Cooldown bewteen random path finding.")] float m_WonderTime = 5f;
    float m_WonderTimer = 0;
    [SerializeField, Tooltip("How far the AI can see.")] float m_SearchDistance = 10f;
    [SerializeField] float m_KillDistance = 1f;

    [SerializeField, Tooltip("NOTE: Changing this will affect the AI path finding!"), Space()] LayerMask m_SearchLayer;

    [Header("Movement")]
    [SerializeField, Range(3, 50), Tooltip("The speed the AI will roam at.")] float m_RoamSpeed = 1f;
    [SerializeField, Range(0.5f, 100), Tooltip("The speed the AI will chase the Player at.")] float m_ChaseSpeed = 5f;

    //=========================================== AI chase
    [SerializeField, Tooltip("How long the AI will look for the player before returning to roaming.")] float m_InterestTime = 5f;

    [Header("Audio")]
    [SerializeField] UnityEvent m_EnterChase;
    [SerializeField] UnityEvent m_ExitChase;

    [Space]

    [SerializeField] UnityEvent m_EnterRoam;
    [SerializeField] UnityEvent m_ExitRoam;
    //===========================================

    NavMeshAgent m_NavMeshAgent;
    EnemyStates m_AiStates = EnemyStates.ROAM;
    float m_InterestTimer;

    //======================================= Locomotion script

    [SerializeField] Animator m_Animator;
    Vector2 m_Velocity = Vector2.zero;
    Vector2 m_SmooothDeltaPosition = Vector2.zero;

    //======================================= externals
    InputManager m_Input;
    GameManger m_GameManger;
    bool m_IsChasing = false;

    void Start()
    {
        if (m_PlayerSearch == PlayerSearch.TAG)
        {
            if (m_SearchTag != null)
            {
                m_Player = GameObject.FindGameObjectWithTag(m_SearchTag);
            }
            else
            {
                m_SearchTag = "Player";
                m_Player = GameObject.FindGameObjectWithTag(m_SearchTag);
            }
        }

        if (m_UseLocomation)
        {
            m_NavMeshAgent.updatePosition = false;
        }
        m_Input = FindObjectOfType<InputManager>();

        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_WonderTimer = m_WonderTime;
        m_AiStates = EnemyStates.ROAM;

        m_Input = FindObjectOfType<InputManager>();
        m_InterestTimer = m_InterestTime;

        m_GameManger = FindObjectOfType<GameManger>();

        ApplyNavSettings();
        SeekPosition();
    }

    void ApplyNavSettings()
    {
        m_NavMeshAgent.speed = m_RoamSpeed;
    }

    void Update()
    {
        switch (m_AiStates)
        {
            case EnemyStates.ROAM:
                SeekArea();
                break;

            case EnemyStates.CHASE:
                ChasePlayer();
                break;
        }

        if (m_UseLocomation)
        {
            UpdateNav();
        }
        DistanceCheck();

        if (m_UseLocomation)
        {
            if (worldDeltaPosition.magnitude > m_NavMeshAgent.radius)
                transform.position = m_NavMeshAgent.nextPosition - 0.9f * worldDeltaPosition;
        }
    }

    void UpdateNav()
    {
        Vector3 worldDeltaPosition = m_NavMeshAgent.nextPosition - transform.position;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);

        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1.0f, Time.deltaTime);
        m_SmooothDeltaPosition = Vector2.Lerp(m_SmooothDeltaPosition, deltaPosition, smooth);

        if (Time.deltaTime > 1e-5f)
        {
            m_Velocity = m_SmooothDeltaPosition / Time.deltaTime;
        }

        bool shouldMove = m_Velocity.magnitude > 0.5f && m_NavMeshAgent.remainingDistance > m_NavMeshAgent.radius;

        m_Animator.SetBool("Bool", shouldMove);
        m_Animator.SetFloat("velx", m_Velocity.x);
        m_Animator.SetFloat("vely", m_Velocity.y);

        if (m_UseLocomation)
        {
            GetComponent<LookAt>().m_LookAtPosition = m_NavMeshAgent.steeringTarget + transform.forward;
        }
    }

    void OnAnimatorMove()
    {
        // Update position based on animation movement using navigation surface height
        Vector3 position = m_Animator.rootPosition;
        position.y = m_NavMeshAgent.nextPosition.y;
        transform.position = position;
    }

    void ChasePlayer()
    {
        m_NavMeshAgent.SetDestination(m_Player.transform.position);
    }

    void DistanceCheck()
    {
        float Distance = Vector3.Distance(transform.position, m_Player.transform.position);



        Vector3 Direction = m_Player.transform.position - transform.position;

        if (Distance < m_SearchDistance && m_Input.GetCandleState() && m_AiStates != EnemyStates.CHASE)
        {
            m_IsChasing = true;
            SetAiSpeed(m_ChaseSpeed);

            m_ExitRoam.Invoke();
            m_EnterChase.Invoke();
            m_AiStates = EnemyStates.CHASE;
        }

        //=======================================================

        if (m_AiStates == EnemyStates.CHASE)
        {
            RaycastHit deathCast;

            if (Physics.Raycast(transform.position, Direction, out deathCast, m_KillDistance))
            {
                if (deathCast.collider.tag == "Player")
                {
                    m_GameManger.ChangeScene(m_GameManger.GetGameOverName());
                }
            }
        }

        if (m_IsChasing)
        {
            m_InterestTimer -= Time.deltaTime;
            if (m_InterestTimer < 1)
            {

                SetAiSpeed(m_RoamSpeed);
                m_IsChasing = false;

                m_ExitChase.Invoke();
                m_EnterRoam.Invoke();
                m_AiStates = EnemyStates.ROAM;
            }
        }
    }

    public void SetAiSpeed(float _amount)
    {
        m_NavMeshAgent.speed = _amount;
    }

    private void SeekArea()
    {
        m_WonderTimer -= Time.deltaTime;
        if (m_WonderTimer < 1)
        {
            Vector3 NewPositon = RandomPosition(transform.position, m_SearchDistance, m_SearchLayer);
            m_NavMeshAgent.SetDestination(NewPositon);
            m_WonderTimer = m_WonderTime;
        }
    }

    private void SeekPosition()
    {
        Vector3 NewPositon = RandomPosition(transform.position, m_SearchDistance, m_SearchLayer);
        m_NavMeshAgent.SetDestination(NewPositon);
        m_WonderTimer = m_WonderTime;
    }

    private Vector3 RandomPosition(Vector3 _currentPos, float _Distance, int _Layer)
    {
        Vector3 newPos = Random.insideUnitSphere * _Distance;
        newPos += _currentPos;
        NavMeshHit hit;
        NavMesh.SamplePosition(newPos, out hit, _Distance, _Layer);
        return hit.position;
    }
}
