using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeController : MonoBehaviour
{
    public Tube[] TubePrefabs;
    public Buff[] baffs;
    public Mutagen mutagenPrefabs;
   
    public Enemy enemyPrefabs;

    private float[] coordinates = { 0.7f, -0.7f };
    public List<Tube> spawnTubes = new List<Tube>();
    private List<Mutagen> mutagenSpawn = new List<Mutagen>();
    public Tube startTube;
    public Mutagen mutagenStart;

    [Range(0, 100)]
    public float[] oddsTubes;

    [Range(0, 100)]
    public float[] oddsBaffs;

    public Player player;
    public GameObject mainTube;
    public GameObject oneSpawnMutagen;
    public GameObject mutagens;
    public float corner;
    public bool pausePosition;
    public float positionTubeZ;
    private float speedCorner;
    public float addSpeed;
    public float numberAddSpeed;
    public float timeAddSpeed;
    public bool velocityTimeBaff;
    public float velocityCoin;
    private float number;
    [SerializeField] private int countCoin;
    private int schemeNumber;
    private int score = 0;
    [SerializeField] private bool stop;
    private int cornerRotate;
    public int jump;
    private bool swipe;
    public int position;
    public bool isSM;

    private Vector2 startPos;
    private Vector2 direction;
    private bool directionChosen;
    public float eulerMutagen;
    private float random;
    private float randomTime;
    private int randomPoint;
    private int randomPointTwo;
    private int randomBefore;
    private int randomAfter;
    private int randomPointBuff;
    private int indexInList;
    public Text m_Text;
    public int numberP;
    string message = "";
    //private string mutagenMod = "oneRight";
    private string [] isMutagenArray = {"one", "two", "three"};
    private string isMutagen;
    private bool isMutagenTwo;
    private bool isMutagenRotate;
    public float baffvelocity;
    public bool IsSpawnTunnels;
    private bool doubleTubeSpawn;


    private float nextActionTimeAddSpeed = 0.0f;
    public float periodAddSpeed = 25;

    private float nextActionTimeSpawnBaff = 0.0f;
    public float periodSpawnBaff;

    void Start()
    {
        IsSpawnTunnels = true;
        doubleTubeSpawn = false;
        schemeNumber = 1;
        addSpeed = 0.1f;
        spawnTubes.Add(startTube);
        periodSpawnBaff = Random.Range(13, 25);
        StartCoroutine(MutagenSpawn());
        StartCoroutine(DoudleTubeSpawn());
        mutagenSpawn.Add(mutagenStart);
        random = Random.Range(3, 7);
        isMutagen = "one";
    }

    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            cornerRotate++;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            cornerRotate--;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            jump = 1;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            jump = -1;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Spawn("mutagen");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Spawn("buff");
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
           //TurnTunnels();
        }

        switch (jump)
        {
            case 1:
               // player.Move("Up");
                break;
            case -1:
                Debug.Log("Down");
                break;
        }
        jump = 0;
        //Debug.Log(Time.timeScale);
    }

    void FixedUpdate()
    {
        #region Touch
      
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            { 
                case TouchPhase.Began:
                    startPos = touch.position;
                    message = "Begun ";
                    swipe = true;
                    break;
                case TouchPhase.Moved:  
                    direction = touch.position - startPos;
                    if(swipe)
                    {
                        if (direction.x < 0 & Mathf.Abs(direction.y) < 20) cornerRotate++; else if (direction.x > 0 & Mathf.Abs(direction.y) < 20) cornerRotate--;
                        if (direction.y < 0 & Mathf.Abs(direction.x) < 20) m_Text.text = "down"; else if (direction.y > 0 & Mathf.Abs(direction.x) < 20) m_Text.text = "up";
                        swipe = false;
                    }  
                    //speedCorner = (direction.x) / 30;
                    //if (Mathf.Abs(speedCorner) < 0.5f) speedCorner = 0;
                    message = "Moving ";
                    break;
                case TouchPhase.Ended:
                    //speedCorner = 0;
                    message = "Ending ";
                    break;
            }
        } else message = "0";

        #endregion
        
        if (spawnTubes[spawnTubes.Count - 1].End.position.z < 40 & IsSpawnTunnels)
        {
            spawnTube();
        }

        if (spawnTubes.Count > 20)
        {
            destoryTube();
        }
        if (!pausePosition)
        {
            speedCorner = -speedCorner;
            speedCorner = Mathf.Clamp(speedCorner, -6f, 6f);
            mainTube.transform.position = new Vector3(0, 0, positionTubeZ);
           // mutagens.transform.position = new Vector3(0, 0, positionTubeZ);
            //mainTube.transform.rotation = Quaternion.Euler(0, 0, corner += speedCorner);
           // mutagens.transform.rotation = Quaternion.Euler(0, 0, corner += speedCorner);
            //oneSpawnMutagen.transform.rotation = Quaternion.Euler(0, 0, corner += speedCorner);
            positionTubeZ = positionTubeZ - addSpeed;
        }

        if (corner == 360 | corner == -360) corner = 0;

        if (!player.burable & velocityTimeBaff)
        {
            addSpeed = baffvelocity;
            velocityTimeBaff = false;
        }

        if (cornerRotate > 0)
            corner += 2.25f; 

        if (cornerRotate < 0)
            corner-=2.25f;

        if (corner % 45 == 0)
        {
            if (cornerRotate > 0) cornerRotate--;
            if (cornerRotate < 0) cornerRotate++;
        }


       // position = Mathf.RoundToInt(mainTube.transform.eulerAngles.z / 45);


        if (Time.time > nextActionTimeAddSpeed) // (period)
        {
            addSpeed += numberAddSpeed;
            nextActionTimeAddSpeed += periodAddSpeed;
        }

        if (Time.time > nextActionTimeSpawnBaff) //(period)
        {
            periodSpawnBaff = Random.Range(5, 20);
            Spawn("buff");
            nextActionTimeSpawnBaff += periodSpawnBaff;
        }
        jump = 0;
    }

    public void spawnTube()
    {
        int choose;
        while (true)
        { 
            choose = Mathf.RoundToInt(ChooseTunnels(oddsTubes));
           if(choose != 5) break;
           else if (doubleTubeSpawn)
           {
               StartCoroutine(DoudleTubeSpawn());
               doubleTubeSpawn = false;
               break;
           }
        }
        Tube newTube = Instantiate(TubePrefabs[choose]);
       /* if (newTube.CompareTag($"DoubleTube"));
            Debug.Log("To");*/
        newTube.GenerateBarrier();
        newTube.transform.position = spawnTubes[spawnTubes.Count - 1].Begin.position - newTube.End.localPosition;
        spawnTubes.Add(newTube);
        newTube.transform.SetParent(mainTube.transform);
        //n newTube.transform.rotation = mainTube.transform.rotation;
        
    }
    private void destoryTube()
    {
        Destroy(spawnTubes[0].gameObject);
        spawnTubes.RemoveAt(0);
    }

    private void Spawn(string nameObject)
    {
        switch (nameObject)
        {
            case "mutagen":
                  Mutagen newMutagen = Instantiate(mutagenPrefabs);
                  newMutagen.transform.position = oneSpawnMutagen.transform.GetChild(randomPoint).position;
                  //newMutagen.transform.SetParent(mainTube.transform);
                  newMutagen.transform.SetParent(mutagens.transform);
                  if (isMutagenTwo)
                  {
                    Mutagen newMutagenTwo = Instantiate(mutagenPrefabs);
                    newMutagenTwo.transform.position = oneSpawnMutagen.transform.GetChild(randomPointTwo).position;
                    //newMutagenTwo.transform.SetParent(mainTube.transform);
                    newMutagenTwo.transform.SetParent(mutagens.transform);
                  }

                  #region Archive
                  /* Mutagen newMutagen = Instantiate(mutagenPrefabs);
                   newMutagen.transform.position = mutagenSpawn[mutagenSpawn.Count - 1].Begin.position - newMutagen.End.localPosition;
                   mutagenSpawn.Add(newMutagen);
                   //newMutagen.transform.SetParent(mainTube.transform);
                   newMutagen.transform.SetParent(mainMutagen.transform);
                   newMutagen.transform.rotation = mainTube.transform.rotation;
                   //newMutagen.transform.Rotate(0, 0, EulerRotate(mutagenSpawn[mutagenSpawn.Count - 1].transform.rotation.z));
                   mutagenSpawn[mutagenSpawn.Count - 2].transform.SetParent(mainTube.transform);*/
                  /*switch (mutagenMod)
                  {
                      case "oneRight":
                          eulerMutagen += 15;
                          mainMutagen.transform.rotation = Quaternion.Euler(0, 0, eulerMutagen);
                          break;
  
                      case "oneLeft":
                          eulerMutagen -= 15;
                          mainMutagen.transform.rotation = Quaternion.Euler(0, 0, eulerMutagen);
                          break;
                  }*/

                  /*if (random > 0)
                  {
                      Spawn("mutagen");
                      random--;
                  }*/
                  #endregion
                  break;
            case "buff":           
                randomPointBuff = Random.Range(0, 6);
                Buff buff = Instantiate(baffs[Mathf.RoundToInt(Choose(oddsBaffs))]);
                buff.transform.SetParent(mutagens.transform);
                //buff.transform.rotation = mainTube.transform.rotation;
                buff.transform.position = oneSpawnMutagen.transform.GetChild(randomPointBuff).position;
                break;
            case "enemy":
                Enemy enemy = Instantiate(enemyPrefabs);
                //enemy.transform.SetParent(mainTube.transform);
                enemy.transform.position = new Vector3(0, -0.7f, 25);
                break;
        }
    }

    public void Tubes()
    {
        Debug.Log("OK");
    }
    
    /*private float EulerRotate(float LastCoordination)
    {
        LastCoordination = mutagenSpawn[mutagenSpawn.Count - 1].transform.rotation.z + number;
        countCoin++;
        if (schemeNumber == 5) schemeNumber = 1;
        if (score == 6) score = 0;
        if(countCoin > 15)
        {
            schemeNumber = Random.Range(1, 5);
            countCoin = 0;
        }
        switch (schemeNumber)
        {
            case 1:
                number += 0;
                break;
            case 2:
                number += 15;
                break;
            case 3:
                number -= 15;
                break;
            case 4:
                if (score < 3) number += 15;
                if (score >= 3)  number -= 15;
                score++;
                break;
            default:
                break;
        }
        if (number == 360) number = 0;
        return LastCoordination;
    }*/

    IEnumerator MutagenSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        Spawn("mutagen");
        switch (isMutagen)
        {
            case "one":
                if (random > 0)
                {
                    StartCoroutine(MutagenSpawn());
                    random--;
                }
                else
                {
                    randomTime = Random.Range(2, 6);
                    random = Random.Range(5, 10);
                    randomPoint = Random.Range(0, 6);
                    isMutagen = isMutagenArray[Random.Range(0, 2)];
                    if(isMutagen == "two")
                    {  
                        randomBefore = Random.Range(1, 4);
                        randomAfter = Random.Range(1, 3);
                        randomPointTwo = Random.Range(0, 6);
                        while (randomPointTwo == randomPoint)
                        {
                            randomPointTwo = Random.Range(0, 6);
                        }
                        random = Random.Range(4, 7);
                    }
                    StartCoroutine(NextMutagen());
                }
                break;
            case "two":
                if(randomBefore > 0)
                {
                    StartCoroutine(MutagenSpawn());
                    randomBefore--;
                }
                else
                {
                    if(random > 0)
                    {
                        isMutagenTwo = true;
                        StartCoroutine(MutagenSpawn());
                        random--;
                    }
                    else
                    {
                        isMutagenTwo = false;
                        if(Random.Range(0, 2) == 1) randomPoint = randomPointTwo;
                        if (randomAfter > 0)
                        {
                            StartCoroutine(MutagenSpawn());
                            randomAfter--;
                        }
                        else
                        {
                            randomTime = Random.Range(2, 6);
                            random = Random.Range(5, 10);
                            randomPoint = Random.Range(0, 6);
                            isMutagen = isMutagenArray[Random.Range(0, 2)];
                            if (isMutagen == "two")
                            {
                                randomBefore = Random.Range(1, 4);
                                randomAfter = Random.Range(1, 3);
                                randomPointTwo = Random.Range(0, 6);
                                while (randomPointTwo == randomPoint)
                                {
                                    randomPointTwo = Random.Range(0, 6);
                                }
                                random = Random.Range(4, 7);
                            }
                            StartCoroutine(NextMutagen());
                        }
                    }
                }
                break;

        }
    }
    IEnumerator NextMutagen()
    {   
        yield return new WaitForSeconds(randomTime);
        if (IsSpawnTunnels)
            StartCoroutine(MutagenSpawn());
        else
        {
            randomTime = 2;
            StartCoroutine(NextMutagen());
        }
    }

    IEnumerator DoudleTubeSpawn()
    {
        yield return new WaitForSeconds(13f);
        doubleTubeSpawn = true;
    }

    float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
    
    float ChooseTunnels(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}


