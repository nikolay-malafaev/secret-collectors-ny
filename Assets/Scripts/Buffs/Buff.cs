using UnityEngine;

public class Buff : MonoBehaviour
{
    public Options options;
    public enum Options
    {
        Null = 0,
        Durable = 1,
        DoubleMutagen = 2,
        NoGravity = 3
    }
    [Header("Time work buff in seconds")]
    [SerializeField] private float timeWork;
    public float TimeWork
    {
        get { return timeWork; }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            BuffController.CurrenBuff = options;
            BuffController.SendBuff.Invoke(true);
            gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag("Mutagen") || col.gameObject.CompareTag("Barrier"))
        {
            gameObject.SetActive(false);
        }
    }
}
