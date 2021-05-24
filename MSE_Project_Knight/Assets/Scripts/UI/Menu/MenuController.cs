using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public RectTransform UpBar;
    public Button setting;

    public void TransferUpbar(UIWindow ui)
    {
        UpBar.SetParent(ui.transform);
        UpBar.localScale = Vector3.one;
    }

    private void Start()
    {
        setting.onClick.AddListener(()=> UIManager.Instance.GetWindow<SettingUI>("SettingUI").Open());
    }
}
