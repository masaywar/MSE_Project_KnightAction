using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIWindow
{
    public Button exit;

    public Transform Buttons;
    public Transform Sliders;

    [SerializeField]
    private Slider effectSlider;
    [SerializeField]
    private Slider bgmSlider;

    private void Start()
    {
        Close();
    }

    public void OnClickBack()
    {
        GameManager.Instance.Play();
        Close();
    }

    public void OnClickQuit()
    {
        
    }

    public void OnClickToMain() { }
}
