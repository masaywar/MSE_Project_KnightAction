using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

//EnemyController, PlayerController¿« subject

public class IngameController : Singleton<IngameController>
{
    private ObjectManager cachedObjectManager;

    [SerializeField]
    private bool isSpawning = false;

    public Transform spawnPlace;
    public Transform enemiesContainer;

    public TextMeshProUGUI comboText;

    public float ultGage = 0f;
    public float feverGage = 0f;

    public float ultRate;
    public float feverRate;

    private int combo = 0;
    private int score;
    private int totalScore;

    public float feverTime;
    public float declination;
    public float hp;

    public PatternGenerator patternGenerator;

    public delegate void dOnPlayerDead();
    public event dOnPlayerDead OnPlayerDead;

    public delegate RaycastHit2D[] dOnPlayerAttack(int attackMode);
    public event dOnPlayerAttack OnPlayerAttack;

    public delegate RaycastHit2D[] dOnPlayerJumpAttack(int attackMode);
    public event dOnPlayerJumpAttack OnPlayerJumpAttack;

    public delegate RaycastHit2D[] dOnPlayerUlt();
    public event dOnPlayerUlt OnPlayerUlt;

    public delegate void dOnPlayerMiss();
    public event dOnPlayerMiss OnPlayerMiss;

    public delegate void dOnPlayerFever(bool isFever);
    public event dOnPlayerFever OnPlayerFever;

    public delegate void dOnHitEnemy(GameObject obj, bool anim, bool force = false);
    public event dOnHitEnemy OnHitEnemy;

    private bool isFever = false;

    private void Start()
    {
        cachedObjectManager = ObjectManager.Instance;
    }

    private void Update()
    {
        hp -= declination * GameManager.Instance.deltaTime;

        if (feverGage >= 100)
        {
            feverGage = 0;
            OnFeverMode();
        }

        if (ultGage >= 100)
        { 
            
        }
        SpawnEnemy();
    }

    public void OnClickAttack()
    {
        var hits = OnPlayerAttack(0);
        DestroyEnemy(hits);
    }

    public void OnClickJumpAttack()
    {
        var hits = OnPlayerJumpAttack(1);
        DestroyEnemy(hits);
    }

    public void OnClickUlt()
    {
        OnPlayerUlt();
    }

    private void SpawnEnemy()
    {
        if (isSpawning) return;

        isSpawning = true;
        var objectAttributes = TransPattern(0);

        StartCoroutine(SpawnRoutine(objectAttributes));
    }

    private IEnumerator SpawnRoutine(List<Tuple<Vector2, string, float>> objectAttributes)
    {
        foreach (var obj in objectAttributes)
        {
            var position = obj.Item1;
            var tag = obj.Item2;
            var wait = obj.Item3;

            var spawned = cachedObjectManager.Spawn<EnemyObject>(tag, position, enemiesContainer);

            print(spawned.tag + wait);

            if (tag != "UnDestroyableEnemy")
                yield return new WaitForSeconds(wait);
            else
                yield return null;
        }

        isSpawning = false;
    }

    private List<Tuple<Vector2, string, float>>  TransPattern(int index)
    {
        Pattern pattern = patternGenerator.GetPattern(index);

        if (pattern == null)
            return null;

        List<Tuple<Vector2, string, float>> objectAttributes = new List<Tuple<Vector2, string, float>>();

        var patternAttributes = pattern.GetAttributes();
        patternAttributes.ForEach(attr =>
        {
            var position = attr.position;
            var type = attr.type;
            var wait = attr.wait;

            bool isGround = position == "D";
            bool isDestroyable = type == "B";

            var items = TransPatternAttr(isGround, isDestroyable, wait);
            objectAttributes.Add(items);
        });

        return objectAttributes;
    }

    private Tuple<Vector2, string, float> TransPatternAttr(bool isGround, bool isDestroyable, float wait)
    {
        var up = spawnPlace.GetChild(0).position;
        var down = spawnPlace.GetChild(1).position;

        Vector2 position = isGround ? up : down;
        string tag = isDestroyable ? "DestroyableEnemy" : "UnDestroyableEnemy";

        return new Tuple<Vector2, string, float>(position, tag, wait);
    }   

    private void DestroyEnemy(RaycastHit2D[] hits, bool force=false)
    {
        if (hits == null)
            return;

        foreach (var hit in hits)
        {
            if (hit.transform.tag == "UnDestroyableEnemy") 
                continue;

            OnHitEnemy(hit.collider.gameObject, true, force);
            ultGage += ultRate;
            feverGage += feverRate;
            if (!isFever) return;
        }
    }

    public void OnFeverMode()
    {
        if (!isFever)
        {
            isFever = true;
            OnPlayerFever(isFever);
            float tempFeverTime = feverTime;

            StartCoroutine(UpdateOnFever(tempFeverTime));
        }
    }

    private IEnumerator UpdateOnFever(float time)
    {
        while (feverTime > 0)
        {
            feverTime -= GameManager.Instance.deltaTime;
            yield return null;
        }

        isFever = false;
        OnPlayerFever(isFever);
        feverTime = time;
    }

}
