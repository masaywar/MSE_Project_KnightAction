using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePlayUI : UIWindow
{
    private InGameController inGameController;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    public Button attack;
    public Button jump;
    public Button ult;
    public Button setting;

    public RectTransform settingPanel;

    private void Start()
    {
    }

    public void OnClickSetting()
    {
        Time.timeScale = 0;
        settingPanel.gameObject.SetActive(true);

    }
    public void ActivateUlt()
    {
        ult.interactable = true;
    }
        
    public void DeactivateUlt()
    {
        ult.interactable = false;
    }
}
