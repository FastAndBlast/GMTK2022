using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public int mainScene = 1;
    public int credits = 2;
    public void Play()
    {
        SceneManager.LoadScene(mainScene);
    }

    public void Credits()
    {
        SceneManager.LoadScene(credits);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
