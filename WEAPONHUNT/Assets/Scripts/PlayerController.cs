﻿using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: HittableController
{

    public enum PlayerAction { Move, Idle, Jump, Punch, Kick, Fall, Defeated, End, GetUp };
    public PlayerAction playerState = PlayerAction.Idle;
    private double time;
    float maxJumpHigh;

    public bool facingRight = true;
    bool stateMovement = true;
    public int playerJumpPower = 1250;
    public float moveX;
    Animator animator;
    SpriteRenderer sprite;
    Rigidbody2D playerRB;
    GameController gameController;

    void Start() {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        gameController = gObj.GetComponent<GameController>();
    }
	
	void Update () {
        Blinking();
        //playerRB.mass = 1;
        //playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (playerState == PlayerAction.Jump)
        {

            PerformeJumpFall();
        }
        time += Time.deltaTime;
        if (ValidateTimeToWait())
        {
            return;
        }
        time = 0;

        if (playerState == PlayerAction.Defeated)
        {
            playerState = PlayerAction.End;
        }
        if (playerState == PlayerAction.GetUp && Hits == 0)
        {
            //New Chancd
            playerState = PlayerAction.Idle;
        }

        if (gameController.LifeAmount - Hits <= 0)
        {
            if (playerState == PlayerAction.End)
            {
                //gameObject.SetActive(false);
                //Destroy(gameObject);
                if (gameController.NewChancePlayer())
                {
                    playerState = PlayerAction.GetUp;
                    Hits = 0;
                    UpdateLifeBar();
                    PlayGetUp();
                }

            }
            else
            {
                playerState = PlayerAction.Defeated;
                PlayDefeated();
            }
        }
        else
        {
            Move();
            Attack();
            Jump();
        }
    }

    bool ValidateTimeToWait()
    {
        bool wait = false;
        if (playerState == PlayerAction.Punch && gameController.LifeAmount - Hits > 0)
        {
            wait = time <= GameController.TIME_PUNCH;
        } else if (playerState == PlayerAction.Kick && gameController.LifeAmount - Hits > 0)
        {
            wait = time <= GameController.TIME_KICK;
        } else if (playerState == PlayerAction.Jump && gameController.LifeAmount - Hits > 0) {
            wait = time <= GameController.TIME_JUMP;
        }
        else if (playerState == PlayerAction.Defeated)
        {
            wait = time <= GameController.TIME_DEFEATED;
        }
        else if (playerState == PlayerAction.GetUp)
        {
            wait = time <= GameController.TIME_GET_UP;
        }
        else
        {
            time = 0;
        }       

        return wait;
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(GameController.LEFT) || moveHorizontal < 0)
        {
            playerState = PlayerAction.Move;
            //move left
            facingRight = false;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play(RUN_L);
            MoveTransform(Vector2.left);
        }
        else if (Input.GetKeyDown(GameController.RIGHT) || moveHorizontal > 0)
        {
            playerState = PlayerAction.Move;
            //move right
            facingRight = true;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play(RUN);
            MoveTransform(Vector2.right);
        }
        else
        {
            playerState = PlayerAction.Idle;
            Animator animation = animator.GetComponent<Animator>();
            if (facingRight)
            {
                animation.Play(IDLE);
            }
            else
            {
                animation.Play(IDLE_L);
            }
        }
    }

    private void Attack()
    {        
        if (Input.GetKeyDown(GameController.ATTACK_1))
        {
            playerState = PlayerAction.Punch;
            Attack(true);
        }
        else if (Input.GetKeyDown(GameController.ATTACK_2))
        {
            playerState = PlayerAction.Kick;
            Attack(false);
        }        
    }

    private void Attack(bool isPunch)
    {
        Animator animation = animator.GetComponent<Animator>();
        HitController[] hcontrollers = gameObject.GetComponentsInChildren<HitController>();
        HitController hcontrollerL = null;
        HitController hcontrollerR = null;

        foreach (HitController hcontroller in hcontrollers)
        {

            if (isPunch)
            {
                if (hcontroller.gameObject.tag == "hitRightP")
                {
                    hcontrollerR = hcontroller;
                }
                else if (hcontroller.gameObject.tag == "hitLeftP")
                {
                    hcontrollerL = hcontroller;
                }
            }
            else
            {
                if (hcontroller.gameObject.tag == "hitRightK")
                {
                    hcontrollerR = hcontroller;
                }
                else if (hcontroller.gameObject.tag == "hitLeftK")
                {
                    hcontrollerL = hcontroller;
                }
            }
        }


        if (facingRight)
        {
            if (isPunch)
            {
                animation.Play(PUNCH);
            } else
            {
                animation.Play(KICK);
            }
        }
        else
        {
            if (isPunch)
            {
                animation.Play(PUNCH_L);
            }
            else
            {
                animation.Play(KICK_L);
            }
        }

        if (CanHit && AimHit != null)
        {
            HittableController hController = AimHit.GetComponent<HittableController>();
            if (hController != null)
            {
                if (facingRight)
                {
                    if (hcontrollerR != null)
                    {
                        hcontrollerR.GetActionHit();
                    }
                }
                else
                {
                    if (hcontrollerL != null)
                    {
                        hcontrollerL.GetActionHit();
                    }
                }

                if (isPunch)
                {
                    hController.GettingHit(GameController.ATTACK_PUNCH);
                }
                else
                {
                    hController.GettingHit(GameController.ATTACK_KICK);
                }
            }
        }
    }

    private void MoveTransform(Vector2 direction)
    {
        float var = 1;

        Vector2 nextPosition = direction * var * GameController.SPEED_CONSTANT * Time.deltaTime;
        if (stateMovement)
        {
            transform.localPosition += (Vector3)nextPosition;
        }
        else
        {
            transform.localPosition -= (Vector3)nextPosition;
            stateMovement = true;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(GameController.JUMP))
        {
            //Jump
            playerState = PlayerAction.Jump;
            Animator animation = animator.GetComponent<Animator>();
            if (facingRight)
            {
                animation.Play(JUMP_AN);
            }
            else
            {
                animation.Play(JUMP_AN_L);
            }            
        }        
    }

    private void PerformeJumpFall()
    {
        float varRun = 1;

        Vector2 nextPositionHorizontal;
        if (Input.GetKey(GameController.LEFT))
        {
            facingRight = false;
            nextPositionHorizontal = Vector2.left * GameController.SPEED_CONSTANT * varRun * Time.deltaTime;
        }
        else if (Input.GetKey(GameController.RIGHT))
        {
            facingRight = true;
            nextPositionHorizontal = Vector2.right * GameController.SPEED_CONSTANT * varRun * Time.deltaTime;
        }
        else
        {
            nextPositionHorizontal = Vector2.zero;
        }

        Vector2 nextPositionVertical = Vector2.zero;

        if (playerState == PlayerAction.Jump)
        {
            if (maxJumpHigh >= sprite.transform.localPosition.y)
            {
                //going high       
                //state = Movement.Jump;
                nextPositionVertical = Vector2.up * GameController.SPEED_JUMP_CONSTANT * Time.deltaTime * 1.2f;
                //print("Up");
            }
            else
            {
                print("Down");
                //Falling
            }
        }        

        transform.localPosition += (Vector3)nextPositionVertical;
        if (stateMovement)
        {
            transform.localPosition += (Vector3)nextPositionHorizontal;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //print(other.gameObject.tag);
        if (other.gameObject.tag == "Ground")
        {
            CalcMaxJumpHigh();
            playerState= PlayerAction.Idle;
        }
        else if (other.gameObject.tag == "Gangman")
        {
            Rigidbody2D rigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            //rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            //playerRB.mass = 100;
        }
        else if (other.gameObject.tag == "Blood")
        {
            Hits--;
            UpdateLifeBar();
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }

    private void CalcMaxJumpHigh()
    {
        float varRun = 2;
        float varSlide = 1;
        maxJumpHigh = transform.localPosition.y + sprite.bounds.size.y * varRun * varSlide;
       
    }

    public override bool IsHitting()
    {
        return playerState == PlayerAction.Punch || playerState == PlayerAction.Kick;
    }

    public override void GettingHit(float power)
    {
        GameObject  gObj = GameObject.FindGameObjectWithTag("GameBar");
        GameController gController = gObj.GetComponent<GameController>();
        gController.GangmanScoreN++;
        Hits++;
        //print(gameObject.tag + " is getting Hit : " + power);
        Blink = true;
        UpdateLifeBar();
        PushedBack(power);
    }

    private void PushedBack(float power)
    {
        Vector2 direction;
        if(facingRight)
        {
            direction = Vector2.left;
        } else
        {
            direction = Vector2.right;

        }

        Vector2 nextPosition = direction * power/10;
        transform.localPosition += (Vector3)nextPosition;
    }

    private void UpdateLifeBar()
    {
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        GameController gController = gObj.GetComponent<GameController>();

        if ((float)Hits / gController.LifeAmount <= 1)
        {
            gController.LifeBar.fillAmount = 1.0f - (float)Hits / gController.LifeAmount;
            if (gController.LifeBar.fillAmount > 0.75)
            {
                gController.LifeBar.color = Color.green;
            }
            else if (gController.LifeBar.fillAmount > 0.25 && gController.LifeBar.fillAmount <= 0.75)
            {
                gController.LifeBar.color = Color.yellow;
            }
            else if (gController.LifeBar.fillAmount > 0 && gController.LifeBar.fillAmount <= 0.25)
            {
                gController.LifeBar.color = Color.red;
            }
        }
    }

    protected override SpriteRenderer GetSprite()
    {
        return sprite;
    }

    private void PlayDefeated()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (facingRight)
        {
            animation.Play(CHAR_FALL);
        }
        else
        {
            animation.Play(CHAR_FALL_L);
        }
    }

    private void PlayGetUp()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (facingRight)
        {
            animation.Play(CHAR_GET_UP);
        }
        else
        {
            animation.Play(CHAR_GET_UP_L);
        }
    }

    public const string IDLE = "CharIdle";
    public const string IDLE_L = "CharIdle_l";

    public const string RUN = "CharRun";
    public const string RUN_L = "CharRun_l";

    public const string JUMP_AN = "CharJump";
    public const string JUMP_AN_L = "CharJump_l";

    public const string PUNCH = "CharPunch";
    public const string PUNCH_L = "CharPunch_l";

    public const string KICK = "CharKick";
    public const string KICK_L = "CharKick_l";

    public const string CHAR_FALL = "CharDefeated";
    public const string CHAR_FALL_L = "CharDefeated_l";

    public const string CHAR_GET_UP = "CharGetUp";
    public const string CHAR_GET_UP_L = "CharGetUp_l";
}
