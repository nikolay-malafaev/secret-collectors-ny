using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIObjects : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] protected GameObject game;
    [SerializeField] protected GameObject menu;
    
    [Header("Distance")]
    [SerializeField] protected Text maxDistance;
    [SerializeField] protected Text maxDistanceMenu;
    [SerializeField] protected Text currentDistance;
    
    [Header("Mutagen")]
    [SerializeField] protected Text countMutagen;
    [SerializeField] protected Text currentCountMutagen;
    
    [Space]
    [SerializeField] protected GameObject pausePanel;
    [SerializeField] protected Text pauseText;
    [Space]
    [SerializeField] protected GameObject defeatGame;
    [Space]
    [SerializeField] protected GameObject buttonPause;
    
    [Header("Settings")]
    [SerializeField] protected Dropdown dropdown;
    [SerializeField] protected GameObject warningQualitySettings;
    [SerializeField] protected Toggle musicToggle;
    [SerializeField] protected Slider musicSlider;
    [SerializeField] protected Toggle soundToggle;
    [SerializeField] protected Slider soundSlider;
    [SerializeField] protected Toggle viewFPS;
    
    [Header("Buffs")]
    [SerializeField] protected GameObject durable;
    [SerializeField] protected GameObject doubleMutagen;
    [SerializeField] protected GameObject noGravity;
    [Space]
    [SerializeField] protected GameObject noGravityButton;
    [Space]
    [SerializeField] protected Image timerBar;
    [SerializeField] protected GameObject timerBarParent;
    
    

}
