using System;
using System.Collections;
using System.Collections.Generic;
using Randomize;
using Unity.Mathematics;
using UnityEngine;
using static Randomize.GetRandom;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class GenerateMoss : MonoBehaviour
{
    [SerializeField] private GameObject stalactitesPrefabs;
    private List<GameObject> stalactites = new List<GameObject>();
    private float timeRemaining;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject newStalactites = Instantiate(stalactitesPrefabs, transform, true);
            stalactites.Add(newStalactites);
        }
        //GenerateStalactites();
        
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 3;
            Debug.LogError("SPAWN");
            Vector3 start = transform.position;
            Vector3 directionRay = new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(0.68f, 1), 0);;
            Debug.DrawRay(start, directionRay, Color.red, 2);
            if(Physics.Raycast(new Ray(start, directionRay), out RaycastHit hit)) Debug.Log("OK");
        }
    }

    public void GenerateStalactites()
    {
        foreach (var t in stalactites)
        {
            for (int j = 0; j < t.transform.childCount; j++)
            {
                t.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
        
        int countStalactites = Random.Range(0, stalactites.Count);
        Debug.Log(countStalactites);

        for (int i = 0; i < countStalactites; i++)
        {
            stalactites[i].transform.position = SelectionPoint();
            if (stalactites[i].transform.position == Vector3.zero)
            {
                Debug.Log("Vector3.zero");
                continue;
            }
            stalactites[i].transform.GetChild(Random.Range(0, stalactites[i].transform.childCount)).gameObject.SetActive(true);
        }
    }

    private Vector3 SelectionPoint()
    {
        Vector3 start = transform.position;
        //Debug.Log(transform.position);
        //Debug.Log(transform.localPosition);
        start = new Vector3(start.x, start.y, start.z + Random.Range(-4.8f, 4.8f));
        Vector3 directionRay = new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(0.68f, 1), 0);
        Debug.DrawRay(start, directionRay, Color.red, 2);
        return Physics.Raycast(new Ray(start, directionRay), out RaycastHit hit) ? hit.point : Vector3.zero;
    }
}
