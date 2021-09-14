using UnityEngine;

public class LevelExit : MonoBehaviour
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
            if (gameController.finishedAllTasks)
                cameraFollow.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("loadNewDay");
        }
    }
}
