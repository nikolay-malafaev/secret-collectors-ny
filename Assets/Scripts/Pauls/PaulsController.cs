using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PaulsController : MonoBehaviour
{
    public Barrier[] BarriersPrefabs;
    public Paul[] Pauls;
    
    private ChunkController chunkController;
    private GameManager gameManager;
    private Player player;
    private int[] countPaulsBetween;

    [Range(0, 100)] public float[] oddsBarriers;
    private int countPauls;
    [HideInInspector] public Paul lastPaul;

    private void Start()
    {
        countPaulsBetween = new int[BarriersPrefabs.Length];
        for (int i = 0; i < BarriersPrefabs.Length; i++)
        {
            countPaulsBetween[i] = 0;
        }
        
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        chunkController = FindObjectOfType<ChunkController>();
        lastPaul = Pauls[0];
    }
    
    
    

    void Update()
    {
        if (transform.childCount > 95)
        {
            DestoryPauls();
        }

        if (chunkController.isSpawnPermit)
        {
            switch (gameManager.direction)
            {
                case 0:
                    if (lastPaul.End.position.z < 55)
                    {
                        SpawnPauls();
                    }

                    break;
                case 1:
                    if (lastPaul.End.position.x < 55)
                    {
                        SpawnPauls();
                    }

                    break;
                case 2:
                    if (lastPaul.End.position.z > -55)
                    {
                        SpawnPauls();
                    }

                    break;
                case 3:
                    if (lastPaul.End.position.x > -55)
                    {
                        SpawnPauls();
                    }

                    break;
            }
        }

        if (Input.GetKey(KeyCode.P))
        {
            SpawnPauls();
        }
        
    }

    private void SpawnPauls()
    {
        int countBarriers;
        int numberBarrier;
        int positionBarrier = 0;
        int busyPosition = 0;
        Vector3 offset;
        
        Paul newPaul = Instantiate(Pauls[1], transform, true);
        
       if (countPauls == 0)
        {
            countPauls = Random.Range(3, 7);
            countBarriers = Random.Range(1, 3);

            for (int i = 0; i < countBarriers; i++)
            {
                numberBarrier = Mathf.RoundToInt(ChooseBarriers(oddsBarriers));
                int sameType = BarriersPrefabs[numberBarrier].sameTypeDistation;
                if (countPaulsBetween[numberBarrier] != 0 & sameType != 0)
                {
                    countPaulsBetween[numberBarrier]--;
                    numberBarrier = Generator(numberBarrier, "paul");
                }


                Barrier barrier = Instantiate(BarriersPrefabs[numberBarrier], newPaul.transform, true);

                if (sameType != 0)
                {
                    countPaulsBetween[numberBarrier] = sameType;
                }

                if ((barrier.oneCountBarriers & i > 0))
                {
                    Destroy(barrier.gameObject);
                    break;
                }

                offset = barrier.offsetBarrier;
                
                switch (barrier.possible)
                {
                    case Barrier.PossiblePosition.Neutral:
                        positionBarrier = Random.Range(0, 3);
                        if (i == 0) busyPosition = positionBarrier;
                        else if (i == 1)
                        {
                            if (busyPosition == positionBarrier)
                            {
                                positionBarrier = Generator(busyPosition, "position");
                            }
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

                if(newPaul.NumberBarriers.Length > 0) barrier.transform.position = newPaul.NumberBarriers[positionBarrier].transform.position + offset;
                
                var rotationBarrier = barrier.transform.rotation.eulerAngles;
                barrier.transform.rotation = Quaternion.Euler(rotationBarrier.x, rotationBarrier.y + newPaul.transform.rotation.eulerAngles.y,rotationBarrier.z);
                if (barrier.anyTypeDistation != 0) countPauls = barrier.anyTypeDistation;
                if(barrier.oneCountBarriers) break;
            }
        }
        else
        {
            GeneratorMilieu generatorMilieu = newPaul.GetComponent<GeneratorMilieu>();
            generatorMilieu.Generate();
            countPauls--;
        }
        
        switch (gameManager.direction) // решить
        {
            case 0:
                newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, 0.989f);
                break;
            case 1:
                newPaul.transform.position = lastPaul.transform.position + new Vector3(0.989f, 0, 0);
                newPaul.transform.rotation = Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y + 90, 0);
                break;
            case 2:
                newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, -0.989f);
                newPaul.transform.rotation = Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y + 180, 0);
                break;
            case 3:
                newPaul.transform.position = lastPaul.transform.position + new Vector3(-0.989f, 0, 0);
                newPaul.transform.rotation = Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y - 90, 0);
                break;
        }

        lastPaul = newPaul;
    }

    private void DestoryPauls()
    {
       Destroy(transform.GetChild(0).gameObject); 
    }

    public int Generator(int unwanted, string name)
    {
        int number = 0;
        int counter = 0;
        while (true)
        {
            if (name == "paul")
            {
                number = Mathf.RoundToInt(ChooseBarriers(oddsBarriers));
            }
            else if( name == "position")
            {
                number = Random.Range(0, 3);
            }
            else
            {
                Debug.Log("Error name!");
                break;
            }
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
    
    
    public float ChooseBarriers(float[] probs)
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
