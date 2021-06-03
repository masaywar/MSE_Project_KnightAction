using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIWindow
{
    public Button ToMain;

    public Transform Buttons;
    public Transform Sliders;

    [SerializeField]
    private Slider effectSlider;
    [SerializeField]
    private Slider bgmSlider;

    private void Start()
    {
        Close();

        effectSlider.minValue = 0;
        effectSlider.maxValue = 1;
        effectSlider.value = 0.5f;

        SoundManager.Instance.GetEffectSource().volume = effectSlider.value;

        effectSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.GetEffectSource().volume = value;
        });

        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 1;
        bgmSlider.value = 0.5f;

        SoundManager.Instance.GetMusicSource().volume = bgmSlider.value;

        bgmSlider.onValueChanged.AddListener(value =>
        {
            SoundManager.Instance.GetMusicSource().volume = value;
        });
    }

    public void OnClickBack()
    {
        Close();
    }

    public void OnClickToMain() 
    {
        ObjectManager.Instance.DespawnAll();

        //ObjectManager.Instance.DespawnAllWithName<EnemyObject>("DestroyableEnemy");
        //ObjectManager.Instance.DespawnAllWithName<EnemyObject>("UnDestroyableEnemy");
        SoundManager.Instance.StopAll();

        Close();    
        GameManager.Instance.gameState = GameManager.GameState.main;
    }

    public override void Open()
    {
        base.Open();
        GameManager.Instance.Pause();
    }

    public override void Close()
    {
        base.Close();
        GameManager.Instance.Play();
    }

    private void OnEnable()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.ingame)
            ToMain.gameObject.SetActive(true);

        else
            ToMain.gameObject.SetActive(false);
    }
}
