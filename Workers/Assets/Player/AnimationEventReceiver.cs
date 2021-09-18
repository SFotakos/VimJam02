using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    InGameSoundManager soundManager;

    private void Start()
    {
        soundManager = InGameSoundManager.instance;
    }

    void StepSound()
    {
        soundManager.PlaySoundEffect(InGameSoundManager.SoundEffectType.STEP);
    }

    void HurtSound()
    {

    }
}
