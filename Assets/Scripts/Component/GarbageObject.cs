using UnityEngine;

public class GarbageObject : MonoBehaviour
{
    public void Collect()
    {
        Debug.Log("🗑️ Garbage collected!");
        Destroy(gameObject);
    }
}
