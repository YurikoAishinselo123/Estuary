using UnityEngine;
using System.IO;

public class NPC : MonoBehaviour, IDetectable
{
    [SerializeField] private string npcName;
    [SerializeField] private string jsonFileName;

    private NPCDialogueModel dialogueModel;
    private NPCController npcController;

    private void Awake()
    {
        npcController = GetComponent<NPCController>();
        LoadDialogue();
    }

    public string GetDisplayName() => npcName;

    public void TriggerDialogue()
{
    if (dialogueModel != null && npcController != null)
    {
        DialogueManager.Instance.StartDialogue(dialogueModel, npcController);
    }
}


    private void LoadDialogue()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Dialogue", $"{jsonFileName}.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            dialogueModel = JsonUtility.FromJson<NPCDialogueModel>(json);
        }
        else
        {
            Debug.LogError($"Dialogue JSON not found for NPC '{npcName}' at path: {path}");
        }
    }
}
