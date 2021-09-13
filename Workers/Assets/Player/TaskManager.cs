using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    private GameController gameController;

    [Header("UI")]
    [SerializeField]
    private RectTransform tasksUI;
    [SerializeField]
    private GameObject taskEntry;
    [SerializeField]
    private Sprite boxSprite;
    [SerializeField]
    private Sprite mopSprite;
    [SerializeField]
    private Sprite moneySprite;

    private List<Task> tasks = new List<Task>();
    private bool finishedAllTasks = false;

    private void Awake()
    {
        GenerateTasks();
        tasksUI.gameObject.SetActive(tasks.Count != 0);
    }

    private void Start()
    {
        gameController = GameController.instance;
    }

    private void CreateTaskEntry(Task.TaskType taskType, int requiredAmount, Sprite taskIcon)
    {
        GameObject _taskEntry = Instantiate(taskEntry, tasksUI);
        Task task = _taskEntry.GetComponent<Task>();
        task.taskType = taskType;
        task.requiredTaskAmount = requiredAmount;
        task.taskImage.sprite = taskIcon;
        tasks.Add(task);
    }

    public void DeliveredBox()
    {
        Task toBeRemovedTask = null;
        foreach (Task task in tasks)
        {
            if (task.taskType == Task.TaskType.BOX_COLLECTION)
            {
                task.TaskProgress();
                if (task.completedTaskAmount == task.requiredTaskAmount)
                    toBeRemovedTask = task;
            }
        }

        if (toBeRemovedTask != null)
        {
            Destroy(toBeRemovedTask.gameObject);
            tasks.Remove(toBeRemovedTask);
        }

        UpdateTaskCompletion();
    }

    private void GenerateTasks()
    {
        CreateTaskEntry(Task.TaskType.BOX_COLLECTION, 3, boxSprite);
        //CreateTaskEntry(Task.TaskType.MOPPING, 2, mopSprite);
        //CreateTaskEntry(Task.TaskType.MONEY_DELIVERY, 5, moneySprite);
    }

    private void UpdateTaskCompletion()
    {
        tasksUI.gameObject.SetActive(tasks.Count != 0);

        finishedAllTasks = tasks.Count == 0;
        gameController.finishedAllTasks = this.finishedAllTasks;
    }
}
