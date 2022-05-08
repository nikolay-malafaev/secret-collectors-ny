using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Barrier"))
        {
            col.GetComponentInParent<Barrier>().gameObject.SetActive(false);
            //col.gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag("Buff"))
        {
            col.GetComponent<Buff>().gameObject.SetActive(false);
        }
    }
}
