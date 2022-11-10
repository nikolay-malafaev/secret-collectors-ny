using UnityEngine;

public class MultiBarrier : Barrier
{
    [SerializeField] private GameObject[] barriers;
    private int isActiveBarrier;

    public void SetActiveBarrier()
    {
        isActiveBarrier = Random.Range(0, barriers.Length);
        barriers[isActiveBarrier].SetActive(true);
    }

    public void SetUnActiveBarrier()
    {
        barriers[isActiveBarrier].SetActive(false);
    }
}
