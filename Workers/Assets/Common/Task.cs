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

    [Header("UI")]
    public Image taskImage;
    public TextMeshProUGUI taskText;
    [SerializeField]
    private Sprite boxSprite;
    [SerializeField]
    private Sprite mopSprite;
    [SerializeField]
    private Sprite moneySprite;

    public void InitializeTask(TaskType taskType, int requiredTaskAmount)
    {
        this.taskType = taskType;
        this.requiredTaskAmount = requiredTaskAmount;
        switch (taskType)
        {
            case TaskType.BOX_COLLECTION:
                taskImage.sprite = boxSprite;
                break;
            case TaskType.MOPPING:
                taskImage.sprite = mopSprite;
                break;
            case TaskType.MONEY_DELIVERY:
                taskImage.sprite = moneySprite;
                break;
        }
        this.taskText.text = completedTaskAmount + "/" + requiredTaskAmount;
    }

    public void TaskProgress()
    {
        completedTaskAmount += 1;
        this.taskText.text = completedTaskAmount + "/" + requiredTaskAmount;
    }
}
