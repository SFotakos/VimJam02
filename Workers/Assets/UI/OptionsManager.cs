using System.Collections;
using System.Collections.Generic;
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
}
