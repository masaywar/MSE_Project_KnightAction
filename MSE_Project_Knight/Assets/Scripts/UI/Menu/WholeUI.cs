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
    public TextMeshProUGUI highscore;

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

        SetUpCompanion(ClientUserData.knight + spriteString);
        highscore.text = ClientUserData.score.ToString();
#endif
    }

    public void UIUpdate()
    {
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();
    }

    public void OnClickStartGame()
    {
#if !TEST   
        unit.DespawnSprite();
#endif
        GameManager.Instance.gameState = GameManager.GameState.loadIngame;
    }

    private void Update()
    {
        UIUpdate();
    }

    private void SetUpCompanion(string name)
    {
        unit = ObjectManager.Instance.Spawn<UnitSprite>(name, playerPanel);
        unit.transform.localPosition = Vector3.down * 100;
        knightName.text = ClientUserData.knight;
    }

    public void PlayerPanelUpdate()
    {
        if (unit == null)
            return;

        unit.DespawnSprite();
        SetUpCompanion(ClientUserData.knight + spriteString);
    }
}
