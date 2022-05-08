using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour
{
    public Road[] RoadPrefabs;
    public GameObject mainSpawn;

    private PaulsController paulsController;
    private ChunkController chunkController;

    private Paul paulForSpawn;
    private Road[] lastRoads = new Road[2];
    private int[] countRoad = new int[2];
    private int[] lenghtRoad = new int[2];
    private bool[] isSpawnRoad= new bool[2]; // 0 - left, 1 - right

    //private int[] positionRoad = new []{0, 0, 0};
    //private bool roadCreativ;
    //private int numberPositionInRoad;
    //private bool newRoad;
    //private int lengthRoad;
    //private int currentLegthRoad;


    //plan
    // generator
    // баг при внутреннем движении
    // 
    // shots (death before 2 shots)


    private void Start()
    {
        chunkController = FindObjectOfType<ChunkController>();
        paulsController = FindObjectOfType<PaulsController>();
        int choose = Random.Range(0, 2);
        StartCoroutine(TimeSpawnRoad(1.5f, choose));
        if (choose > 0) choose = 0;
        else choose = 1;
        StartCoroutine(TimeSpawnRoad(Random.Range(4, 8), choose));
        isSpawnRoad[0] = false;
        isSpawnRoad[1] = false;
    }


    private void Update()
    {
        if (isSpawnRoad[0])
        {
            switch (chunkController.gameManager.direction)
            {
                case 0:
                    if (lastRoads[0].transform.position.z < 40)
                    {
                        SpawnRoad(0);
                    }

                    break;
                case 1:
                    if (lastRoads[0].transform.position.x < 55)
                    {
                        SpawnRoad(0);
                    }

                    break;
                case 2:
                    if (lastRoads[0].transform.position.z > -55)
                    {
                        SpawnRoad(0);
                    }

                    break;
                case 3:
                    if (lastRoads[0].transform.position.x > -55)
                    {
                        SpawnRoad(0);
                    }

                    break;
            }
        }
        if (isSpawnRoad[1])
        {
            switch (chunkController.gameManager.direction)
            {
                case 0:
                    if (lastRoads[1].transform.position.z < 40)
                    {
                        SpawnRoad(1);
                    }

                    break;
                case 1:
                    if (lastRoads[1].transform.position.x < 55)
                    {
                        SpawnRoad(1);
                    }

                    break;
                case 2:
                    if (lastRoads[1].transform.position.z > -55)
                    {
                        SpawnRoad(1);
                    }

                    break;
                case 3:
                    if (lastRoads[1].transform.position.x > -55)
                    {
                        SpawnRoad(1);
                    }

                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Replacement(Target.Direction.Left);
        }
    }

    public void SpawnRoad(int n)
    {
        int number = SeletionRoad(n);
        Road newRoad = Instantiate(RoadPrefabs[number], transform, true);
        if (n == 0)
        {
            if(number == 1) newRoad.transform.rotation = Quaternion.Euler(-90, 180, 0);
        }

        switch (chunkController.gameManager.direction)
        {
            case 0:
                newRoad.transform.position = lastRoads[n].transform.position + new Vector3(0, 0, 0.989f);
                break;
            case 1:
                newRoad.transform.position = lastRoads[n].transform.position + new Vector3(0.989f, 0, 0);
                newRoad.transform.rotation = Quaternion.Euler(-90, newRoad.transform.rotation.y + 90, 0);
                break;
            case 2:
                newRoad.transform.position = lastRoads[n].transform.position + new Vector3(0, 0, -0.989f);
                newRoad.transform.rotation = Quaternion.Euler(-90, newRoad.transform.rotation.y + 180, 0);
                break;
            case 3:
                newRoad.transform.position = lastRoads[n].transform.position + new Vector3(-0.989f, 0, 0);
                newRoad.transform.rotation = Quaternion.Euler(-90, newRoad.transform.rotation.y -90 , 0);
           
                break;
        }
        
        lastRoads[n] = newRoad;
        countRoad[n]++;
    }


    private int SeletionRoad(int n)
    {
        int number = 1;
        if (countRoad[n] == lenghtRoad[n])
        {
            number = 2;
            isSpawnRoad[n] = false;
            countRoad[n] = 0;
            StartCoroutine(TimeSpawnRoad(Random.Range(5, 10), n));
        }
        return number;
    }

    public void Replacement(Target.Direction direction)
    {
        int n = 0;
        switch (direction)
        {
            case Target.Direction.Left:
                n = 0;
                break;
            case Target.Direction.Right:
                n = 1;
                break;
        }

        if (lastRoads[n] != null)
        {
            isSpawnRoad[n] = false;
            if (lastRoads[n].gameObject.CompareTag("BeginRoad"))
            {
                Destroy(lastRoads[n].gameObject);
            }
            else if (lastRoads[n].gameObject.CompareTag("EndRoad"))
            {

            }
            else
            {
                Road newRoad = Instantiate(RoadPrefabs[2], transform, true);
                newRoad.transform.position = lastRoads[0].transform.position + new Vector3(0, 0, 0.989f);
            }
        }

        StartCoroutine(TimeSpawnRoad(Random.Range(4, 15), n));
    }
    

    IEnumerator TimeSpawnRoad(float time, int n)
    {
        yield return new WaitForSeconds(time);
        isSpawnRoad[n] = true;
        lastRoads[n] = Instantiate(RoadPrefabs[0], transform, true);
        Transform dotSpawn = null;
        if (n == 0)
        {
            dotSpawn = mainSpawn.transform.GetChild(2);
            lastRoads[n].transform.position = new Vector3(dotSpawn.position.x - 0.25f, -0.964f, dotSpawn.position.z);
        }
        if (n == 1)
        {
            dotSpawn = mainSpawn.transform.GetChild(0);
            lastRoads[n].transform.position = new Vector3(dotSpawn.position.x + 0.25f, -0.964f, dotSpawn.position.z);
        }
        lastRoads[n].transform.rotation = Quaternion.Euler(0, chunkController.gameManager.direction * 90, 0);
        lenghtRoad[n] = Random.Range(12, 18);
    }
}