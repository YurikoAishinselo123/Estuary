using UnityEngine;
using System.IO;

public class NPC : MonoBehaviour, IDetectable
{
    [SerializeField] private string npcName;
    [SerializeField] private string baseFileName;

    private NPCDialogueModel dialogueModel;
    private NPCController npcController;

    private void Awake()
    {
        npcController = GetComponent<NPCController>();
        LoadDialogueBasedOnStage();
    }

    public string GetDisplayName() => npcName;

    public void TriggerDialogue()
    {
        if (dialogueModel != null && npcController != null)
        {
            // 🔥 Special case: Dayat angry because player hasn't cleaned yet
            if (npcName == "Dayat" &&
                GameProgressManager.Instance.GetMissionStage() == 4 &&
                !GameProgressManager.Instance.HasTalkedToDayatAtStage4)
            {
                // Load special "angry" dialogue
                Debug.Log("HasTalkedToDayatAtStage4: " + GameProgressManager.Instance.HasTalkedToDayatAtStage4);
                string specialFileName = $"{baseFileName}_4_alt.json";
                string path = Path.Combine(Application.streamingAssetsPath, "Dialogue", specialFileName);

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var angryDialogue = JsonUtility.FromJson<NPCDialogueModel>(json);
                    DialogueManager.Instance.StartDialogue(angryDialogue, npcController);
                    GameProgressManager.Instance.HasTalkedToDayatAtStage4 = true;
                    Debug.Log("[NPC] Triggered angry Dayat dialogue.");
                    return;
                }
                else
                {
                    Debug.LogWarning("[NPC] Special angry Dayat dialogue not found, fallback to normal.");
                }
            }

            // 🗣️ Normal/default dialogue
            DialogueManager.Instance.StartDialogue(dialogueModel, npcController);
        }
    }


    private void LoadDialogueBasedOnStage()
    {
        if (GameProgressManager.Instance == null)
        {
            Debug.LogError("[NPC] GameProgressManager.Instance is null");
            return;
        }

        int stage = GameProgressManager.Instance.GetMissionStage();
        Debug.Log("Stage : " + stage);
        string fileName = $"{baseFileName}_{stage}.json";
        string path = Path.Combine(Application.streamingAssetsPath, "Dialogue", fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            dialogueModel = JsonUtility.FromJson<NPCDialogueModel>(json);
            Debug.Log($"[NPC] Loaded dialogue: {fileName}");
        }
        else
        {
            Debug.LogError($"[NPC] Dialogue JSON not found for NPC '{npcName}' at path: {path}");
        }
    }

    private void OnEnable()
    {
        if (GameProgressManager.Instance != null)
            GameStateManager.Instance.OnStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        if (GameProgressManager.Instance != null)
            GameStateManager.Instance.OnStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        LoadDialogueBasedOnStage(); // Reload dialogue when state might have changed
    }

    
}
