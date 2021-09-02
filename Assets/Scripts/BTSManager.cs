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

    public Animator transition;
    public float transitionTime = 1.0f;
    void Start()
    {
        transition.speed = transition.speed = 1.0f / transitionTime;
    }

    void Update()
    {
        
    }

    public void LoadFirstRoomScene()
    {
        StartCoroutine(LoadLevel(firstRoomSceneIndex));
    }

    public void LoadOpeningMenuScene()
    {
        StartCoroutine(LoadLevel(openingMenuSceneIndex));
    }

    public void LoadLoseGameScene()
    {
        StartCoroutine(LoadLevel(loseGameSceneIndex));
    }

    public void LoadWinGameScene()
    {
        StartCoroutine(LoadLevel(winGameSceneIndex));
    }

    IEnumerator LoadLevel(int index)
    {
        transition.speed = 1.0f / transitionTime;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
