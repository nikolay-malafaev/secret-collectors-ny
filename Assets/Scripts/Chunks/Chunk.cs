using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private bool isDoubleChunks;
    
    public bool IsDoubleChunks
    {
        get { return isDoubleChunks; }
    }

    [HideInInspector] public DoubleChunks doubleChunks;
    [HideInInspector] public GenerateStalagmites generateStalagmites;

    private void Awake()
    {
        if (isDoubleChunks) doubleChunks = GetComponent<DoubleChunks>();
        generateStalagmites = GetComponent<GenerateStalagmites>();
    }
}

