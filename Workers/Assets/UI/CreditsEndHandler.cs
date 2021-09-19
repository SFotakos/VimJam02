using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsEndHandler : MonoBehaviour
{
    void OnCreditsEnd()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
