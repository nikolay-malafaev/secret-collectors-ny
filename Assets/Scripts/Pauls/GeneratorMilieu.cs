using System.Collections.Generic;
using UnityEngine;

public class GeneratorMilieu : MonoBehaviour
{
    [SerializeField] private Transform[] milieusPrefabs;
    [SerializeField] private int maxCountMilieus;
    private List<Transform> milieus = new List<Transform>();
    private int maxCountMilieu;

    private void Awake()
    {
        switch (GameManager.CurrenQualitySettings)
        {
            case 0:
                maxCountMilieu = 2;
                break;
            case 1:
                maxCountMilieu = 3;
                break;
            case 2:
                maxCountMilieu = 4;
                break;
        }
        foreach (var prefabs in milieusPrefabs)
        {
            for (int j = 0; j < prefabs.childCount; j++)
            {
                Transform newMilieus = Instantiate(prefabs.GetChild(j), transform);
                newMilieus.gameObject.SetActive(false);
                milieus.Add(newMilieus);
            }
        }
    }
    public void GenerateMilieus()
    {
        int countMilieus = Random.Range(0, maxCountMilieu);
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
            float positionY = newMilieus.CompareTag("Stone") ? 0.125f : 0.232f;
            newMilieus.localPosition =  new Vector3(Random.Range(-1.4f, 1.4f), positionY, Random.Range(-0.4f, 0.4f));  //random position
            newMilieus.gameObject.SetActive(true);
        }
    }
    public void DestroyMilieu()
    {
        foreach (var t in milieus)
        {
            if(t.gameObject.activeSelf) t.gameObject.SetActive(false);
        }
    }
    private Vector3 SelectScale(Vector3 scale)
    {
        float percent = Random.Range(-4.9f, 4.9f);
        scale = scale + ((scale / 100) * percent);
        return scale;
    }
}
