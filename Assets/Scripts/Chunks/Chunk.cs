using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    public Transform Begin;
    public Transform End;
    public bool isDoubleChunks;
    [HideInInspector] public DoubleChunks doubleChunks;
    public GenerateMoss generateMoss;

    private void Awake()
    {
        if (isDoubleChunks) doubleChunks = GetComponent<DoubleChunks>();
        
    }
}

