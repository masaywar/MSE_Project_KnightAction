using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;

public class GamePlayUI : UIWindow
{
    private IngameController ingameController;

    public CanvasGroup Buttons;
    public CanvasGroup UpBar;

    public Button setting;
    private Dictionary<string, Button> interactableDict = new Dictionary<string, Button>();

    public TextMeshProUGUI score;
    public TextMeshProUGUI combo;
    public Image HpBar;

    private float hpFilled;
    private float curHp;

    private void Start()
    {
        ingameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<IngameController>(); ;
        ingameController.OnFullUltGage += ActivateButton;
        ingameController.UIUpdatePlayerInfo += InfoUpdate;

        hpFilled = ingameController.hp;
        curHp = hpFilled;

        for (int k = 0; k < Buttons.transform.childCount; k++)
        {
            var button = Buttons.transform.GetChild(k);
            interactableDict.Add(button.name, button.GetComponent<Button>());
        }
    }

    private void Update()
    {
        curHp = ingameController.hp;
        HpBar.fillAmount = curHp / hpFilled;
    }

    private void ActivateButton(string name, bool activate)
    {
        var button = interactableDict[name];
        button.interactable = activate;
    }

    public void OnClickSetting()
    {
        var window = UIManager.Instance.GetWindow<SettingUI>("SettingUI");
        window.Open();
    }

    private void InfoUpdate(int combo, int score, float hp)
    {
        this.combo.text = combo.ToString();
        this.score.text = score.ToString();
    }
}
