using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ChunkController : MonoBehaviour
{
    [Range(0, 100)]
    public float[] oddsChunks;
    
    public float numberAddSpeed;
    public float periodAddSpeed = 10;
    public GameObject mutagens;
    [HideInInspector] public Player player;
    [HideInInspector] public bool isSpawnPermit;
    [HideInInspector] public PaulsController paulsController;
    [HideInInspector] public List<Chunk> spawnChunks = new List<Chunk>();
    [HideInInspector] public bool pausePosition;
    [HideInInspector] public GameManager gameManager;
    
    [HideInInspector] public Vector3 position;

    [SerializeField] private Chunk[] ChunksPrefabs;
    [SerializeField] private Chunk startChunk;
    [SerializeField] private GameObject mainTube;
    [SerializeField] private int BeforeDouble;
    
    private float nextActionTimeAddSpeed;
    private float addSpeed;
    private int countChunksBeforeDouble;

    void Start()
    {
        paulsController = GetComponentInChildren<PaulsController>();
        addSpeed = 0.104f;
        numberAddSpeed = 0.0005f;
        nextActionTimeAddSpeed = 0;
        spawnChunks.Add(startChunk);
       // StartCoroutine(DoudleTubeSpawn());
        isSpawnPermit = true;
        position.y = 0;
        countChunksBeforeDouble = 0;
    }

    void FixedUpdate()
    {
        if (!pausePosition)
        {
            mainTube.transform.position = new Vector3(position.x, position.y, position.z);
            switch (gameManager.direction)
            {
                case 0:
                    position.z -= addSpeed;
                    if (spawnChunks[spawnChunks.Count - 1].End.position.z < 40)
                    {
                        SpawnChunk();
                    }

                    break;
                case 1:
                    position.x -= addSpeed;
                    if (spawnChunks[spawnChunks.Count - 1].End.position.x < 50)
                    {
                        SpawnChunk();
                    }

                    break;
                case 2:
                    position.z += addSpeed;
                    if (spawnChunks[spawnChunks.Count - 1].End.position.z > -50)
                    {
                        SpawnChunk();
                    }

                    break;
                case 3:
                    position.x += addSpeed;
                    if (spawnChunks[spawnChunks.Count - 1].End.position.x > -50)
                    {
                        SpawnChunk();
                    }

                    break;
            }
        }

        if (spawnChunks.Count > 20)
        {
            DestoryChunk();
        }

        
        if (Time.time > nextActionTimeAddSpeed) // (period)
        {
            addSpeed += numberAddSpeed;
            nextActionTimeAddSpeed += periodAddSpeed;
        }
    }

    private void SpawnChunk()
    {
        if (!isSpawnPermit)
        {
            return;
        }
        
        
        int choose = Mathf.RoundToInt(Choose(oddsChunks));
        if (choose > 0 & countChunksBeforeDouble != 0)
        {
            countChunksBeforeDouble--;
            choose = 0;
        } else if (choose > 0 & countChunksBeforeDouble == 0)
        {
            countChunksBeforeDouble = BeforeDouble;
        }

        Chunk newChunk = Instantiate(ChunksPrefabs[choose], mainTube.transform, true);
        Vector3 lastPosition = spawnChunks[spawnChunks.Count - 1].transform.position;
        
        switch (gameManager.direction)
        {
            case 0:
                newChunk.transform.position = lastPosition + new Vector3(0, 0, 10);
                break;
            case 1:
                newChunk.transform.rotation = Quaternion.Euler(0, newChunk.transform.rotation.eulerAngles.y + 90, 0);
                newChunk.transform.position = lastPosition + new Vector3(10, 0, 0);
                // camera controller
                // удаление (360 расстояние)
               break;
            case 2:
                newChunk.transform.rotation = Quaternion.Euler(0, newChunk.transform.rotation.eulerAngles.y + 180, 0);
                newChunk.transform.position = lastPosition + new Vector3(0, 0, -10);
                break;
            case 3:
                newChunk.transform.rotation = Quaternion.Euler(0, newChunk.transform.rotation.eulerAngles.y - 90, 0);
                newChunk.transform.position = lastPosition + new Vector3(-10, 0, 0);
                break;
        }
        spawnChunks.Add(newChunk);
    }
    
    private void DestoryChunk()
    {
        if (spawnChunks[0] != null)
        {
            Destroy(spawnChunks[0].gameObject);
            spawnChunks.RemoveAt(0);
        } else spawnChunks.RemoveAt(0);
    }
    
    
    float Choose(float[] probs)
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


