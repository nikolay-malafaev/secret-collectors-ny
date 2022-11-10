using System.Collections.Generic;
using Randomize;
using UnityEngine;

public class GenerateStalagmites : MonoBehaviour
{
    [SerializeField] private GameObject[] stalagmitesPrefabs;
    [SerializeField] private GameObject pointsCircle;
    private List<GameObject> stalagmites = new List<GameObject>();
    private int maxCountStalagmites;

    void Start()
    {
        switch (GameManager.CurrenQualitySettings)
        {
            case 0:
                maxCountStalagmites = 0;
                return;
            case 1:
                maxCountStalagmites = 2;
                break;
            case 2:
                maxCountStalagmites = 3;
                break;
        }
        
        foreach (var stalagmite in stalagmitesPrefabs)
        {
            for (int i = 0; i < maxCountStalagmites - 1; i++)
            {
                GameObject newStalagmites = Instantiate(stalagmite, transform, true);
                newStalagmites.SetActive(false);
                stalagmites.Add(newStalagmites);
            }
        }

        Generate();
    }
    
    public void Generate()
    {
        if(GameManager.CurrenQualitySettings == 0) return;
        foreach (var oneStalagmites in stalagmites)
        {
            oneStalagmites.SetActive(false);
        }

        int countStalagmites = Random.Range(0, maxCountStalagmites);
        for (int i = 0; i < countStalagmites; i++)
        {
            int currenMoss = Random.Range(0, stalagmites.Count);
            stalagmites[currenMoss].transform.localPosition = SelectionPoint(i);
            stalagmites[currenMoss].SetActive(true);
        }
    }

    private Vector3 SelectionPoint(int i)
    {
        int randomPoint = Random.Range(0, pointsCircle.transform.childCount);
        float randomPosition = Random.Range(-4.5f, 4.5f);
        float addY = 0;
        Transform positionMoss = pointsCircle.transform.GetChild(randomPoint);;
        if(GetRandom.GetBool()) addY = Random.Range(0, 0.3f);
        addY += 0.1f;
        return new Vector3(positionMoss.localPosition.x, positionMoss.position.y + addY, randomPosition);
    }
}
