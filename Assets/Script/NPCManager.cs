using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;

public enum NPCState
{
    Idle,
    Walk,
    Think,
    Bravo
}


public class NPCManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    [TextArea(3, 10)]
    [SerializeField] private List<string> artworkDescription;
    [SerializeField] private string waitingMessage;
    [SerializeField] private string startMessage;

    private Vector3 initialPosition;
    private float maxDistanceNPCandPlayer = 4.3f;
    private float minDistanceNPCandPlayer = 1.3f;
    private NPCState currentState = NPCState.Idle;
    private bool isProcessingArtwork = false;
    private bool isStopped = false;
    private bool isRotating = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position;
        text.text = "";
    }
    void Update()
    {
        if (isProcessingArtwork)
        {
            if (isRotating)
            {
                
                RotateTowardsEuler(ArtworkManager.instance.GetArtworkRotation(ArtworkManager.instance.currentIndex));
            }
            return;
        }

        if (isStopped && agent.isStopped) 
        {
            agent.isStopped = false;
            text.text = "";
        }


        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (currentState == NPCState.Walk)
        {

            if (distanceToPlayer > maxDistanceNPCandPlayer && !isStopped)
            {
                agent.isStopped = true;
                animator.SetFloat("speed", 0f);
                text.text = waitingMessage;
                return;
            }
            
            if (distanceToPlayer < minDistanceNPCandPlayer)
            {
                agent.isStopped = false;
                text.text = "";
            }

            animator.SetFloat("speed", agent.velocity.magnitude);
        }
        
        
        if (currentState == NPCState.Bravo && !isStopped)
        {
            if (ArtworkManager.instance.currentIndex == ArtworkManager.instance.totalArtworks - 1)
            {
                StopGuidedTour();
                return;
            }
            ArtworkManager.instance.NextArtwork();
            MoveToNextArtwork();
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance 
            && Vector3.Distance(transform.position, initialPosition) > 5f && !isStopped)
        {
            StartCoroutine(ArtworkSequence());
        }
    }

    public void StartGuidedTour()
    {
        isStopped = false;
        StartCoroutine(WaitAndGoToFirstArtwork());
    }

    private IEnumerator WaitAndGoToFirstArtwork()
    {
        animator.SetFloat("speed", 0f);
        text.text = startMessage;

        yield return new WaitForSeconds(2f);

        MoveToNextArtwork();
    }


    public void StopGuidedTour()
    {
        isStopped = true;

        currentState = NPCState.Walk;
        agent.SetDestination(initialPosition);
        ArtworkManager.instance.ResetTour();
    }

    private void MoveToNextArtwork()
    {
        currentState = NPCState.Walk;
        isStopped = false;
        // isRotating = true;
        text.text = "";

        Vector3 nextArtworkPosition = ArtworkManager.instance.GetArtworkPosition(ArtworkManager.instance.currentIndex);
        if (nextArtworkPosition != Vector3.zero)
        {
            agent.SetDestination(nextArtworkPosition);
        }
        else
        {
            currentState = NPCState.Idle;
            agent.SetDestination(initialPosition);
        }
    }

    private IEnumerator ArtworkSequence()
    {
        if (isProcessingArtwork) yield break;
        isProcessingArtwork = true;

        agent.isStopped = true;
        animator.SetFloat("speed", 0f);
        isRotating = true;

        yield return new WaitForSeconds(1f);


        // thinking
        currentState = NPCState.Think;
        animator.SetBool("isThinking", true);
        animator.SetBool("isBravo", false);
        text.text = artworkDescription[UnityEngine.Random.Range(0, artworkDescription.Count)];
        yield return new WaitForSeconds(6f);

        // bravo
        currentState = NPCState.Bravo;
        animator.SetBool("isThinking", false);
        animator.SetBool("isBravo", true);
        yield return new WaitForSeconds(3f);

        // reset
        animator.SetBool("isBravo", false);
        text.text = "";
        agent.isStopped = false;
        isProcessingArtwork = false;
        yield return new WaitForSeconds(1f);

        animator.SetFloat("speed", agent.velocity.magnitude);
    }

private void RotateTowardsEuler(Vector3 targetEulerAngles)
{
    Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

    // Mượt dần từ rotation hiện tại tới target
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

    // Kiểm tra gần tới target chưa
    if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
    {
        isRotating = false;
        transform.rotation = targetRotation; // đảm bảo chính xác
    }
}

}