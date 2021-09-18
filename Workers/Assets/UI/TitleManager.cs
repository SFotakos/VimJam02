using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public enum ButtonType
    {
        PLAY,
        OPTIONS,
        CREDITS,
        QUIT,
        CLEAR_SAVE
    }

    private AudioSource audioSource;
    [Header("Audio Clips")]
    [Space(5)]
    [SerializeField]
    private AudioClip hoverClip;
    [SerializeField]
    private AudioClip clickClip;

    [Header("Play variables")]
    [Space(5)]
    [SerializeField]
    private RectTransform clearSaveBtn;
    [SerializeField]
    private TextMeshProUGUI playText;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("FinishedTutorial"))
        {
            playText.text = "Continue";
            clearSaveBtn.gameObject.SetActive(true);
        } 
    }

    public void OnClick(int buttonType)
    {
        audioSource.PlayOneShot(clickClip);
        switch ((ButtonType)buttonType)
        {
            case ButtonType.PLAY:
                if (!PlayerPrefs.HasKey("FinishedTutorial"))
                {
                    SceneManager.LoadScene("FirstTutorial");
                }
                else
                {
                    SceneManager.LoadScene("Factory");
                }
                //SceneManager.LoadScene("Factory"));; //If played the tutorial
                break;
            case ButtonType.OPTIONS:
                Debug.Log("Options");
                break;
            case ButtonType.CREDITS:
                Debug.Log("Credits");
                break;
            case ButtonType.QUIT:
                Application.Quit();
                break;
            case ButtonType.CLEAR_SAVE:
                playText.text = "Play";
                clearSaveBtn.gameObject.SetActive(false);
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                break;
        }
    }
}
