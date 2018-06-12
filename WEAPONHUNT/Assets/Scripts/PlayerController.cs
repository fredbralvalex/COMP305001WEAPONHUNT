using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: HittableController
{

    public enum PlayerAction { Move, Idle, Jump, Punch, Kick };
    PlayerAction playerState = PlayerAction.Idle;
    private double time;
    float maxJumpHigh;

    public bool facingRight = true;
    bool stateMovement = true;
    public int playerJumpPower = 1250;
    public float moveX;
    Animator animator;
    SpriteRenderer sprite;

    void Start() {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        PerformeJumpFall();
        time += Time.deltaTime;
        if (ValidateTimeToWait())
        {
            return;
        }        
        Move();
        Attack();
        jump();

    }

    bool ValidateTimeToWait()
    {
        bool wait = false;
        if (playerState == PlayerAction.Punch)
        {
            wait = time <= GameController.TIME_PUNCH;
        } else if (playerState == PlayerAction.Jump) {
            wait = time <= GameController.TIME_JUMP;
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
            animation.Play(GameController.RUN_L);
            MoveTransform(Vector2.left);
        }
        else if (Input.GetKeyDown(GameController.RIGHT) || moveHorizontal > 0)
        {
            playerState = PlayerAction.Move;
            //move right
            facingRight = true;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play(GameController.RUN);
            MoveTransform(Vector2.right);
        }
        else
        {
            playerState = PlayerAction.Idle;
            Animator animation = animator.GetComponent<Animator>();
            if (facingRight)
            {
                animation.Play(GameController.IDLE);
            }
            else
            {
                animation.Play(GameController.IDLE_L);
            }
        }
    }

    private void Attack()
    {        
        if (Input.GetKeyDown(GameController.ATTACK_1))
        {
            playerState = PlayerAction.Punch;
            Animator animation = animator.GetComponent<Animator>();
            HitController[] hcontrollers = gameObject.GetComponentsInChildren<HitController>();
            HitController hcontrollerL = null;
            HitController hcontrollerR = null;

            foreach (HitController hcontroller in hcontrollers)
            {
                if (hcontroller.gameObject.tag == "hitRight")
                {
                    hcontrollerR = hcontroller;
                }
                else if (hcontroller.gameObject.tag == "hitLeft")
                {
                    hcontrollerL = hcontroller;
                }
            }
            if (facingRight)
            {
                animation.Play(GameController.PUNCH);

                if (hcontrollerR != null)
                {
                    hcontrollerR.GetActionHit();
                }
            } else
            {
                animation.Play(GameController.PUNCH_L);
                if (hcontrollerR != null)
                {
                    hcontrollerL.GetActionHit();
                }
            }
            if (CanHit && AimHit != null)
            {
                HittableController hController = AimHit.GetComponent<HittableController>();
                if (hController != null)
                {                                        
                    hController.GettingHit(GameController.ATTACK_POWER_1);
                }
            }
        }
        else if (Input.GetKeyDown(GameController.ATTACK_2))
        {
            playerState = PlayerAction.Kick;
            //TODO
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

    private void jump()
    {
        if (Input.GetKeyDown(GameController.JUMP))
        {
            //Jump
            playerState = PlayerAction.Jump;
            Animator animation = animator.GetComponent<Animator>();
            if (facingRight)
            {
                animation.Play(GameController.JUMP_AN);
            }
            else
            {
                animation.Play(GameController.JUMP_AN_L);
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
                nextPositionVertical = Vector2.up * GameController.SPEED_JUMP_CONSTANT * Time.deltaTime;
                //print("Up");
            }
            else
            {
                //print("Down");
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
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Ground")
        {
            CalcMaxJumpHigh();
        }
    }

    private void CalcMaxJumpHigh()
    {
        float varRun = 1;
        float varSlide = 1;
        maxJumpHigh = transform.localPosition.y + sprite.bounds.size.y * varRun * varSlide;
       
    }

    public override bool IsHitting()
    {
        return playerState == PlayerAction.Punch || playerState == PlayerAction.Kick;
    }

    public override void GettingHit(float power)
    {
        print(gameObject.tag + " is getting Hit : " + power);   
    }
}
