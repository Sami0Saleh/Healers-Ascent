using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string WALK = "Walk";
    const string CRAWL = "Prone Crawl";
    const string PRONE_IDLE = "Prone Idle";
    const string WALK_TO_PRONE = "Walk To Crouch";
    const string PRONE_TO_WALK = "Prone To Crouch";

    private bool isProne;
    private bool isInTransition;

    CustomActions input;

    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;

    public bool IsProne { get => isProne; set => isProne = value; }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        input = new CustomActions();
        AssignInputs();
    }

    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.Stealth.performed += ctx => StealthState();
    }

    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
        {
            agent.destination = hit.point;
            if (clickEffect != null)
            { Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation); }
        }
    }

    void StealthState()
    {
        if (!isProne)
        { isProne = true; isInTransition = true; animator.Play(WALK_TO_PRONE); }
        else if (isProne)
        { isProne = false; isInTransition = true; animator.Play(PRONE_TO_WALK); }
    }

    void TurnOffTransition()
    {
        isInTransition = false;
    }

    void OnEnable()
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }

    void Update()
    {
        FaceTarget();
        SetAnimations();
    }

    void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }

    void SetAnimations()
    {
        if (!isInTransition)
        {
            if (agent.velocity == Vector3.zero && !isProne)
            { animator.Play(IDLE); }
            else if (agent.velocity != Vector3.zero && !isProne)
            { animator.Play(WALK); }
            else if (agent.velocity == Vector3.zero && isProne)
            { animator.Play(PRONE_IDLE); }
            else if (agent.velocity != Vector3.zero && isProne)
            { animator.Play(CRAWL); }
        }
        else
        { Invoke("TurnOffTransition", 5); } // need to find a better fix

    }
}
