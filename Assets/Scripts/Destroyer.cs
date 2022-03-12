using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Barrier") || col.gameObject.CompareTag("Mutagen") || col.gameObject.CompareTag("Buff")) 
          Destroy(col.gameObject);
    }
     
}
