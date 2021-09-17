using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public enum ButtonType
    {
        PLAY,
        OPTIONS,
        CREDITS,
        QUIT
    }

    private AudioSource audioSource;
    [Header("Audio Clips")]
    [Space(5)]
    [SerializeField]
    private AudioClip hoverClip;
    [SerializeField]
    private AudioClip clickClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnClick(int buttonType)
    {
        audioSource.PlayOneShot(clickClip);
        switch ((ButtonType) buttonType)
        {
            case ButtonType.PLAY:
                SceneManager.LoadScene("FirstTutorial");
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
        }
    }

   public void OnHover()
   {
        audioSource.PlayOneShot(hoverClip);
    }
}
