using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : ScriptObject
{
    public Vector2 upDownDist;
    public float rayDist;
    public float declination;
    public float hp;
    public int damage;

    private bool isOver = false;
    private bool isMove = false;
    private bool isAttack = false;

    private bool state = true; // true is Not jumping
    private SPUM_Prefabs prefab;

    private RaycastHit2D hit;

    private void Start()
    {
        prefab = gameObject.GetComponent<SPUM_Prefabs>();
        prefab.PlayAnimation(1);
        upDownDist = new Vector2(0, rectTransform.anchoredPosition.y);
    }
    private void Move()
    {
        rectTransform.anchoredPosition = (state) ? rectTransform.anchoredPosition - upDownDist * 2 : rectTransform.anchoredPosition + upDownDist * 2;
        state = !state;

        int num = !state ? 0 : 1;
        prefab.PlayAnimation(num);
    }
    // GameManager에서 입력 받는걸로 바꿔야 됨.
    private void Update()
    {
        if (hp < 0)
        {
            hp = 0;
            isOver = true;
            prefab.PlayAnimation(2);
            StartCoroutine(OnAnimateDeath());
        }

        if (!isOver)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                isAttack = true;
                prefab.PlayAnimation(4);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                isMove = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isOver)
        {
            if (isMove)
            {
                Move();
                isMove = false;
            }

            if (isAttack)
            {
                Attack();
                isAttack = false;
            }
            hp -= declination * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(string.Format("Collision Detected with {0}", collision.gameObject.tag));

        if (collision.gameObject.tag == "Destroyable" || collision.gameObject.tag == "Undestroyable")
        {
            hp -= damage;
            print("Ouch!");
            prefab.PlayAnimation(3);
            StartCoroutine(OnAnimateStun());
            collision.gameObject.SetActive(false);
        }
    }

    private void Attack()
    {
        Ray2D ray = new Ray2D(transform.position, transform.right);
        hit = Physics2D.Raycast(ray.origin, ray.direction, rayDist, LayerMask.GetMask("InPlayGame"));

        Debug.DrawRay(ray.origin, ray.direction, Color.red, rayDist);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Destroyable")
            {
                hit.collider.gameObject.SetActive(false);
            }

        }
    }

    private IEnumerator OnAnimateStun()
    {
        yield return new WaitForSeconds(0.5f);
        prefab.PlayAnimation(1);
    }

    private IEnumerator OnAnimateDeath()
    {
        yield return new WaitForSeconds(.5f);
        Time.timeScale = 0;
    }
}
