using UnityEngine;

public class Money : MonoBehaviour
{
    GameController gameController;
    void Start()
    {
        gameController = GameController.instance;
        gameObject.SetActive(gameController.currentDay == GameController.DayEnum.THIRD);
    }
}
