using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsManager : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("Audio Clips")]
    [Space(5)]
    [SerializeField]
    private AudioClip hoverClip;
    [SerializeField]
    private AudioClip clickClip;

    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    public void OnBackClicked()
    {
        audioSource.PlayOneShot(clickClip);
        SceneManager.UnloadSceneAsync("OptionsScene");
    }

    public void OnHover()
    {
        audioSource.PlayOneShot(hoverClip);
    }

    public void SceneUnloaded(Scene scene)
    {
        if (scene.name.Equals("OptionsScreen"))
        {
            PlayerPrefs.Save();
        }
    }
}
