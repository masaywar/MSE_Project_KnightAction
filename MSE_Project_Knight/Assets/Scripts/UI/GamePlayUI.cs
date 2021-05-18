using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class GamePlayUI : UIWindow
{
    private IngameController inGameController;

    [SerializeField]
    private float cachedTimeScale;

    public CanvasGroup Buttons;
    public CanvasGroup UpBar;

    public SettingUI settingUI;

    public IngameController ingameController;
    public RectTransform settingPanel;

    private Dictionary<string, Button> interactableDict = new Dictionary<string, Button>();
    private void Start()
    {
        ingameController = IngameController.Instance;
        ingameController.OnFullUltGage += ActivateButton;

        for (int k = 0; k < Buttons.transform.childCount; k++)
        {
            var button = Buttons.transform.GetChild(k);
            interactableDict.Add(button.name, button.GetComponent<Button>());
        }
    }

    public void OnClickSetting()
    {
        GameManager.Instance.Pause();

        settingUI.Open();
        settingUI.transform.DOScale(1, 0.2f).SetUpdate(true);

        Buttons.blocksRaycasts = false;
        UpBar.blocksRaycasts = false;
    }

    public void OnCloseSetting()
    {
        settingUI.transform.DOScale(0.2f, 0.2f).
            SetUpdate(true).
            OnComplete(() => {
                settingUI.OnClickBack();
                Buttons.blocksRaycasts = true;
                UpBar.blocksRaycasts = true;
            });
    }

    private void ActivateButton(string name, bool activate)
    {
        var button = interactableDict[name];
        button.interactable = activate;
    }
}
