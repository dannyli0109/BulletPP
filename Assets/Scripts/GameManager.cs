using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Game,
    Shop,
    Casual,
    Transitional,
    Pause
}


public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [SerializeField]
    private GameState gameState = GameState.Shop;
    public Shop shop;
    public Animator transition;
    public float transitionTime;


    void Awake()
    {
        current = this;
    }
    private void Start()
    {
        transition.speed = 1.0f / transitionTime;
    }

    void Update()
    {
        if (gameState == GameState.Shop)
        {
            shop?.gameObject.SetActive(true);
          
        }
        else
        {
            shop?.gameObject.SetActive(false);
        }
    }

    public void ChangeStateImmdeiate(GameState newState)
    {
        gameState = newState;
    }

    public void ChangeState(GameState newState)
    {
        if (newState != gameState)
        {
            StartCoroutine(TransitionState(newState));
        }
    }

    void UpdateBackgroundMusic()
    {

    }

    public bool GameTransitional()
    {
        return gameState == GameState.Shop || gameState == GameState.Transitional||gameState==GameState.Pause;
    }

    public GameState GetState()
    {
        return gameState;
    }

    IEnumerator TransitionState(GameState newState)
    {
        transition.speed = 1.0f / transitionTime;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        gameState = newState;
        transition.SetTrigger("End");
    }
}
