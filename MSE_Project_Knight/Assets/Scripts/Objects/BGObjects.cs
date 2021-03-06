using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGObjects : ScriptObject
{
    public RectTransform respawnPlace;
    public RectTransform despawnPlace;


    private void FixedUpdate()
    {
        rectTransform.position += Vector3.left * GameManager.Instance.deltaTime * speed;

        if (rectTransform.position.x < despawnPlace.position.x)
        {
            rectTransform.position = new Vector2(respawnPlace.position.x, rectTransform.position.y);
        }
    }
}
