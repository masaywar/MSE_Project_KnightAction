using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoUI : UIWindow
{
    public MenuController menuController;

    public TextMeshProUGUI email;
    public TextMeshProUGUI username;
    public TextMeshProUGUI coin;
    public TextMeshProUGUI highscore;

    private void Start()
    {
        Close();
        TryInitialize();
    }

    private void TryInitialize()
    {
        email.text = ClientUserData.email;
        username.text = ClientUserData.name;
        coin.text = ClientUserData.coin.ToString();
        highscore.text = ClientUserData.score.ToString();
    }

    public override void Open()
    {
        base.Open();
        menuController.TransferUpbar(this);
    }

    public override void Close()
    {
        base.Close();

        var window = UIManager.Instance.GetTop();
        menuController.TransferUpbar(window);
    }
}
