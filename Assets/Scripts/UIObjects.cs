using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjects : MonoBehaviour
{
    public Text colMutagen;
    public Text thisColMutagen;
    public Text maxDistation;
    public Text thisDistation;
    public GameObject Game;
    public GameObject Menu;
    public GameObject ButtonStart;
    public GameObject ButtonPause;
    public GameObject panel;
    public GameObject TextPause;
    public GameObject Approval;
    public GameObject gameover;
    
    
    public GameObject timerImage;
    public GameObject doubleMutagenImage;
    public GameObject NoGravityButton;
    public Image timeBaff;
    

    public void Transition(bool type)
    {
        Game.SetActive(type);
        ButtonStart.SetActive(!type);
        Menu.SetActive(!type);
    }
    
}
