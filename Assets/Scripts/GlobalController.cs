using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("DoubleTube"))
        {
            Tube tube = col.gameObject.GetComponentInParent<Tube>();
            tube.TurnTunnels();
            //Player player = col.gameObject.GetComponent<Player>();
            //player.tubeController.TurnTunnels();
        }
    }
}
