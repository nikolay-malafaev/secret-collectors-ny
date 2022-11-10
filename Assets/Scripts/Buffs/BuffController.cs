using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public static Buff.Options CurrenBuff;
    public static Buff.Options LastBuff;
    
    public static float Time;
    public static bool IsWorkBuff;
    public const float DistanceDestroyBarriers = 23;
    
    [SerializeField] private GameObject mainSpawn;
    [SerializeField] private Buff[] buffsPrefabs;
    [SerializeField] private int distanceDestroyBuff = 3;
    [SerializeField] private ParticleSystem blastParticleSystem;
    private Buff buffInScene;
    private GameManager gameManager;

    [Range(0, 100)]
    [SerializeField] private float[] oddsBuffs;
    private List<Buff> buffs = new List<Buff>();
    private float timeRemaining;
    private float period;
    private float timeWorkBuff;
    
    public static Action<bool> SendBuff;
    public static Action SendBlastBuff;
    
    private void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.SendResetGame += ResetGame;
        SendBuff += Buff;
        SendBlastBuff += Blast;
        foreach (var buff in buffsPrefabs)
        {
            Buff newBuff = Instantiate(buff, transform, true);
            newBuff.gameObject.SetActive(false);
            buffs.Add(newBuff);
        }
        Spawn();
    }

    private void Update()
    {
        if (IsWorkBuff)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= UnityEngine.Time.deltaTime;
            }
            else
            {
                timeRemaining = period; 
                Time -= 0.01f;
                if (Time <= 0) SendBuff.Invoke(false);
            }
        }
        
        if (buffInScene == null) return;
        switch (gameManager.Direction)
        {
            case 0:
                if (buffInScene.transform.position.z < -distanceDestroyBuff) Spawn();
                break;
            case 1:
                if (buffInScene.transform.position.x < -distanceDestroyBuff) Spawn();
                break;
            case 2:
                if (buffInScene.transform.position.z > distanceDestroyBuff) Spawn();
                break;
            case 3:
                if (buffInScene.transform.position.x > distanceDestroyBuff) Spawn();
                break;
        }
    }
    private void Spawn()
    {
        if (buffInScene != null) buffInScene.gameObject.SetActive(false);
        
        int randomPointBuff = UnityEngine.Random.Range(0, 5);
        int i = Mathf.RoundToInt(Choose(oddsBuffs));
        buffs[i].gameObject.SetActive(true);
        buffs[i].transform.position = mainSpawn.transform.GetChild(randomPointBuff).position;
        buffs[i].transform.rotation = mainSpawn.transform.rotation;
        buffInScene = buffs[i];
    }
    private void Buff(bool value)
    {
        IsWorkBuff = value;
        if (!value)
        {
            LastBuff = CurrenBuff;
            CurrenBuff = global::Buff.Options.Null;
            return;
        }
        timeWorkBuff = buffs[(int)CurrenBuff - 1].TimeWork;
        period = timeWorkBuff / 100;
        Time = 1;
    }

    private void Blast()
    {
        blastParticleSystem.Play();
    }
    private void ResetGame()
    {
        Spawn();
    }
    private float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = UnityEngine.Random.value * total;

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

    private void OnDestroy()
    {
        GameManager.SendResetGame -= ResetGame;
    }
}
