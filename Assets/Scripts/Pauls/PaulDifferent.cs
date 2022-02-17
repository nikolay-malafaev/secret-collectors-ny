using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PaulDifferent : MonoBehaviour
{
    public Property property;
    public enum Property
    {
        None = 0,
        Pit = 1,
        Tube = 2
    }
}
