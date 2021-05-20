using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WholeUI : UIWindow
{
    public Button startGame;
    public Button settingButton;

    private void Start()
    {
        settingButton.onClick.AddListener(OnClickSetting);
    }

    public void OnClickStartGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.loadIngame;
    }

    private void OnClickSetting()
    {
        var window = UIManager.Instance.GetWindow<SettingUI>("SettingUI");
        window.Open();
    }
}
