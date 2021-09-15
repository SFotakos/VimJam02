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
  
    private void Start()
    {
        gameController = GameController.instance;
    }

    public void OpenDoor()
    {
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
            if (gameController.finishedAllTasks || ignoreFinishedAllTasks)
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
