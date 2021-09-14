using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplay : MonoBehaviour
{
    [Header("UI Variables")]
    [Space(5)]
    [SerializeField] GameObject dialogUI;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] Image portraitImage;
    [SerializeField] List<Sprite> portraitList;

    // Typewritter effect
    public float typewriterDefaultCharacterRevealTime = 0.045f;
    private Coroutine typewriterCoroutine = null;
    private bool typewriter = false;

    // Dialog
    private DialogManager dialogManager;
    private Dialog currentDialog;

    private static DialogDisplay _instance = null;
    public static DialogDisplay instance
    {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(DialogDisplay)) as DialogDisplay;
            }

            if (_instance == null)
            {
                var obj = new GameObject("DialogDisplay");
                _instance = obj.AddComponent<DialogDisplay>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        dialogManager = DialogManager.instance;
    }

    private void Update()
    {
        if (IsDialogBeingShown() && Input.anyKeyDown)
        {
            Continue();
        }
    }

    private void OnApplicationQuit()
    {
        _instance = null;
    }

    public void ShowDialog(Dialog dialog, bool typewriter = false)
    {
        currentDialog = dialog;
        dialogUI.SetActive(true);
        this.typewriter = typewriter;
        ShowLine(dialogManager.GetUnreadLine(dialog));
        Time.timeScale = 0f;
    }

    public void ShowLine(Line line)
    {
        Sprite portraitSprite = null;
        if (line.speakerId - 1 >= 0 && line.speakerId <= portraitList.Count)
        {
            portraitSprite = portraitList[line.speakerId - 1];
        }
        portraitImage.gameObject.SetActive(portraitSprite != null);
        if (portraitImage)
            portraitImage.sprite = portraitSprite;

        string text = line.text;
        if (typewriter)
        {
            // This allows for the textInfo to be populated without flashing the whole text on the screen
            dialogText.pageToDisplay = 1;
            dialogText.maxVisibleCharacters = 0;
            dialogText.text = text;
            dialogText.ForceMeshUpdate(true);

            if (typewriterCoroutine != null)
                StopCoroutine(typewriterCoroutine);

            typewriterCoroutine = StartCoroutine(TypewriterEffect());
        }
        else
        {
            dialogText.text = text;
            dialogText.maxVisibleCharacters = dialogText.text.Length;
        }
    }

    public void HideDialog()
    {
        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);
        dialogText.text = "";
        dialogText.pageToDisplay = 1;
        dialogUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Continue()
    {
        int currentPageLastCharacterIndex = dialogText.textInfo.pageInfo[dialogText.pageToDisplay - 1].lastCharacterIndex;
        // There's no more text and no more characters to show
        if (dialogText.pageToDisplay == dialogText.textInfo.pageCount && dialogText.maxVisibleCharacters >= currentPageLastCharacterIndex + 1)
        {
            Line line = dialogManager.GetUnreadLine(currentDialog);
            if (line != null)
                ShowLine(line);
            else
                HideDialog();
        }
        else
        {
            if (this.typewriter)
            {
                if (currentPageLastCharacterIndex == dialogText.maxVisibleCharacters-1)
                {
                    dialogText.pageToDisplay++;
                    if (typewriterCoroutine != null)
                        StopCoroutine(typewriterCoroutine);

                    typewriterCoroutine = StartCoroutine(TypewriterEffect());
                } else
                {
                    if (typewriterCoroutine != null)
                        StopCoroutine(typewriterCoroutine);
                    dialogText.maxVisibleCharacters = currentPageLastCharacterIndex + 1;
                }                
            } else
            {
                dialogText.pageToDisplay++;
            }           
        }
    }

    public bool IsDialogBeingShown()
    {
        return dialogUI.activeSelf;
    }

    IEnumerator TypewriterEffect()
    { 
        int currentPage = dialogText.pageToDisplay-1;
        int currentPageFirstCharacter = dialogText.textInfo.pageInfo[currentPage].firstCharacterIndex;
        int currentPageLastCharacter = dialogText.textInfo.pageInfo[currentPage].lastCharacterIndex;
        for (int visibleCount = currentPageFirstCharacter; visibleCount <= currentPageLastCharacter + 1; visibleCount++)
        {
            yield return new WaitForSecondsRealtime(typewriterDefaultCharacterRevealTime);
            dialogText.maxVisibleCharacters = visibleCount;
        }

    }
}
