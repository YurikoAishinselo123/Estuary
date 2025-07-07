using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }

    private const string MISSION_STAGE_KEY = "MissionStage";
    private const string TALKED_TO_DAYAT_KEY = "TalkedToDayat";
    private const string LEFT_RUANG_ATASAN_KEY = "LeftRuangAtasan";
    private const string VISITED_OCEAN_KEY = "VisitedOcean";

    public bool HasTalkedToDayat
    {
        get => PlayerPrefs.GetInt(TALKED_TO_DAYAT_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(TALKED_TO_DAYAT_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool HasLeftRuangAtasanAfterTalking
    {
        get => PlayerPrefs.GetInt(LEFT_RUANG_ATASAN_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(LEFT_RUANG_ATASAN_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool HasVisitedOcean
    {
        get => PlayerPrefs.GetInt(VISITED_OCEAN_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(VISITED_OCEAN_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadMissionStage();
    }

    public int GetMissionStage()
    {
        int stage = 0;
        Debug.Log("Has visited ocean : " + HasVisitedOcean);

        if (!HasTalkedToDayat) stage = 0;
        else if (MissionSaveHelper.IsMissionCompleted(3)) stage = 4;
        else if (HasVisitedOcean) stage = 3;
        else if (MissionSaveHelper.IsMissionCompleted(1)) stage = 2;
        else if (HasLeftRuangAtasanAfterTalking) stage = 1;

        SaveMissionStage(stage);
        return stage;
    }

    private void SaveMissionStage(int stage)
    {
        PlayerPrefs.SetInt(MISSION_STAGE_KEY, stage);
        PlayerPrefs.Save();
        Debug.Log($"[GameProgressManager] Mission stage saved: {stage}");
    }

    private void LoadMissionStage()
    {
        int stage = PlayerPrefs.GetInt(MISSION_STAGE_KEY, 0);
        Debug.Log($"[GameProgressManager] Mission stage loaded: {stage}");
    }

    public void UpdateFromMissionProgress()
    {
        if (MissionSaveHelper.IsMissionCompleted(1))
        {
            HasTalkedToDayat = true;
            HasLeftRuangAtasanAfterTalking = true;
        }

        if (MissionSaveHelper.IsMissionCompleted(2))
        {
            Debug.Log("[GameProgressManager] Mission 2 complete: Bersihkan Sampah");
        }

        if (MissionSaveHelper.IsMissionCompleted(3))
        {
            Debug.Log("[GameProgressManager] Mission 3 complete.");
        }

        Debug.Log("[GameProgressManager] Progress updated from mission completion.");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(MISSION_STAGE_KEY);
        PlayerPrefs.DeleteKey(TALKED_TO_DAYAT_KEY);
        PlayerPrefs.DeleteKey(LEFT_RUANG_ATASAN_KEY);
        PlayerPrefs.DeleteKey(VISITED_OCEAN_KEY);
        PlayerPrefs.Save();

        Debug.Log("[GameProgressManager] Progress reset.");
    }
}
