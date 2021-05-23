using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public RectTransform UpBar;

    public void TransferUpbar(UIWindow ui)
    {
        UpBar.SetParent(ui.transform);
        UpBar.localScale = Vector3.one;
    }
}
