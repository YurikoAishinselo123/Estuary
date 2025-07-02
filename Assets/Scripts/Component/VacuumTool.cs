using UnityEngine;

public class VacuumTool : MonoBehaviour
{
    public float suckRadius = 3f;

    public void Suck()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, suckRadius);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out GarbageObject garbage))
            {
                garbage.Collect();
            }
        }
    }
}