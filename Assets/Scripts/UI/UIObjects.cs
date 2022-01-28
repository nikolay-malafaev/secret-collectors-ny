using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjects : MonoBehaviour
{
    [Header("Text")]
    public Text colMutagen;
    public Text thisColMutagen;
    public Text maxDistation;
    public Text maxDistationInGame;
    public Text thisDistation;
    
    [Header("UIObjects")]
    public GameObject Game;
    public GameObject Menu;
    public GameObject ButtonStart;
    public GameObject ButtonPause;
    public GameObject panel;
    public GameObject TextPause;
    public GameObject Approval;
    public GameObject gameover;
    
    [Header("Buffs")]
    public Image timerBar;
    public GameObject timerImage;
    public GameObject doubleMutagen;
    public GameObject noGravityButton;
    public GameObject burable;
}
