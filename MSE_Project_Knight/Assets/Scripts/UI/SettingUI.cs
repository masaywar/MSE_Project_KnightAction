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

    public void OnClickToMain() { }

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
}
