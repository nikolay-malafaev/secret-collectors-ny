using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulsController : MonoBehaviour
{
    public Paul[] PaulPrefabs; 
    public Paul StartPaul;
    [HideInInspector] public List<Paul> spawnPauls = new List<Paul>();
    public TubeController tubeController;
   
    [Range(0, 100)]
    public float[] oddsPaul;

    private void Start()
    {
        spawnPauls.Add(StartPaul);
    }

    void FixedUpdate()
    {
        if (spawnPauls[spawnPauls.Count - 1].End.position.z < 55 & tubeController.IsSpawnTunnels)
        {
            SpawnPauls();
        }
        
        if (spawnPauls.Count > 95)
        {
            destoryPauls();
        }
    }

    private void SpawnPauls()
    {
        Paul newPaul = Instantiate(PaulPrefabs[0]);
        newPaul.transform.position = spawnPauls[spawnPauls.Count - 1].Begin.position - newPaul.End.localPosition;
        spawnPauls.Add(newPaul);
        newPaul.transform.SetParent(transform);
        newPaul.transform.rotation = transform.rotation;
    }
    
    private void destoryPauls()
    {
        Destroy(spawnPauls[0].gameObject);
        spawnPauls.RemoveAt(0);
    }
}
