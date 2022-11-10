using System;
using System.Collections;
using Randomize;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool test;
    public bool Test
    {
        get { return test; }
    }
    private int direction;
    public int Direction
    {
        get { return direction; }
        set
        {
            if (value == 4) direction = 0;
            else if (value == -1) direction = 3;
            else direction = value;
        }
    }
    //     0
    //     | 
    // 3 -   - 1
    //    |
    //    2
    private bool game;
    public bool Game
    { 
        get { return game; }
    }
    private int countMutagen;
    public int CountMutagen
    {
        get { return countMutagen; }
        set { countMutagen = value >= 0 ? value : 0; }
    }
    private int distance;
    public int Distance
    {
        get { return distance; }
    }
    [SerializeField] private ChunkController chunkController;
    [SerializeField] private Player player;
    [SerializeField] private UI UI;
    [SerializeField] private GameObject mainSpawn;
    [SerializeField] private Light light;
    [SerializeField] private GameObject tv;
    //number:       0          1           2         3
    //type:    burable   doubleMutagen   blast    noGravity
    //timer:    true        true         false      true
    private float timeRemaining = 1;
    private float timeBlinking = 1;
    private bool pause;
    

    public static int CurrenQualitySettings;

    #region Events

    public static Action SendTransitionToGame;
    public static Action SendPauseGame;
    public static Action SendResetGame;
    public static Action SendDefeatGame;
    public static Action<Target.Direction> SendTurn;

    #endregion
    
    #region Singleton

    public static GameManager Instance { get; private set; }

    #endregion
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SendTransitionToGame += TransitionToGame;
        SendTurn += Turn;
        SendPauseGame += Pause;
        SendDefeatGame += DefeatGame;
        SendResetGame += ResetGame;
        game = test;
        if (!test) StartTransform();
        direction = 0;
    }
    void Update()
    {
        if (!game && !pause)
        {
            if (timeBlinking > 0)
            {
                timeBlinking -= Time.deltaTime;
            }
            else
            {
                timeBlinking = UnityEngine.Random.Range(0.05f, 0.3f);
                light.intensity = GetRandom.GetRandomInterval(UnityEngine.Random.Range(0.8f, 0.3f),
                    UnityEngine.Random.Range(2, 2.3f));
            }
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0.25f;
            if(game) distance++;
        }
    }
    private void TransitionToGame()
    {
        game = true;
        Time.timeScale = 1;
        light.intensity = 0.5f;
        tv.SetActive(false);
    }
    private void Pause()
    {
        game = !game;
        pause = !pause;
        //Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
    private void DefeatGame()
    {
        game = false;
    }
    private void Turn(Target.Direction directionTurn)
    {
        Transform mainSpawn = this.mainSpawn.transform;

        if (direction == -1) direction = 3;
        if (direction == 4) direction = 0;

        light.transform.SetParent(player.transform);
        StartCoroutine(TimeAfter(0.37f));
        
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
    }
    private void ResetGame()
    {
        mainSpawn.transform.position = new Vector3(0, 0.08f, 50);
        light.transform.position = new Vector3(0, -0.1f, -3);
        SaveProgress();
        direction = 0;
        distance = 0;
        game = false;
        StartTransform();
        UI.UpdateProgress();
    }
    private void StartTransform()
    {
        tv.SetActive(true);
        tv.transform.position = new Vector3(-0.845f, -0.989f, 0.139f);
        tv.transform.rotation = new Quaternion(-0.293f, 0.643f, 0.643f, 0.293f);
    }
    public Vector3 ChooseDirectionPosition(float number)
    {
         Vector3 position = new Vector3();
         switch (direction)
         {
             case 0:
                 position.z = number;
                 break;
             case 1:
                 position.x = number;
                 break;
             case 2:
                 position.z = -number;
                 break;
             case 3:
                 position.x = -number;
                 break;
            
         }
         return position;
    }
    public float ChooseDirectionRotation()
    {
        float y = 0;
        switch (direction)
        {
            case 0:
                y = 0;
                break;
            case 1:
                y = 90;
                break;
            case 2:
                y = 180;
                break;
            case 3:
                y = - 90;
                break; 
        }
        return y;
    }
    IEnumerator TimeAfter(float time)
    {
        yield return new WaitForSeconds(time);
        chunkController.IsSpawnPermit = true;
        light.transform.parent = null;
    }
    public void OnSendTransitionToGame()
    {
        SendTransitionToGame.Invoke();
    }
    public void OnSendPauseGame()
    {
        SendPauseGame.Invoke();
    }
    public void OnSendDefeatGame()
    {
        SendDefeatGame.Invoke();
    }
    public void OnSendResetGame()
    {
        SendResetGame.Invoke();
    }
    public void ChangeQualitySettings(int value)
    {
        CurrenQualitySettings = value;
        QualitySettings.SetQualityLevel(value, true);
        PlayerPrefs.SetInt("qualitySettings", value);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnApplicationQuit()
    {
        SaveProgress();
    }
    private void SaveProgress()
    {
        if (PlayerPrefs.GetFloat($"maxDistance") < distance)
        {
            PlayerPrefs.SetFloat($"maxDistance", distance);
        }

        PlayerPrefs.SetInt("countMutagen", PlayerPrefs.GetInt("countMutagen") + countMutagen);
        countMutagen = 0;
    }

    private void OnDestroy()
    {
        SendTransitionToGame -= TransitionToGame;
        SendTurn -= Turn;
        SendPauseGame -= Pause;
        SendDefeatGame -= DefeatGame;
        SendResetGame -= ResetGame;
    }
}

