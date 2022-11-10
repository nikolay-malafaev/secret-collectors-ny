using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private Text text;

    private float accum = 0.0f;
    private int frames = 0;
    private float timeleft;
    private static float fps;
    public static float fpsCount
    {
        get { return fps; }
    }

    private GameManager gameManager;
    
  
    void Start()
    {
        gameManager = GameManager.Instance;
        timeleft = updateInterval;
    }
    
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0)
        {
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
        if(gameManager.Game) text.text = fps.ToString("F2") + " FPS";
    }
}
