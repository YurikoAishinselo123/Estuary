using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance { get; private set; }
    public static SceneName? nextScene = null;
    public bool inOcean = false;

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
                Debug.Log("From :" + SpawnManager.LastSceneEnteredFrom + " To : " + nextScene.Value);

                if (controller != null)
                {
                    controller.enabled = false;
                    player.transform.position = spawnPos;
                    controller.enabled = true;
                }
                else
                {
                    Debug.Log("Cannot find CharacterController on Player");
                }

                if (player.TryGetComponent(out PlayerController playerController))
                {
                    if (nextScene.Value == SceneName.Laut)
                        playerController.SetMovementMode(MovementMode.Diving);
                    else
                        playerController.SetMovementMode(MovementMode.Walking);
                }
            }

            // ✅ Cek: Jika keluar dari Ruang Atasan setelah bicara dengan Dayat
            if (SpawnManager.LastSceneEnteredFrom == SceneName.RuangAtasan &&
                nextScene.Value != SceneName.RuangAtasan &&
                GameProgressManager.Instance.HasTalkedToDayat &&
                !GameProgressManager.Instance.HasLeftRuangAtasanAfterTalking)
            {
                GameProgressManager.Instance.HasLeftRuangAtasanAfterTalking = true;
                Debug.Log("Player telah keluar dari Ruang Atasan setelah berbicara dengan Dayat.");
            }

            // ✅ Cek: Pertama kali masuk ke Laut
            if (nextScene.Value == SceneName.Laut)
            {
                GuidanceManager.Instance.ShowNextGuidance();
                GameProgressManager.Instance.HasVisitedOcean = true;
                inOcean = true;
                Debug.Log("[GameProgress] Player has entered Laut scene.");
            }
            else
            {
                inOcean = false;
            }

            // ✅ Cek: Pertama kali masuk ke Ruang Atasan
            if (nextScene.Value == SceneName.RuangAtasan &&
                !GameProgressManager.Instance.HasEnteredRuangAtasanForTheFirstTime)
            {
                GameProgressManager.Instance.HasEnteredRuangAtasanForTheFirstTime = true;
                Debug.Log("Player pertama kali masuk ke Ruang Atasan.");

                if (GuidanceManager.Instance != null)
                {
                    Debug.Log("go to ruang atasan for the first time");
                    GuidanceManager.Instance.ShowNextGuidance();
                }
            }

            Debug.Log($"[LoadSceneManager] Loading from {SpawnManager.LastSceneEnteredFrom} to {nextScene.Value}");
            Debug.Log($"[GameProgress] TalkedToDayat: {GameProgressManager.Instance.HasTalkedToDayat}");
            Debug.Log($"[GameProgress] HasLeftAfterTalking: {GameProgressManager.Instance.HasLeftRuangAtasanAfterTalking}");

            // Update LastSceneEnteredFrom setelah semua logika dieksekusi
            SpawnManager.LastSceneEnteredFrom = nextScene.Value;

            nextScene = null;
        }
    }
}
