using TMPro;
using UnityEngine;

public class DetectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI detectionText;

    private void Start()
    {
        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.OnDetect += UpdateDetectionText;
        }
        else
        {
            Debug.LogError("[DetectionUI] DetectionManager.Instance is null in Start()");
        }
    }

    private void OnDisable()
    {
        if (DetectionManager.Instance != null)
            DetectionManager.Instance.OnDetect -= UpdateDetectionText;
    }

    private void UpdateDetectionText(string detectedName)
    {
        detectionText.text = string.IsNullOrEmpty(detectedName) ? "" : $"Detected: {detectedName}";
    }
}
