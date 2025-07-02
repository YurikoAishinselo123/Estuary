using UnityEngine;

public class NPCController : MonoBehaviour, ITalkable
{
    [Header("Rotation Settings")]
    [SerializeField] private bool facePlayerOnTalk = true;
    [SerializeField] private float rotationSpeed = 5f;

    private Animator animator;
    private Transform playerTransform;
    private bool isTalking = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerTransform = Camera.main?.transform; // or assign via GameManager/Player
    }

    private void Update()
    {
        if (isTalking && facePlayerOnTalk && playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void OnDialogueStarted()
    {
        isTalking = true;
        animator?.SetBool("IsTalking", true);
    }

    public void OnDialogueEnded()
    {
        isTalking = false;
        animator?.SetBool("IsTalking", false);
    }
}
