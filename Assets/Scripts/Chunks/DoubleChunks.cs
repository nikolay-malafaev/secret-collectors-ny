using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleChunks : MonoBehaviour
{

    public GameObject Pauls;
    private ChunkController chunkController;
    private PaulsController paulsController;
    public Chunk newChunk;
    public Paul attachingPaul; //
    private int[] countPaulsBetween;
    [HideInInspector] public Animator animator;
    private Target target;
    private Paul lastPaul;



    void Start()
    {
        chunkController = GetComponentInParent<ChunkController>();
        animator = GetComponent<Animator>();
        paulsController = chunkController.GetComponentInChildren<PaulsController>();
        countPaulsBetween = new int[paulsController.BarriersPrefabs.Length];
        target = GetComponentInChildren<Target>();
        StartCoroutine(SpawnPaulsTime());
        lastPaul = attachingPaul;
    }
    
    
    public void TurnChunks(Target.Direction direction) 
    {
        
        chunkController.isSpawnPermit = false;
        newChunk.transform.SetParent(chunkController.transform);
        chunkController.spawnChunks.Add(newChunk);
        chunkController.isSpawnPermit = true;
        paulsController.lastPaul = lastPaul;
        
        
        switch (direction)
        {
            case Target.Direction.Right:
                chunkController.gameManager.direction += 1;
                if (chunkController.gameManager.direction == 4) chunkController.gameManager.direction = 0;
                chunkController.player.vetricalQuaternion = Quaternion.Euler(0, chunkController.player.vetricalQuaternion.eulerAngles.y + 90, 0);
                chunkController.player.animator.SetTrigger("run");
                chunkController.player.ChangeLane(-1);
                break;
            case Target.Direction.Left:
                chunkController.gameManager.direction -= 1;
                if (chunkController.gameManager.direction == -1) chunkController.gameManager.direction = 3;
                chunkController.player.vetricalQuaternion = Quaternion.Euler(0, chunkController.player.vetricalQuaternion.eulerAngles.y - 90, 0);
                chunkController.player.ChangeLane(1);
                break;
        }
        chunkController.gameManager.Turn();
    }
    
    
    IEnumerator SpawnPaulsTime()
    {
        yield return new WaitForSeconds(1.1f);
        SpawnPauls();
    }

    private void SpawnPauls()
    {
        int countPauls = 0;
        int countBarriers;
        int paulNumber;
        int positionBarrier = 0;
        int busyPosition = 0; 
        Vector3 offset;
        for (int i = 0; i < 30; i++)
        {
            Paul newPaul = Instantiate(chunkController.paulsController.Pauls[1], Pauls.transform, true);
            
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
                        paulNumber = Mathf.RoundToInt( paulsController.ChooseBarriers(paulsController.oddsBarriers));
                        if (countPaulsBetween[paulNumber] != 0 & paulsController.BarriersPrefabs[paulNumber].sameTypeDistation != 0)
                        {
                            countPaulsBetween[paulNumber]--;
                            paulNumber = paulsController.Generator(paulNumber, "paul");
                        }

                        Barrier barrier = Instantiate(paulsController.BarriersPrefabs[paulNumber], newPaul.transform, true);
 
                        if (barrier.sameTypeDistation != 0) countPaulsBetween[paulNumber] = barrier.sameTypeDistation;

                        if ((barrier.oneCountBarriers & p > 0))
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
                            newPaul.NumberBarriers[positionBarrier].transform.position + offset;
                        
                        var rotationBarrier = barrier.transform.rotation;
                        barrier.transform.rotation = Quaternion.Euler(rotationBarrier.x, rotationBarrier.y + newPaul.transform.rotation.eulerAngles.y,rotationBarrier.z);
                        
                        if (barrier.anyTypeDistation != 0) countPauls = barrier.anyTypeDistation;
                        
                        if(barrier.oneCountBarriers) break;
                    }
                }
                else
                {
                    countPauls--;
                }
            }
        }
    }
}
