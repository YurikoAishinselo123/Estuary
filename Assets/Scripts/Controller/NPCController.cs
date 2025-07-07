using UnityEngine;

public class NPCController : MonoBehaviour, ITalkable
{
    [Header("Rotation Settings")]
    [SerializeField] private bool facePlayerOnTalk = true;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private Animator animator;
    private Transform playerTransform;
    private bool isTalking = false;


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
        }
    }

    public void OnDialogueStarted()
    {
        isTalking = true;
        animator?.SetBool("Talk", true);
        animator?.SetBool("Idle", false);
    }

    public void OnDialogueEnded()
    {
        GameProgressManager.Instance.HasTalkedToDayat = true;
        isTalking = false;
        animator?.SetBool("Talk", false);
        animator?.SetBool("Idle", true);
    }
}
