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
    public AudioSource audioSource;

    public TextMeshProUGUI comboText;

    public float ultGage = 0f;
    public float feverGage = 0f;

    public float ultRate;
    public float feverRate;
    
    [SerializeField]
    private int combo = 0;
    private int score;
    private int totalScore;

    public float feverTime;
    public float declination;
    public float hp;
    public bool isOver;
    public int damage;

    public PatternGenerator patternGenerator;

    // Player Event
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

    //EnemyEvent
    public delegate void dOnHitEnemy(GameObject obj, bool anim, bool force = false);
    public event dOnHitEnemy OnHitEnemy;

    //UIEvent
    public delegate void dOnFullUltGage(string name, bool activate);
    public event dOnFullUltGage OnFullUltGage;
    public delegate void dUIUpdatePlayerInfo(int combo, int score, float hp);
    public event dUIUpdatePlayerInfo UIUpdatePlayerInfo;


    private bool isFever = false;
    private bool canUlt = false;
    private List<List<Tuple<Vector2, string, float>>> transedPatterns = new List<List<Tuple<Vector2, string, float>>>();

    private void Start()
    {
        cachedObjectManager = ObjectManager.Instance;
        TransPattern();

        SoundManager.Instance.PlayMusic("Fable", true);
    }

    private void Update()
    {
        if (isOver) return;
        HpUpdate();
        if (feverGage >= 100 && !isFever)
        {
            feverGage = 0;
            OnFeverMode();
        }

        if (ultGage >= 100 && !canUlt)
        {
            canUlt = true;
            OnFullUltGage("Ult", true);
        }
        SpawnEnemy();

        UIUpdatePlayerInfo(combo, totalScore, hp);
    }

    private void HpUpdate()
    {
        if (!CheckPlayerDead())
        {
            hp -= declination * GameManager.Instance.deltaTime;
            declination += GameManager.Instance.deltaTime;
        }
        else 
        {
            OnPlayerDead();
            StartCoroutine(ExtensionMethod.DoWaitForSeconds(1f, ()=> {
                GameManager.Instance.Pause();
                UIManager.Instance.BlockAllOpenWindow();
            }));
        }
    }

    private bool CheckPlayerDead()
    {
        if (hp <= 0)
            isOver = true;

        return isOver;
    }

    private void Attack(RaycastHit2D[] hits, bool force = false)
    {
        if (hits == null)
        {
            combo = 0;
            SoundManager.Instance.PlayOneShot("Swing");
            hp -= damage;
            return;
        }
        foreach (var hit in hits)
        {
            DestroyEnemy(hit, isFever || force);
            SoundManager.Instance.PlayOneShot("Jab");
            if (!isFever) break;
        }
    }

    public void OnClickAttack()
    {
        if (isOver) return;

        var hits = OnPlayerAttack(0);
        Attack(hits);
    }

    public void OnClickJumpAttack()
    {
        if (isOver) return;

        var hits = OnPlayerJumpAttack(1);
        Attack(hits);
    }

    public void OnClickUlt()
    {
        ultGage = 0;

        var hits = OnPlayerUlt();
        foreach (var hit in hits)
        {
            DestroyEnemy(hit, true);
        }
        canUlt = false;
        OnFullUltGage("Ult", false);
    }

    private void SpawnEnemy()
    {
        if (isSpawning) return;

        isSpawning = true;
        var objectAttributes = transedPatterns[0];

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

            if (tag != "UnDestroyableEnemy")
                yield return new WaitForSeconds(wait);
            else
                yield return null;
        }

        isSpawning = false;
    }

    private void TransPattern()
    {
        Pattern[] patternArray = patternGenerator.GetAllPatterns();

        foreach (var pattern in patternArray)
        {
            if (pattern == null)
                continue;

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

            transedPatterns.Add(objectAttributes);
        }
    }

    private Tuple<Vector2, string, float> TransPatternAttr(bool isGround, bool isDestroyable, float wait)
    {
        var up = spawnPlace.GetChild(0).position;
        var down = spawnPlace.GetChild(1).position;

        Vector2 position = isGround ? up : down;
        string tag = isDestroyable ? "DestroyableEnemy" : "UnDestroyableEnemy";

        return new Tuple<Vector2, string, float>(position, tag, wait);
    }   

    private void DestroyEnemy(RaycastHit2D hit, bool force=false)
    {
        OnHitEnemy(hit.collider.gameObject, true, force);
        combo += 1;
        if (!canUlt)
            ultGage += ultRate;
        if (!isFever)
        {
            feverGage += feverRate;
        }
    }

    public void OnFeverMode()
    {
        if (!isFever)
        {
            isFever = true;
            OnPlayerFever(isFever);
            float tempFeverTime = feverTime;
            Time.timeScale = 2.5f;

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
        feverGage = 0;

        Time.timeScale = 1f;
    }

    public void Miss()
    {
        OnPlayerMiss();
        hp -= damage;
    }
}

