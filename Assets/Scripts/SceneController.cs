using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // will change scene to string passed in
    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    // Reloads current scene
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Loads Title scene
    public void ToTitleScene()
    {
        GameController.instance.controlType = ControlType.Normal;
        SceneManager.LoadScene("Title");
    }

    // gets active scenes name
    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    // quits game
    public void QuitGame()
    {
        Application.Quit();
    }
}
