using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIWindow
{
    public Button exit;
    public Button toMain;

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

    private void Update()
    {
        toMain.gameObject.SetActive(GameManager.Instance.gameState == GameManager.GameState.ingame);
    }

    public void OnClickBack()
    {
        GameManager.Instance.Play();
        Close();
    }

    public void OnClickQuit()
    {
        var window = UIManager.Instance.GetWindow<QuitUI>("QuitUI");
        window.Open();
    }

    public void OnClickToMain() 
    {
    
    }

    private void OnEnable()
    {
        GameManager.Instance.Pause();
    }

    private void OnDisable()
    {
        GameManager.Instance.Play();
    }
}
