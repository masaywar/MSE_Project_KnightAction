using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : ScriptObject
{

    // Will remove feverTime variable.
    public float feverTime;

    public float rayDist;
    public float declination;
    public float hp;

    public int damage;

    public float ultGage = 0f;
    public float feverGage = 0f;

    public float ultRate;
    public float feverRate;

    public Transform upTransform;
    public Transform downTransform;
    public Transform feverTransform;


    public enum State
    { 
        idle,
        Over,
        Fever
    }
    public enum AttackMode
    { 
        Normal,
        Fever,

    }

    public State state;
    public AttackMode attackMode;

    private bool isGround = true; // true is Not jumping
    private bool isFever = false;

    private SPUM_Prefabs prefab;
    private RaycastHit2D[] hits;

    private float deltaTime;

    private void Start()
    {
        deltaTime = GameManager.Instance.deltaTime;
        collider = gameObject.GetComponent<BoxCollider2D>();
        prefab = gameObject.GetComponent<SPUM_Prefabs>();
        prefab.PlayAnimation(1);

        state = State.idle;
        attackMode = AttackMode.Normal;
    }   

    private void FixedUpdate()
    {
        if (!CheckPlayerDead())
        {
            OnPlayerAlive();
            UpdateByState();
        }
    }
    
    private bool CheckPlayerDead()
        {
            if (hp < 0)
            {
                hp = 0;
                state = State.Over;
                return true;
            }

            return false;
        }

    private void OnPlayerAlive()
        {
            hp -= declination * deltaTime;
        }

    private void UpdateByState()
        {
            switch (state)
            {
            case State.idle:
                break;

            case State.Over:
                StartCoroutine(OnAnimateDeath());
                break;

            case State.Fever:
                OnFeverMode();
                break;
            }
        }

    private void Move()
    {
        if (state == State.Fever)
            return;

        rectTransform.position = isGround ? upTransform.position : downTransform.position;

        isGround = !isGround;
        int num = !isGround ? 0 : 1;
        prefab.PlayAnimation(num);
    }

    private void Attack()
     {

        hits = attackMode == AttackMode.Normal ?
            Physics2D.BoxCastAll(collider.bounds.center, collider.bounds.size, 0f, Vector2.right, rayDist, LayerMask.GetMask("InPlayGame")) :
            Physics2D.BoxCastAll(collider.bounds.center, new Vector2(collider.bounds.size.x, collider.bounds.size.y*10), 0f, Vector2.right, rayDist, LayerMask.GetMask("InPlayGame"));

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Destroyable")
            {
                hit.collider.gameObject.SetActive(false);
                UpdateFeverGage();
                UpdateUltGage();
            }
        }
    }

    private void Ult()
    {
        ObjectManager.Instance.DespawnAllByTag<EnemyObject>("Destroyable");
        ObjectManager.Instance.DespawnAllByTag<EnemyObject>("Undestroyables");
    }

    // Will Remove OnClickMethod.

    public void OnclickAttack()
    {
        Attack();
        prefab.PlayAnimation(4);
    }

    public void OnClickJump()
    {
        Move();
    }

    public void OnClickUlt()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(string.Format("Collision Detected with {0}", collision.gameObject.tag));

        if (collision.gameObject.tag == "Destroyable" || collision.gameObject.tag == "Undestroyable")
        {
            hp -= damage;
            StartCoroutine(OnAnimateStun());
            collision.gameObject.SetActive(false);
        }
    }
    
    private void UpdateUltGage()
    {
        ultGage += ultRate;
        if (ultGage >= 100)
        {
            ultGage = 100;
            //UltButton Activate.
        }
    }

    private void UpdateFeverGage()
    {
        if (feverGage >= 100)
        {
            feverGage = 0;
            state = State.Fever;
        }
        else
        {
            feverGage += feverRate;
        }
    }

    private void OnFeverMode()
    {
        attackMode = AttackMode.Fever;

        Vector2 tempPosition = rectTransform.position;

        rectTransform.position = feverTransform.position;

        if (!isFever)
        {
            isFever = true;
            StartCoroutine(OnFeverState(tempPosition));
        }
    }

    private IEnumerator OnAnimateStun()
    {
        prefab.PlayAnimation(3);
        yield return new WaitForSeconds(0.5f);
        prefab.PlayAnimation(1);
    }

    private IEnumerator OnAnimateDeath()
    {
        prefab.PlayAnimation(2);
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 0;
    }

    private IEnumerator OnFeverState(Vector2 tempPos)
    {
        float tempTime = feverTime;
        while (feverTime > 0)
        {
            feverTime -= 1 * deltaTime; 
            yield return null;
        }

        attackMode = AttackMode.Normal;
        state = State.idle;
        isFever = false;
        feverTime = tempTime;
        rectTransform.position = tempPos;
    }
}
