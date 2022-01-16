using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    public TubeController tubeController;
    void Start()
    {
        tubeController = GetComponent<TubeController>();
    }
    
    void Update()
    {
        
    }
    
    public void TurnTunnels()
    {
        for (int p = 0; p < tubeController.transform.childCount; p++)
        {
            if (tubeController.transform.GetChild(p).name == "Mutagens")
            {
                for (int i = 0; i < tubeController.transform.GetChild(p).childCount; i++)
                {
                    //if(tubeController.transform.GetChild(0).GetChild(i).CompareTag($"Mutagen"))
                    Destroy(tubeController.transform.GetChild(p).GetChild(i).gameObject);
                }
            }
        }
    }
    
    
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("DoubleTube"))
        {
          
        }
    }
}
