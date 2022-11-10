using UnityEngine;

public class Mutagen : MonoBehaviour
{
    private GameManager gameManager;
    private MutagenController mutagenController;

    private void Start()
    {
        //gameManager = FindObjectOfType<GameManager>();
        gameManager = GameManager.Instance;
        mutagenController = FindObjectOfType<MutagenController>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Barrier") || col.gameObject.CompareTag("Buff"))
        {
            gameObject.SetActive(false);
            //Debug.LogError("Mutagen is hide");
        }

        if (col.gameObject.CompareTag("Player"))
        {
            gameManager.CountMutagen++;
            if (BuffController.CurrenBuff == Buff.Options.DoubleMutagen) gameManager.CountMutagen++;
            mutagenController.DestroyMutagen(transform);
        }
    }
}
