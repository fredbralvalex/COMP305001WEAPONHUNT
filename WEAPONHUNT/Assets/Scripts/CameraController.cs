﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    private PlayerController playerController;
    public GameObject gameBarPrefab;
    private GameObject gameBarInstatiated;
    private GameController gameController;
    private Vector3 offset;
    private float fixedY;
    public int Level;

    public bool StopCamera { get; set; }
    float horizontalPosition = 0;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        gameController = GetComponentInChildren<GameController>();
        if (gameController == null)
        {
            gameBarInstatiated = Instantiate(gameBarPrefab, GetComponent<Camera>().transform);
            gameController = gameBarInstatiated.GetComponent<GameController>();
            if (Level != 0)
            {
                GameStateController.level = Level;

            }
            //gameBarInstatiated.transform.parent = GetComponent<Camera>().transform;
            gameController.transform.parent = gameBarInstatiated.transform;
        }
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 1.22f, transform.transform.localPosition.z);
        //gameBarInstatiated.transform.position = new Vector3(0, 2.55f, 9);
        //offset = transform.position - new Vector3(0,0,0);
        //UpdateOffSetPlayer();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                
                OffSetPlayer offsetdel = UpdateOffSetPlayer;
                PlayerController controller = player.GetComponent<PlayerController>();
                controller.offsetdel = offsetdel;
                //UpdateOffSetPlayer();
            }
        }
    }

    public delegate void OffSetPlayer();

    public void UpdateOffSetPlayer()
    {
        if (player != null)// && (playerController != null && !playerController.InPosition)
        {
            Vector3 playerPosition = new Vector3();
            playerPosition = player.transform.position;
            fixedY = player.transform.localPosition.y;// - 1.22f;

            playerController = player.GetComponent<PlayerController>();
            offset = transform.position - playerPosition;
        }        
        //Debug.Log(offset);

    }

    private Vector3 PositionDiff;
    // Update is called once per frame
    void LateUpdate () {

        if (player != null && playerController != null)
        {
            if (gameController.FreezesCamera() || player.transform.position.x < 0 || StopCamera)// && playerController.isMovingBack()
            {
                PositionDiff = transform.localPosition - player.transform.position;
            } else
            {
                if (player.transform.position.x + offset.x >= transform.localPosition.x)// gameController.GetEndPositionX()
                {
                }


                if (PositionDiff.x == offset.x || (PositionDiff.x < offset.x + 0.1f && PositionDiff.x > offset.x - 0.1f))
                {
                    horizontalPosition = player.transform.position.x;

                } else if(PositionDiff.x > offset.x)
                {
                    horizontalPosition = transform.position.x - 0.1f;
                } else if (PositionDiff.x < offset.x)
                {
                    horizontalPosition = transform.position.x + 0.1f;
                } else
                {
                    //
                }
                PositionDiff = transform.localPosition - player.transform.position;
                /*
                if (horizontalPosition >= player.transform.position.x)
                {
                    horizontalPosition = horizontalPosition - 0.1f;
                }
                else if (horizontalPosition < player.transform.position.x)
                {
                    horizontalPosition = horizontalPosition + 0.1f;

                }*/
                transform.localPosition = new Vector3(horizontalPosition, fixedY, player.transform.localPosition.z) + offset;                
            }
        }
	}
}
