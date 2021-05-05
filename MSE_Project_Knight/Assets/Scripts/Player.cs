using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : ScriptObject, Observable
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

    public RectTransform upTransform;
    public RectTransform downTransform;
    public RectTransform feverTransform;

    public enum State
    { 
        idle,
        Over,
        Fever
    }
    public enum AttackMode
    { 
        Normal,
        Jump,
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

            hp -= declination * deltaTime;
            return false;
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

    private void Attack()
    {
        if (TrySetRaycastHitByAttackMode(attackMode, out hits))
        {
            foreach (var hit in hits)
            {
                OnRaycastHit(hit.collider);

                if (state != State.Fever) break;
            }
        }

    }

    private bool TrySetRaycastHitByAttackMode(AttackMode attackMode, out RaycastHit2D[] hits)
    {

        switch (attackMode)
        {
            case AttackMode.Normal:
                hits = Physics2D.BoxCastAll(
                    downTransform.position,
                    downTransform.sizeDelta.normalized,
                    0,
                    Vector2.right,
                    rayDist, LayerMask.GetMask("InPlayGame"));

                Debug.DrawRay(downTransform.position, Vector2.right * rayDist, Color.red);
                break;

            case AttackMode.Jump:
                hits = Physics2D.BoxCastAll(
                    upTransform.position,
                    upTransform.sizeDelta.normalized,
                    0,
                    Vector2.right,
                    rayDist, LayerMask.GetMask("InPlayGame"));

                Debug.DrawRay(upTransform.position, Vector2.right * rayDist, Color.red);
                break;

            case AttackMode.Fever:
                hits = Physics2D.BoxCastAll(
                    collider.bounds.center, 
                    new Vector2(collider.bounds.size.x, collider.bounds.size.y * 10), 
                    0f, 
                    Vector2.right, 
                    rayDist, LayerMask.GetMask("InPlayGame"));

                break;

            default:
                hits = null;
                break;
        }

        return hits.Length > 0; 
    }

    private void OnRaycastHit(Collider2D collider, bool isUlt=false)
    {
        if (collider != null && collider)
        {
            collider.gameObject.SetActive(false);

            if (!isUlt)
            {
                UpdateFeverGage();
                UpdateUltGage();
            }
        }
    }


    private void Ult()
    {
        Physics2D.BoxCastAll(
            Vector2.zero,
            new Vector2(16,9),
            0,
            Vector2.zero,
            0, LayerMask.GetMask("InPlayGame")
            ).ForEach<RaycastHit2D>(hit=> OnRaycastHit(hit.collider));
    }

    // Will Remove OnClickMethod.

    public void OnclickAttack()
    {
        if (attackMode != AttackMode.Fever)
            attackMode = AttackMode.Normal;

        Attack();
    }   

    public void OnClickJump()
    {
        if(attackMode != AttackMode.Fever)
            attackMode = AttackMode.Jump;

        Attack();
    }

    public void OnClickUlt()
    {
        Ult();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Destroyable" || collision.gameObject.tag == "Undestroyable")
        {
            hp -= damage;
            StartCoroutine(OnAnimateStun());
            //collision.gameObject.SetActive(false);
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
        if (feverGage < 100)
            feverGage += feverRate;

        else
        {
            feverGage = 0;
            state = State.Fever;
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
