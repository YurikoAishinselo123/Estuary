using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int currentChapter = 1;
    public List<string> collectedItemIDs = new List<string>();
}
