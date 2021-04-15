using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForDebugCodes : MonoBehaviour
{
    public Player player;
    public Text text;
    private float life;


    private void Awake()
    {
       
    }

    private void Update()
    {
        text.text = player.hp.ToString();
    }
}
