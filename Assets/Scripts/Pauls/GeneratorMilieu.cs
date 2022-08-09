using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratorMilieu : MonoBehaviour
{
    [SerializeField] private GameObject[] milieusPrefabs;
    [SerializeField] private int maxCountMilieus;
    private int number;
    
    //генерировать так же как и мох в самом начале
    
    public void Generate()
    {
        
        int countMilieus = Random.Range(0, maxCountMilieus);
        for (int i = 0; i < countMilieus; i++)
        {
            int type = SelectType();  // выбрать тип объекта - камень, трава,
            GameObject milieus = Instantiate(milieusPrefabs[type].gameObject.transform.GetChild(number).gameObject, transform);
            Vector3 changeScale = SelectScale(milieus.transform.localScale);
            
            milieus.transform.localScale = new Vector3(changeScale.x, changeScale.y, changeScale.z);    //random scale
            milieus.transform.localPosition =  new Vector3(Random.Range(-1.4f, 1.4f), 0.17f, Random.Range(-0.4f, 0.4f));  //random position

        }
    }

    
    private int SelectType()
    {
        int type = Random.Range(0, milieusPrefabs.Length);
        number = Random.Range(0, milieusPrefabs[type].transform.childCount);  // выбрать случайный объект нужного типа
        return type;
    }

    private Vector3 SelectScale(Vector3 scale)
    {
        float percent = Random.Range(-4.9f, 4.9f);
        scale = scale + ((scale / 100) * percent);
        return scale;
    }
}
