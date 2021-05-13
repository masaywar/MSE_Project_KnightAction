using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePlayUI : UIWindow, IWindowObserver
{
    private InGameController inGameController;

    [SerializeField]
    private List<Button> buttons = new List<Button>();

    public Button attack;
    public Button jump;
    public Button ult;


    private void Start()
    {
        inGameController = InGameController.Instance;
        inGameController.Subscribe(this);
    }

    public void OnClickJump() 
    {
        Notify(this, inGameController.PlayerJump);
    }
    public void OnClickAttack() 
    {
        Notify(this, inGameController.PlayerAttack);
    }
    public void OnClickUlt() 
    {
        Notify(this, inGameController.PlayerUlt);
        DeactivateUlt();
    }

    public void Notify(IObserver o, Action action) 
    {
        action();
    }

    public void ActivateUlt()
    {
        ult.interactable = true;
    }

    public void DeactivateUlt()
    {
        ult.interactable = false;
    }


    private void OnDisable()
    {
        inGameController.Unsubscribe(this);
    }
}
