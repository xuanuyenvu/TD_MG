using UnityEngine;

public class AnimationRobotManager : MonoBehaviour
{
    public float distance = 2f;
    private Animator animator;
    private GameObject player;
    public float originalSpeed;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (animator.speed > 0 && Vector3.Distance(transform.position, player.transform.position) <= distance)
        {
            originalSpeed = animator.speed;
            animator.speed = 0f;
        }
        else if (animator.speed == 0 && Vector3.Distance(transform.position, player.transform.position) > distance)
        {
            animator.speed = originalSpeed;
        }
    }
}
