using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

//EnemyController, PlayerController¿« subject

public class TempIngameController : Singleton<TempIngameController>
{
    private ObjectManager cachedObjectManager;
    private bool isSpawning = false;

    public Transform spawnPlace;
    public Transform activeContainer;

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

    private void DestroyEnemy(RaycastHit2D[] hits, bool force=false)
    {
        if (hits == null)
            return;

        hits.ForEach(hit => {
            OnHitEnemy(hit.collider.gameObject, true, force);
            ultGage += ultRate;
            feverGage += feverRate;
            if (!isFever) return;
        });
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
