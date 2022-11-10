using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public TypeSetting typeSetting;
    public TypeUI typeUI;
    private UI ui;
    private bool isLoad = false;
    private void Start()
    {
        ui = FindObjectOfType<UI>();
        isLoad = true;
    }

    public enum TypeSetting
    {
        Music = 0,
        Sound = 1
    }

    public enum TypeUI
    {
        Slider = 0,
        Toggle = 1
    }

    public void CheckSlider(float value)
    {
        if(!isLoad) return;
        ui.CheckSlider(value, typeSetting);
    }

    public void CheckToggle(bool value)
    {
        if(!isLoad) return;
        ui.CheckToggle(value, typeSetting);
    }
}
