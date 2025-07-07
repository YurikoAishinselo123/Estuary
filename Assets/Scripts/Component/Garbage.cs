using UnityEngine;

public class Garbage : MonoBehaviour, IDetectable
{
    [SerializeField] private GarbageType garbageType;

    public string GetDisplayName()
    {
        return garbageType.ToString().Replace("_", " ");
    }

    public GarbageType GetGarbageType()
    {
        return garbageType;
    }

    public string GetFact()
    {
        return GarbageFactHelper.GetFact(garbageType);
    }
}
