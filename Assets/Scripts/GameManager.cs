using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEventBus;


public class GameManager : MonoBehaviour
{
    public bool test;
                             //     0
    public int direction;   //     | 
                            // 3 -   - 1
                            //    |
                            //    2
    [HideInInspector] public bool game;
    [HideInInspector] public float[] globalTimeBuff;
    
    [HideInInspector] public float time;
    
    [SerializeField] private ChunkController chunkController;
    [SerializeField] private BuffController buffController;
    [SerializeField] private Player player;
    [SerializeField] private UI UI;
    [SerializeField] private GameObject MainSpawn;
    [SerializeField] private GameObject light;
    private bool timer;
    private int numberBuff;
    private GameObject ntv;
    
    [SerializeField] private CameraController cameraController;

    [SerializeField] private GameObject tv;
        //number:       0         1          2       3
    //type:    burable doubleMutagen blast  noGravity
    //timer:    true      true       false    true

    [HideInInspector] public bool[] buffs = new bool[4];
    private float nextAction;
    private float period;

    void Start()
    {
        period = 1f;
        buffs = new bool[4];
        globalTimeBuff = new float[] {10f, 10f, 10f, 10f};
        chunkController.pausePosition = !test;
        game = test;
        if (!test)
        {
            ntv = Instantiate(tv);
            ntv.transform.position = new Vector3(-0.845f, -0.989f, 0.139f); // Vector3(-0.845000029,-0.989000022,0.138999999)
            ntv.transform.rotation = new Quaternion(-0.293f, 0.643f, 0.643f, 0.293f);
        }
        direction = 0;
    }

    void Update()
    {
        if (player.healthPlayer == 0)
        {
            game = false;
            chunkController.pausePosition = true;
            player.healthPlayer = -1;
            UI.GameOver();
            player.animator.SetTrigger("fall");
        }

       
        
        switch (direction) // сделать поворот света direction
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
        
       
        /* if (PlayerPrefs.GetFloat($"distation") < Mathf.Abs(tubeController.positionTubeZ))
         {
             PlayerPrefs.SetFloat($"distation", Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)));
         }*/
    }


    public void TransitionGame()
    {
        cameraController.ToGame();
        player.ToGame();
        game = !game;
        chunkController.pausePosition = !game;
        Destroy(ntv.gameObject);
    }

    public void AddMutagen(int countOneAdd)
    {
        PlayerPrefs.SetInt("colMutagen", PlayerPrefs.GetInt("colMutagen") + countOneAdd);
    }
    public void Pause()
    {
        chunkController.pausePosition = !chunkController.pausePosition;
        UI.Pause(chunkController.pausePosition);
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
            this.numberBuff = numberBuff;
            timer = true;
            nextAction = 0;
            time = 1;
            StartCoroutine(TimeBuff(isTimer, numberBuff));
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
    
    //Prefab
    /*
     *  switch (gameManager.direction)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            
        }
     */
    public void Turn(Target.Direction directionTurn)
    {
        Transform mainSpawn = MainSpawn.transform;
        
        if (direction == -1) direction = 3;
        if (direction == 4) direction = 0;
      
        light.transform.SetParent(player.transform);
        StartCoroutine(timeAften(0.37f));
        switch (direction)
        {
            case 0:
                mainSpawn.position = new Vector3(0, 0.08f, 50);
                mainSpawn.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                mainSpawn.position = new Vector3(50, 0.08f, 0);
                mainSpawn.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 2:
                mainSpawn.position = new Vector3(0, 0.08f, -50);
                mainSpawn.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case 3:
                mainSpawn.position = new Vector3(-50, 0.08f, 0);
                mainSpawn.rotation = Quaternion.Euler(0, -90, 0);
                break;
        }
        buffController.Spawn();
    }

    IEnumerator timeAften(float time)
    {
        yield return new WaitForSeconds(time);
        light.transform.parent = null;
    }
}

