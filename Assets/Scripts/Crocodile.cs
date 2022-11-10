using System.Collections;
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

        if(player.Health == 2 & colBarrier)
        {
            bite = true;
            StartCoroutine(Bite());
            colBarrier = false;
        }

        if(player.Health == 1 & !colBarrier)
        {
            bite = true;
            StartCoroutine(Bite());
            colBarrier = true;
        }

        if (player.Health <= 0)
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
