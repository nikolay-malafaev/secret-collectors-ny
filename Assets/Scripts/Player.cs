using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health = 1;
    public int Health
    {
        get { return health; }
    }
    
    private int currentLane = 1;
    public int CurrenLane
    {
        get { return currentLane; }
    }

    [SerializeField] private float movePlayerSpeed;
    [SerializeField] private float jumpLength;
    [SerializeField] private GameObject lantern;
    private float jumpStart;
    private bool jumping;
    private bool isFly = false;
    private Vector3 targetPosition;
    private Vector3 verticalTargetPosition;
    private Quaternion verticalQuaternion;
    private Animator animator;
    private float positionY;
    private bool lockMove;
    private bool onGame;
    private bool isSwiping;
    private bool isNoGravity;
    private Vector2 startingTouch;
    private float lastMovePlayerSpeed;
    private float countCrash;
    private int chooseAnimation;
    private Quaternion startRotation = Quaternion.Euler(0, 130, 0);
    private bool isChange;
    private GameManager gameManager;
    private ChunkController chunkController;
    private CameraController cameraController;
    

    void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.SendTransitionToGame += TransitionToGame;
        GameManager.SendTurn += Turn;
        GameManager.SendDefeatGame += DefeatGame;
        GameManager.SendResetGame += ResetGame;
        GameManager.SendPauseGame += Pause;
        BuffController.SendBuff += Buff;
        cameraController = FindObjectOfType<CameraController>();
        chunkController = FindObjectOfType<ChunkController>();
        animator = GetComponentInChildren<Animator>();
        lastMovePlayerSpeed = movePlayerSpeed;
        positionY = -1;
        targetPosition = new Vector3(0, positionY, 0);

        if (gameManager.Test)
        {
            animator.SetTrigger("run");
        }
        else
        {
            StartTransform();
        }
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

#else
        if (Input.touchCount == 1)
        {
            if(isSwiping)
            {
                Vector2 diff = Input.GetTouch(0).position - startingTouch;

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
						
                    isSwiping = false;
                }
            }
            
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startingTouch = Input.GetTouch(0).position;
                isSwiping = true;
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isSwiping = false;
            }
        }
