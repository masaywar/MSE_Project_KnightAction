using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : UIWindow
{
    public MenuController menuController;
    private Transform UpBar;


    private void Start()
    {
        Close();
    }

    private void BuyItem(int price)
    {
        //Not Implemented.
        ClientUserData.coin -= price;
        UserDataManager.UpdatUserData(ClientUserData.name, ClientUserData.score, ClientUserData.coin, ClientUserData.knight);
    }

    public override void Open()
    {
        base.Open();
        menuController.TransferUpbar(this);
    }

    public override void Close()
    {
        base.Close();

        var wholeUI = UIManager.Instance.GetActiveWindow<WholeUI>("WholeUI");
        menuController.TransferUpbar(wholeUI);
    }
}
