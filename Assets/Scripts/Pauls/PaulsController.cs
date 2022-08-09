using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using static Randomize.GetRandom;

public class PaulsController : MonoBehaviour
{
    public Barrier[] BarriersPrefabs;
    public Paul paulPrefab;
    public GameObject barriersPool;
    [HideInInspector] public Paul lastPaul;
    
    private ChunkController chunkController;
    private GameManager gameManager;
    private Player player;
    private int[] countPaulsBetween;

    [Range(0, 100)] public float[] oddsBarriers;
    private int countPauls;
    
    [SerializeField] private List<Paul> pauls;

    [SerializeField] private Paul startPaul;
    private PoolManager poolManager;
    private int minAnyTypeDistance;
    private int[] minSameTypeDistance;


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
                Barrier newBarrier = Instantiate(BarriersPrefabs[i],  barriersPool.transform, true);
                poolManager.barriersPool[i, j] = newBarrier;
                newBarrier.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        countPaulsBetween = new int[BarriersPrefabs.Length];
        minSameTypeDistance = new int[BarriersPrefabs.Length];
        for (int i = 0; i < BarriersPrefabs.Length; i++)
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
        GenerateBarrier(pauls[0]); 
        lastPaul = pauls[0];
        pauls.Add(pauls[0]);
        pauls.RemoveAt(0);
       
    }

    public void GenerateBarrier(Paul newPaul)
    {
        int numberBarrier = ChoiсeBarrier(newPaul);
        if(numberBarrier < 0 ) return;
        newPaul.countBarriers++;

        int place = ChoicePossiblePosition(numberBarrier, newPaul);
        newPaul.busyNumberBarriers[place] = true;
        if(place < 0) return;

        Barrier barrier = null;
        for (int o = 0; o < poolManager.barriersPool.GetLength(1); o++)
        {
            if (poolManager.barriersPool[numberBarrier, o] == null)
            {
                poolManager.barriersPool[numberBarrier, o] = Instantiate(BarriersPrefabs[numberBarrier], barriersPool.transform, true);
            }
            if (!poolManager.barriersPool[numberBarrier, o].isJob)
            {
                barrier = poolManager.barriersPool[numberBarrier, o];
                barrier.isJob = true;
                break;
            }
        }

        if (barrier == null)
        {
            //Debug.Log(numberBarrier);
            barrier = Instantiate(BarriersPrefabs[numberBarrier], barriersPool.transform.GetChild(numberBarrier),
                true); //количество барьеров создается меньше, чем нужжно
        }
        
        
        
        barrier.transform.SetParent(newPaul.transform);
        barrier.transform.position = newPaul.numberBarriers[place].transform.position + barrier.offsetBarrier; 
        barrier.gameObject.SetActive(true);
        newPaul.barriers.Add(barrier);
        newPaul.numberBarriersInPool.Add(numberBarrier);
        
        if(newPaul.countBarriers < 2) GenerateBarrier(newPaul);
        pauls[0].countBarriers = 0;
        pauls[0].busyNumberBarriers = new[] {false, false, false};
    }

    public void DestroyBarrier(Paul newPaul)
    {
        for (int i = 0; i < newPaul.barriers.Count; i++)
        {
            newPaul.barriers[i].transform.SetParent(barriersPool.transform);
            newPaul.barriers[i].gameObject.SetActive(false);
            newPaul.barriers[i].isJob = false;
            newPaul.barriers.RemoveAt(i);
        }
    }

    private int ChoiсeBarrier(Paul paul)
    {
        int numberBarrier = (int) Choose(oddsBarriers);
        if (paul.countBarriers == 0)
        {
            if (BarriersPrefabs[numberBarrier].oneCountBarrier) paul.countBarriers = 2;
            else paul.countBarriers += Random.Range(0, 2);
            //Debug.Log(countBarriersInPaul);
        }
        
        if (minAnyTypeDistance > 0)
        {
            minAnyTypeDistance--;
            return -1;
        }
        else minAnyTypeDistance = BarriersPrefabs[numberBarrier].anyTypeDistance;

        if (minSameTypeDistance[numberBarrier] > 0)
        {
            minSameTypeDistance[numberBarrier]--;
            for (int i = 0; i < oddsBarriers.Length; i++)
            {
                numberBarrier = (int)ChooseException(oddsBarriers, numberBarrier);
                if(minSameTypeDistance[numberBarrier] == 0) break;
            }
            
        }
        else minSameTypeDistance[numberBarrier] = BarriersPrefabs[numberBarrier].sameTypeDistance;
        
        return numberBarrier;
    }

    private int ChoicePossiblePosition(int numberBarrier, Paul paul)
    {
        int position = 0;
        switch (BarriersPrefabs[numberBarrier].possible)
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
}
