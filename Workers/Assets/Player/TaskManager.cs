using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{ 
    private static TaskManager _instance;
    public static TaskManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TaskManager>();
            }
            return _instance;
        }
    }

    private GameController gameController;

    [Header("UI")]
    [SerializeField]
    private RectTransform tasksUI;
    [SerializeField]
    private GameObject taskEntry;

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
        gameController.finishedAllTasks = tasks.Count == 0;
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

    public bool hasTaskOfType(Task.TaskType taskType)
    {
        foreach (Task task in tasks)
        {
            if (task.taskType == taskType)
            {
                return true;
            }
        }
        return false;
    }

    private void GenerateTasks()
    {
        CreateTaskEntry(Task.TaskType.BOX_COLLECTION, 3);
        //CreateTaskEntry(Task.TaskType.MOPPING, 2);
        //CreateTaskEntry(Task.TaskType.MONEY_DELIVERY, 5);
    }

    private void CreateTaskEntry(Task.TaskType taskType, int requiredAmount)
    {
        GameObject _taskEntry = Instantiate(taskEntry, tasksUI);
        Task task = _taskEntry.GetComponent<Task>();
        task.InitializeTask(taskType, requiredAmount);
        tasks.Add(task);
    }

    private void UpdateTaskCompletion()
    {
        tasksUI.gameObject.SetActive(tasks.Count != 0);

        finishedAllTasks = tasks.Count == 0;
        gameController.finishedAllTasks = this.finishedAllTasks;
    }
}
