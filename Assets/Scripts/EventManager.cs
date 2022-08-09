using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<Target.Direction> SpawnDoubleChunks;
    public static Action<int, string> ChangeRoad;
    public static Action<int> EndWay;
}
