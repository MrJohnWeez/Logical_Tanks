using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    [SerializeField] private Slider _audioSlider = null;
    [SerializeField] private Slider _qualitySlider = null;

    protected override void Awake()
    {
        base.Awake();
        _qualitySlider.value = SaveData.QualityLevel;
        _audioSlider.value = SaveData.MusicLevel;
        _qualitySlider.onValueChanged.AddListener(QualitySettingChanged);
        _audioSlider.onValueChanged.AddListener(AudioChanged);
    }

    private void AudioChanged(float newValue)
    {
        SaveData.MusicLevel = (int)newValue;
    }

    private void QualitySettingChanged(float newValue)
    {
        SaveData.QualityLevel = (int)newValue;
        QualitySettings.SetQualityLevel(SaveData.QualityLevel);
    }

    protected override void CloseMenu()
    {
        SaveData.SaveGameData();
        base.CloseMenu();
    }
}
