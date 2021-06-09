using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : UIWindow
{
    public MenuController menuController;
    private Transform UpBar;

    public ItemBox[] items;

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

        CompData compData = new CompData();
        compData.companion = itemBox.companion;
        compData.userName = ClientUserData.name;
        compData.level = 1;

        var data = CompDataManager.InsertComp(compData);
            
        ClientUserData.coin -= price;
        if (UserDataManager.UpdatUserData(ClientUserData.name, ClientUserData.score, ClientUserData.coin, ClientUserData.knight) == 0)
            return;

        ClientUserData.companions.Add(compData);
        itemBox.OnActivate();
    }

    public override void Open()
    {
        base.Open();
        items.ForEach(item => item.OnActivate());
        menuController.TransferUpbar(this);
    }

    public override void Close()
    {
        base.Close();

        var wholeUI = UIManager.Instance.GetActiveWindow<WholeUI>("WholeUI");
        menuController.TransferUpbar(wholeUI);
    }
}
