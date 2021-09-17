using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    
    [SerializeField]
    List<Sprite> sprites = new List<Sprite>();
    [SerializeField]
    List<RuntimeAnimatorController> runtimeAnimator = new List<RuntimeAnimatorController>();

    const int EVIL_BOSS_INDEX = 0;
    const int ORANGE_GUY_INDEX = 1;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        GameController gameController = GameController.instance;
        if (gameController.currentDay == GameController.DayEnum.FOURTH)
        {
            spriteRenderer.sprite = sprites[ORANGE_GUY_INDEX];
            animator.runtimeAnimatorController = runtimeAnimator[ORANGE_GUY_INDEX];
        } else
        {
            spriteRenderer.sprite = sprites[EVIL_BOSS_INDEX];
            animator.runtimeAnimatorController = runtimeAnimator[EVIL_BOSS_INDEX];
        }
    }
}
