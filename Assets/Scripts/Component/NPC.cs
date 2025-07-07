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
