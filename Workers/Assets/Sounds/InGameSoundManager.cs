using System;
using System.Collections;
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

    int crushingHammerStartCount = 0;
    int crushingHammerHitCount = 0;

    public void PlaySoundEffect(SoundEffectType soundEffectType)
    {
        AudioClip clip = Array.Find(soundEffects, item => item.effectType == soundEffectType).audioClip;
        if (soundEffectType == SoundEffectType.CRUSHING_HAMMER_START)
        {
            if (crushingHammerStartCount < 1)
            {
                crushingHammerStartCount++;
                oneShotSource.PlayOneShot(clip);
                StartCoroutine(SubtractAudioCountAfterDelay(clip.length, soundEffectType));
            }
        }
        else if (soundEffectType == SoundEffectType.CRUSHING_HAMMER_HIT)
        {
            if (crushingHammerHitCount < 1)
            {
                crushingHammerHitCount++;
                oneShotSource.PlayOneShot(clip);
                StartCoroutine(SubtractAudioCountAfterDelay(clip.length, soundEffectType));
            }
        }
        oneShotSource.PlayOneShot(clip);
    }

    IEnumerator SubtractAudioCountAfterDelay(float delay, SoundEffectType soundEffectType)
    {
        yield return new WaitForSeconds(delay);
        if (soundEffectType == SoundEffectType.CRUSHING_HAMMER_START)
            crushingHammerStartCount--;
        else if (soundEffectType == SoundEffectType.CRUSHING_HAMMER_HIT)
            crushingHammerHitCount--;
    }
}
