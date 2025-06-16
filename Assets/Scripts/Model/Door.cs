using UnityEngine;

public class Door : MonoBehaviour, IDetectable
{
    [SerializeField] private string doorName;

    public string GetDisplayName() => doorName;
}
