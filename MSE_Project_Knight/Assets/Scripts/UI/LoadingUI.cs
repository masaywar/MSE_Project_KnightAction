using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingUI : UIWindow
{
    public TextMeshProUGUI loadingText;
    public Button loadingButton;

    private void Update()
    {
        loadingText.text = GameManager.Instance.progressMessage;
    }

    public bool OnClickLoadButton()
    {
        return true;
    }
}
