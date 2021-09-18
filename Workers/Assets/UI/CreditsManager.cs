using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{

    public void OnLinkClicked(TextMeshProUGUI textMeshPro)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        { 
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }

    public void OnSkip()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
