using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    private Vector3 positionCamera;
    private Vector3 verticalTargetPosition;
    private Vector3 startPositionCamera = new Vector3(1.4f,-0.32f,-1.2f);  //Vector3(1.39999998,-0.319999993,-1.29999995)
    private Quaternion startRotationCamera = Quaternion.Euler(4.9f, -50, 0);
    private float maxPositionX;
    [SerializeField] private float MovidCameraSpeed;
    private float RotateCameraSpeed = 85;
    [SerializeField] private GameManager gameManager;
    private Quaternion vetricalQuaternion;
    [HideInInspector] public bool isFollow;


    void Start()
    {
        isFollow = true;
        if (!gameManager.test)
        {
            transform.position = startPositionCamera;
            transform.rotation = startRotationCamera;
            positionCamera = transform.position;
        }
        else
        {
            vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
            positionCamera = new Vector3(0, 0.54f, -2.93f);
        }
        maxPositionX = 0.7f;
    }


    void Update()
    {
        float positionCameraFollow = Mathf.Clamp(Mathf.Sin((player.m_CurrentLane - 1) * 1.4f), -maxPositionX, maxPositionX);;
        switch (gameManager.direction)
        {
            case 0:
                positionCamera = new Vector3(positionCameraFollow, 0.54f, -2.93f);
                break;
            case 1:
                positionCamera = new Vector3(-2.93f, 0.54f, -positionCameraFollow);
                break;
            case 2:
                positionCamera = new Vector3(-positionCameraFollow, 0.54f, 2.93f);
                break;
            case 3:
                positionCamera = new Vector3(2.93f, 0.54f, positionCameraFollow);
                break;
        }
        
        
        if (gameManager.game & isFollow)
        {
            verticalTargetPosition = positionCamera;
            transform.position = Vector3.MoveTowards(transform.position, verticalTargetPosition,
                MovidCameraSpeed * UnityEngine.Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, vetricalQuaternion,
                RotateCameraSpeed * UnityEngine.Time.deltaTime);
        }

       
    }

    public void MoveHole()
    {
        positionCamera = new Vector3(positionCamera.x , player.transform.position.y + 0.35f, 1.9f);
        maxPositionX = 0.9f;
        StartCoroutine(Time(1, "hole"));
    }
    
    
    
    public void ToGame()
    {
        vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
        positionCamera = new Vector3(0, 0.54f, 1.2f);
        RotateCameraSpeed = 65;
    }

    public void Turn()
    {
        isFollow = false;
        StartCoroutine(Time(0.37f, "follow"));
        
    }
    
    IEnumerator Time(float time, string name)
    {
        yield return new WaitForSeconds(time);
        switch (name)
        {
            case "hole":
                maxPositionX = 0.7f;
                positionCamera.y = 0.54f;
                positionCamera.z = 1.2f;
                break;
            case "follow":
                isFollow = true;
                transform.parent = null;
                transform.rotation = Quaternion.Euler(6.3f, player.transform.rotation.eulerAngles.y, 0);
                vetricalQuaternion = transform.rotation;
                break;
        }
      
    }

    public void Rotate(float directionRotate)
    {
        vetricalQuaternion = Quaternion.Euler(0,directionRotate,0);
    }
}
