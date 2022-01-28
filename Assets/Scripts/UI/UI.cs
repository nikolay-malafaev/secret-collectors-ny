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
    private Animator animator;
    [SerializeField]private bool timer;


    void Start()
    {
        animator = GetComponent<Animator>();
        ui = GetComponent<UIObjects>();
        ui.maxDistation.text = PlayerPrefs.GetFloat($"distation").ToString();
        ui.colMutagen.text = PlayerPrefs.GetInt($"colMutagen").ToString();
        if (!gameManager.test)
        {
            bool game = gameManager.game;
            ui.Game.SetActive(game);
            ui.ButtonStart.SetActive(!game);
            ui.Menu.SetActive(!game);
        }
    }

    void Update()
    {
        ui.thisDistation.text = Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)).ToString();
        ui.maxDistationInGame.text = PlayerPrefs.GetFloat($"distation").ToString();
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

        /*if (timer)
        {
            ui.timerBar.fillAmount = gameManager.time;
        }*/
        if (timer)
        {
            ui.timerBar.fillAmount = gameManager.time;
        }
    }

    public void BuffsUI(bool isTimer, bool timer, int buffNumer)
    {
        this.timer = timer;
        if (isTimer)
        {
            ui.timerImage.SetActive(timer);
            //animator.SetBool("timer", timer);
        }

        switch (buffNumer)
        {
            case 0:
                ui.burable.SetActive(timer);
                break;
            case 1:
                ui.doubleMutagen.SetActive(timer);
                break;
            case 2:
                animator.SetTrigger($"blast");
                break;
            case 3:
                ui.noGravityButton.SetActive(timer);
                if (!timer & player.transform.position.y > 1) player.NoGravity();
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
        bool game = gameManager.game;
        ui.Game.SetActive(game);
        ui.ButtonStart.SetActive(!game);
        ui.Menu.SetActive(!game);
    }

}
