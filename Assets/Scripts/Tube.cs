using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tube : MonoBehaviour
{
    public Transform Begin;
    public Transform End;
    public Tube NewTube;
    public bool isGenerateBarrier;
    public GameObject barrier;
    public float[] barrierPositionsX;
    public float[] barrierRotationY;
    Vector3 verticalTargetRotation;
    Vector3 verticalTargetPosition;
    Vector3 defoltRotation = new Vector3(0, 0, 0);
    public GameObject TubeNumberTwo;
    private TubeController tubeController;



    private void Update()
    {
       
        Vector3 direction = Vector3.RotateTowards( transform.forward, verticalTargetRotation, 1.7f * Time.deltaTime, 0);
        //transform.position = Vector3.MoveTowards( transform.position, verticalTargetPosition, 1 * Time.deltaTime);
       transform.rotation = Quaternion.LookRotation(direction);
    }

    public void TurnTunnels()
    {
        tubeController = GetComponentInParent<TubeController>();
        for (int i = 0; i < tubeController.spawnTubes.Count; i++)
        {
            tubeController.spawnTubes[i].transform.SetParent(transform);
        }
        TubeNumberTwo.SetActive(false);
        verticalTargetRotation = new Vector3(-0.5f, 0, 0);
        transform.position = new Vector3(0.41f, 0, transform.position.z);
        verticalTargetPosition = new Vector3(0.375f, 0, 0);
        
        tubeController.IsSpawnTunnels = false;
        StartCoroutine(FreeSpawnTube());
    }

    IEnumerator FreeSpawnTube()
    {
        yield return new WaitForSeconds(1.15f);
        //NewTube.transform.SetParent(tubeController.transform);
        tubeController.spawnTubes[tubeController.spawnTubes.Count - 1] = NewTube;
        tubeController.IsSpawnTunnels = true;
        for (int i = 0; i < tubeController.spawnTubes.Count; i++)
        {
            tubeController.spawnTubes[i].transform.SetParent(tubeController.transform);
        }
    }
    
    public void GenerateBarrier()
    {
        if(!isGenerateBarrier || (barrierPositionsX.Length != barrierRotationY.Length))
            return;

        int r =  Random.Range(0, barrierPositionsX.Length);
        var position = barrier.transform.position;
        var rotation = barrier.transform.rotation;
        position = new Vector3(barrierPositionsX[r], position.y, position.z);
        barrier.transform.position = position;
        barrier.transform.rotation = new Quaternion(rotation.x, barrierRotationY[r], rotation.z, rotation.w);
    }
}
