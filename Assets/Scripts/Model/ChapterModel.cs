using System;
using System.Collections.Generic;

[Serializable]
public class ChapterModel
{
    public int chapterId;
    public string chapterTitle;
    public string chapterDescription;
    public List<MissionModel> missions;
}
