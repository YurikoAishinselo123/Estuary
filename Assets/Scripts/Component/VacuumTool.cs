using UnityEngine;

public class VacuumTool : MonoBehaviour, ITool
{
    [Header("References")]
    [SerializeField] public GameObject vacuumObject;
    private VacuumController vacuumController;

    [Header("Tool Transforms")]
    [SerializeField] private Vector3 activeLocalPosition;
    [SerializeField] private Vector3 inactiveLocalPosition;

    [Header("Visual Effects")]
    // [SerializeField] private GameObject vacuumEffect;

    private bool isSelected;

    private void Awake()
    {
        vacuumController = GetComponent<VacuumController>();
        vacuumObject?.SetActive(false);
    }

    public void Activate()
    {
        isSelected = true;
        vacuumObject?.SetActive(true);
        // vacuumObject.transform.localPosition = inactiveLocalPosition; 
        // vacuumEffect?.SetActive(false);
    }

    public void Deactivate()
    {
        isSelected = false;
        vacuumObject.SetActive(false);
        // vacuumEffect?.SetActive(false);
    }

    public void Use()
    {
        if (!isSelected) return;
        Debug.Log("Vacuum use");
        // vacuumObject.transform.localPosition = activeLocalPosition;
        // vacuumEffect?.SetActive(true);
        vacuumController?.Suck();
    }
}
