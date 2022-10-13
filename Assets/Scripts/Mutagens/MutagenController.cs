using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static Randomize.GetRandom;

public class MutagenController : MonoBehaviour
{
    [SerializeField] private Mutagen mutagenPrefabs;
    [SerializeField] private GameObject mainSpawn;
    [SerializeField] private GameObject mutagenPool;
    private Transform lastMutagen;
    private float timeRemaining;


    private void Start()
    {
        for (int i = 0; i < 65; i++)
        {
            Mutagen newMutagen = Instantiate(mutagenPrefabs, mutagenPool.transform, true);
            newMutagen.gameObject.SetActive(false);
        }

        lastMutagen = mainSpawn.transform.GetChild(Random.Range(0, 3));
    }

    private void Update()
    {
        if (transform.childCount > 45)
        {
            DestroyMutagen(transform.GetChild(0));
        }

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = Random.Range(3, 7);
            Spawn();
        }
    }

    private void Spawn()
    {
        bool isDoubleMutagen = GetBool(); // добавить заивисимость от расстояния. Чем дальше, тем чаще. Но есть предел.
        int randomPoint = Random.Range(0, 3);
        int randomPointDouble = 0;
        if (isDoubleMutagen) randomPointDouble = GetRandomException(0, 3, randomPoint);

        int countMutagen = Random.Range(5, 15);
        int beginPosition = Random.Range(0, 5);
        int endPosition = Random.Range(6, 10);
        for (int i = 0; i < countMutagen; i++)
        {
            GenerateMutagen(randomPoint, true);
            if (isDoubleMutagen && (i >= beginPosition && i <= endPosition)) GenerateMutagen(randomPointDouble, false);
        }

        lastMutagen = mainSpawn.transform.GetChild(Random.Range(0, 3));
    }

    private void GenerateMutagen(int randomPoint, bool useLastMutagen)
    {
        Transform newMutagen = null;
        if (mutagenPool.transform.childCount > 0) newMutagen = mutagenPool.transform.GetChild(0);
        else
        {
            Mutagen mutagen = Instantiate(mutagenPrefabs, mutagenPool.transform, true);
            newMutagen = mutagen.transform;
            Debug.LogError("Child dot't find!");
        }

        newMutagen.gameObject.SetActive(true);
        newMutagen.SetParent(transform);
        newMutagen.transform.position = new Vector3(mainSpawn.transform.GetChild(randomPoint).position.x, -0.6f,
            lastMutagen.transform.position.z + 1);
        if (useLastMutagen) lastMutagen = newMutagen;
    }

    public void DestroyMutagen(Transform newMutagen)
    {
        newMutagen.gameObject.SetActive(false);
        newMutagen.transform.SetParent(mutagenPool.transform);
    }
}
