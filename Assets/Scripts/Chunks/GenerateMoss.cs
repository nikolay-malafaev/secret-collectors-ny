using System;
using System.Collections;
using System.Collections.Generic;
using Randomaze;
using Unity.Mathematics;
using UnityEngine;
using static Randomaze.GetRandom;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

public class GenerateMoss : MonoBehaviour
{
    private Vector3 directionRay;
    private Vector3 start;
    private int numberMoss;

    private int countMoss;
    public GameObject[] mossPrefabs;
    public Transform milieu;
    

    void Start()
    { 
        //CelectionPoint();
    }

    public void CelectionPoint()
    {
        start = transform.localPosition;
        ChangePoint();
        countMoss = Random.Range(8, 15);
        
        for (int i = 0; i < countMoss; )
        {
            if (Physics.Raycast(new Ray(start, directionRay), out RaycastHit hit))
            {
                float sing;
                GameObject moss;
                Vector3 mossScale = Vector3.zero;
                if (directionRay.x > 0) sing = 1; else sing = -1;
                if (directionRay.y > 0) // изогнутая
                {
                    if (directionRay.y > 0.68 && GetChooise(50))
                    {
                        int j = 0;
                        moss = Instantiate(mossPrefabs[2].transform.GetChild(3).gameObject, transform);
                        moss.transform.position = hit.point;
                        for (int k = 0; k < 3; k++)
                        {
                            int number = Random.Range(0, 3);
                            moss = Instantiate(mossPrefabs[2].transform.GetChild(number).gameObject, transform);
                            moss.transform.position = hit.point;
                            float offset = Random.Range(-0.05f, 0.05f);
                            moss.transform.position += new Vector3(offset, 0,offset);
                        }
                    }
                    else
                    {
                        moss = Instantiate(mossPrefabs[1], transform);
                        float angle = sing * Vector3.Angle(new Vector3(sing, 0, 0),
                            new Vector3(directionRay.x, directionRay.y, 0));
                        moss.transform.rotation = Quaternion.Euler(angle, -90, -90);
                        if (sing < 0)
                            moss.transform.rotation = Quaternion.Euler(moss.transform.rotation.eulerAngles.x + 180,
                                moss.transform.rotation.eulerAngles.y, moss.transform.rotation.eulerAngles.z);

                        moss.transform.position = hit.point;

                        float offset = 0.08f;
                        moss.transform.position -= new Vector3(sing * offset, offset, 0);
                        mossScale = new Vector3(Random.Range(33.1f, 37.1f), Random.Range(33.1f, 37.1f), 35); // z = 35
                        moss.transform.localScale = mossScale;
                    }
                }
                else
                {
                    moss = Instantiate(mossPrefabs[0], transform);
                    if(sing > 0) moss.transform.rotation = Quaternion.Euler(moss.transform.rotation.eulerAngles.x, moss.transform.rotation.eulerAngles.y + 180, moss.transform.rotation.eulerAngles.z);
                    //mossScale = new Vector3(0.001f, Random.Range(0.4f, 1.5f), Random.Range(0.5f, 0.9f)); //x = 0.001f;
                    moss.transform.position = hit.point;
                    
                    float offset = 0.04f;
                    moss.transform.position -= new Vector3(sing *  offset, 0, 0);
                    mossScale = new Vector3(Random.Range(33.1f, 37.1f), Random.Range(33.1f, 37.1f), 35); // z = 35
                    moss.transform.localScale = mossScale;
                }
                ChangePoint();
            }
            else Debug.Log("no collision");
            i++;
        }
    }

    private void ChangePoint()
    {
        start = transform.position;
        float point = start.z + Random.Range(-4.8f, 4.8f);
        start = new Vector3(start.x, start.y, point);
        
        directionRay.y = Random.Range(-0.5f, 1);
        if (directionRay.y > 0)
        {
            directionRay.x = Random.Range(-1.1f, 1.1f);
        }
        else
        {
            float one = Random.Range(-1, 1);
            if (one < 0)
            {
                directionRay.x = -1;
            }
            else
            {
                directionRay.x = 1;
            }
        }

        numberMoss = Random.Range(1, 14);
    }
}
