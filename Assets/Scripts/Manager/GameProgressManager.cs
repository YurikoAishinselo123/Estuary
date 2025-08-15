using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance { get; private set; }
    public Texture2D DirtyEnvironmentPhoto { get; private set; }
    public Texture2D CleanEnvironmentPhoto { get; private set; }
    public bool hasTalkedToDayatForTheFirstTime = false;

    private const string MISSION_STAGE_KEY = "MissionStage";
    private const string TALKED_TO_DAYAT_KEY = "TalkedToDayat";
    private const string LEFT_RUANG_ATASAN_KEY = "LeftRuangAtasan";
    private const string VISITED_OCEAN_KEY = "VisitedOcean";
    private const string ENTERED_RUANG_ATASAN_KEY = "EnteredRuangAtasan";
    private const string DETECTED_GARBAGE_KEY = "DetectedGarbage";
    private const string CAPTURED_DIRTY_ENV_KEY = "CapturedDirtyEnv";
    private const string SHOWN_REFLECTION_KEY = "ShownReflection";
    private const string REPORTED_TO_DAYAT_KEY = "ReportedToDayat";
    private const string TALKED_TO_DAYAT_STAGE4_KEY = "TalkedToDayatStage4";
    private const string COMPLETED_ALL_OBJECTIVES_KEY = "CompletedAllObjectives";

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

    public void SetDirtyPhoto(Texture2D photo)
    {
        DirtyEnvironmentPhoto = photo;
    }

    public void SetCleanPhoto(Texture2D photo)
    {
        CleanEnvironmentPhoto = photo;
    }

    // 🗣️ Menyimpan apakah pemain sudah bicara dengan Dayat
    public bool HasTalkedToDayat
    {
        get => PlayerPrefs.GetInt(TALKED_TO_DAYAT_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(TALKED_TO_DAYAT_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("[GameProgressManager] Player has talked to Dayat.");
            if (!hasTalkedToDayatForTheFirstTime)
            {
                hasTalkedToDayatForTheFirstTime = true;
                if (GuidanceManager.Instance != null)
                {
                    GuidanceManager.Instance.ShowNextGuidance();
                }
            }

        }
    }

    // 🚪 Menyimpan apakah pemain sudah meninggalkan ruang atasan setelah bicara
    public bool HasLeftRuangAtasanAfterTalking
    {
        
        get => PlayerPrefs.GetInt(LEFT_RUANG_ATASAN_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(LEFT_RUANG_ATASAN_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    // 🌊 Menyimpan apakah pemain sudah pernah ke laut
    public bool HasVisitedOceanForTheFirstTime
    {
        get => PlayerPrefs.GetInt(VISITED_OCEAN_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(VISITED_OCEAN_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    // 🏢 Menyimpan apakah pemain pertama kali masuk ruang atasan
    public bool HasEnteredRuangAtasanForTheFirstTime
    {
        get => PlayerPrefs.GetInt(ENTERED_RUANG_ATASAN_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(ENTERED_RUANG_ATASAN_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    // 🗑️ Menyimpan apakah pemain sudah mendeteksi sampah untuk pertama kali
    public bool HasDetectedGarbageForTheFirstTime
    {
        get => PlayerPrefs.GetInt(DETECTED_GARBAGE_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(DETECTED_GARBAGE_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("[GameProgressManager] Player has detected garbage for the first time.");
        }
    }

    public bool HasCapturedDirtyEnvironmentForTheFirstTime
    {
        get => PlayerPrefs.GetInt(CAPTURED_DIRTY_ENV_KEY, 0) == 1;
        set
        {
            if (!HasDetectedGarbageForTheFirstTime)
            {
                Debug.LogWarning("[GameProgressManager] Tidak bisa capture dirty environment sebelum mendeteksi sampah.");
                return;
            }

            PlayerPrefs.SetInt(CAPTURED_DIRTY_ENV_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("[GameProgressManager] Player has captured dirty environment for the first time.");

            if (value && GuidanceManager.Instance != null)
            {
                GuidanceManager.Instance.ShowNextGuidance();
            }
        }
    }

    private const string CAPTURED_CLEAN_ENV_KEY = "CapturedCleanEnv";

    public bool HasCapturedCleanEnvironmentForTheFirstTime
    {
        get => PlayerPrefs.GetInt(CAPTURED_CLEAN_ENV_KEY, 0) == 1;
        set
        {
            if (!HasCapturedDirtyEnvironmentForTheFirstTime)
            {
                Debug.LogWarning("[GameProgressManager] Tidak bisa capture dirty environment sebelum mendeteksi sampah.");
                return;
            }

            PlayerPrefs.SetInt(CAPTURED_CLEAN_ENV_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("[GameProgressManager] Player has captured clean environment for the first time.");

            if (value && GuidanceManager.Instance != null)
            {
                GuidanceManager.Instance.ShowNextGuidance();
            }
        }
    }

    public bool HasShownReflection
    {
        get => PlayerPrefs.GetInt(SHOWN_REFLECTION_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(SHOWN_REFLECTION_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log($"[GameProgressManager] HasShownReflection set to {value}");
        }
    }

    public bool HasReportedToDayat
    {
        get => PlayerPrefs.GetInt(REPORTED_TO_DAYAT_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(REPORTED_TO_DAYAT_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("[GameProgressManager] Player has reported to Dayat.");
        }
    }

    public bool HasTalkedToDayatAtStage4
    {
        get => PlayerPrefs.GetInt(TALKED_TO_DAYAT_STAGE4_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(TALKED_TO_DAYAT_STAGE4_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public bool CompletedAllGameObjective
    {
        get => PlayerPrefs.GetInt(COMPLETED_ALL_OBJECTIVES_KEY, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(COMPLETED_ALL_OBJECTIVES_KEY, value ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log("[GameProgressManager] CompletedAllGameObjective set to: " + value);
        }
    }

    // 📊 Menentukan misi tahap ke berapa
    public int GetMissionStage()
    {
        int stage = 0;

        if (!HasTalkedToDayat)
            stage = 0;
        else if (HasShownReflection)
            stage = 5;
        else if (HasDetectedGarbageForTheFirstTime && !HasReportedToDayat && !HasTalkedToDayatAtStage4)
            stage = 4;
        else if (HasVisitedOceanForTheFirstTime)
            stage = 3;
        else if (MissionSaveHelper.IsMissionCompleted(1))
            stage = 2;
        else if (HasLeftRuangAtasanAfterTalking)
            stage = 1;

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

    // 🔁 Update dari helper misi (misalnya saat Load Game)
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

    // 🔄 Reset seluruh progress
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(MISSION_STAGE_KEY);
        PlayerPrefs.DeleteKey(TALKED_TO_DAYAT_KEY);
        PlayerPrefs.DeleteKey(LEFT_RUANG_ATASAN_KEY);
        PlayerPrefs.DeleteKey(VISITED_OCEAN_KEY);
        PlayerPrefs.DeleteKey(ENTERED_RUANG_ATASAN_KEY);
        PlayerPrefs.DeleteKey(DETECTED_GARBAGE_KEY);
        PlayerPrefs.DeleteKey(CAPTURED_DIRTY_ENV_KEY);
        PlayerPrefs.DeleteKey(CAPTURED_CLEAN_ENV_KEY);
        PlayerPrefs.DeleteKey(SHOWN_REFLECTION_KEY);
        PlayerPrefs.DeleteKey(REPORTED_TO_DAYAT_KEY);
        PlayerPrefs.DeleteKey(TALKED_TO_DAYAT_STAGE4_KEY);
        PlayerPrefs.DeleteKey(COMPLETED_ALL_OBJECTIVES_KEY);

        PlayerPrefs.Save();

        Debug.Log("[GameProgressManager] Progress reset.");
    }
}
