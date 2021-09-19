using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class CreditsManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenURLInExternalWindow(string url);

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnLinkClicked(TextMeshProUGUI textMeshPro)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        { 
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                OpenURLInExternalWindow(linkInfo.GetLinkID());
            } else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }
    }

    public void OnSkip()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
