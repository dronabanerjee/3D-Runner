﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * CameraScript class controls the camera in the game which keeps following the player and makes sure that 
 * the player occupies constant screen space. 
 */
public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Transform player;

    [SerializeField]
    float maxAngle = 7f;
    //7f is a good value

    private Vector3 offsetPosition;

    // Start is called before the first frame update
    void Start()
    {
        //distance between cam and player 
        offsetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.TransformPoint(offsetPosition);

        //Camera goes up unnecessarily if we don't put -2f
        var targetRotation = Quaternion.LookRotation(player.position-new Vector3(transform.position.x,transform.position.y-2f,transform.position.z));

        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,maxAngle);
    
    }
}
