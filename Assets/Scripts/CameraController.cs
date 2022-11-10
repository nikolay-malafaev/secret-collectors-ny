using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float MovidCameraSpeed;
    private Vector3 positionCamera;
    private Vector3 verticalTargetPosition;
    private Vector3 positionCameraChange;
    private Vector3 startPositionCamera = new Vector3(1.4f,-0.32f,-1.2f);  //Vector3(1.39999998,-0.319999993,-1.29999995)
    private Quaternion startRotationCamera = Quaternion.Euler(4.9f, -50, 0);
    private float maxPositionX;
    private float RotateCameraSpeed = 85;
    private GameManager gameManager;
    private Quaternion vetricalQuaternion;
    private float positionCameraFollow;
    private bool isFollow = true;


    void Start()
    {
        gameManager = GameManager.Instance;
        GameManager.SendTransitionToGame += TransitionToGame;
        GameManager.SendTurn += Turn;
        GameManager.SendResetGame += ResetGame;
        
        if (!gameManager.Test)
        {
            StartTransform();
        }
        else
        {
            vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
            positionCameraChange.y = 0.54f;
            positionCameraChange.z = 2.93f;
        }
        maxPositionX = 0.7f;
       
    }


    void Update()
    {
        positionCameraFollow = Mathf.Clamp(Mathf.Sin((player.CurrenLane - 1) * 1.4f), -maxPositionX, maxPositionX);;
        switch (gameManager.Direction)
        {
            case 0:
                positionCamera = new Vector3(positionCameraFollow, positionCameraChange.y, -positionCameraChange.z);
                break;
            case 1:
                positionCamera = new Vector3(-positionCameraChange.z, positionCameraChange.y, -positionCameraFollow);
                break;
            case 2:
                positionCamera = new Vector3(-positionCameraFollow, positionCameraChange.y, positionCameraChange.z);
                break;
            case 3:
                positionCamera = new Vector3(positionCameraChange.z, positionCameraChange.y, positionCameraFollow);
                break;
        }
        
        
        if (gameManager.Game & isFollow)
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
        //positionCamera = new Vector3(positionCamera.x , player.transform.position.y + 0.35f, 1.9f);
        positionCameraChange = new Vector3(0, player.transform.position.y + 0.65f, 1.9f);  
        maxPositionX = 1f;
        StartCoroutine(Time(1, "hole"));
    }
    

    private void Turn(Target.Direction directionTurn) 
    {
        int sing = directionTurn == Target.Direction.Right ? 1 : -1;
        int last = gameManager.Direction - sing;
        if (last == 4) last = 0;
        else if (last == -1) last = 3;
        
        Vector3 position = transform.position;
        isFollow = false;
        transform.SetParent(player.transform);
        switch (last)
        {
            case 0:
                transform.position = new Vector3(sing, position.y, position.z);
                break;
            case 1:
                transform.position = new Vector3(position.x, position.y, -sing);
                break;
            case 2:
                transform.position = new Vector3(-sing, position.y, position.z);
                break;
            case 3:
                transform.position = new Vector3(position.x, position.y, sing);
                break;
        }
       
        StartCoroutine(Time(0.37f, "follow"));
    }
    
    IEnumerator Time(float time, string name)
    {
        yield return new WaitForSeconds(time);
        switch (name)
        {
            case "hole":
                maxPositionX = 0.7f;
                positionCameraChange = new Vector3(0, 0.54f, 2.93f); 
                break;
            case "follow":
                isFollow = true;
                transform.parent = null;
                transform.rotation = Quaternion.Euler(6.3f, player.transform.rotation.eulerAngles.y, 0); // переменстить в Turn и присвоить vetricalQuaternion
                vetricalQuaternion = transform.rotation; 
                break;
        }
      
    }
    
    private void TransitionToGame()
    {
        vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
        positionCameraChange.y = 0.54f;
        positionCameraChange.z = 2.93f;
        //RotateCameraSpeed = 65;
    }

    private void ResetGame()
    {
        StartTransform();
    }

    private void StartTransform()
    {
        transform.position = startPositionCamera;
        transform.rotation = startRotationCamera;
        positionCamera = transform.position;
    }

    public void Rotate(float directionRotate)
    {
        vetricalQuaternion = Quaternion.Euler(0,directionRotate,0);
    }

    private void OnDestroy()
    {
        GameManager.SendTransitionToGame -= TransitionToGame;
        GameManager.SendTurn -= Turn;
        GameManager.SendResetGame -= ResetGame;
    }
}
