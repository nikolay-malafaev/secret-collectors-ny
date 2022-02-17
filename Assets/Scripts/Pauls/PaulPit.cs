using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulPit : MonoBehaviour
{
    public GameObject[] bags;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void ActiveBag(int numberBag)
    {
        bags[numberBag].SetActive(true);
    }
}
