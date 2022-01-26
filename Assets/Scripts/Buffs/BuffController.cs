using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffController : MonoBehaviour
{
    [SerializeField] private GameObject mainSpawn;
    public GameObject destroyerBuffs;
    public Buff[] baffs;
    
    [Range(0, 100)]
    public float[] oddsBaffs;

    private void Start()
    {
        StartCoroutine(BuffsSpawn());
    }

    public void Spawn()
    { 
        int randomPointBuff = Random.Range(0, 5);
        Buff buff = Instantiate(baffs[Mathf.RoundToInt(Choose(oddsBaffs))], transform, true);
        buff.transform.position = mainSpawn.transform.GetChild(randomPointBuff).position;
        StartCoroutine(BuffsSpawn());
    }

    IEnumerator BuffsSpawn()
    {
        yield return new WaitForSeconds(13f);
        Spawn();
    }

    public void Blast()
    {
        destroyerBuffs.SetActive(true);
        StartCoroutine(BlastTime());
    }
    IEnumerator BlastTime()
    {
        yield return new WaitForSeconds(0.3f);
        destroyerBuffs.SetActive(false);
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
