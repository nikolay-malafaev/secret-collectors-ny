using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paul : MonoBehaviour
{
    [Space(20)]
    [Header("2 - Left")]
    [Header("1 - Right")]
    [Header("0 - Center")]
    
    public Transform[] numberBarriers;
    public List<Barrier> barriers;
    public List<int> numberBarriersInPool;
    [HideInInspector] public bool[] busyNumberBarriers = new bool[3];
    [HideInInspector] public int countBarriers;

    private void Start()
    {
        countBarriers = 0;
    }
}