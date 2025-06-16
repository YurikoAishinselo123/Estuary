
using UnityEngine;

public class NPC : MonoBehaviour, IDetectable
{
    [SerializeField] private string npcName;

    public string GetDisplayName() => npcName;
}
