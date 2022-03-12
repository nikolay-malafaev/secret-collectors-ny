using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [HideInInspector] public int healthPlayer;
    [HideInInspector] public int colMutagen;
    public int m_CurrentLane;
    public float MovidPlayerSpeed;
    public float jumpLength = 6;
    private float m_JumpStart;

    //Buffs
    private bool burable;
    private bool blastScreen;
    private bool doubleMutagen;
    private bool timer;
    [HideInInspector] public bool IsNoGravity;

    
    [SerializeField] private GameObject Lantern;
    public bool m_Jumping;
    public bool m_IsFly;
    public Vector3 m_TargetPosition;
    private Vector3 verticalTargetPosition;
    public GameManager gameManager;
    public ChunkController chunkController;
    public CameraController camera;
    [HideInInspector] public Quaternion vetricalQuaternion;
    [HideInInspector] public Animator animator;
    [HideInInspector] public int eulerCorner;
    [HideInInspector] public float yCorner;
    [HideInInspector] public bool isNoGravityBaff;
    public UI UI;
    private float positionY;
    private bool tabooOnMove;
    private bool onGame;

    private bool m_IsSwiping = false;
    private Vector2 m_StartingTouch;
    private float lastMovidPlayerSpeed;
    private float countCrash;
    private int chooseAnimation;
    private Quaternion startRotation = Quaternion.Euler(0, 130, 0);

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lastMovidPlayerSpeed = MovidPlayerSpeed;
        IsNoGravity = false;
        positionY = -1;
        m_TargetPosition = new Vector3(0, positionY, 0);
        healthPlayer = 1;
        m_IsFly = false;

        if (gameManager.test)
        {
            animator.SetTrigger("run");
        }
        else
        {
            Lantern.SetActive(false);
            vetricalQuaternion = startRotation;
            transform.rotation = startRotation;
            StartCoroutine(AnimationFun());
        }
    }
    
    IEnumerator AnimationFun()
    {
        yield return new WaitForSeconds(5);
        chooseAnimation = Random.Range(0, 3);
        animator.SetInteger("choose", chooseAnimation);
        if(!gameManager.game) StartCoroutine(AnimationFun());
    }
    
    

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
           NoGravity();
        }
#else
        
        if (Input.touchCount == 1)
        {
            if(m_IsSwiping)
            {
                Vector2 diff = Input.GetTouch(0).position - m_StartingTouch;

                // Put difference in Screen ratio, but using only width, so the ratio is the same on both
                // axes (otherwise we would have to swipe more vertically...)
                diff = new Vector2(diff.x/Screen.width, diff.y/Screen.width);

                if(diff.magnitude > 0.01f) //we set the swip distance to trigger movement to 1% of the screen width
                {
                    if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        /*if(TutorialMoveCheck(2) && diff.y < 0)
                        {
                            Slide();
                        }*/
                        //else if(TutorialMoveCheck(1))
                        //{
                            Jump();
                        //}
                    }
                    else //if(TutorialMoveCheck(0))
                    {
                        if(diff.x < 0)
                        {
                            ChangeLane(-1);
                        }
                        else
                        {
                            ChangeLane(1);
                        }
                    }
						
                    m_IsSwiping = false;
                }
            }


            // Input check is AFTER the swip test, that way if TouchPhase.Ended happen a single frame after the Began Phase
            // a swipe can still be registered (otherwise, m_IsSwiping will be set to false and the test wouldn't happen for that began-Ended pair)
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                m_StartingTouch = Input.GetTouch(0).position;
                m_IsSwiping = true;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                m_IsSwiping = false;
            }
        }
