using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEventBus;


public class GameManager : MonoBehaviour
{
    public bool test;
    [HideInInspector] public bool game;
    [HideInInspector] public int[] globalTimeBuff;
    [HideInInspector] public float time;
    
    [SerializeField] private TubeController tubeController;
    [SerializeField] private BuffController buffController;
    [SerializeField] private Player player;
    [SerializeField] private UI UI;
    private bool timer;
    
    [SerializeField] private CameraController cameraController;
    //number:       0         1          2       3
    //type:    burable doubleMutagen blast  noGravity
    //timer:    true      true       false    true

    [HideInInspector] public bool[] buffs = new bool[4];
    
    void Start()
    {
        buffs = new bool[4];
        globalTimeBuff = new int[] {10, 10, 10, 10};
        tubeController.pausePosition = !test;
        game = test;
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
    
    public void Buffs(string nameBuff,  bool isTimer)
    {
        int numberBuff = -1;
        switch (nameBuff)
        {
            case "burable":
                numberBuff = 0;
                break;
            case "doubleMutagen":
                numberBuff = 1;
            break;
            case "blast":
                numberBuff = 2;
                buffController.Blast();
                break;
            case "noGravity":
                numberBuff = 3;
                break;
        }
        
        buffs[numberBuff] = true;
        if (isTimer & numberBuff > -1)
        {
            timer = true;
            time = 1;
            StartCoroutine(TimeBuff(isTimer, numberBuff));
            //StartCoroutine(Timer(numberBuff));
        }
        UI.BuffsUI(isTimer, timer, numberBuff);
    }
    
    IEnumerator TimeBuff(bool isTimer, int number)
    {
        yield return new WaitForSeconds(globalTimeBuff[number]);
        timer = false;
        buffs[number] = false;
        UI.BuffsUI(isTimer, timer, number);
    }
    
    /*IEnumerator Timer(int number)
    {
        yield return new WaitForSeconds(globalTimeBuff[number] / 100);
        time -= globalTimeBuff[number] / 100;
        Debug.Log(time);
        if(!timer) yield break;
        StartCoroutine(Timer(number));
    }*/
}

