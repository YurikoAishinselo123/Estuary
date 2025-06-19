using UnityEngine;

public class FishNPCController : MonoBehaviour
{
    [Header("Data")]
    public FishSO fishData;

    private FishModel model;

    private void Start()
    {
        model = new FishModel();
        EnterIdleState();
    }

    private void Update()
    {
        switch (model.currentState)
        {
            case FishState.Idle:
                HandleIdle();
                break;
            case FishState.Swimming:
                HandleSwimming();
                break;
        }
    }

    private void HandleIdle()
    {
        model.idleTimer -= Time.deltaTime;
        if (model.idleTimer <= 0f)
        {
            PickNewTarget();
            model.currentState = FishState.Swimming;
        }
    }

    private void HandleSwimming()
    {
        transform.position = Vector3.MoveTowards(transform.position, model.targetPosition, fishData.swimSpeed * Time.deltaTime);
        transform.LookAt(model.targetPosition);

        if (Vector3.Distance(transform.position, model.targetPosition) < 0.1f)
        {
            EnterIdleState();
        }
    }

    private void PickNewTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * fishData.swimRange;
        randomDirection.y = 0;
        model.targetPosition = transform.position + randomDirection;
    }

    private void EnterIdleState()
    {
        model.currentState = FishState.Idle;
        model.idleTimer = fishData.idleDuration;
    }
}
