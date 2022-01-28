using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    private Vector3 positionCamera;
    private Vector3 verticalTargetPosition;
    private Vector3 startPositionCamera = new Vector3(1.4f,-0.32f,3.1f);
    private Quaternion startRotationCamera = Quaternion.Euler(4.9f, -50, 0);
    private float maxPositionX;
    [SerializeField] private float MovidCameraSpeed;
    private float RotateCameraSpeed;
    [SerializeField] private GameManager gameManager;
    private Quaternion vetricalQuaternion;


    void Start()
    {
        if (!gameManager.test)
        {
            transform.position = startPositionCamera;
            transform.rotation = startRotationCamera;
            positionCamera = transform.position;
        }
        else
        {
            vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
            positionCamera = new Vector3(0, 0.54f, 1.2f);
        }
        maxPositionX = 0.7f;
    }


    void Update()
    {
        if (gameManager.game)
        {
         //   positionCamera = new Vector3(player.transform.position.x, positionCamera.y, positionCamera.z);
           // positionCamera.x = Mathf.Clamp(positionCamera.x, -maxPositionX, maxPositionX);
                 positionCamera.x = Mathf.Clamp(Mathf.Sin((player.m_CurrentLane - 1) * 1.4f), -maxPositionX, maxPositionX);
        }
        verticalTargetPosition = positionCamera;
        if (gameManager.game)
        {
            transform.position = Vector3.MoveTowards(transform.position, verticalTargetPosition,
                MovidCameraSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, vetricalQuaternion,
                RotateCameraSpeed * Time.deltaTime);
        }
    }

    public void MoveHole()
    {
        positionCamera = new Vector3(positionCamera.x , player.transform.position.y + 0.55f, 1.9f);
        maxPositionX = 0.9f;
        StartCoroutine(TimeMoveHole());
    }

    IEnumerator TimeMoveHole()
    {
        yield return new WaitForSeconds(1f);
        maxPositionX = 0.7f;
        positionCamera.y = 0.54f;
        positionCamera.z = 1.2f;
    }

    public void ToGame()
    {
        vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
        positionCamera = new Vector3(0, 0.54f, 1.2f);
        RotateCameraSpeed = 65;
    }
}
