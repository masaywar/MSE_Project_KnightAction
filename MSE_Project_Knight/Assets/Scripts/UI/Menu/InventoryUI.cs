using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : UIWindow
{
    private Transform UpBar;
    private List<ScriptObject> iconList = new List<ScriptObject>();

    public RectTransform content;
    public MenuController menuController;


    private void Start()
    {
        Close();
        SetupItems();
        SetupItemActivity();
    }

    private void SetupItems()
    {
        var companions = ClientUserData.companions;

        /// Spawn companion icon
        /// icon is button that can switch the main charater when it's clicked.
        /// main character's icon will be setted as (interactable = fasle) which make it not clicked.
        /// the lock will be unfreeze when player click other interactable icon.

        companions.ForEach(companion =>{
            var spawnedObj = ObjectManager.Instance.Spawn<ScriptObject>(companion.companion + "Icon", content);

            iconList.Add(spawnedObj);
            spawnedObj.transform.localScale = Vector3.one;
        });
    }

    private void SetupItemActivity()
    {
        iconList.ForEach(icon => {
            var button = icon.GetComponent<Button>();
            button.onClick.AddListener(delegate {
                ChangeCompanionAsMain(button, icon.prefabName.Replace("Icon", ""));
            });
        });
    }

    private void ChangeCompanionAsMain(Button button, string name)
    {
        ClientUserData.knight = name;
        button.interactable = false;

        UserDataManager.UpdatUserData(ClientUserData.name, ClientUserData.score, ClientUserData.coin, ClientUserData.knight);

        iconList.ForEach(icon => {
            var temp = icon.GetComponent<Button>();

            if (temp != button)
                temp.interactable = true;
        });
    }

    public override void Open()
    {
        base.Open();
        menuController.TransferUpbar(this);

        iconList.ForEach(icon => {
            icon.gameObject.SetActive(true);

            if (icon.prefabName.Replace("Icon", "") == ClientUserData.knight)
                icon.GetComponent<Button>().interactable = false;
        });
    }

    public override void Close()
    {
        var wholeUI = UIManager.Instance.GetActiveWindow<WholeUI>("WholeUI");

        iconList.ForEach(icon => {
            icon.transform.SetParent(
                ObjectManager.Instance.childrenTransform.
                Find(child => 
                child.name == icon.prefabName));

            ObjectManager.Instance.Despawn<ScriptObject>(icon);
        });

        menuController.TransferUpbar(wholeUI);
        base.Close();
    }
}
