using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorStep : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private GameController gameController;
    [SerializeField]
    private Collider2D doorCollider;
    public bool ignoreFinishedAllTasks = false;

    InGameSoundManager soundManager;
  
    private void Start()
    {
        gameController = GameController.instance;
        soundManager = InGameSoundManager.instance;
    }

    public void OpenDoor()
    {
        soundManager.PlaySoundEffect(InGameSoundManager.SoundEffectType.DOOR);
        animator.SetBool("openDoor", true);
        animator.SetBool("closeDoor", false);
        doorCollider.enabled = false;
    }

    public void CloseDoor()
    {
        animator.SetBool("openDoor", false);
        animator.SetBool("closeDoor", true);
        doorCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameController.finishedAllTasks || ignoreFinishedAllTasks || !gameController.startedLevel || gameController.snapped)
                OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (animator.GetBool("openDoor"))
                CloseDoor();
        }
    }
}
