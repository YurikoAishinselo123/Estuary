using System;
using System.Collections.Generic;

[Serializable]
public class GarbageFactListWrapper
{
    public List<GarbageFactModel> facts;
}

[Serializable]
public class GarbageFactModel
{
    public string type;
    public string fact;
}
