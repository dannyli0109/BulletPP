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

    void Awake()
    {
        current = this;
    }
}
