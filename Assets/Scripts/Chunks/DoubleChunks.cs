using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoubleChunks : MonoBehaviour
{
    public List<Paul> pauls;
    public GameObject stop;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Target target;
    [HideInInspector] public float defoltEuler;
    [SerializeField] private Chunk attachingChunk;
    [SerializeField] private Paul lastPaul;
    [SerializeField] private Transform paulsTransform;
    private ChunkController chunkController;
    private PaulsController paulsController;
    




    private void Awake()
    {
        target = GetComponentInChildren<Target>();
        defoltEuler = transform.rotation.eulerAngles.y;
    }

    void Start()
    {
        target = GetComponentInChildren<Target>();
        chunkController = FindObjectOfType<ChunkController>();
        animator = GetComponent<Animator>();
        
        //paulsController = chunkController.GetComponentInChildren<PaulsController>();
        for (int i = 0; i < 30; i++)
        {
            Paul newPaul = Instantiate(chunkController.paulsController.paulPrefab, paulsTransform, true);
            pauls.Add(newPaul);
            switch (target.direction)
            {
                case Target.Direction.Right:
                    newPaul.transform.rotation = Quaternion.Euler(0, 90, 0);
                    newPaul.transform.position = lastPaul.transform.position + new Vector3(0.989f, 0, 0);
                    break;
                case Target.Direction.Left:
                    newPaul.transform.rotation = Quaternion.Euler(0, -90, 0);
                    newPaul.transform.position = lastPaul.transform.position + new Vector3(-0.989f, 0, 0);
                    break;
            }

            newPaul.typePaul = "doublePaul";
            lastPaul = newPaul;
        }
        gameObject.SetActive(false);
    }

   

    public void TurnChunks(Target.Direction direction) 
    {
        chunkController.isSpawnPermit = false;
        chunkController.lastChunk = attachingChunk;
        chunkController.paulsController.lastPaul = lastPaul;

        int lastDirection = chunkController.gameManager.direction;
        switch (direction)
        {
            case Target.Direction.Right:
                chunkController.gameManager.direction += 1;
                if (chunkController.gameManager.direction == 4) chunkController.gameManager.direction = 0;
                chunkController.player.vetricalQuaternion = Quaternion.Euler(0, chunkController.player.vetricalQuaternion.eulerAngles.y + 90, 0);
                chunkController.player.animator.SetTrigger("run");
                chunkController.player.ChangeLane(-1);
                chunkController.player.camera.Turn(1, lastDirection);
                break;
            case Target.Direction.Left:
                chunkController.gameManager.direction -= 1;
                if (chunkController.gameManager.direction == -1) chunkController.gameManager.direction = 3;
                chunkController.player.vetricalQuaternion = Quaternion.Euler(0, chunkController.player.vetricalQuaternion.eulerAngles.y - 90, 0);
                chunkController.player.ChangeLane(1);
                chunkController.player.camera.Turn(-1, lastDirection);
                break;
        }
        chunkController.gameManager.Turn(direction);
    }

    public void BackToPool(int i)
    {
        if (i == 0)
        {
            transform.SetParent(chunkController.pool.transform);
            transform.gameObject.SetActive(false);
            if (stop.activeSelf) stop.SetActive(false);
            chunkController.isDoubleChunksInScene = false;
            //return barriers;
        }
        else StartCoroutine(TimeBackToPool());
    }

    private IEnumerator TimeBackToPool()
    {
        yield return new WaitForSeconds(10);
        BackToPool(0);
    }

    /*private void SpawnPauls()
    {
        int countPauls = 0;
        int countBarriers;
        int paulNumber;
        int positionBarrier = 0;
        int busyPosition = 0; 
        Vector3 offset;
        for (int i = 0; i < 30; i++)
        {
            Paul newPaul = Instantiate(chunkController.paulsController.Pauls[1], pauls.transform, true);
            
            switch (target.direction)
            {
                case Target.Direction.Right:
                    switch (chunkController.gameManager.direction)
                    {
                        case 3:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, 0.989f);
                            break;
                        case 0:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(0.989f, 0, 0);
                            newPaul.transform.rotation =
                                Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y + 90, 0);
                            break;
                        case 1:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, -0.989f);
                            newPaul.transform.rotation =
                                Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y + 180, 0);
                            break;
                        case 2:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(-0.989f, 0, 0);
                            newPaul.transform.rotation =
                                Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y - 90, 0);
                            break;
                    }
                    break;
                case Target.Direction.Left:
                    switch (chunkController.gameManager.direction)
                    {
                        case 1:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, 0.989f);
                            break;
                        case 2:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(0.989f, 0, 0);
                            newPaul.transform.rotation =
                                Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y + 90, 0);
                            break;
                        case 3:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(0, 0, -0.989f);
                            newPaul.transform.rotation =
                                Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y + 180, 0);
                            break;
                        case 0:
                            newPaul.transform.position = lastPaul.transform.position + new Vector3(-0.989f, 0, 0);
                            newPaul.transform.rotation =
                                Quaternion.Euler(0, newPaul.transform.rotation.eulerAngles.y - 90, 0);
                            break;
                    }
                    break;
            }  
            
            lastPaul = newPaul;
            
           if (i > 10)
            {
                if (countPauls == 0)
                {
                    countPauls = Random.Range(3, 7);
                    countBarriers = Random.Range(1, 3);
                    for (int p = 0; p < countBarriers; p++)
                    {
                        paulNumber = Mathf.RoundToInt( paulsController.Choose(paulsController.oddsBarriers));
                        if (countPaulsBetween[paulNumber] != 0 & paulsController.BarriersPrefabs[paulNumber].sameTypeDistance != 0)
                        {
                            countPaulsBetween[paulNumber]--;
                            paulNumber = paulsController.Generator(paulNumber, "paul");
                        }

                        Barrier barrier = Instantiate(paulsController.BarriersPrefabs[paulNumber], newPaul.transform, true);
 
                        if (barrier.sameTypeDistance != 0) countPaulsBetween[paulNumber] = barrier.sameTypeDistance;

                        if ((barrier.oneCountBarrier & p > 0))
                        {
                            Destroy(barrier.gameObject);
                            continue;
                        }
                        
                        offset = barrier.offsetBarrier;
                        switch (barrier.possible)
                        {
                            case Barrier.PossiblePosition.Neutral:
                                positionBarrier = Random.Range(0, 3);
                                if (p == 0) busyPosition = positionBarrier;
                                else if (p == 1)
                                {
                                    if (busyPosition == positionBarrier)
                                    {
                                        positionBarrier = paulsController.Generator(busyPosition, "position");
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

                        barrier.transform.position =
                            newPaul.numberBarriers[positionBarrier].transform.position + offset;
                        
                        var rotationBarrier = barrier.transform.rotation;
                        barrier.transform.rotation = Quaternion.Euler(rotationBarrier.x, rotationBarrier.y + newPaul.transform.rotation.eulerAngles.y,rotationBarrier.z);
                        
                        if (barrier.anyTypeDistance != 0) countPauls = barrier.anyTypeDistance;
                        
                        if(barrier.oneCountBarrier) break;
                    }
                }
                else
                {
                    countPauls--;
                }
            }
        }
    }*/
}
