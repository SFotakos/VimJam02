using System.Collections.Generic;
using UnityEngine;

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

            if (_instance == null)
            {
                var obj = new GameObject("TaskManager");
                _instance = obj.AddComponent<TaskManager>();
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
    private bool increasedTaskAmount = false;

    private void Start()
    {
        gameController = GameController.instance;
        gameController.finishedAllTasks = false;
        increasedTaskAmount = bool.Parse(PlayerPrefs.GetString("IncreasedTasksAmount"));

        if (gameController.GetSceneType() == GameController.SceneType.STRESS_TUTORIAL)
        {
            GenerateTasks();
        }
    }

    public void DoTask(Task.TaskType taskType)
    {
        foreach (Task task in tasks)
        {
            if (task.taskType == taskType)
            {
                task.TaskProgress();
            }
        }

        UpdateTaskCompletion();
    }

    public bool hasTaskOfType(Task.TaskType taskType)
    {
        foreach (Task task in tasks)
        {
            if (task.taskType == taskType && task.completedTaskAmount < task.requiredTaskAmount)
            {
                return true;
            }
        }
        return false;
    }

    public void GenerateTasks()
    {
        switch (gameController.GetSceneType())
        {
            case GameController.SceneType.BOXES_TUTORIAL:
                CreateTaskEntry(Task.TaskType.BOX_COLLECTION, 3);
                break;
            case GameController.SceneType.STRESS_TUTORIAL:
                CreateTaskEntry(Task.TaskType.BOX_COLLECTION, 2);
                break;
            case GameController.SceneType.FACTORY:
                switch (gameController.currentDay)
                {
                    case GameController.DayEnum.FIRST:
                        CreateTaskEntry(Task.TaskType.BOX_COLLECTION);
                        break;
                    case GameController.DayEnum.SECOND:
                        CreateTaskEntry(Task.TaskType.BOX_COLLECTION);
                        CreateTaskEntry(Task.TaskType.MOPPING);
                        break;
                    case GameController.DayEnum.THIRD:
                        CreateTaskEntry(Task.TaskType.BOX_COLLECTION);
                        CreateTaskEntry(Task.TaskType.MOPPING);
                        CreateTaskEntry(Task.TaskType.MONEY_DELIVERY);
                        break;
                    case GameController.DayEnum.FOURTH:
                        CreateTaskEntry(Task.TaskType.BOX_COLLECTION);
                        break;
                }
                break;
            default:
                CreateTaskEntry(Task.TaskType.BOX_COLLECTION, 3);
                break;
        }        
        tasksUI.gameObject.SetActive(tasks.Count != 0);
    }

    public int GetTaskAmount(Task.TaskType taskType)
    {
        int taskAmount = 0;
        switch (taskType)
        {
            case Task.TaskType.BOX_COLLECTION:
                switch (gameController.currentDay)
                {
                    case GameController.DayEnum.FIRST:
                        taskAmount = 3;
                        break;
                    case GameController.DayEnum.SECOND:
                        taskAmount = 4;
                        break;
                    case GameController.DayEnum.THIRD:
                        taskAmount = 5;
                        break;
                    case GameController.DayEnum.FOURTH:
                        taskAmount = 4;
                        break;
                }
                break;
            case Task.TaskType.MOPPING:
                switch (gameController.currentDay)
                {
                    case GameController.DayEnum.SECOND:
                        taskAmount = 2;
                        break;
                    case GameController.DayEnum.THIRD:
                        taskAmount = 3;
                        break;
                }
                break;
            case Task.TaskType.MONEY_DELIVERY:
                taskAmount = 3;
                break;
        }
        if ((taskType == Task.TaskType.BOX_COLLECTION || taskType == Task.TaskType.MOPPING) && increasedTaskAmount)
            taskAmount++;

        return taskAmount;
    }

    private void CreateTaskEntry(Task.TaskType taskType)
    {
        GameObject _taskEntry = Instantiate(taskEntry, tasksUI);
        Task task = _taskEntry.GetComponent<Task>();
        task.InitializeTask(taskType, GetTaskAmount(taskType));
        tasks.Add(task);
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
        bool _finishedAllTasks = true;
        foreach (Task task in tasks) {
            if (task.completedTaskAmount != task.requiredTaskAmount)
                _finishedAllTasks = false;
        }
        gameController.finishedAllTasks = _finishedAllTasks;
    }
}
