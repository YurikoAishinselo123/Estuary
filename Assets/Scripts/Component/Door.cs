using UnityEngine;

public class Door : MonoBehaviour, IDetectable
{
    [SerializeField] private SceneName targetScene;

    public string GetDisplayName()
    {
        return targetScene.GetDisplayName();
    }
    public SceneName GetTargetScene() => targetScene;
}
