using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InGameController : Singleton<InGameController>
{
    private System.Random random = new System.Random();
    private float time = 0;


    private ObjectManager cachedObjectManager;
    private bool isSpawning = false;

    private float waitTime = 0.2f;

    public Transform spawnPlace;
    public Transform activeContainer;

    public Player player;
    public Pattern pattern;

    private List<IObserver> observers = new List<IObserver>(); 

    private void Start()
    {
        cachedObjectManager = ObjectManager.Instance;
        activeContainer = spawnPlace.GetChild(2);

        pattern = GameManager.Instance.pattern;
    }

    private void Update()
    {
        SpawnEnemy(0);
        time += GameManager.Instance.deltaTime;
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

            Subscribe(spawned);

            yield return new WaitForSeconds(e.wait); 
        }

        isSpawning = false;
    }

    public void OnDestroyEnemy(GameObject enemy, bool force = false)
    {
        int index = observers.FindIndex(o => (o as ScriptObject).gameObject.Equals(enemy));

        var tempEnemy = observers[index];

        Unsubscribe(tempEnemy);

        Notify(tempEnemy, ()=>
        {
            var temp = (EnemyObject)tempEnemy;
            temp.DestroyWithAnim(force);
        });
    }

    public void OnPlayerDead()
    {
        cachedObjectManager.allObjectDict.ForEach(pair => pair.Value.ForEach(e => e.Stop()));
    }

    public void Subscribe(IObserver o)
    {
        observers.Add(o);   
    }

    public void Unsubscribe(IObserver o)
    {
        observers.Remove(o);
    }

    public void PlayerAttack()
    {
        player.OnclickAttack();  
    }

    public void PlayerJump()
    {
        player.OnClickJump();
    }

    public void PlayerUlt()
    {
        player.OnClickUlt();
    }

    public void OnUlt()
    {
        var temp = observers.Find(o => o.GetType() == typeof(GamePlayUI));
        (temp as GamePlayUI).ActivateUlt();
    }

    public void Notify(IObserver o, Action action)
    {
        action();
    }
}
