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
        DoubleMutagen = 2,
        NoGravity = 3
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player = col.gameObject.GetComponent<Player>();
            if (options == Options.Burable)
            {
                player.gameManager.Buffs("burable", true);
            } else if (options == Options.Blast)
            {
                player.gameManager.Buffs("blast", false);
            } else if (options == Options.DoubleMutagen)
            {
                player.gameManager.Buffs("doubleMutagen", true);
            }
            else if (options == Options.NoGravity)
            {
                player.gameManager.Buffs("noGravity", true);
                player.isNoGravityBaff = true;
            }
            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("Mutagen"))
        {
            Destroy(gameObject);
        }
    }
}
