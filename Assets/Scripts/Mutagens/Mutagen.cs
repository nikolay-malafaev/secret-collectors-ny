using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutagen : MonoBehaviour
{
    private GameManager gameManager;
    private MutagenController mutagenController;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        mutagenController = FindObjectOfType<MutagenController>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Barrier") || col.gameObject.CompareTag("Buff"))
        {
            
            gameObject.SetActive(false);
            //Debug.LogError("Mutagen is hide");
        }

        if (col.gameObject.CompareTag("Player"))
        {
            if (gameManager.buffs[1])
            {
                gameManager.countMutagen+=2;
                gameManager.AddMutagen(2);
            }
            else
            {
                gameManager.AddMutagen(1);
                gameManager.countMutagen++;
            }
            mutagenController.DestroyMutagen(transform);
        }
    }
}
