using System;
using System.Collections.Generic;
using UnityEngine;
using static Randomize.GetRandom;

public class ChunkController : MonoBehaviour
{
    [Range(0, 100)] public float oddsDoubleChunks;
    public float numberAddSpeed;
    public Chunk lastChunk;
    [HideInInspector] public List<Chunk> doubleChunks = new List<Chunk>();
    private PoolManager poolManager;
    public bool isDoubleChunksInScene;
    private Player player;
    private bool isSpawnPermit;

    public bool IsSpawnPermit
    {
        get { return isSpawnPermit;} set { isSpawnPermit = value; } 
    }

    private PaulsController paulsController;
    
    [SerializeField] private AnimationCurve chance;
    [SerializeField] private Chunk[] chunksPrefabs;
    [SerializeField] private Chunk startChunk;

    public List<Chunk> chunks = new List<Chunk>();
    private GameManager gameManager;
    private const float periodAddSpeed = 10;
    private float nextActionTimeAddSpeed;
    private float addSpeed = 0.104f;
    private int currenDoubleChunk;
    private Vector3 position;
    private bool pausePosition;

    private void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.SendPauseGame += Pause;
        GameManager.SendTransitionToGame += TransitionToGame;
        GameManager.SendDefeatGame += DefeatGame;
        GameManager.SendResetGame += ResetGame;
        poolManager = FindObjectOfType<PoolManager>();
        paulsController = GetComponentInChildren<PaulsController>();
        player = FindObjectOfType<Player>();
        isSpawnPermit = true;
        chunks.Add(startChunk);
        lastChunk = startChunk;
        for (int i = 0; i < 9; i++)
        {
            Chunk newChunk = Instantiate(chunksPrefabs[0], transform, true);
            chunks.Add(newChunk);
            newChunk.transform.position = lastChunk.transform.position + new Vector3(0, 0, 10.35f);
            lastChunk = newChunk;
        }

        for (int i = 1; i < 3; i++)
        {
            doubleChunks.Add(Instantiate(chunksPrefabs[i], transform, true));
            doubleChunks[i - 1].gameObject.transform.SetParent(poolManager.transform);
        }

        numberAddSpeed /= 10000;
        pausePosition = !gameManager.Test;
    }


    private void FixedUpdate()
    {
        if (!pausePosition)
        {
            transform.position = new Vector3(position.x, position.y, position.z);
            position -= gameManager.ChooseDirectionPosition(addSpeed);
            float dist = Vector3.Distance(player.transform.position, lastChunk.transform.position);
            if (dist < 50) // 70
            {
                GenerateChunk();
                if (chunks[0].IsDoubleChunks) DestroyDoubleChunk(true);
                if (GetChoice(Convert.ToInt32(oddsDoubleChunks)) && !isDoubleChunksInScene)  GenerateDoubleChunk();
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
        chunks[0].transform.rotation = Quaternion.Euler(0, gameManager.ChooseDirectionRotation(), 0);
        chunks[0].transform.position = lastChunk.transform.position + gameManager.ChooseDirectionPosition(10.35f);
        lastChunk = chunks[0];
        chunks[0].generateStalagmites.Generate();
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
        doubleChunks[i].transform.rotation = Quaternion.Euler(0, doubleChunks[i].doubleChunks.defaultEuler + gameManager.ChooseDirectionRotation(), 0);
        doubleChunks[i].transform.position =
            lastChunk.transform.position + gameManager.ChooseDirectionPosition(10.35f);
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
    private void DestroyDoubleChunk(bool delay)
    {
        doubleChunks[currenDoubleChunk].doubleChunks.BackToPool(delay);
        chunks.RemoveAt(chunks.IndexOf(doubleChunks[currenDoubleChunk]));

        foreach (var pauls in doubleChunks[currenDoubleChunk].doubleChunks.pauls)
        {
            paulsController.DestroyBarrier(pauls);
        }
    }
    private void TransitionToGame()
    {
        pausePosition = false;
    }
    private void Pause()
    {
        pausePosition = !pausePosition;
    }
    private void DefeatGame()
    {
        pausePosition = true;
    }
    private void ResetGame()
    {
        // reset all moss
        pausePosition = true;
        position = Vector3.zero;
        transform.position = Vector3.zero;
        lastChunk.transform.position = new Vector3(0, 0, -25);
        for (int i = 0; i < chunks.Count; i++)
        {
            if (chunks[i].IsDoubleChunks)
            {
                DestroyDoubleChunk(false);
                continue;
            }

            chunks[i].transform.position = lastChunk.transform.position + new Vector3(0, 0, 10.35f);
            chunks[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            lastChunk = chunks[i];
        }
    }
    private void OnDestroy()
    {
        GameManager.SendPauseGame -= Pause;
        GameManager.SendTransitionToGame -= TransitionToGame;
        GameManager.SendDefeatGame -= DefeatGame;
        GameManager.SendResetGame -= ResetGame;
    }
}