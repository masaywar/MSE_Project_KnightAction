using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIWindow
{
    public Button exit;

    public void OnClickExit()
    {
        Time.timeScale = 1;

        Close();
    }
}
