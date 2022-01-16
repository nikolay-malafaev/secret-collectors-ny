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

    void Start()
    {
        IsNoGravity = false;
        m_TargetPosition = new Vector3(0, -0.708f, 4.31f);
        healthPlayer = 3;
        m_IsFly = false;
    }

    void Update()
    {
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

        if (burable || doubleMutagen || isNoGravityBaff)
            StartCoroutine(TimeBuff());
        healthPlayer = Mathf.Clamp(healthPlayer, -1, 1);
        verticalTargetPosition = m_TargetPosition;
        if(IsNoGravity & !isNoGravityBaff) NoGravity();

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
        isNoGravityBaff = false;
        timer = false;
    }
}
