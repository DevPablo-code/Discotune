using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public int startScene = 1;
    public GameObject settingsPanel;

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void QuitGame() 
    {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void OpenSettings() 
    {
        settingsPanel.SetActive(true);
    }
}
