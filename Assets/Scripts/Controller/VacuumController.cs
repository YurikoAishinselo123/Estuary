using UnityEngine;
using System.Collections.Generic;

public class VacuumController : MonoBehaviour
{
    [Header("Vacuum Settings")]
    [SerializeField] private Transform vacuumPoint;
    [SerializeField] private float rayLength = 5f;
    [SerializeField] private int rayCount = 20;
    [SerializeField] private float raySpreadAngle = 30f;
    [SerializeField] private float absorbSpeed = 15f;
    [SerializeField] private float destroyDistance = 3f;
    [SerializeField] private LayerMask garbageLayer;

    private readonly List<Transform> grabbedObjects = new();

    /// <summary>
    /// Main method to run vacuum logic: scan and pull objects.
    /// Call this from VacuumTool.Use().
    /// </summary>
    /// 
    public void Suck()
    {
        if (!LoadSceneManager.Instance.inOcean)
        {
            Debug.Log("[VacuumController] Vacuum can only be used in the ocean.");
            return;
        }
        ScanForGarbage();
        PullObjects();
    }

    private void ScanForGarbage()
    {
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = Quaternion.Euler(
                Random.Range(-raySpreadAngle, raySpreadAngle),
                Random.Range(-raySpreadAngle, raySpreadAngle),
                0f) * vacuumPoint.forward;

            if (Physics.Raycast(vacuumPoint.position, direction, out RaycastHit hit, rayLength, garbageLayer))
            {
                if (!grabbedObjects.Contains(hit.transform))
                {
                    grabbedObjects.Add(hit.transform);
                }
            }
        }
    }

    private void PullObjects()
    {
        for (int i = grabbedObjects.Count - 1; i >= 0; i--)
        {
            Transform obj = grabbedObjects[i];

            if (obj == null)
            {
                grabbedObjects.RemoveAt(i);
                continue;
            }

            obj.position = Vector3.MoveTowards(obj.position, vacuumPoint.position, absorbSpeed * Time.deltaTime);

            float distance = Vector3.Distance(obj.position, vacuumPoint.position);
            if (distance <= destroyDistance)
            {
                OnGarbageCollected(obj.gameObject);
                grabbedObjects.RemoveAt(i);
            }
        }
    }

    private void OnGarbageCollected(GameObject garbage)
    {
        // AudioManager.Instance?.SFXCollectGarbage();
        // MissionManager.Instance?.OnGarbageCollected();
        Destroy(garbage);
    }

    private void OnDrawGizmosSelected()
    {
        if (vacuumPoint == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 direction = Quaternion.Euler(
                Random.Range(-raySpreadAngle, raySpreadAngle),
                Random.Range(-raySpreadAngle, raySpreadAngle),
                0f) * vacuumPoint.forward;

            Gizmos.DrawRay(vacuumPoint.position, direction * rayLength);
        }
    }
}
