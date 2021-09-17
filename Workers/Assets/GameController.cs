using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool isPaused = false;
    private bool canPause = true;
    private bool _snapped = false;
    public bool snapped
    {
        get => _snapped;
        set
        {
            _snapped = value;
            NewWorker();
        }
    }
    public bool finishedAllTasks = false;
    public bool startedLevel = false;
    public bool isTutorial = false;
    public bool disableStress { get { return (GetSceneType() == SceneType.BOXES_TUTORIAL || snapped || finishedAllTasks || !startedLevel); } }

    public RectTransform crossfade;
    Animator crossfadeAnimator;
    Image crossfadeImage;
    TextMeshProUGUI crossfadeText;
    public float timeBetweenTransitions = 1.5f;

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

    private void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerSprite"))
        {
            PlayerPrefs.SetInt("PlayerSprite", Random.Range(0, 2));
            PlayerPrefs.Save();
        }
    }

    private void Awake()
    {
        crossfadeImage = crossfade.GetComponentInChildren<Image>();
        crossfadeText = crossfade.GetComponentInChildren<TextMeshProUGUI>();
        crossfadeAnimator = crossfade.GetComponentInChildren<Animator>();

        string currentDayString = PlayerPrefs.GetString("CurrentDay");
        currentDay = !string.IsNullOrEmpty(currentDayString) ? (DayEnum)System.Enum.Parse(typeof(DayEnum), currentDayString) : DayEnum.FIRST;

        _instance = FindObjectOfType<GameController>();
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName.ToLower().Contains("tutorial"))
        {
            isTutorial = true;
            PlayerPrefs.DeleteAll();

            crossfadeText.text = "TRAINING DAY.";
        } else if (sceneName.ToLower().Contains("factory"))
        {
            crossfadeText.text = currentDay.ToString().ToUpper() + " DAY.";
        }

        //LockCursor();
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
        if (!isTutorial)
            crossfadeAnimator.SetTrigger("out");
        StartCoroutine(NextSceneAfterAnimation(!isTutorial ? timeBetweenTransitions : 0f));
    }

    public SceneType GetSceneType()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneType sceneType = SceneType.FACTORY;
        if (scene.name.Equals("FirstTutorial"))
        {
            sceneType = SceneType.BOXES_TUTORIAL;
        } else if (scene.name.Equals("SecondTutorial"))
        {
            sceneType = SceneType.STRESS_TUTORIAL;
        }
        return sceneType;
    }

    public int GetPlayerSprite()
    {
        return PlayerPrefs.GetInt("PlayerSprite");
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetString("CurrentDay", currentDay.ToString());
    }

    IEnumerator NextSceneAfterAnimation(float timeBetweenTransition)
    {
        yield return new WaitForSeconds(timeBetweenTransition);
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.ToLower().Contains("tutorial"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (currentScene.name.ToLower().Contains("factory"))
        {
            if (currentDay != DayEnum.FOURTH)
            {
                currentDay++;
                SavePrefs();
                RestartScene();
            }
            else
            {
                //SceneManager.LoadScene("CreditsScene");
            }
        }
    }

    public bool IsNewWorker()
    {
        return PlayerPrefs.HasKey("NewWorker");
    }

    public void NewWorker()
    {
        PlayerPrefs.SetString("NewWorker", "true");
        PlayerPrefs.DeleteKey("PlayerSprite");
        PlayerPrefs.Save();
    }
}