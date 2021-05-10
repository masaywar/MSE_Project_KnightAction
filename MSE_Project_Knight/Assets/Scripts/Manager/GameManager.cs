using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isOver = false;
    public float deltaTime;
    public RectTransform spawnRect;

    //For Debug

    string path = "Patterns/pattern_1";

    public enum GameState
    { 
        start, load, main
    };

    public GameState gameState = GameState.start;

    //For test
    public Pattern pattern;
    private void Awake()
    {
        deltaTime = Time.deltaTime;
        pattern = new Pattern(path);
        switch (gameState)
        {
            case GameState.start:
                gameState = GameState.load;
                break;

            case GameState.load:
                gameState = GameState.main;
                break;

            case GameState.main:
                break;
        }
    }


}
