using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    private Vector3 positionCamera;
    private Vector3 verticalTargetPosition;
    private float positionX;
    private float positionY;
    private float positionZ;
    private float maxPositionX;
    [SerializeField] private float MovidCameraSpeed;
    private float RotateCameraSpeed;
    [SerializeField] private GameManager gameManager;
    private Quaternion vetricalQuaternion;


    void Start()
    {
        if (!gameManager.test)
        {
            transform.position = new Vector3(1.7f, -0.6f, 4);
            transform.rotation = Quaternion.Euler(-3.8f, -80, 0);
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
            positionCamera = new Vector3(player.transform.position.x, positionCamera.y, positionCamera.z);
            positionCamera.x = Mathf.Clamp(positionCamera.x, -maxPositionX, maxPositionX);
        }
        verticalTargetPosition = positionCamera;
        transform.position = Vector3.MoveTowards(transform.position, verticalTargetPosition, MovidCameraSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards( transform.rotation, vetricalQuaternion, RotateCameraSpeed * Time.deltaTime);
    }

    public void MoveHole()
    {
        positionCamera.y = player.transform.position.y + 0.55f;
        maxPositionX = 0.9f;
        StartCoroutine(TimeMoveHole());
    }

    IEnumerator TimeMoveHole()
    {
        yield return new WaitForSeconds(1f);
        maxPositionX = 0.7f;
        positionCamera.y = 0.54f;
    }

    public void ToGame()
    {
        vetricalQuaternion = Quaternion.Euler(6.3f,0,0);
        positionCamera = new Vector3(0, 0.54f, 1.2f);
        RotateCameraSpeed = 100;
    }
}
