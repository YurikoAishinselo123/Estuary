using System;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    public static DetectionManager Instance { get; private set; }

    public event Action<string> OnDetect;

    [Header("Detection Settings")]
    private float detectionRange = 3f; 
    private float detectionAngle = 45f;
    private float detectionFrequency = 0.1f;
    [SerializeField] private LayerMask detectionLayer;
    private int rayCount = 5;

    [Header("Camera Reference")]
    [SerializeField] private Transform playerCamera;

    private float timer;
    private IDetectable currentDetected;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= detectionFrequency)
        {
            timer = 0f;
            PerformDetection();
        }
    }

    private void PerformDetection()
    {
        currentDetected = null;

        // Step 1: OverlapSphere broad detection
        Collider[] hits = Physics.OverlapSphere(playerCamera.position, detectionRange, detectionLayer);

        foreach (Collider hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - playerCamera.position).normalized;
            float angle = Vector3.Angle(playerCamera.forward, dirToTarget);

            if (angle < detectionAngle / 2f)
            {
                IDetectable detectable = hit.GetComponent<IDetectable>();
                if (detectable != null)
                {
                    currentDetected = detectable;
                    OnDetect?.Invoke(detectable.GetDisplayName());
                    return;
                }
            }
        }

        // Step 2: Raycast fan sweep for better precision (especially up close)
        float step = detectionAngle / (rayCount - 1);
        float startAngle = -detectionAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = startAngle + (step * i);
            Quaternion rotation = Quaternion.Euler(0f, angleOffset, 0f);
            Vector3 direction = rotation * playerCamera.forward;

            if (Physics.Raycast(playerCamera.position, direction, out RaycastHit hit, detectionRange, detectionLayer))
            {
                IDetectable detectable = hit.collider.GetComponent<IDetectable>();
                if (detectable != null)
                {
                    currentDetected = detectable;
                    OnDetect?.Invoke(detectable.GetDisplayName());
                    Debug.DrawRay(playerCamera.position, direction * detectionRange, Color.green, 0.1f);
                    return;
                }
            }

            Debug.DrawRay(playerCamera.position, direction * detectionRange, Color.red, 0.1f);
        }

        // Nothing detected
        OnDetect?.Invoke(string.Empty);
    }

    public IDetectable GetCurrentDetected()
    {
        return currentDetected;
    }

    private void OnDrawGizmosSelected()
    {
        if (Camera.main == null) return;

        Gizmos.color = Color.yellow;
        Vector3 origin = Camera.main.transform.position;
        Vector3 forward = Camera.main.transform.forward;

        Quaternion leftRayRotation = Quaternion.AngleAxis(-detectionAngle / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(detectionAngle / 2, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * forward;
        Vector3 rightRayDirection = rightRayRotation * forward;

        Gizmos.DrawRay(origin, leftRayDirection * detectionRange);
        Gizmos.DrawRay(origin, rightRayDirection * detectionRange);
        Gizmos.DrawWireSphere(origin, detectionRange);
    }
}
