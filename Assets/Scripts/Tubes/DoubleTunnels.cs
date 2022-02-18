using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTunnels : MonoBehaviour
{
    Vector3 verticalTargetRotation;
    Vector3 verticalTargetPosition;
    public GameObject TubeNumberTwo;
    public GameObject Pauls;
    private TubeController tubeController;
    private PaulsController paulsController;
    public Tube NewTube;
    public Paul attachingPaul; 
    public GlobalController globalController;
    private List<Paul> spawnPaulInDT = new List<Paul>();
    private int[] countPaulsBetween;
    void Start()
    {
        tubeController = GetComponentInParent<TubeController>();
        globalController = GetComponentInParent<GlobalController>();
        paulsController = tubeController.GetComponentInChildren<PaulsController>();
        countPaulsBetween = new int[paulsController.BarriersPrefabs.Length];
        spawnPaulInDT.Add(attachingPaul);
        SpawnPauls();
    }
    
    void Update()
    {
        Vector3 direction = Vector3.RotateTowards( transform.forward, verticalTargetRotation, 1.7f * Time.deltaTime, 0);
        //transform.position = Vector3.MoveTowards( transform.position, verticalTargetPosition, 1 * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public void TurnTunnels() 
    {
        tubeController.isSpawnTunnels = false;
        tubeController.player.camera.MoveHole();
        
        for (int i = 0; i < tubeController.spawnTubes.Count; i++)
        {
            tubeController.spawnTubes[i].transform.SetParent(transform);
        }
        tubeController.mutagens.transform.SetParent(transform);
        paulsController.transform.SetParent(transform);
        //TubeNumberTwo.SetActive(false);
        verticalTargetRotation = new Vector3(-0.5f, 0, 0);
        verticalTargetPosition = new Vector3(0.375f, 0, 0);
        StartCoroutine(FreeSpawnTube());
    }

    IEnumerator FreeSpawnTube() 
    {
        yield return new WaitForSeconds(0.71f);
        tubeController.player.ChangeLane(-1);
        transform.position = new Vector3(0.41f, 0, transform.position.z); // 0.41f
        yield return new WaitForSeconds(1.15f);
        tubeController.spawnTubes[tubeController.spawnTubes.Count - 1] = NewTube;
        spawnPaulInDT[spawnPaulInDT.Count - 1].transform.SetParent(paulsController.transform);
        paulsController.spawnPauls[paulsController.spawnPauls.Count - 1] = spawnPaulInDT[spawnPaulInDT.Count - 1];
        for (int i = 0; i < tubeController.spawnTubes.Count; i++)
        {
            tubeController.spawnTubes[i].transform.SetParent(tubeController.transform);
            
        }
        tubeController.mutagens.transform.SetParent(tubeController.transform);
        paulsController.transform.SetParent(tubeController.transform);
        tubeController.isSpawnTunnels = true;
    }

    private void SpawnPauls()
    {
        int countPauls = 0;
        int countBarriers;
        int paulNumber;
        int positionBarrier = 0;
        int busyPosition = 0; 
        Vector3 offset;
        for (int i = 0; i < 71; i++)
        {
            Paul newPaul = Instantiate(tubeController.paulsController.Pauls[1], Pauls.transform, true);
            newPaul.transform.Rotate(0, 90,0);
            newPaul.transform.position = spawnPaulInDT[spawnPaulInDT.Count - 1].Begin.position - newPaul.End.position;
            spawnPaulInDT.Add(newPaul);

            if (i > 10)
            {
                if (countPauls == 0)
                {
                    countPauls = Random.Range(3, 7);
                    countBarriers = Random.Range(1, 3);
                    for (int p = 0; p < countBarriers; p++)
                    {
                        paulNumber = paulsController.PaulGenerator();
                        if (countPaulsBetween[paulNumber] != 0 & paulsController.BarriersPrefabs[paulNumber].sameTypeDistation != 0)
                        {
                            countPaulsBetween[paulNumber]--;
                            paulNumber = paulsController.PaulGenerator(paulNumber);
                        }

                        Barrier barrier = Instantiate(paulsController.BarriersPrefabs[paulNumber], newPaul.transform, true);
                        var rotationBarrier = barrier.transform.rotation;
                        barrier.transform.Rotate(rotationBarrier.x, rotationBarrier.x + 90, rotationBarrier.z);
                        if (barrier.sameTypeDistation != 0) countPaulsBetween[paulNumber] = barrier.sameTypeDistation;
                        if (barrier.oneCountBarriers )
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
                                        positionBarrier = paulsController.PositionGenerator(busyPosition);
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
                        if (barrier.anyTypeDustation != 0) countPauls = barrier.anyTypeDustation;
                        if (barrier.oneCountBarriers) break;
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
