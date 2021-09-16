using UnityEditor;
using UnityEngine;

public class PlayerPrefsEditor : MonoBehaviour
{
    [MenuItem("Window/Delete PlayerPrefs (All)")]
    static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
