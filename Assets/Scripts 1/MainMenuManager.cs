using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public int mainScene = 1;

    public void Play()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
