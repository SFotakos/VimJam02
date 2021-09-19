using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("Audio Clips")]
    [Space(5)]
    [SerializeField]
    private AudioClip hoverClip;
    [SerializeField]
    private AudioClip clickClip;

    [SerializeField]
    Image[] playerImages;

    [SerializeField]
    Color grayedOutColor;
    
    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        PlayerPrefs.SetInt("PlayerSprite", 0);
    }

    public void OnSelectedSprite(string spriteName)
    {
        PlayerPrefs.SetInt("PlayerSprite", spriteName.Equals("CoolGlasses") ? 0 : 1);
        foreach (Image image in playerImages)
        {
            if (image.name.Equals(spriteName)) {
                image.color = Color.white;
            } else
            {
                image.color = grayedOutColor;
            }
        }
    }

    public void OnPlayClicked()
    {
        audioSource.PlayOneShot(clickClip);
        SceneManager.LoadScene("FirstTutorial");
        PlayerPrefs.Save();
    }
   
    public void OnBackClicked()
    {
        SceneManager.UnloadSceneAsync("GameSettingsScene");
    }

    public void OnHover()
    {
        audioSource.PlayOneShot(hoverClip);
    }
}
