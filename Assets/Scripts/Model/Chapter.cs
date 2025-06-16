using System;
using System.Collections.Generic;

[Serializable]
public class Chapter
{
    public int chapterId;
    public string chapterTitle;
    public string chapterDescription;
    public List<Mission> missions;
}
