using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WholeUI : UIWindow
{
    public Button startGame;

    public void OnClickStartGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.loadIngame;
    }
}
