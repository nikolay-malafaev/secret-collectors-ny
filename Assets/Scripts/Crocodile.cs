using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
    public ChunkController chunkController;
    public Player player;
    public Animator animator;

    public bool bite;
    public bool colBarrier;

    void Start()
    {
        
    }


    void FixedUpdate()
    {

        if(player.healthPlayer == 2 & colBarrier)
        {
            bite = true;
            StartCoroutine(Bite());
            colBarrier = false;
        }

        if(player.healthPlayer == 1 & !colBarrier)
        {
            bite = true;
            StartCoroutine(Bite());
            colBarrier = true;
        }

        if (player.healthPlayer <= 0)
        {
            animator.SetTrigger("dead");
        }

        if (bite)
        {
            animator.SetTrigger("bite");
        }
    }




    IEnumerator Bite ()
    {
       yield return new WaitForSeconds(1);
        bite = false;
    }
}
