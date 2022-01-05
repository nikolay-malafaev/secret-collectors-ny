using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutagen : MonoBehaviour
{
    public Transform Begin;
    public Transform End;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Barrier") | col.gameObject.CompareTag("Buff"))
        {
            gameObject.SetActive(false);
        }
    }
}
