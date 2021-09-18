
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public enum ButtonType
    {
        RESUME,
        OPTIONS,
        MENU,
        QUIT,
    }

    private AudioSource audioSource;
    [Header("Audio Clips")]
    [Space(5)]
    [SerializeField]
    private AudioClip hoverClip;
    [SerializeField]
    private AudioClip clickClip;

    GameController gameController;

    private void Start()
    {
        gameController = GameController.instance;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClick(int buttonType)
    {
        audioSource.PlayOneShot(clickClip);
        switch ((ButtonType) buttonType)
        {
            case ButtonType.RESUME:
                gameController.ResumeGame();
                break;
            case ButtonType.OPTIONS:
                gameController.canResume = false;
                SceneManager.LoadScene("OptionsScene", LoadSceneMode.Additive);
                break;
            case ButtonType.MENU:
                gameController.MainMenu();
                break;
            case ButtonType.QUIT:
                gameController.Quit();
                break;
        }
    }
}
