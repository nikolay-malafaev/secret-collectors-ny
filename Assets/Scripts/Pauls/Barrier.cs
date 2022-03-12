using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Barrier : MonoBehaviour
{
    public int sameTypeDistation; // минимальное расстояние до barrer того же типа
    public int anyTypeDistation; // минимальное расстояние до barrer любого типа
    // typeBarriers
    public Vector3 offsetBarrier;
    public bool oneCountBarriers = false;
    public PossiblePosition possible;

    public enum PossiblePosition
    {
        Neutral = 0,
        Center = 1, 
        Right = 2,
        Left = 3,
    }
}
