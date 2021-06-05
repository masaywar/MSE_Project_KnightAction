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

    public void BuyItem(ItemBox itemBox)
    {
        int price = itemBox.price;
        var coin = ClientUserData.coin;

        if (coin < price)
        {
#if !UNITY_EDITOR
            ToastMessenger.ShowToast("Lack of money");
#endif
            return;
        }

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
