using System.Collections.Generic;

[System.Serializable]
public class Dialogs
{
    public List<Dialog> dialogs;
}

[System.Serializable]
public class Dialog
{
    public int dialogId;
    public List<Line> lines;

    public bool read = false;
}

[System.Serializable]
public class Line
{
    public int lineId;
    public string text;
    public int speakerId;
    public string speakerName;

    public bool read = false;
}