#endif
        
        
        onGame = gameManager.Game;
        targetPosition = new Vector3(targetPosition.x, positionY, targetPosition.z);
        health = Mathf.Clamp(health, -1, 1);
        verticalTargetPosition = targetPosition;

        if (jumping)
        {
            float correctJumpLength = jumpLength * (1.0f + chunkController.numberAddSpeed);
            float ratio = 0;
            if(gameManager.Direction == 0 || gameManager.Direction == 2) ratio = (Mathf.Abs(chunkController.transform.position.z) - jumpStart) / correctJumpLength;
            if(gameManager.Direction == 1 || gameManager.Direction == 3) ratio = (Mathf.Abs(chunkController.transform.position.x) - jumpStart) / correctJumpLength;
            if (Mathf.Abs(ratio) >= 1.5f)   // System.Math.Round(transform.position.y, 1)  > 0.23f
            {
                jumping = false;
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * 0.2f) * 0.6f;
            }
        }

        if (onGame)
        {
            transform.position = Vector3.MoveTowards( transform.position, verticalTargetPosition, movePlayerSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards( transform.rotation, verticalQuaternion, 250 * Time.deltaTime);
        }
       
        if (Math.Round(transform.position.y - 0.1f, 1) > positionY) isFly = true;  //-0.7
        else isFly = false;

        float x = transform.position.x;
        x = Mathf.Abs(x);
        if (x > 0.1f & x < 0.9)
        {
            isChange = true;
        }
        else isChange = false;
        
    }

    private void Jump()
    {
        if(isNoGravity || isFly || lockMove || !onGame) return;

        if (!jumping)
        {
           //float correctJumpLength = jumpLength * (1.0f + chunkController.numberAddSpeed);
           if(gameManager.Direction == 0 || gameManager.Direction == 2) jumpStart = Mathf.Abs(chunkController.transform.position.z);
           if(gameManager.Direction == 1 || gameManager.Direction == 3) jumpStart = Mathf.Abs(chunkController.transform.position.x);
           jumping = true;
           animator.SetTrigger("jump");
        }
    }
    
    private void ChangeLane(int direction)
    {
        if(isNoGravity || lockMove || !onGame) return;
        
        int targetLane = currentLane + direction;
        
        if (targetLane < 0 || targetLane > 2)
            return;
        currentLane = targetLane;
        
        switch (gameManager.Direction)
        {
            case 0:
                targetPosition = new Vector3((currentLane - 1), positionY, 0);
                break;
            case 1:
                targetPosition = new Vector3(0, positionY, -(currentLane - 1));
                break;
            case 2:
                targetPosition = new Vector3(-(currentLane - 1), positionY, 0);
                break;
            case 3:
                targetPosition = new Vector3(0, positionY, (currentLane - 1));
                break;
        }
        
        
        if(direction > 0 & !isFly) animator.SetTrigger("right");
        if(direction < 0 & !isFly) animator.SetTrigger("left");
    }

    private void OnTriggerEnter(Collider col)
    {
        switch (col.gameObject.tag)
        {
            case "Barrier":
                if (!gameManager.Test)
                {
                    if (BuffController.CurrenBuff == global::Buff.Options.Durable)
                    {
                        BuffController.SendBlastBuff.Invoke();
                        //col.gameObject.SetActive(false);
                        BuffController.SendBuff.Invoke(false);
                        //gameManager.Buffs("blast", false);
                    }
                    else if(health > 1)
                    {
                        health--;
                    }
                    else gameManager.OnSendDefeatGame();
                }
                else
                {
                    countCrash++;
                    Debug.LogWarning("Game over: " + countCrash);
                }
                break;
            case "PaulHole":
                lockMove = true;
                cameraController.MoveHole();
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag($"PaulHole"))
        {
            lockMove = false;
        }
    }
    private void NoGravity(bool gravity)
    {
       if(lockMove || !onGame) return;

       targetPosition = new Vector3(0, positionY, 0);
       isNoGravity = !isNoGravity;
       float eulerCorner;
       
        if (gravity)
        {
            eulerCorner = 180;
            positionY = 1.249f;
            currentLane = 1;
        }
        else
        {
            eulerCorner = 0;
            positionY = -0.708f;
        }
        
        verticalQuaternion = Quaternion.Euler(0, 0,eulerCorner);
    }
    private void TransitionToGame()
    {
        lantern.SetActive(true);
        animator.SetTrigger("run");
        verticalQuaternion = Quaternion.Euler(0, 0, 0);
    }
    private void Buff(bool value)
    {
        if(BuffController.LastBuff == global::Buff.Options.NoGravity) NoGravity(false);
    }
    private void DefeatGame()
    {
        animator.SetTrigger("fall");
    }
    private void StartTransform()
    {
        animator.Rebind();
        lantern.SetActive(false);
        verticalQuaternion = startRotation;
        transform.rotation = startRotation;
        StartCoroutine(AnimationIdle());
    }
    private void ResetGame()
    {
        if(currentLane == 0) ChangeLane(-1);
        else if (currentLane == 2) ChangeLane(1);
        transform.position = new Vector3(0, positionY, 0);
        currentLane = 1;
        targetPosition = new Vector3(0, positionY, 0);
        animator.speed = 1;
        StartTransform();
    }
    private void Turn(Target.Direction directionTurn)
    {
        int sing = directionTurn == Target.Direction.Right ? 1 : -1;
        verticalQuaternion = Quaternion.Euler(0, verticalQuaternion.eulerAngles.y + (90 * sing), 0);
        ChangeLane(-sing);
    }
    IEnumerator AnimationIdle()
    {
        yield return new WaitForSeconds(5);
        chooseAnimation = UnityEngine.Random.Range(0, 3);
        animator.SetInteger("choose", chooseAnimation);
        if(!gameManager.Game) StartCoroutine(AnimationIdle());
    }
    public void NoGravityButton()
    {
        NoGravity(!isNoGravity);
    }

    private void Pause()
    {
        animator.speed = animator.speed == 0 ? 1 : 0;
    }
    private void OnDestroy()
    {
        GameManager.SendTransitionToGame -= TransitionToGame;
        GameManager.SendTurn -= Turn;
        GameManager.SendPauseGame -= Pause;
        GameManager.SendDefeatGame -= DefeatGame;
        GameManager.SendResetGame -= ResetGame;
        BuffController.SendBuff -= Buff;
    }
}
