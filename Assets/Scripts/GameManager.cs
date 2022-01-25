using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [HideInInspector] public bool game;
    public bool test;
    [SerializeField] private TubeController tubeController;
    [SerializeField] private Player player;
    [SerializeField] private UI UI;
    [SerializeField] private CameraController cameraController;

    void Start()
    {
        if (!test)
        {
            tubeController.pausePosition = true;
        }
        else
        {
            tubeController.pausePosition = false;
            game = true;
        }
    }

    void Update()
    {
        if (player.healthPlayer == 0)
        {
            tubeController.pausePosition = true;
            player.healthPlayer = -1;
            UI.GameOver();
        }
        
        if (PlayerPrefs.GetFloat($"distation") < Mathf.Abs(tubeController.positionTubeZ))
        {
            PlayerPrefs.SetFloat($"distation", Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)));
        }

    }

    public void TransitionGame()
    {
        cameraController.ToGame();
        game = !game;
        tubeController.pausePosition = !game;
    }

    public void AddMutagen(int countOneAdd)
    {
        PlayerPrefs.SetInt("colMutagen", PlayerPrefs.GetInt("colMutagen") + countOneAdd);
    }
    public void Pause()
    {
        tubeController.pausePosition = !tubeController.pausePosition;
        UI.Pause(tubeController.pausePosition);
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        else Time.timeScale = 0;
    }
    
    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    
    public void BuffsTriggers(string nameBuff)
    {
        UI.BuffsUI(nameBuff);
        switch (nameBuff)
        {
            case "burable":
                
                break;
        }
    }
    
    /*IEnumerator TimeBuff()
    {
        /*yield return new WaitForSeconds(10f);
        doubleMutagen = false;
        burable = false;
        timer = false;
        if (isNoGravityBaff)
        {
            if(IsNoGravity) NoGravity();
            isNoGravityBaff = false;
        }
    }*/
}
