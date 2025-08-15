using System;
using UnityEngine;

public class DetectionManager : MonoBehaviour
{
    public static DetectionManager Instance { get; private set; }

    public event Action<string> OnDetect;
    public event Action<string> OnFactDetected;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private float detectionFrequency = 0.1f;
    [SerializeField] private int rayCount = 5;

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
        bool detectedSomething = false;

        float step = detectionAngle / (rayCount - 1);
        float startAngle = -detectionAngle / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = startAngle + (step * i);
            Quaternion rotation = Quaternion.Euler(0f, angleOffset, 0f);
            Vector3 direction = rotation * playerCamera.forward;

            if (Physics.Raycast(playerCamera.position, direction, out RaycastHit hit, detectionRange))
            {
                IDetectable detectable = hit.collider.GetComponent<IDetectable>();
                if (detectable != null)
                {
                    currentDetected = detectable;
                    OnDetect?.Invoke(detectable.GetDisplayName());
                    detectedSomething = true;

                    // If it's garbage, show the fact
                    Garbage garbage = hit.collider.GetComponent<Garbage>();
                    if (garbage != null)
                    {
                        string fact = garbage.GetFact();
                        Debug.Log("fact : " + fact);
                        OnFactDetected?.Invoke(fact);

                        if (!GameProgressManager.Instance.HasDetectedGarbageForTheFirstTime)
                        {
                            GameProgressManager.Instance.HasDetectedGarbageForTheFirstTime = true;
                            GuidanceManager.Instance?.ShowNextGuidance();
                        }
                    }

                    Debug.DrawRay(playerCamera.position, direction * detectionRange, Color.green, 0.1f);
                    return;
                }
            }

            Debug.DrawRay(playerCamera.position, direction * detectionRange, Color.red, 0.1f);
        }

        if (!detectedSomething)
        {
            OnDetect?.Invoke(string.Empty);
            OnFactDetected?.Invoke(string.Empty);
        }
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
