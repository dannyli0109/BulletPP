using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class BTSManager : MonoBehaviour
{
    public int openingMenuSceneIndex = 0;
    public int firstRoomSceneIndex = 1;
    public int loseGameSceneIndex = 2;
    public int winGameSceneIndex = 3;
    void Start()
    {
     //   ScreenCapture.CaptureScreenshot("UNITY BULLET ++");
    }

    void Update()
    {
        
    }

    public void LoadFirstRoomScene()
    {
        SceneManager.LoadScene(firstRoomSceneIndex);
    }

    public void LoadOpeningMenuScene()
    {
        SceneManager.LoadScene(openingMenuSceneIndex);
    }

    public void LoadLoseGameScene()
    {
        Debug.Log("BTS load lose");
        SceneManager.LoadScene(loseGameSceneIndex);
    }

    public void LoadWinGameScene()
    {
        SceneManager.LoadScene(winGameSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
