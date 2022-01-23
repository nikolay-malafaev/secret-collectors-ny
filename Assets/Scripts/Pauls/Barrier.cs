using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Barrier : MonoBehaviour
{
    public int sameTypeDistation;
    public int anyTypeDustation;
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
