using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public enum TaskType
    {
        BOX_COLLECTION,
        MOPPING,
        MONEY_DELIVERY
    }

    [HideInInspector] public int requiredTaskAmount;
    [HideInInspector] public int completedTaskAmount;
    [HideInInspector] public TaskType taskType;

    public Image taskImage;
    public TextMeshProUGUI taskText;

    public Task(TaskType taskType, int taskAmount, Sprite taskIcon)
    {
        this.taskType = taskType;
        this.requiredTaskAmount = taskAmount;
        this.taskImage.sprite = taskIcon;
        this.taskText.text = completedTaskAmount + "/" + requiredTaskAmount;
    }

    public void TaskProgress()
    {
        completedTaskAmount += 1;
        this.taskText.text = completedTaskAmount + "/" + requiredTaskAmount;
    }
}
