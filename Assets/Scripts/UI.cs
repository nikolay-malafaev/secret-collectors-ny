using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;
    public TubeController tubeController;

    public Text distation;
    public Text maxDistation;
    public Text Mutagen;
    public GameObject panel;
    public GameObject pauseButton;
    public GameObject pauseText;
    public GameObject gameoverText;
    public GameObject timerImage;
    public GameObject doubleMutagenImage;
    public Image timeBaff;

    public Animator animator;
    private float saveDist;


    void Start()
    {
        maxDistation.text = PlayerPrefs.GetFloat("Distation").ToString();
    }

    void FixedUpdate()
    {


        distation.text = Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)).ToString();
        Mutagen.text = player.colMutagen.ToString();

        if (player.healthPlayer == 0)
        {
                player.healthPlayer = -1;
                gameoverText.SetActive(true);
                pauseText.SetActive(true);
                Pause();
                pauseButton.SetActive(false);
        }

        if (PlayerPrefs.GetFloat("Distation") < Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)))
        {
            PlayerPrefs.SetFloat("Distation", Mathf.Round(Mathf.Abs(tubeController.positionTubeZ)));
        }

        if (player.timer)
        {
            timerImage.SetActive(true);
            animator.SetBool("timer", true);
        }
        else { timerImage.SetActive(false); animator.SetBool("timer", false); }

        if (player.blastScreen)
            { animator.SetTrigger("blast"); player.blastScreen = false; }

        if (player.doubleMutagen)
        {
            doubleMutagenImage.SetActive(true);
        } else doubleMutagenImage.SetActive(false);


    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        panel.SetActive(!panel.activeInHierarchy);
        pauseText.SetActive(!pauseText.activeInHierarchy);
        tubeController.pausePosition = !tubeController.pausePosition;
        player.gameManager.Pause();
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        maxDistation.text = PlayerPrefs.GetFloat("Distation").ToString();
    }

}
