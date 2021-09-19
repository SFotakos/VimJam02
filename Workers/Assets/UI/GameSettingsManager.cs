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

    [SerializeField]
    Toggle[] difficultySettings;

    string selectedSprite = "CoolGlasses";

    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    public void OnSelectedSprite(string spriteName)
    {
        selectedSprite = spriteName;
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
        foreach (Toggle toggle in difficultySettings)
        {
            PlayerPrefs.SetString(toggle.name, toggle.isOn.ToString());
        }
        audioSource.PlayOneShot(clickClip);
        if (((Toggle) GameObject.Find("SkipTutorial").GetComponent<Toggle>()).isOn)
        {
            SceneManager.LoadScene("Factory");
        } else
        {
            SceneManager.LoadScene("FirstTutorial");
        }
        PlayerPrefs.SetInt("PlayerSprite", selectedSprite.Equals("CoolGlasses") ? 0 : 1);

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
