using System;
using System.Collections;
using System.Collections.Generic;
using Randomize;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static Randomize.GetRandom;
using Vector2 = System.Numerics.Vector2;

public class ChunkController : MonoBehaviour
{
     [Range(0, 100)] public float oddsDoubleChunks;

    public float numberAddSpeed;
    public float periodAddSpeed = 10;
    public GameObject pool;
    public AnimationCurve Chance;
    [HideInInspector] public bool isDoubleChunksInScene;
    [HideInInspector] public Chunk lastChunk;
    [HideInInspector] public Player player;
    [HideInInspector] public bool isSpawnPermit;
    [HideInInspector] public PaulsController paulsController;
    [HideInInspector] public List<Chunk> chunks = new List<Chunk>();
    [HideInInspector] public List<Chunk> doubleChunks = new List<Chunk>();
    [HideInInspector] public bool pausePosition;
    [HideInInspector] public GameManager gameManager;
    [HideInInspector] public Vector3 position;

    [SerializeField] private Chunk[] ChunksPrefabs;
    [SerializeField] private Chunk startChunk;

    private RoadController roadController;
    private float nextActionTimeAddSpeed;
    private float addSpeed;
    private int currenDoubleChunk;

    void Start()
    {
        roadController = FindObjectOfType<RoadController>();
        paulsController = GetComponentInChildren<PaulsController>();
        addSpeed = 0.104f;
        numberAddSpeed = 0.0005f;
        nextActionTimeAddSpeed = 0;
        isSpawnPermit = true;
        position.y = 0;
        chunks.Add(startChunk);
        lastChunk = startChunk;
        for (int i = 0; i < 9; i++)
        {
            Chunk newChunk = Instantiate(ChunksPrefabs[0], transform, true);
            chunks.Add(newChunk);
            newChunk.transform.position = lastChunk.transform.position + new Vector3(0, 0, 10.35f);
            lastChunk = newChunk;
        }

        for (int i = 1; i < 3; i++)
        {
            doubleChunks.Add(Instantiate(ChunksPrefabs[i], transform, true));
            doubleChunks[i - 1].gameObject.transform.SetParent(pool.transform);
        }
    }


    void FixedUpdate()
    {
        if (!pausePosition)
        {
            transform.position = new Vector3(position.x, position.y, position.z);
            position -= gameManager.ChooiseDirectionPosition(addSpeed);
            float dist = Vector3.Distance(player.transform.position, lastChunk.transform.position);
            if (dist < 50) // 70
            {
                GenerateChunk();
                if (GetChoice(Convert.ToInt32(oddsDoubleChunks)) && !isDoubleChunksInScene) GenerateDoubleChunk();
                if (chunks[0].CompareTag("DoubleTube")) DestroyDoubleChunk();
            }
        }

        if (Time.time > nextActionTimeAddSpeed) // (period)
        {
            addSpeed += numberAddSpeed;
            nextActionTimeAddSpeed += periodAddSpeed;
        }
    }

    private void GenerateChunk()
    {
        chunks[0].transform.rotation = Quaternion.Euler(0, gameManager.ChooiseDirectionRotarion(), 0);
        chunks[0].transform.position = lastChunk.transform.position + gameManager.ChooiseDirectionPosition(10.35f);
        lastChunk = chunks[0];
        //chunks[0].generateMoss.GenerateStalactites();
        chunks.Add(chunks[0]);
        chunks.RemoveAt(0);
    }


    private void GenerateDoubleChunk()
    {
        int i = 0;
        if (GetBool()) i = 1;
        currenDoubleChunk = i;

        doubleChunks[i].transform.gameObject.SetActive(true);
        doubleChunks[i].transform.SetParent(transform);
        doubleChunks[i].transform.rotation = Quaternion.Euler(0, doubleChunks[i].doubleChunks.defoltEuler + gameManager.ChooiseDirectionRotarion(), 0);
        doubleChunks[i].transform.position =
            lastChunk.transform.position + gameManager.ChooiseDirectionPosition(10.35f);
        if (Vector3.Distance(transform.position, new Vector3(0, 0, 0)) > 50000)
        {
            doubleChunks[i].doubleChunks.stop.gameObject.SetActive(true);
        }

        lastChunk = doubleChunks[i];
        chunks.Add(doubleChunks[i]);
        isDoubleChunksInScene = true;
        for (int j = 0; j < doubleChunks[i].doubleChunks.pauls.Count; j++)
        {
            paulsController.GenerateBarrier(doubleChunks[i].doubleChunks.pauls[j]);
        }
    }


    private void DestroyDoubleChunk()
    {
        if (Vector3.Distance(player.transform.position, chunks[0].transform.position) > 90)
        {
            chunks[0].doubleChunks.BackToPool(0);
        }
        else chunks[0].doubleChunks.BackToPool(1);

        chunks.RemoveAt(0);

        for (int i = 0; i < doubleChunks[currenDoubleChunk].doubleChunks.pauls.Count; i++)
        {
            paulsController.DestroyBarrier(doubleChunks[currenDoubleChunk].doubleChunks.pauls[i]);
        }
    }
}