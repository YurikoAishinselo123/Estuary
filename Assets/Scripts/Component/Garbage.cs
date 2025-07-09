using UnityEngine;

public class Garbage : MonoBehaviour, IDetectable
{
    [SerializeField] private GarbageType type;

    public string GetDisplayName() => type.ToString();

    public string GetFact()
    {
        return GarbageFactDatabase.Instance?.GetFact(type);
    }

    public GarbageType GetGarbageType() => type;
}

