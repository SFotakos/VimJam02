using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelEntry : MonoBehaviour
{
    Camera cam;
    CameraFollow cameraFollow;
    GameController gameController;
    
    void Start()
    {
        cam = Camera.main;
        cameraFollow = cam.GetComponent<CameraFollow>();
        gameController = GameController.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraFollow.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!gameController.snapped)
            {
                cameraFollow.enabled = true;
                collision.GetComponent<NavMeshAgent>().enabled = false;
                gameController.startedLevel = true;
            }

        }
    }
}
