using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TubeController tubeController;
    [SerializeField] private GameManager gameManager; 
    private UIObjects ui;
    

    public Animator animator;


    void Start()
    {
        ui = GetComponent<UIObjects>();
        ui.maxDistation.text = PlayerPrefs.GetFloat($"distation").ToString();
        ui.colMutagen.text = PlayerPrefs.GetInt($"colMutagen").ToString();
        if(!gameManager.test) ui.Transition(false);
    }

    void Update()
    {
        ui.thisDistation.text = Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)).ToString();
        ui.thisColMutagen.text = player.colMutagen.ToString();


        

       

       /* if (player.timer)
        {
            timerImage.SetActive(true);
            animator.SetBool("timer", true);
        }
        else { timerImage.SetActive(false); animator.SetBool("timer", false); }*/

       /* if (player.blastScreen)
        { animator.SetTrigger("blast"); player.blastScreen = false; }*/
        
        /*if(player.isNoGravityBaff) NoGravityButton.SetActive(true);
        else NoGravityButton.SetActive(false);*/
        
        /*if (player.doubleMutagen)
        {
            doubleMutagenImage.SetActive(true);
        } else doubleMutagenImage.SetActive(false);*/
        
    }
    
    public void BuffsUI(string nameBuff)
    {
        switch (nameBuff)
        {
            case "DoubleMutagen":
                
                break;
        }
    }


    public void Pause(bool pause)
    {
        ui.panel.SetActive(pause);
        ui.ButtonPause.SetActive(!pause);
        ui.TextPause.SetActive(pause);
    }
    public void GameOver()
    {
        ui.gameover.SetActive(true);
        ui.panel.SetActive(true);
        ui.ButtonPause.SetActive(false);
    }
    public void Approval()
    {
        ui.Approval.SetActive(!ui.Approval.activeSelf);
    }
    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    public void Transition()
    {
        gameManager.TransitionGame();
        ui.Transition(gameManager.game);
    }

}
