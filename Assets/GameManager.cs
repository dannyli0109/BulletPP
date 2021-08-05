using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Game,
    Shop,
    Casual
}


public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public GameState gameState = GameState.Game;
    public Shop shop;

    void Awake()
    {
        current = this;
    }

    void Update()
    {
        if (gameState == GameState.Shop)
        {
            shop.gameObject.SetActive(true);
          
        }
        else
        {
            shop.gameObject.SetActive(false);
        }
    }
}
