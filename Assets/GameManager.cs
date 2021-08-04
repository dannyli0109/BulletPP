using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Game,
    Shop
}


public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public GameState gameState = GameState.Shop;
    public GameObject shop;

    void Awake()
    {
        current = this;
    }

    void Update()
    {
        if (gameState == GameState.Shop)
        {
            shop.SetActive(true);
        }
        else
        {
            shop.SetActive(false);
        }
    }
}
