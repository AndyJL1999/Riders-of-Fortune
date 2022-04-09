using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script is atteched to the 'Canvas' of each scene.
public class SceneController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
