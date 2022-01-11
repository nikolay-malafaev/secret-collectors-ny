using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //col.gameObject.
            Player player = col.gameObject.GetComponent<Player>();
            //player.tubeController.TurnTunnels();
        }
    }
}
