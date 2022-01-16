using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Tube : MonoBehaviour
{
    public Transform Begin;
    public Transform End;
    public bool isGenerateBarrier;
    public GameObject barrier;
    public float[] barrierPositionsX;
    public float[] barrierRotationY;
    
    public void GenerateBarrier()
    {
        if(!isGenerateBarrier || (barrierPositionsX.Length != barrierRotationY.Length))
            return;

        int r =  Random.Range(0, barrierPositionsX.Length);
        var position = barrier.transform.position;
        var rotation = barrier.transform.rotation;
        position = new Vector3(barrierPositionsX[r], position.y, position.z);
        barrier.transform.position = position;
        barrier.transform.rotation = new Quaternion(rotation.x, barrierRotationY[r], rotation.z, rotation.w);
    }
}
