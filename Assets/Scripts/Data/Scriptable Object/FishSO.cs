using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Fish")]
public class FishSO : ScriptableObject
{
    public float swimSpeed = 2f;
    public float idleDuration = 2f;
    public float changeDirectionInterval = 3f;
    public float swimRange = 5f;
}
