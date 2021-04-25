using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isOver = false;
    public float deltaTime;
    public RectTransform spawnRect;

    public enum GameState
    { 
        start, load, main
    };

    public GameState gameState = GameState.start;

    private bool isCoroutineActive = false;


    private void Awake()
    {
        deltaTime = Time.deltaTime;
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

    private void Update()
    {
        if (!isOver) 
        {
            if(!isCoroutineActive)
                StartCoroutine(SpawnEnemy("DestroyableEnemy"));

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                print(touch.position);
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator SpawnEnemy(string type) 
    {
        isCoroutineActive = true;
        yield return new WaitForSeconds(3f);
        EnemyObject spawnedObj = ObjectManager.Instance.Spawn<EnemyObject>(type, spawnRect.position, spawnRect);
        print(spawnedObj.transform.position);
        isCoroutineActive = false;
    }
}
