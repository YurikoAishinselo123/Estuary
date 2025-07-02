using System;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    public static DetectionManager Instance { get; private set; }

    public event Action<string> OnDetect;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private float detectionFrequency = 0.1f;
    [SerializeField] private LayerMask detectionLayer;

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

        Collider[] hits = Physics.OverlapSphere(playerCamera.position, detectionRange, detectionLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = (hit.transform.position - playerCamera.position).normalized;
            float angle = Vector3.Angle(playerCamera.forward, directionToTarget);

            if (angle < detectionAngle / 2f)
            {
                IDetectable detectable = hit.GetComponent<IDetectable>();
                if (detectable != null)
                {
                    currentDetected = detectable;
                    string detectedName = detectable.GetDisplayName();
                    Debug.Log($"[DetectionManager] Detected: {detectedName}");
                    OnDetect?.Invoke(detectedName);
                    return;
                }
            }
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
