using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WholeUI : UIWindow
{
    public Button startGame;

    public TextMeshProUGUI username;
    public TextMeshProUGUI coin;

    private void Start()
    {
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();
    }

    public void UIUpdate()
    {
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();
    }

    public void OnClickStartGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.loadIngame;
    }

    private void Update()
    {
        UIUpdate();
    }
}
