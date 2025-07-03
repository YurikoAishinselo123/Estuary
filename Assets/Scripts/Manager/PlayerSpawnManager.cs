using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private SceneName startingScene;
    [SerializeField] private DoorSO doorSO;

    private void Start()
    {
        // Only spawn if player doesn't already exist (first scene like Splashscreen/Mainmenu)
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Vector3 spawnPosition = doorSO.GetSpawnPosition(startingScene, startingScene);
            Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