#endif
        onGame = gameManager.game;
        m_TargetPosition = new Vector3(m_TargetPosition.x, positionY, m_TargetPosition.z);
        
       // if (burable || doubleMutagen || isNoGravityBaff)
            //StartCoroutine(TimeBuff());
        
        
        healthPlayer = Mathf.Clamp(healthPlayer, -1, 1);
        verticalTargetPosition = m_TargetPosition;

        if (m_Jumping)
        {
            float correctJumpLength = jumpLength * (1.0f + chunkController.numberAddSpeed);
            float ratio = 0;
            if(gameManager.direction == 0 || gameManager.direction == 2) ratio = (Mathf.Abs(chunkController.transform.position.z) - m_JumpStart) / correctJumpLength;
            if(gameManager.direction == 1 || gameManager.direction == 3) ratio = (Mathf.Abs(chunkController.transform.position.x) - m_JumpStart) / correctJumpLength;
            if (Mathf.Abs(ratio) >= 1.5f)   // System.Math.Round(transform.position.y, 1)  > 0.23f
            {
                m_Jumping = false;
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * 0.2f) * 0.6f;
            }
        }

        if (onGame)
        {
            transform.position = Vector3.MoveTowards( transform.position, verticalTargetPosition, MovidPlayerSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards( transform.rotation, vetricalQuaternion, 250 * Time.deltaTime);
        }
       
        if (Math.Round(transform.position.y - 0.1f, 1) > positionY) m_IsFly = true;  //-0.7
        else m_IsFly = false;
    }
    

   
    public void Jump()
    {
        if(m_IsFly || tabooOnMove || !onGame) return;

        if (!m_Jumping)
        {
           //float correctJumpLength = jumpLength * (1.0f + chunkController.numberAddSpeed);
           if(gameManager.direction == 0 || gameManager.direction == 2) m_JumpStart = Mathf.Abs(chunkController.transform.position.z);
           if(gameManager.direction == 1 || gameManager.direction == 3) m_JumpStart = Mathf.Abs(chunkController.transform.position.x);
           m_Jumping = true;
           animator.SetTrigger("jump");
        }
    }
    
    public void ChangeLane(int direction)
    {
        if(IsNoGravity || tabooOnMove || !onGame) return;

        int targetLane = m_CurrentLane + direction;
        if (targetLane < 0 || targetLane > 2)
            return;
        m_CurrentLane = targetLane;
        
        switch (gameManager.direction)
        {
            case 0:
                m_TargetPosition = new Vector3((m_CurrentLane - 1), positionY, 0);
                break;
            case 1:
                m_TargetPosition = new Vector3(0, positionY, -(m_CurrentLane - 1));
                break;
            case 2:
                m_TargetPosition = new Vector3(-(m_CurrentLane - 1), positionY, 0);
                break;
            case 3:
                m_TargetPosition = new Vector3(0, positionY, (m_CurrentLane - 1));
                break;
        }
        
        if(direction > 0) animator.SetTrigger("right");
        if(direction < 0) animator.SetTrigger("left");
    }

    
    
    private void OnTriggerEnter(Collider col)
    {
        
        /*if (col.gameObject.CompareTag(""))
        {
            positionY = -0.708f;
            Debug.Log("Pos");
        }

        if (col.gameObject.CompareTag("UpTrack"))
        {
            positionY = -0.32f;
            Debug.Log("UpPos");
        }*/

        switch (col.gameObject.tag)
        {
            case "Barrier":
                if (!gameManager.test)
                {
                    if (gameManager.buffs[0])
                    {
                        gameManager.Buffs("blast", false);
                    }
                    else healthPlayer--;
                }
                else if (gameManager.test)
                {
                    countCrash++;
                    Debug.LogError("Game over: " + countCrash);
                }
                
                break;
            case "Mutagen":
                if (gameManager.buffs[1])
                {
                    colMutagen+=2;
                    gameManager.AddMutagen(2);
                }
                else
                {
                    gameManager.AddMutagen(1);
                    colMutagen++;
                }
                Destroy(col.gameObject);
                break;
            case "Target":
                //Target target = col.gameObject.GetComponent<Target>();
                
                break;
            case "PaulHole":
                tabooOnMove = true;
                camera.MoveHole();
                break;
            
        }
        /* if (col.gameObject.CompareTag("Rise"))
         {
             StartCoroutine(MovedPlayerSpeed());
             positionY = -0.32f;
             //m_TargetPosition = new Vector3(transform.position.x, -0.32f, 4.31f);
 
         }
         
         if (col.gameObject.CompareTag("UndRise"))
         {
             StartCoroutine(MovedPlayerSpeed());
             positionY = -0.708f;
             //m_TargetPosition = new Vector3(transform.position.x, -0.708f, 4.31f);
         }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"PaulHole"))
        {
            tabooOnMove = false;
        }
    }
    
    public void NoGravity()
    {
        if(!isNoGravityBaff || tabooOnMove || !onGame) return;
        
        IsNoGravity = !IsNoGravity;
        if (IsNoGravity)
        {
            eulerCorner = 180;
            positionY = 1.249f;
        }
        else
        {
            eulerCorner = 0;
            positionY = -0.708f;
            m_CurrentLane = 1;
        }
        vetricalQuaternion = Quaternion.Euler(0, 0,eulerCorner);
        m_TargetPosition = new Vector3(0, positionY, 0);
    }

    public void ToGame()
    {
        Lantern.SetActive(true);
        animator.SetTrigger("run");
        vetricalQuaternion = Quaternion.Euler(0, 0, 0);
    }

    
    /*IEnumerator MovedPlayerSpeed()
    {
        MovidPlayerSpeed = 1.5f;
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Stop");
        MovidPlayerSpeed = lastMovidPlayerSpeed;
    }*/

}
