using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private bool isPaused = false;
    private bool canPause = true;
    public bool snapped = false;
    public bool finishedAllTasks = false;
    public bool startedLevel = false;

    public enum DayEnum
    {
        FIRST,
        SECOND,
        THIRD
    }
    public DayEnum dayController = DayEnum.FIRST;

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
        //LockCursor();
    }

    public void Restart()
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
}
