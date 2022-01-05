using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int healthPlayer;
    public bool colAddHealt;
    public TubeController tubeController;
    public bool burable;
    public int colMutagen;
    public bool blastScreen;
    public bool doubleMutagen;
    public bool timer;
    public GameManager gameManager;
    public Animator animator;

    void Start()
    {
        healthPlayer = 3;
    }

    void FixedUpdate()
    {
        healthPlayer = Mathf.Clamp(healthPlayer, -1, 1);

        if (burable || doubleMutagen)
            StartCoroutine(TimeBuff());
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Barrier"))
        {
            if (!burable)
                healthPlayer--;
            else if(burable)
            {
                blastScreen = true;
                burable = false;
                timer = false;
                StopCoroutine(TimeBuff());
                for (int i = 0; i < tubeController.transform.childCount; i++)
                {
                    if (tubeController.transform.GetChild(i).childCount > 2)
                    {
                        if (tubeController.transform.GetChild(i).GetChild(0).childCount > 0)
                        {
                            if (tubeController.transform.GetChild(i).GetChild(0).GetChild(0).CompareTag("Barrier"))
                                Destroy(tubeController.transform.GetChild(i).GetChild(0).GetChild(0).gameObject);
                        }
                    }
                }
            }
        }

        if(col.gameObject.CompareTag("Mutagen"))
        {
            if(doubleMutagen) colMutagen+=2;
               else colMutagen++;
            Destroy(col.gameObject);
        }
    }

    public void Move(string directMove)
    {
        switch (directMove)
        {
            case "Up":
                animator.SetTrigger("Up");
                break;
            case "Down":

                break;
        }
    }

    IEnumerator TimeBuff()
    {
        yield return new WaitForSeconds(10f);
        doubleMutagen = false;
        burable = false;
        timer = false;
    }
}
