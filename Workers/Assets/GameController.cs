using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool isPaused = false;
    private bool canPause = true;
    public bool snapped = false;
    public bool finishedAllTasks = false;
    public bool startedLevel = false;
    public bool isTutorial = false;
    public bool disableStress { get { return (GetSceneType() == SceneType.BOXES_TUTORIAL || snapped || finishedAllTasks || !startedLevel); } }

    public enum DayEnum
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH
    }
    public DayEnum currentDay;

    public enum SceneType
    {
        BOXES_TUTORIAL,
        STRESS_TUTORIAL,
        FACTORY
    }

    private static GameController _instance;
    public static GameController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }

            if (_instance == null)
            {
                var obj = new GameObject("GameController");
                _instance = obj.AddComponent<GameController>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = FindObjectOfType<GameController>();
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.ToLower().Contains("tutorial"))
            isTutorial = true;

        //LockCursor();

        string currentDayString = PlayerPrefs.GetString("CurrentDay");
        currentDay = !string.IsNullOrEmpty(currentDayString) ? (DayEnum) System.Enum.Parse(typeof(DayEnum), currentDayString) : DayEnum.FIRST;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; ;
        if (!canPause && !isPaused)
            isPaused = true;
        UnlockCursor();
    }

    public void ResumeGame()
    {
        LockCursor();
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int index = SceneManager.GetActiveScene().buildIndex;
        if (currentScene.name.ToLower().Contains("tutorial"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        } else if (currentScene.name.ToLower().Contains("factory"))
        {
            if (currentDay != DayEnum.FOURTH)
            {
                currentDay++;
                SavePrefs();
                RestartScene();
            } else
            {
                //SceneManager.LoadScene("CreditsScene");
            }
        } 
    }

    public SceneType GetSceneType()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.Equals("TestScene"))
            return (SceneType) 99;
        else 
            return (SceneType) scene.buildIndex;
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetString("CurrentDay", currentDay.ToString());
    }
}
