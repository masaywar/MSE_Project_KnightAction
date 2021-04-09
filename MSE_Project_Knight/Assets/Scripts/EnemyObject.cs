using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : ScriptObject
{
    public int speed;
    public bool destroyable = true;
    public int givenScore;
    public virtual void Move()
    {
        rectTransform.anchoredPosition += Vector2.left*speed*Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Destroy()
    {
        if (destroyable)
        {
            gameObject.SetActive(false);
        }
    }
}
