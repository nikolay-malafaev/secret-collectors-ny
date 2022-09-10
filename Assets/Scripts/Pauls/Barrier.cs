using System;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Barrier : MonoBehaviour
{
    public float chance;
    public int anyTypeDistance; // минимальное расстояние до Barrier любого типа
    public int sameTypeDistance; // минимальное расстояние до Barrier того же типа
    public Vector3 offsetBarrier;
    public bool oneCountBarrier;
    [HideInInspector] public bool isJob;
    
    public PossiblePosition possible;
    public enum PossiblePosition
    {
        Center = 0, 
        Right = 1,
        Left = 2,
        Neutral = 3,
    }

    //public bool haveСlones;
    //[ShowIf(ShowIfAttribute.ActionOnConditionFail.DontDraw, ShowIfAttribute.ConditionOperator.And, nameof(haveСlones))]
    //public Barrier clones;
    
    
    [EditorButton("SetChance")]
    public void SetChance()
    {
        PaulsController paulsController = FindObjectOfType<PaulsController>();
        for (int i = 0; i < paulsController.barriersPrefabs.Length; i++)
        {
            if (paulsController.barriersPrefabs[i] == this)
            {
                paulsController.oddsBarriers[i] = chance;
            }
        }
    }
}

