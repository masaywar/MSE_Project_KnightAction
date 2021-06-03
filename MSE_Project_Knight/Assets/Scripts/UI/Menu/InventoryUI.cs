using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : UIWindow
{
    private Transform UpBar;
    public MenuController menuController;


    private void Start()
    {
        Close();
    }

    private void SetupItems()
    {
        var companions = ClientUserData.companions;

        
    }


    public override void Open()
    {
        base.Open();
        menuController.TransferUpbar(this);
    }

    public override void Close()
    {
        var wholeUI = UIManager.Instance.GetActiveWindow<WholeUI>("WholeUI");
        menuController.TransferUpbar(wholeUI);
        base.Close();
    }
}
