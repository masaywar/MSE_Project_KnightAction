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

    public RectTransform playerPanel;

    private UnitSprite unit;

    private void Start()
    {
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();

        unit = ObjectManager.Instance.Spawn<UnitSprite>(ClientUserData.knight, playerPanel.position);
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

    public void PlayerPanelUpdate()
    {
        if (unit == null)
            return;

        ObjectManager.Instance.Despawn<UnitSprite>(unit);
        unit = ObjectManager.Instance.Spawn<UnitSprite>(ClientUserData.knight, playerPanel.position);
    }
}
