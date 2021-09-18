using System;
using UnityEngine;

public class InGameSoundManager : MonoBehaviour
{
    private static InGameSoundManager _instance;
    public static InGameSoundManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<InGameSoundManager>();

            if (_instance == null)
                throw new InvalidOperationException("You need at least one InGameSoundManagerObject");

            return _instance;
        }
    }

    [Header("Audio Sources")]
    [Space(5)]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioSource oneShotSource;

    [Header("Audio Clips")]
    [Space(5)]
    [SerializeField] SoundEffect[] soundEffects;

    [Serializable]
    public struct SoundEffect
    {
        public SoundEffectType effectType;
        public AudioClip audioClip;
    }
    public enum SoundEffectType
    {
        STEP,
        TALK,
        HURT,
        DOOR,
        CRUSHING_HAMMER_START,
        CRUSHING_HAMMER_HIT,
        MOPPING
    }

    public void PlaySoundEffect(SoundEffectType soundEffectType)
    {
        oneShotSource.PlayOneShot(Array.Find(soundEffects, item => item.effectType == soundEffectType).audioClip);
    }   
}
