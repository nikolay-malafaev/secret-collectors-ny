using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public Direction direction;
     public enum Direction
    {
        Right = 0,
        Left = 1
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            DoubleChunks doubleChunks = GetComponentInParent<DoubleChunks>();
            doubleChunks.animator.SetTrigger("death");
            doubleChunks.TurnChunks(direction);
            //player.tubeController.TurnTunnels();
            StartCoroutine(TimeLife());
        }
    }

    IEnumerator TimeLife()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
