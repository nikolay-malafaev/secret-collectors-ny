using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Barrier : MonoBehaviour
{
    public int anyTypeDistance; // минимальное расстояние до Barrier любого типа
    public int sameTypeDistance; // минимальное расстояние до Barrier того же типа
    public Vector3 offsetBarrier;
    public bool oneCountBarrier;
    [HideInInspector] public bool isJob;

    public PossiblePosition possible;
    public enum PossiblePosition
    {
        Neutral = 0,
        Center = 1, 
        Right = 2,
        Left = 3,
    }
}
