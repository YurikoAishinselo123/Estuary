using System.Collections.Generic;

[System.Serializable]
public class DialogueEntry
{
    public string speaker;
    public string text;
}

[System.Serializable]
public class NPCDialogueModel
{
    public string conversationId;
    public string npcName;
    public List<DialogueEntry> dialogues;
}
