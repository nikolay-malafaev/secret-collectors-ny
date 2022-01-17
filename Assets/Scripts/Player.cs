using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int healthPlayer;
    public int colMutagen;
    public int m_CurrentLane;
    public float MovidPlayerSpeed;
    public float jumpLength = 6;
    private float m_JumpStart;
    public bool burable;
    public bool blastScreen;
    public bool doubleMutagen;
    public bool timer;
    public bool m_Jumping;
    public bool m_IsFly;
    public Vector3 m_TargetPosition;
    private Vector3 verticalTargetPosition;
    public GameManager gameManager;
    public TubeController tubeController;
    public Animator animator;
    public GameObject Camera;
    private Quaternion vetricalQuaternion;
    private bool IsNoGravity;
    private int eulerCorner;
    private float yCorner;
    public bool isNoGravityBaff;
    
    private bool m_IsSwiping = false;
    private Vector2 m_StartingTouch;

    void Start()
    {
        IsNoGravity = false;
        m_TargetPosition = new Vector3(0, -0.708f, 4.31f);
        healthPlayer = 3;
        m_IsFly = false;
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

        if (burable || doubleMutagen || isNoGravityBaff)
            StartCoroutine(TimeBuff());
        healthPlayer = Mathf.Clamp(healthPlayer, -1, 1);
        verticalTargetPosition = m_TargetPosition;

        if (m_Jumping)
        {
            float correctJumpLength = jumpLength * (1.0f + tubeController.numberAddSpeed);
            float ratio = (Mathf.Abs(tubeController.mainTube.transform.position.z) - m_JumpStart) / correctJumpLength;
            if (ratio >= 0.23f)   // System.Math.Round(transform.position.y, 1)  > 0.23f
            {
                m_Jumping = false;
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * 1.2f;
            }
        }
        transform.position = Vector3.MoveTowards( transform.position, verticalTargetPosition, MovidPlayerSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards( transform.rotation, vetricalQuaternion, 250 * Time.deltaTime);
        if (System.Math.Round(transform.position.y, 1) > -0.7f) m_IsFly = true;  //-0.708
        else m_IsFly = false;
    }
    
    public void NoGravity()
    {
        if(!isNoGravityBaff) return;
        
        IsNoGravity = !IsNoGravity;
        if (IsNoGravity)
        {
            eulerCorner = 180;
            yCorner = 1.249f;
        }
        else
        {
            eulerCorner = 0;
            yCorner = -0.708f;
            m_CurrentLane = 1;
        }
        vetricalQuaternion = Quaternion.Euler(0, 0,eulerCorner);
        m_TargetPosition = new Vector3(0, yCorner, 4.31f);
    }
    public void Jump()
    {
        if(m_IsFly)
            return;
        
        if (!m_Jumping)
        { 
            float correctJumpLength = jumpLength * (1.0f + tubeController.numberAddSpeed);
           m_JumpStart = Mathf.Abs(tubeController.mainTube.transform.position.z);
           m_Jumping = true;
        }
    }
    public void ChangeLane(int direction)
    {
        if(IsNoGravity)
            return;
        
        int targetLane = m_CurrentLane + direction;
        if (targetLane < 0 || targetLane > 2)
            // Ignore, we are on the borders.
            return;
        m_CurrentLane = targetLane;
        m_TargetPosition = new Vector3((m_CurrentLane - 1), -0.708f, 4.31f); //*trackManager.laneOffset
    }

    
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Barrier"))
        {
            if (!burable)
                healthPlayer--;
            else if(burable)
            {
                blastScreen = true;
                burable = false;
                timer = false;
                StopCoroutine(TimeBuff());
                for (int i = 0; i < tubeController.transform.childCount; i++)
                {
                    if (tubeController.transform.GetChild(i).childCount > 2)
                    {
                        if (tubeController.transform.GetChild(i).GetChild(0).childCount > 0)
                        {
                            if (tubeController.transform.GetChild(i).GetChild(0).GetChild(0).CompareTag("Barrier"))
                                Destroy(tubeController.transform.GetChild(i).GetChild(0).GetChild(0).gameObject);
                        }
                    }
                }
            }
        }
        if(col.gameObject.CompareTag("Mutagen"))
        {
            if(doubleMutagen) colMutagen+=2;
               else colMutagen++;
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag($"Target"))
        {
            DoubleTunnels doubleTunnels = col.gameObject.GetComponentInParent<DoubleTunnels>();
            doubleTunnels.TurnTunnels();
        }
    }
    

    IEnumerator TimeBuff()
    {
        yield return new WaitForSeconds(10f);
        doubleMutagen = false;
        burable = false;
        timer = false;
        if (isNoGravityBaff)
        {
            if(IsNoGravity) NoGravity();
            isNoGravityBaff = false;
        }
    }
}
