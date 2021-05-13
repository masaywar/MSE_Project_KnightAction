using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameController : Singleton<InGameController>
{
    private System.Random random = new System.Random();

    private ObjectManager cachedObjectManager;
    private bool isSpawning = false;

    private float waitTime = 0.2f;
    private WaitForSeconds wait;

    public Transform spawnPlace;
    public Transform activeContainer;

    public Player player;

    public List<EnemyObject> enemyList = new List<EnemyObject>();

    public Pattern pattern;

    private void Start()
    {
        cachedObjectManager = ObjectManager.Instance;

        activeContainer = spawnPlace.GetChild(2);
        wait = new WaitForSeconds(waitTime);

        pattern = GameManager.Instance.pattern;
    }

    private void Update()
    {
        SpawnEnemy(0);
    }

    public void SpawnEnemy(int index)
    {
        if (isSpawning)
            return;

        isSpawning = true;

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        foreach (var e in pattern.itemList)
        {
            bool isGround = e.position == "D" ? true : false;
            string tag = e.type == "B" ? "DestroyableEnemy" : "UnDestroyableEnemy";

            int spawnIdx = isGround ? 0 : 1;
            var spawned = cachedObjectManager.Spawn<EnemyObject>(tag, spawnPlace.GetChild(spawnIdx).position, spawnPlace.GetChild(2));
            enemyList.Add(spawned);

            yield return new WaitForSeconds(e.wait); 
        }

        isSpawning = false;
    }

    public void OnDestroyEnemy(GameObject enemy, bool force = false)
    {
        EnemyObject tempEnemy = null;
        int findIdx = 0;

        tempEnemy = enemyList[findIdx = enemyList.FindIndex(e => enemy.Equals(e.gameObject))];

        enemyList.RemoveAt(findIdx);
        tempEnemy.DestroyWithAnim(force);
    }

    public void Unsubscribe(EnemyObject enemy)
    {
        enemyList.Remove(enemy);
    }
}
