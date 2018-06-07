using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour {

    public int playerSpeed = 10;
    public bool facingRight = true;
    public int playerJumpPower = 1250;
    public float moveX;


	
	// Update is called once per frame
	void Update () {
        playerMove();
		
	}

    private void playerMove()
    {
        //Controls
        moveX = Input.GetAxis("Horizontal");

        //player Directions

        if(moveX < 0.0f && facingRight == true)
        {
            FlipPlayer();
        } else if (moveX > 0.0f && facingRight == false)
        {
            FlipPlayer();
        }

        //Physics
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

    }

    private void FlipPlayer()
    {
        
    }
}
