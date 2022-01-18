using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Player player;
    private Vector3 positionPlayer;
    private Vector3 positionCamera;
    private float speedPositionX;
    private float thisSpeed;
    private float positionX;
    [SerializeField] private float MovidCameraSpeed;
    void Start()
    {
        
    }

   
    void Update()
    {
        positionPlayer = player.transform.position;
        positionX = positionPlayer.x;
        positionX = Mathf.Clamp(positionX, -0.7f, 0.7f);

        positionCamera = new Vector3(positionX, 0.54f, 1.2f);

        transform.position = Vector3.MoveTowards(transform.position, positionCamera, MovidCameraSpeed * Time.deltaTime);
        //if(player.m_CurrentLane > 0)
    }
}
