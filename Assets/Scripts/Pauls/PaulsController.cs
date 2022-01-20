using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulsController : MonoBehaviour
{
    public Paul[] PaulPrefabs;
    public Paul[] PaulTwoPrefabs;
    public Paul StartPaul;
    public TubeController tubeController;
    [HideInInspector] public List<Paul> spawnPauls = new List<Paul>();
    
    [Range(3, 15)]
    [SerializeField] private int[] countPaulsBetween;
    [SerializeField] private int[] countPaulsBetweenStay;
    [SerializeField] private int paulsStay;
    [SerializeField] private bool isGeneratePaulTwo;
    
    [Range(0, 100)]
    public float[] oddsPauls;
    private int countPauls;
    
    private int countPaulsTwo;

    private void Start()
    {
        //countPaulsTwo = Random.Range(5, 11);
        countPaulsTwo = 10;
        paulsStay = 3;
        countPaulsBetweenStay = new int[countPaulsBetween.Length];
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
        Paul newPaul = new Paul();
        if(!isGeneratePaulTwo) newPaul = Instantiate(PaulPrefabs[PaulsOneController()]);
        else if(isGeneratePaulTwo) newPaul = Instantiate(PaulTwoPrefabs[1]);
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

    private int PaulsOneController()
    {
        int paulNumber;
        int thisPailNumber;
        paulNumber = Mathf.RoundToInt(Choose(oddsPauls));
        if (paulNumber == 0) return paulNumber;


        if (paulsStay != 0)
        {
            paulsStay--;
            paulNumber = 0;
        }
        else
        {
            paulsStay = 3;
            if (countPaulsBetweenStay[paulNumber] != 0)
            {
                thisPailNumber = paulNumber;
                countPaulsBetweenStay[paulNumber]--;
                while (true)
                {
                    paulNumber = Mathf.RoundToInt(Choose(oddsPauls)); 
                    if(thisPailNumber != paulNumber) break;
                }
            }
            else if (countPaulsBetweenStay[paulNumber] == 0)
            {
                countPaulsBetweenStay[paulNumber] = countPaulsBetween[paulNumber];
            }
        }
        return paulNumber;
    }

    private int PaulsTwoController()
    {
        int paulNumber;
        if (countPaulsTwo == 1 || countPaulsTwo == 10)
        {
            paulNumber = 0;
        }
        else paulNumber = 1;
        
        return paulNumber;
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
