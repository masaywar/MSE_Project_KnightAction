using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyObject
{
    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        
    }
}