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
    public TextMeshProUGUI knightName;

    [SerializeField]
    private RectTransform playerPanel;
    private UnitSprite unit;

    private string spriteString = "Sprite";

    private void Start()
    {
#if TEST
        unit.transform.SetParent(playerPanel);
        unit.transform.localPosition = Vector3.zero;
#else
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();

        unit = ObjectManager.Instance.Spawn<UnitSprite>(ClientUserData.knight + spriteString, playerPanel);
        unit.transform.localPosition = Vector3.zero;

        knightName.text = ClientUserData.knight;
#endif
    }

    public void UIUpdate()
    {
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();
    }

    public void OnClickStartGame()
    {
        unit.DespawnSprite();
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

        unit.DespawnSprite();
        unit = ObjectManager.Instance.Spawn<UnitSprite>(ClientUserData.knight + spriteString, playerPanel);
        unit.transform.localPosition = Vector3.zero;
        knightName.text = ClientUserData.knight;
    }
}
