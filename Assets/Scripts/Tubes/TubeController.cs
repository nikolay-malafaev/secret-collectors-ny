using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeController : MonoBehaviour
{
    [Range(0, 100)]
    public float[] oddsTubes;
    
    public float numberAddSpeed;
    public float periodAddSpeed = 10;
    public GameObject mutagens;
    [HideInInspector] public Player player;
    [HideInInspector] public bool isSpawnTunnels;
    [HideInInspector] public PaulsController paulsController;
    [HideInInspector] public List<Tube> spawnTubes = new List<Tube>();
    [HideInInspector] public bool pausePosition;
    [HideInInspector] public float positionTubeZ;
     
    
    [SerializeField] private Tube[] tubePrefabs;
    [SerializeField] private Tube startTube;
    [SerializeField] private GameObject mainTube;
    [SerializeField] private GameManager gameManager;
    private bool doubleTubeSpawn;
    private float nextActionTimeAddSpeed;
    private float addSpeed;
    
    void Start()
    {
        paulsController = GetComponentInChildren<PaulsController>();
        doubleTubeSpawn = false;
        addSpeed = 0.104f;
        numberAddSpeed = 0.0005f;
        nextActionTimeAddSpeed = 0;
        spawnTubes.Add(startTube);
        StartCoroutine(DoudleTubeSpawn());
        isSpawnTunnels = true;
    }

    void FixedUpdate()
    {
        if (spawnTubes[spawnTubes.Count - 1].End.position.z < 40 & isSpawnTunnels)
        {
            SpawnTube();
        }

        if (spawnTubes.Count > 20)
        {
            DestoryTube();
        }
        
        if (!pausePosition)
        {
            mainTube.transform.position = new Vector3(0, 0, positionTubeZ);
            positionTubeZ -= addSpeed;
        }
        
        if (Time.time > nextActionTimeAddSpeed) // (period)
        {
            addSpeed += numberAddSpeed;
            nextActionTimeAddSpeed += periodAddSpeed;
        }
    }

    private void SpawnTube()
    {
        int choose;
        while (true)
        { 
            choose = Mathf.RoundToInt(Choose(oddsTubes));
           if(choose != 1) break;
           else if (doubleTubeSpawn)
           {
               StartCoroutine(DoudleTubeSpawn());
               doubleTubeSpawn = false;
               break;
           }
        }
        Tube newTube = Instantiate(tubePrefabs[choose], mainTube.transform, true);
        newTube.GenerateBarrier();
        newTube.transform.position = spawnTubes[spawnTubes.Count - 1].Begin.position - newTube.End.localPosition;
        spawnTubes.Add(newTube);
        newTube.transform.rotation = mainTube.transform.rotation;
        
    }
    private void DestoryTube()
    {
        Destroy(spawnTubes[0].gameObject);
        spawnTubes.RemoveAt(0);
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
}


