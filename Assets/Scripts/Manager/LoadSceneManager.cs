using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    public static SceneName? nextScene = null;

    [SerializeField] private DoorSO doorSO;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(SceneName sceneToLoad)
    {
        nextScene = sceneToLoad;
        SceneManager.LoadScene(sceneToLoad.ToString());
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (nextScene.HasValue)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterController controller = player.GetComponent<CharacterController>();
                Vector3 spawnPos = doorSO.GetSpawnPosition(nextScene.Value, SpawnManager.LastSceneEnteredFrom);
                Debug.Log("From :" + SpawnManager.LastSceneEnteredFrom + "To : " + nextScene.Value);


                if (controller != null)
                {
                    controller.enabled = false;
                    player.transform.position = spawnPos;
                    controller.enabled = true;
                }
                else
                {
                    Debug.Log("Cant find the controller");
                }

                if (player.TryGetComponent(out PlayerController playerController))
                {
                    if (nextScene.Value == SceneName.Laut)
                        playerController.SetMovementMode(MovementMode.Diving);
                    else
                        playerController.SetMovementMode(MovementMode.Walking);
                }
            }

            nextScene = null;
        }
    }
}
