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

    [SerializeField]
    private float cachedTimeScale;

    public CanvasGroup Buttons;
    public CanvasGroup UpBar;

    public Button setting;
    private Dictionary<string, Button> interactableDict = new Dictionary<string, Button>();

    public TextMeshProUGUI score;
    public TextMeshProUGUI combo;
    public Image HpBar;

    private void Start()
    {
        ingameController = IngameController.Instance;
        ingameController.OnFullUltGage += ActivateButton;
        ingameController.UIUpdatePlayerInfo += InfoUpdate;

        setting.onClick.AddListener(OnClickSetting);

        for (int k = 0; k < Buttons.transform.childCount; k++)
        {
            var button = Buttons.transform.GetChild(k);
            interactableDict.Add(button.name, button.GetComponent<Button>());
        }
    }

    private void ActivateButton(string name, bool activate)
    {
        var button = interactableDict[name];
        button.interactable = activate;
    }

    private void OnClickSetting()
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
