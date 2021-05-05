using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : ScriptObject, Observable
{
    public int speed;

    public bool isDestroyable = true;
    public bool isUp = false;

    public Rigidbody2D despawnPlace;
    public Transform despawnContainer;


    private void FixedUpdate()
    {
        if (CheckValidPos())
            Move();
        else
            DestroyForced();
    }

    public virtual void Move()
    {
        rectTransform.anchoredPosition += Vector2.left * speed * GameManager.Instance.deltaTime;
    }

    public void OnEnemySpawn(bool onGround)
    {
        isUp = onGround ? false : true;
    }

    public void Destroy()
    {
        Destroy(false);
    }

    public void DestroyForced()
    {
        Destroy(true);
    }

    private void Destroy(bool force)
    {
        if (isDestroyable || force)
            gameObject.SetActive(false);
    }

    private bool CheckValidPos()
    {
        return despawnPlace.position.x < rectTransform.position.x * rectTransform.localScale.x;
    }

    private void OnDisable()
    {
        Initialize();
        ObjectManager.Instance.Despawn<ScriptObject>(this);
    }
}   
