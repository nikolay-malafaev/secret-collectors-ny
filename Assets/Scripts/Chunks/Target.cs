using UnityEngine;

public class Target : MonoBehaviour
{
    public Direction direction;
    [SerializeField] private DoubleChunks doubleChunks;
    
    public enum Direction
    {
        Right = 0,
        Left = 1
    }

    private void Awake()
    {
        doubleChunks.direction = direction;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player")) doubleChunks.Turn(direction);
    }
}
