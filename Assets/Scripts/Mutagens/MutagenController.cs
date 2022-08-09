using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MutagenController : MonoBehaviour
{
    [SerializeField] private Mutagen mutagenPrefabs;
    [SerializeField] private GameObject mainSpawn;
    private bool isMutagenTwo;
    private int randomPoint;
    private int randomPointTwo;
    private int randomCountMutagens;
    private string isMutagen;
    private string[] isMutagenArray = new string[] {"one", "two"};
    private int randomTimeAmidMutagens;
    private int randomBeforeMutagens;
    private int randomAfterMutagens;


    private void Start()
    {
       // StartCoroutine(MutagenSpawn());
       // isMutagen = "one";
       // randomTimeAmidMutagens = Random.Range(3, 7);
    }

    private void Update()
    {
        if (transform.childCount > 45)
        {
            DestroyMetagen();
        }
    }

    private void DestroyMetagen()
    {
        Destroy(transform.GetChild(0).gameObject); 
    }

    public void Spawn()
    {

        Mutagen newMutagen = Instantiate(mutagenPrefabs, transform, true);
        newMutagen.transform.position = mainSpawn.transform.GetChild(randomPoint).position;
        if (isMutagenTwo)
        {
            Mutagen newMutagenTwo = Instantiate(mutagenPrefabs, transform, true);
            newMutagenTwo.transform.position = mainSpawn.transform.GetChild(randomPointTwo).position;
            //newMutagenTwo.transform.SetParent(mainTube.transform);
        }
    }
    
     IEnumerator MutagenSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        Spawn();
        switch (isMutagen)
        {
            case "one":
                if (randomCountMutagens > 0)
                {
                    StartCoroutine(MutagenSpawn());
                    randomCountMutagens--;
                }
                else
                {
                    randomTimeAmidMutagens = Random.Range(2, 6);
                    randomCountMutagens = Random.Range(5, 10);
                    randomPoint = Random.Range(0, 3);
                    isMutagen = isMutagenArray[Random.Range(0, 2)];
                    if(isMutagen == "two")
                    {  
                        randomBeforeMutagens = Random.Range(1, 4);
                        randomAfterMutagens = Random.Range(1, 3);
                        randomPointTwo = Random.Range(0, 3);
                        while (randomPointTwo == randomPoint)
                        {
                            randomPointTwo = Random.Range(0, 3);
                        }
                        randomCountMutagens = Random.Range(4, 7);
                    }
                    StartCoroutine(NextMutagen());
                }
                break;
            case "two":
                if(randomBeforeMutagens > 0)
                {
                    StartCoroutine(MutagenSpawn());
                    randomBeforeMutagens--;
                }
                else
                {
                    if(randomCountMutagens > 0)
                    {
                        isMutagenTwo = true;
                        StartCoroutine(MutagenSpawn());
                        randomCountMutagens--;
                    }
                    else
                    {
                        isMutagenTwo = false;
                        if(Random.Range(0, 2) == 1) randomPoint = randomPointTwo;
                        if (randomAfterMutagens > 0)
                        {
                            StartCoroutine(MutagenSpawn());
                            randomAfterMutagens--;
                        }
                        else
                        {
                            randomTimeAmidMutagens = Random.Range(2, 6);
                            randomCountMutagens = Random.Range(5, 10);
                            randomPoint = Random.Range(0, 3);
                            isMutagen = isMutagenArray[Random.Range(0, 2)];
                            if (isMutagen == "two")
                            {
                                randomBeforeMutagens = Random.Range(1, 4);
                                randomAfterMutagens = Random.Range(1, 3);
                                randomPointTwo = Random.Range(0, 3);
                                while (randomPointTwo == randomPoint)
                                {
                                    randomPointTwo = Random.Range(0, 3);
                                }
                                randomCountMutagens = Random.Range(4, 7);
                            }
                            StartCoroutine(NextMutagen());
                        }
                    }
                }
                break;

        }
    }
    IEnumerator NextMutagen()
    {   
        yield return new WaitForSeconds(randomTimeAmidMutagens);
       /* if (IsSpawnTunnels)
            StartCoroutine(MutagenSpawn());
        else
        {
            randomTimeAmidMutagens = 2;
            StartCoroutine(NextMutagen());
        }*/
       StartCoroutine(MutagenSpawn());
    }
}
