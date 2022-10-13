using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using static Randomize.GetRandom;

public class PaulsController : MonoBehaviour
{
    public Barrier[] barriersPrefabs;
    public Paul paulPrefab;
    
    [HideInInspector] public Paul lastPaul;
    
    private ChunkController chunkController;
    private GameManager gameManager;
    private Player player;
    private int[] countPaulsBetween;

    [Range(0, 100)] public float[] oddsBarriers;
    private int countPauls;
    
    [SerializeField] private List<Paul> pauls;

    [SerializeField] private GameObject barriersPool;
    [SerializeField] private Paul startPaul;
    private PoolManager poolManager;
    /// <summary>
    /// 0 - singlePaul, 1 - doublePaul
    /// </summary>
    private int[] minAnyTypeDistance; 
    /// <summary>
    /// 0 - typePaul (single or doublePaul), 1 - minSameTypeDistance curren Paul
    /// </summary>
    private int[,] minSameTypeDistance;
    private int lastNumberBarrier;


    private void Awake()
    {
        poolManager = FindObjectOfType<PoolManager>();
        int maxValue = 0;
        for (int u = 0; u < oddsBarriers.Length; u++)
        {
            if (oddsBarriers[u] > maxValue)
            {
                maxValue = (int)oddsBarriers[u];
            }
        }

        maxValue += 3;
        
        poolManager.barriersPool = new Barrier[oddsBarriers.Length, maxValue];
        for (int i = 0; i < oddsBarriers.Length; i++)
        {
            for (int j = 0; j <  maxValue; j++) // создавать в зависимости от odds barriers
            {
                if (oddsBarriers[i] == 0) continue;
                Barrier newBarrier = Instantiate(barriersPrefabs[i],  barriersPool.transform, true);
                poolManager.barriersPool[i, j] = newBarrier;
                newBarrier.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        countPaulsBetween = new int[barriersPrefabs.Length];
        minSameTypeDistance = new int[2, barriersPrefabs.Length];
        minAnyTypeDistance = new int[2];
        for (int i = 0; i < barriersPrefabs.Length; i++)
        {
            countPaulsBetween[i] = 0;
        }
        
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        chunkController = FindObjectOfType<ChunkController>();
       
        pauls.Add(startPaul);
        lastPaul = startPaul;
        for (int i = 0; i < 94; i++)
        {
            Paul newPaul = Instantiate(paulPrefab, transform, true);
            pauls.Add(newPaul);
            newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, 0.989f);
            if (i > 20)
            {
                GenerateBarrier(newPaul); 
            }

            newPaul.typePaul = "singlePaul";
            lastPaul = newPaul;
        }
    }
    

    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, lastPaul.transform.position);
        if (dist < 70 && chunkController.isSpawnPermit)
        {
            GeneratePaul();
        }
    }

    private void GeneratePaul()
    {
        if(pauls[0].barriers.Count > 0) DestroyBarrier(pauls[0]);
        pauls[0].transform.rotation = Quaternion.Euler(0, gameManager.ChooiseDirectionRotarion(), 0);
        pauls[0].transform.position = lastPaul.transform.position + gameManager.ChooiseDirectionPosition(0.989f);
        pauls[0].generatorMilieu.GenerateMilieus();
        GenerateBarrier(pauls[0]); 
        lastPaul = pauls[0];
        pauls.Add(pauls[0]);
        pauls.RemoveAt(0);
    }

    public void GenerateBarrier(Paul paul)
    {
        int numberBarrier = ChoiсeBarrier(paul);
        
        if (numberBarrier < 0)
        {
            return;
        }
        int place = ChoicePossiblePosition(numberBarrier, paul);
        
        if (place < 0)
        {
            return;
        }
        
        paul.busyNumberBarriers[place] = true;
        paul.countBarriers++;

        Barrier barrier = null;
        for (int o = 0; o < poolManager.barriersPool.GetLength(1); o++)
        {
            if (poolManager.barriersPool[numberBarrier, o] == null)
            {
                poolManager.barriersPool[numberBarrier, o] = Instantiate(barriersPrefabs[numberBarrier], barriersPool.transform, true);
            }
            if (!poolManager.barriersPool[numberBarrier, o].isJob)
            {
                barrier = poolManager.barriersPool[numberBarrier, o];
                barrier.isJob = true;
                break;
            }
        }

        if (barrier == null) //когда количество барьеров создается меньше, чем нужжно
        {
            //Debug.Log(numberBarrier);
            barrier = Instantiate(barriersPrefabs[numberBarrier], barriersPool.transform.GetChild(numberBarrier),
                true); 
        }
        
        
        
        barrier.transform.SetParent(paul.transform);
        barrier.transform.position = paul.numberBarriers[place].transform.position + barrier.offsetBarrier; 
        barrier.gameObject.SetActive(true);

        var rotationBarrier = barrier.transform.rotation;
        rotationBarrier = Quaternion.Euler(rotationBarrier.eulerAngles.x,  paul.transform.rotation.eulerAngles.y, rotationBarrier.eulerAngles.z);
        barrier.transform.rotation = rotationBarrier;

        if(barrier.isMultiBarrier) barrier.multiBarrier.SetActiveBarrier();
        paul.barriers.Add(barrier);
        paul.numberBarriersInPool.Add(numberBarrier);
        
        
        if(paul.countBarriers < 2) GenerateBarrier(paul);
        pauls[0].countBarriers = 0;
        pauls[0].busyNumberBarriers = new[] {false, false, false, true};
    }

    public void DestroyBarrier(Paul newPaul)
    {
        for (int i = 0; i < newPaul.barriers.Count; i++)
        {
            newPaul.barriers[i].transform.SetParent(barriersPool.transform);
            newPaul.barriers[i].gameObject.SetActive(false);
            newPaul.barriers[i].isJob = false;
            if(newPaul.barriers[i].isMultiBarrier) newPaul.barriers[i].multiBarrier.SetUnActiveBarrier();
            newPaul.barriers.RemoveAt(i);
        }
    }

    private int ChoiсeBarrier(Paul paul)
    {
        int currenPaul = 0;
        int numberBarrier;
        if (paul.typePaul == "singlePaul") currenPaul = 0;
        else if (paul.typePaul == "doublePaul") currenPaul = 1;
        
        if (minAnyTypeDistance[currenPaul] > 0 && paul.countBarriers == 0)
        {
            minAnyTypeDistance[currenPaul]--;
            return -1;
        }
        
        numberBarrier = (int)Choose(currenPaul);
        if (numberBarrier < 0) return -1;

        minSameTypeDistance[currenPaul, numberBarrier] = barriersPrefabs[numberBarrier].sameTypeDistance;
        
        
        if (paul.countBarriers == 0)
        {
            if (barriersPrefabs[numberBarrier].oneCountBarrier) paul.countBarriers = 1;
            else paul.countBarriers += Random.Range(0, 2);
            minAnyTypeDistance[currenPaul] = barriersPrefabs[numberBarrier].anyTypeDistance;
            lastNumberBarrier = numberBarrier;
        }
        else if (paul.countBarriers == 1)
        {
            if (barriersPrefabs[numberBarrier].oneCountBarrier) return -1;
        }
        
        return numberBarrier;
    }

    private int ChoicePossiblePosition(int numberBarrier, Paul paul) // если заята position, то нужно менять
    {
        int position = 0;
        switch (barriersPrefabs[numberBarrier].possible)
        {
            case Barrier.PossiblePosition.Neutral:
                position = Random.Range(0, 3);
                
                if (paul.busyNumberBarriers[position])
                {
                    for (int i = 0; i < 3; i++)
                    {
                        position = Random.Range(0, 3);
                        if(!paul.busyNumberBarriers[position]) break;
                    }
                }
                break;
            case Barrier.PossiblePosition.Center:
                position = 0; 
                if (paul.busyNumberBarriers[position]) return -1;
                break;
            case Barrier.PossiblePosition.Right:
                position = 1;
                if (paul.busyNumberBarriers[position]) return -1;
                break;
            case Barrier.PossiblePosition.Left:
                position = 2;
                if (paul.busyNumberBarriers[position]) return -1;
                break;
        }

        
        return position;
    }
    
    private float Choose(int type)
    {
        float total = 0;
        float[] probability = new float[oddsBarriers.Length];
            

        for (int i = 0; i < oddsBarriers.Length; i++)
        {
            if (minSameTypeDistance[type, i] == 0)
            {
                probability[i] = oddsBarriers[i];
            }
            else
            {
                probability[i] = 0;
                minSameTypeDistance[type, i]--;
            }
        }

        foreach (float elem in probability)
        {
            total += elem;
        }
            
        float randomPoint = Random.value * total;

        for (int i = 0; i < probability.Length; i++)
        {
            if (randomPoint < probability[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probability[i];
            }
        }
        return -1;
        //return probability.Length - 1;
    }
}
