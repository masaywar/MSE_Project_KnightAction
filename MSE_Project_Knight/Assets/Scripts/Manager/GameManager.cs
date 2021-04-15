using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isOver = false;
    public float deltaTime;
    public RectTransform spawnRect;

    private bool isCoroutineActive = false;

    private void Awake()
    {
        deltaTime = Time.deltaTime;
    }

    private void Update()
    {
        if (!isOver) 
        {
            if(!isCoroutineActive)
                StartCoroutine(SpawnEnemy("DestroyableEnemy"));
        }
    }

    private void FixedUpdate()
    {
        
    }

    private IEnumerator SpawnEnemy(string type) 
    {
        isCoroutineActive = true;
        yield return new WaitForSeconds(3f);
        ObjectManager.Instance.Spawn<EnemyObject>(type, spawnRect.position, spawnRect);
        isCoroutineActive = false;
    }
}
