using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    /*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeController : MonoBehaviour
{
    public Tube[] TubePrefabs;
    public Buff[] baffs;
    public Mutagen mutagenPrefabs;
    public Enemy enemyPrefabs;

    private float[] coordinates = { 0.7f, -0.7f };
    private List<Tube> spawnTubes = new List<Tube>();
    private List<Mutagen> mutagenSpawn = new List<Mutagen>();
    public Tube startTube;
    public Mutagen mutagenStart;

    [Range(0, 100)]
    public float[] oddsTube;

    [Range(0, 100)]
    public float[] oddsBaffs;

    public Player player;
    public GameObject mainTube;
    public float corner;
    public bool pausePosition;
    public float positionTubeZ;
    private float speedCorner;
    public float addSpeed;
    public float numberAddSpeed;
    public float timeAddSpeed;
    public bool velocityTimeBaff;
    public float velocityCoin;
    private float number;
    [SerializeField] private int countCoin;
    private int schemeNumber;
    private int score = 0;
    [SerializeField] private bool stop;
    private int cornerRotate;
    private int jump;
    private bool swipe;
    public int position;

    private Vector2 startPos;
    private Vector2 direction;
    private bool directionChosen;
    public Text m_Text;
    public int numberP;
    string message;
    public float baffvelocity;

    void Start()
    {
        schemeNumber = 1;
        addSpeed = 0.1f;
        spawnTubes.Add(startTube);
        StartCoroutine(TimeSpawnBuff());

        StartCoroutine(AddSpeed());
        mutagenSpawn.Add(mutagenStart);
        mutagenStart.transform.Rotate(0, 0, Random.Range(0, 360));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            cornerRotate++;

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            cornerRotate--;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            jump++;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            jump--;
        }

    }

    void FixedUpdate()
    {
        #region Touch
        //m_Text.text = "������� : " + message + "� ����������� " + touch.position;
        // ������������ ������ ������� � �������� �������� ���������� ������������.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ������������ �������� ������� � ����������� �� ���� �������
            switch (touch.phase)
            {
                //��� ������ ����������� ������� �������� ��������� � �������� ��������� �������
                case TouchPhase.Began:
                    // �������� ��������� ��������� �������
                    startPos = touch.position;
                    message = "Begun ";
                    swipe = true;
                    break;

                //����������, �������� �� ������� ���������� ��������
                case TouchPhase.Moved:
                    // ���������� �����������, ��������� ������� ��������� ������� � �������� 
                    direction = touch.position - startPos;
                    if(swipe)
                    {
                        if (direction.x < 0 & Mathf.Abs(direction.y) < 20) cornerRotate++; else if (direction.x > 0 & Mathf.Abs(direction.y) < 20) cornerRotate--;
                        if (direction.y < 0 & Mathf.Abs(direction.x) < 20) m_Text.text = "down"; else if (direction.y > 0 & Mathf.Abs(direction.x) < 20) m_Text.text = "up";
                        swipe = false;
                    }  
                    //speedCorner = (direction.x) / 30;
                    //if (Mathf.Abs(speedCorner) < 0.5f) speedCorner = 0;
                    message = "Moving ";
                    break;

                case TouchPhase.Ended:
                    //��������, ��� ������� �����������, ����� ��� ����������
                    //speedCorner = 0;
                    message = "Ending ";
                    break;
            }
        }

        #endregion

        if (spawnTubes[spawnTubes.Count - 1].End.position.z < 40)
        {
            spawnTube();
        }

        if (spawnTubes.Count > 10)
        {
            destoryTube();
        }

        if (mutagenSpawn[mutagenSpawn.Count - 1].End.position.z < 40) Spawn("mutagen");

        //if (mutagenSpawn[0].End.position.z < -10) DestroyCoin();



        if (!pausePosition)
        {
            speedCorner = -speedCorner;
            speedCorner = Mathf.Clamp(speedCorner, -6f, 6f);
            mainTube.transform.position = new Vector3(0, 0, positionTubeZ);
            mainTube.transform.rotation = Quaternion.Euler(0, 0, corner += speedCorner);
            positionTubeZ = positionTubeZ - addSpeed;
        }

        if (corner == 360 | corner == -360) corner = 0;

        if (!player.burable & velocityTimeBaff)
        {
            addSpeed = baffvelocity;
            velocityTimeBaff = false;
        }

        if (cornerRotate > 0)
            corner += 2.25f; 

        if (cornerRotate < 0)
            corner-=2.25f;

        if (corner % 45 == 0)
        {
            if (cornerRotate > 0) cornerRotate--;
            if (cornerRotate < 0) cornerRotate++;
        }


        if (Input.GetKey(KeyCode.N))
        {

            Spawn("enemy");
        }

        position = Mathf.RoundToInt(mainTube.transform.eulerAngles.z / 45);

        if(mutagenSpawn[0] == null)
        {
            mutagenSpawn.RemoveAt(0);
        }
    }

    private void spawnTube()
    {
        Tube newTube = Instantiate(TubePrefabs[Random.Range(0, 5)]);
        newTube.transform.position = spawnTubes[spawnTubes.Count - 1].Begin.position - newTube.End.localPosition;
        spawnTubes.Add(newTube);
        newTube.transform.SetParent(mainTube.transform);
        newTube.transform.rotation = mainTube.transform.rotation;
    }
    private void destoryTube()
    {
        Destroy(spawnTubes[0].gameObject);
        spawnTubes.RemoveAt(0);
    }

    private void Spawn(string nameObject)
    {
        switch (nameObject)
        {
            case "mutagen":
                Mutagen newMutagen = Instantiate(mutagenPrefabs);
                newMutagen.transform.position = mutagenSpawn[mutagenSpawn.Count - 1].Begin.position - newMutagen.End.localPosition;
                mutagenSpawn.Add(newMutagen);
                newMutagen.transform.SetParent(mainTube.transform);
                newMutagen.transform.rotation = mainTube.transform.rotation;
                newMutagen.transform.Rotate(0, 0, EulerRotate(mutagenSpawn[mutagenSpawn.Count - 1].transform.rotation.z));
                break;
            case "buff":
                Buff buff;

                switch (Mathf.RoundToInt(Choose(oddsBaffs)))
                {
                    case 0:
                        buff = Instantiate(baffs[0]);
                        buff.transform.SetParent(mainTube.transform);
                        buff.transform.rotation = mainTube.transform.rotation;
                        buff.transform.position = new Vector3(0, coordinates[Random.Range(0, 2)], Random.Range(20, 35));
                        break;
                    case 1:
                        buff = Instantiate(baffs[1]);
                        buff.transform.SetParent(mainTube.transform);
                        buff.transform.rotation = mainTube.transform.rotation;
                        buff.transform.position = new Vector3(0, coordinates[Random.Range(0, 2)], Random.Range(20, 35));
                        break;
                    case 2:
                        buff = Instantiate(baffs[2]);
                        buff.transform.SetParent(mainTube.transform);
                        buff.transform.rotation = mainTube.transform.rotation;
                        buff.transform.position = new Vector3(0, coordinates[Random.Range(0, 2)], Random.Range(20, 35));
                        break;
                }
                break;
            case "enemy":
                Enemy enemy = Instantiate(enemyPrefabs);
                enemy.transform.SetParent(mainTube.transform);
                enemy.transform.position = new Vector3(0, -0.7f, 25);
                break;
        }
    }

    private float EulerRotate(float LastCoordination)
    {
        LastCoordination = mutagenSpawn[mutagenSpawn.Count - 1].transform.rotation.z + number;
        countCoin++;
        if (schemeNumber == 5) schemeNumber = 1;
        if (score == 6) score = 0;
        if(countCoin > 15)
        {
            schemeNumber = Random.Range(1, 5);
            countCoin = 0;
        }
        switch (schemeNumber)
        {
            case 1:
                number += 0;
                break;
            case 2:
                number += 15;
                break;
            case 3:
                number -= 15;
                break;
            case 4:
                if (score < 3) number += 15;
                if (score >= 3)  number -= 15;
                score++;
                break;
            default:
                break;
        }
        if (number == 360) number = 0;
        return LastCoordination;
    }
    IEnumerator TimeSpawnBuff()
    {
        yield return new WaitForSeconds(Random.Range(5, 20));  // ��������������� ��������� ��������
        Spawn("buff");
        StartCoroutine(TimeSpawnBuff());
    }

    IEnumerator AddSpeed()
    {
        yield return new WaitForSeconds(timeAddSpeed);  // ��������������� ��������� ��������
        addSpeed += numberAddSpeed;
        StartCoroutine(AddSpeed());
    }

    float Choose(float[] probs)
    {

        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }
}
*/

}
