using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTunnels : MonoBehaviour
{
    Vector3 verticalTargetRotation;
    Vector3 verticalTargetPosition;
    public GameObject TubeNumberTwo;
    private TubeController tubeController;
    public Tube NewTube;
    public Paul NewPaul;
    public GlobalController globalController;
    
    void Start()
    {
       
    }
    
    void Update()
    {
        Vector3 direction = Vector3.RotateTowards( transform.forward, verticalTargetRotation, 1.7f * Time.deltaTime, 0);
        //transform.position = Vector3.MoveTowards( transform.position, verticalTargetPosition, 1 * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public void TurnTunnels() 
    {
        tubeController = GetComponentInParent<TubeController>();
        globalController = GetComponentInParent<GlobalController>();
        tubeController.IsSpawnTunnels = false;
        for (int i = 0; i < tubeController.spawnTubes.Count; i++)
        {
            tubeController.spawnTubes[i].transform.SetParent(transform);
        }
        tubeController.mutagens.transform.SetParent(transform);
        tubeController.paulsController.transform.SetParent(transform);
        TubeNumberTwo.SetActive(false);
        verticalTargetRotation = new Vector3(-0.5f, 0, 0);
        verticalTargetPosition = new Vector3(0.375f, 0, 0);
        StartCoroutine(FreeSpawnTube());
    }

    IEnumerator FreeSpawnTube() 
    {
        yield return new WaitForSeconds(0.71f);
        transform.position = new Vector3(0.41f, 0, transform.position.z);
        yield return new WaitForSeconds(1.15f);
        tubeController.spawnTubes[tubeController.spawnTubes.Count - 1] = NewTube;
        tubeController.paulsController.spawnPauls[tubeController.paulsController.spawnPauls.Count - 1] = NewPaul;
        for (int i = 0; i < tubeController.spawnTubes.Count; i++)
        {
            tubeController.spawnTubes[i].transform.SetParent(tubeController.transform);
            
        }
        tubeController.mutagens.transform.SetParent(tubeController.transform);
        tubeController.paulsController.transform.SetParent(tubeController.transform);
        tubeController.IsSpawnTunnels = true;
    }
}
