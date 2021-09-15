using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private static DialogManager _instance = null;
    private static Dialogs dialogs;

    public static DialogManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DialogManager)) as DialogManager;
            }

            if (_instance == null)
            {
                var obj = new GameObject("DialogManager");
                _instance = obj.AddComponent<DialogManager>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        TextAsset dialogAsset = Resources.Load("DialogLines") as TextAsset;
        dialogs = JsonUtility.FromJson<Dialogs>(dialogAsset.text);
    }

    public Line GetUnreadLine(Dialog dialog)
    {
        foreach (Line line in dialog.lines)
        {
            if (!line.read)
            {
                line.read = true;
                return line;
            }
        }
        dialog.read = true;
        return null;
    }

    // Passing no value returns the first not read dialog.
    public Dialog GetDialog(int id = -1)
    {
        foreach (Dialog dialog in dialogs.dialogs)
        {
            if (id != -1 && dialog.dialogId == id)
                return dialog;
        }
        return null;
    }
}
