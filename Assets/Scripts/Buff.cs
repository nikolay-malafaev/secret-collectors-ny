using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buff : MonoBehaviour
{
    public Buff buff;
    public Options options;
    public Player player;
    public enum Options
    {
        Burable = 0,
        Blast = 1,
        DoubleMutagen = 2
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (options == Options.Burable)
            {
                player = col.gameObject.GetComponent<Player>();
               // player.gameManager.Pause(10);
                player.burable = true;
                player.timer = true;
                Destroy(gameObject);

            } else if (options == Options.Blast)
            {
                player = col.gameObject.GetComponent<Player>();
                player.blastScreen = true;
                for (int i = 0; i < player.tubeController.transform.childCount; i++)
                {
                    if (player.tubeController.transform.GetChild(i).childCount > 2)
                    {
                        if (player.tubeController.transform.GetChild(i).GetChild(0).childCount > 0)
                        {
                            if(player.tubeController.transform.GetChild(i).GetChild(0).GetChild(0).CompareTag("Barrier"))
                                   Destroy(player.tubeController.transform.GetChild(i).GetChild(0).GetChild(0).gameObject); 
                        }
                    }
                }
                Destroy(gameObject);
            } else if (options == Options.DoubleMutagen)
            {
                player = col.gameObject.GetComponent<Player>();
                player.doubleMutagen = true;
                player.timer = true;
                Destroy(gameObject);
            }
        }
    }
}
