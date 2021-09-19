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
        if (HasStartedGame())
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
                if (!HasStartedGame())
                {
                    SceneManager.LoadScene("GameSettingsScene", LoadSceneMode.Additive);
                }
                else
                {
                    if (!PlayerPrefs.HasKey("FinishedTutorial"))
                    {
                        SceneManager.LoadScene("FirstTutorial");
                    }
                    else
                    {
                        SceneManager.LoadScene("Factory");
                    }
                }
                break;
            case ButtonType.OPTIONS:
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;
            case ButtonType.CREDITS:
                SceneManager.LoadScene("CreditsScene");
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

    public void OnHover()
    {
        audioSource.PlayOneShot(hoverClip);
    }

    private bool HasStartedGame()
    {
        return PlayerPrefs.HasKey("PlayerSprite") || PlayerPrefs.HasKey("DyingQuitting") 
            || PlayerPrefs.HasKey("IncreasedTasksAmount") || PlayerPrefs.HasKey("RandomizedTasksLocation");
    }
}
