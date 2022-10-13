using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorMilieu : MonoBehaviour
{
    [SerializeField] private Transform[] milieusPrefabs;
    [SerializeField] private int maxCountMilieus;
    public List<Transform> milieus;


    private void Start()
    {
        for (int i = 0; i < milieusPrefabs.Length; i++)
        {
            for (int j = 0; j < milieusPrefabs[i].transform.childCount; j++)
            {
                Transform newMilieus = Instantiate(milieusPrefabs[i].transform.GetChild(j), transform);
                newMilieus.gameObject.SetActive(false);
                milieus.Add(newMilieus);
            }
        }
        GenerateMilieus();
    }

    public void GenerateMilieus()
    {
        foreach (var t in milieus)
        {
            if(t.gameObject.activeSelf) t.gameObject.SetActive(false);
        }
        
        int countMilieus = Random.Range(0, maxCountMilieus + 1);
        //Debug.Log(countMilieus);
        bool[] selected = new bool[milieus.Count];
        for (int i = 0; i < countMilieus; i++)
        {
            int random = 0;
            foreach (var t in milieus)
            {
                random = Random.Range(0, milieus.Count);
                if(!selected[random]) break;
            }
            Transform newMilieus = milieus[random];
            selected[random] = true;
            Vector3 changeScale = SelectScale(newMilieus.transform.localScale);
            newMilieus.localScale = new Vector3(changeScale.x, changeScale.y, changeScale.z);    //random scale
            newMilieus.localPosition =  new Vector3(Random.Range(-1.4f, 1.4f), 0.17f, Random.Range(-0.4f, 0.4f));  //random position
            newMilieus.gameObject.SetActive(true);
        }
    }
    
    private Vector3 SelectScale(Vector3 scale)
    {
        float percent = Random.Range(-4.9f, 4.9f);
        scale = scale + ((scale / 100) * percent);
        return scale;
    }
}
