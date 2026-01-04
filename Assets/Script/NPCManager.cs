using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

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
    public Animator animator;

    [TextArea(3, 10)]
    [SerializeField] private List<string> artworkDescription;
    [SerializeField] private string waitingMessage;
    [SerializeField] private string startMessage;

    private Vector3 initialPosition;
    private float maxDistanceNPCandPlayer = 4.3f;
    private float minDistanceNPCandPlayer = 1.3f;
    public NPCState currentState = NPCState.Idle;
    public bool isProcessingArtwork = false;
    public bool isStopped = false;

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
        if (isProcessingArtwork) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // if (currentState == NPCState.Bravo && !isProcessingArtwork)
        // {
        //     ArtworkManager.instance.NextArtwork();
        //     Debug.Log("index after bravo: " + ArtworkManager.instance.currentIndex);
        //     MoveToNextArtwork();
        // }
        if (currentState == NPCState.Walk && !isStopped)
        {

            if (distanceToPlayer > maxDistanceNPCandPlayer)
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
        
        
        if (currentState == NPCState.Bravo)
        {
            ArtworkManager.instance.NextArtwork();
            MoveToNextArtwork();
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance 
            && Vector3.Distance(transform.position, initialPosition) > 1f)
        {
            StartCoroutine(ArtworkSequence());
        }

        // if (currentState == NPCState.Bravo && distanceToPlayer < maxDistanceNPCandPlayer)
        // {

        // }
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
        Debug.Log("Stop Guided Tour");
        currentState = NPCState.Walk;
        agent.SetDestination(initialPosition);
        ArtworkManager.instance.ResetTour();
    }

    private void MoveToNextArtwork()
    {
        currentState = NPCState.Walk;
        isStopped = false;
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

        animator.SetFloat("speed", 0f);
        agent.isStopped = true;
        yield return new WaitForSeconds(1f);

        RotateTowards(ArtworkManager.instance.GetArtworkPosition(ArtworkManager.instance.currentIndex));

        // thinking
        currentState = NPCState.Think;
        animator.SetBool("isThinking", true);
        animator.SetBool("isBravo", false);
        text.text = artworkDescription[Random.Range(0, artworkDescription.Count)];
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
        animator.SetFloat("speed", agent.velocity.magnitude);

        isProcessingArtwork = false;
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f; 

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        }
    }

}
