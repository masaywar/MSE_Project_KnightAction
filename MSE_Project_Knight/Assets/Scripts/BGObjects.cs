using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGObjects : ScriptObject
{
    public int speed;
    public bool isInvisible;

    public RectTransform respawnPlace;
    public RectTransform despawnPlace;

    private void Update()
    {
        rectTransform.anchoredPosition += Vector2.left * GameManager.Instance.deltaTime * speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        rectTransform.anchoredPosition = respawnPlace.anchoredPosition;
    }
}
