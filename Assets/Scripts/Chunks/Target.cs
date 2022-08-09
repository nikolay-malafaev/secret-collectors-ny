using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public Direction direction;
    private DoubleChunks doubleChunks;
     public enum Direction
    {
        Right = 0,
        Left = 1
    }

     private void Start()
     {
         doubleChunks = GetComponentInParent<DoubleChunks>();
     }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            
            //doubleChunks.animator.SetTrigger("death");
            doubleChunks.TurnChunks(direction);
            //player.tubeController.TurnTunnels();
           // StartCoroutine(TimeLife());
        }
    }

   /* IEnumerator TimeLife()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }*/
}
