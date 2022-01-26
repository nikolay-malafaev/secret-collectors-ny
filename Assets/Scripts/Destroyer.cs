using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Barrier")) 
          Destroy(col.gameObject);
    }
     
}
