using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleChunks : Chunk
{
    public List<Paul> pauls;
    public GameObject stop;
    [HideInInspector] public Target.Direction direction;
    [HideInInspector] public float defaultEuler;
    [SerializeField] private Chunk attachingChunk;
    [SerializeField] private Paul lastPaul;
    [SerializeField] private Transform paulsTransform;
    private ChunkController chunkController;
    private PaulsController paulsController;
    private PoolManager poolManager;
    private GameManager gameManager;

    private void Awake()
    {
        defaultEuler = transform.rotation.eulerAngles.y;
    }

    void Start()
    {
        chunkController = FindObjectOfType<ChunkController>();
        paulsController = FindObjectOfType<PaulsController>();
        poolManager = FindObjectOfType<PoolManager>();
        gameManager = GameManager.Instance;

        for (int i = 0; i < 30; i++)
        {
            Paul newPaul = Instantiate(paulsController.paulPrefab, paulsTransform, true);
            pauls.Add(newPaul);
            switch (direction)
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
            newPaul.generatorMilieu.GenerateMilieus();
            newPaul.typePaul = "doublePaul";
            lastPaul = newPaul;
        }
        gameObject.SetActive(false);
    }

    
    public void Turn(Target.Direction directionTurn) 
    {
        chunkController.IsSpawnPermit = false;
        chunkController.lastChunk = attachingChunk;
        paulsController.lastPaul = lastPaul;
        int sing = directionTurn switch
        {
            Target.Direction.Right => 1,
            Target.Direction.Left => -1,
            _ => 0
        };
        
        gameManager.Direction += sing;
        GameManager.SendTurn.Invoke(directionTurn);
    }

    public void BackToPool(bool delay)
    {
        if (!delay)
        {
            transform.SetParent(poolManager.transform);
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
        BackToPool(false);
    }
}
