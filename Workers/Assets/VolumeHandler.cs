using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    public AudioMixer mixer;
    Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.value = GetVolumeFromPrefs();
    }

    public void SetVolumeLevel(float value)
    {
        float newVolume = Mathf.Log10(value) * 20;
        mixer.SetFloat(gameObject.name, newVolume);
        PlayerPrefs.SetFloat(gameObject.name, value);
    }
    
    public float GetVolumeFromPrefs()
    {
        return PlayerPrefs.GetFloat(gameObject.name, 1f);
    }
}
