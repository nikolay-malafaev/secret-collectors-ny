using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PaulsController : MonoBehaviour
{
    public Barrier[] BarriersPrefabs;
    public Road[] RoadPrefabs;
    public Paul[] Pauls;
    public TubeController tubeController;
    [HideInInspector] public List<Paul> spawnPauls = new List<Paul>();

    private int[] countPaulsBetween;

    [Range(0, 100)] public float[] oddsBarriers;
    [Range(0, 100)] public float[] oddsPaul;
    private int countPauls;
    private int[] positionRoad = new []{0, 0, 0};
    private bool roadCreativ;
    private int numberPositionInRoad;
    private bool newRoad;
    private int lengthRoad;
    private int currentLegthRoad;
    private int numberPaul = 1;

    private void Start()
    {
        countPaulsBetween = new int[BarriersPrefabs.Length];
        for (int i = 0; i < BarriersPrefabs.Length; i++)
        {
            countPaulsBetween[i] = 0;
        }

        spawnPauls.Add(Pauls[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RoadGenerator();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            //numberPaul = 2;
        }
    }

    void FixedUpdate()
    {
        if (spawnPauls[spawnPauls.Count - 1].End.position.z < 55 & tubeController.isSpawnTunnels)
        {
            SpawnPauls();
        }

        if (spawnPauls.Count > 95)
        {
            DestoryPauls();
        }

        
    }

    private void SpawnPauls()
    {
        int countBarriers;
        int numberBarrier;
        int positionBarrier = 0;
        int busyPosition = 0;
        Vector3 offset;

        numberPaul = Mathf.RoundToInt(ChoosePauls(oddsPaul));
        Paul newPaul = Instantiate(Pauls[numberPaul], transform, true);
        
        if (numberPaul == 2)
        {
            PaulPit paulPit = newPaul.GetComponent<PaulPit>();
            paulPit.ActiveBag(Random.Range(0, 3));
            numberPaul = 1;
        }
        newPaul.transform.position = spawnPauls[spawnPauls.Count - 1].Begin.position - newPaul.End.localPosition;
        spawnPauls.Add(newPaul);
        //newPaul.transform.rotation = transform.rotation;

        if (countPauls == 0)
        {
            countPauls = Random.Range(3, 7);
            countBarriers = Random.Range(1, 3);
            for (int i = 0; i < countBarriers; i++)
            {
                numberBarrier = PaulGenerator();
                if (countPaulsBetween[numberBarrier] != 0 & BarriersPrefabs[numberBarrier].sameTypeDistation != 0)
                {
                    countPaulsBetween[numberBarrier]--;
                    numberBarrier = PaulGenerator(numberBarrier);
                }

                Barrier barrier = Instantiate(BarriersPrefabs[numberBarrier], newPaul.transform, true);
                if (barrier.sameTypeDistation != 0) countPaulsBetween[numberBarrier] = barrier.sameTypeDistation;
                if ((barrier.oneCountBarriers & i > 0) || (barrier.oneCountBarriers & roadCreativ))
                {
                    Destroy(barrier.gameObject);
                    break;
                }

                offset = barrier.offsetBarrier;
                switch (barrier.possible)
                {
                    case Barrier.PossiblePosition.Neutral:
                        if (!roadCreativ)
                        {
                            positionBarrier = Random.Range(0, 3);
                            if (i == 0) busyPosition = positionBarrier;
                            else if (i == 1)
                            {
                                if (busyPosition == positionBarrier)
                                {
                                    positionBarrier = PositionGenerator(busyPosition);
                                }
                            }
                        }
                        else
                        {
                            positionBarrier = PositionGenerator(numberPositionInRoad);
                        }

                        break;
                    case Barrier.PossiblePosition.Center:
                        positionBarrier = 0;
                        break;
                    case Barrier.PossiblePosition.Right:
                        positionBarrier = 1;
                        break;
                    case Barrier.PossiblePosition.Left:
                        positionBarrier = 2;
                        break;
                }

                barrier.transform.position = newPaul.NumberBarriers[positionBarrier].transform.position + offset;
                if (barrier.anyTypeDustation != 0) countPauls = barrier.anyTypeDustation;
                if (barrier.oneCountBarriers) break;
            }
        }
        else
        {
            countPauls--;
        }

        
        
        /*if (roadCreativ)
        {
            Road road;
            if (lengthRoad == currentLegthRoad)
            {
                road = Instantiate(RoadPrefabs[0], newPaul.transform, true);
                newRoad = false;
            }
            else if(lengthRoad == 1)
            {
                road = Instantiate(RoadPrefabs[0], newPaul.transform, true);
                road.transform.Rotate(0, 0, 180);
                road.transform.GetChild(0).tag = "UndRise";
            }
            else
            {
                road = Instantiate(RoadPrefabs[1], newPaul.transform, true);
            }
            

            road.transform.position = newPaul.NumberBarriers[numberPositionInRoad].transform.position +
                                      new Vector3(0f, 0.3f, 0);
            lengthRoad--;
            if (lengthRoad == 0) roadCreativ = false;
        }*/
    }

    private void DestoryPauls()
    {
        Destroy(spawnPauls[0].gameObject);
        spawnPauls.RemoveAt(0);
    }

    public int PaulGenerator()
    {
        int number;
        number = Mathf.RoundToInt(ChooseBarriers(oddsBarriers));
        return number;
    }

    public int PaulGenerator(int unwanted)
    {
        int number;
        int counter = 0;
        while (true)
        {
            number = Mathf.RoundToInt(ChooseBarriers(oddsBarriers));
            if (number != unwanted) break;
            counter++;
            if (counter > 500)
            {
                Debug.LogError("while!");
                break;
            }
        }

        return number;
    }

    public int PositionGenerator(int unwanted)
    {
        
        int numberPosition;
        int couner = 0;
        while (true)
        {
            numberPosition = Random.Range(0, 3);
            if (numberPosition != unwanted) break;
            couner++;
            if (couner > 500)
            {
                Debug.LogError("Position (WHILE!)");
                break;
            }
        }

        return numberPosition;
    }

    private void RoadGenerator()
    {
        lengthRoad = Random.Range(10, 25);
        
        currentLegthRoad = lengthRoad;
        int[] position = new []{1, 2};
        numberPositionInRoad = position[Random.Range(0, 2)];
        newRoad = true;
        roadCreativ = true;
    }
    


    float ChooseBarriers(float[] probs)
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
    
    float ChoosePauls(float[] probs)
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
