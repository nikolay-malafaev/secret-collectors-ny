using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    private Vector3 positionCamera;
    private float positionX;
    private float positionY;
    private float maxPositionX;
    [SerializeField] private float MovidCameraSpeed;


    void Start()
    {
        positionY = 0.54f;
        maxPositionX = 0.7f;
    }


    void Update()
    {
        positionX = player.transform.position.x;
        //positionY = player.transform.position.y;
        //positionY = player.transform.position.y;
        positionX = Mathf.Clamp(positionX, -maxPositionX, maxPositionX);

        positionCamera = new Vector3(positionX, positionY, 1.2f);
        transform.position = Vector3.MoveTowards(transform.position, positionCamera, MovidCameraSpeed * Time.deltaTime);
    }

    public void MoveHole()
    {
        positionY = player.transform.position.y + 0.55f;
        maxPositionX = 0.9f;
        StartCoroutine(TimeMoveHole());
    }

    IEnumerator TimeMoveHole()
    {
        yield return new WaitForSeconds(1f);
        maxPositionX = 0.7f;
        positionY = 0.54f;
    }

}
