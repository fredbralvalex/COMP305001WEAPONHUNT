using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class GangmanController : EnemyController
    {
        public enum GangmanAction { Move, Idle, Jump, Punch, Kick, Cooldown};
        public enum GangmanCommands { Move, Idle, Punch, Kick };
        GangmanAction GangmanState = GangmanAction.Idle;
        private double time;
        float maxJumpHigh;
        GangmanCommands command;

        public bool facingRight = true;
        bool stateMovement = true;
        public int jumpPower = 1250;
        public float moveX;
        Animator animator;
        SpriteRenderer sprite;

        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            sprite = gameObject.GetComponent<SpriteRenderer>();
            command = GangmanCommands.Idle;            
        }

        void Update()
        {            
            time += Time.deltaTime;
            if (ValidateTimeToWait())
            {
                return;
            }
                //CanHitPlayer();

            Move();
            //Attack();
        }

        bool ValidateTimeToWait()
        {
            bool wait = false;
            if (GangmanState == GangmanAction.Punch)
            {
                wait = time <= GameController.TIME_PUNCH;
                GangmanState = GangmanAction.Cooldown;
                if (!wait)
                {
                }
            } else if (GangmanState == GangmanAction.Cooldown)
            {
                wait = time <= GameController.COOL_DOWN_TIME_PUNCH;
                if (!wait)
                {
                    GangmanState = GangmanAction.Idle;
                }
            }
            else
            {
                time = 0;
            }
            return wait;

        }

        private void Move()
        {            
            if (command == GangmanCommands.Move)
            {
                //GangmanState = GangmanAction.Move;
                if (facingRight)
                {
                    //move right
                    Animator animation = animator.GetComponent<Animator>();
                    animation.Play(GameController.GANGMAN_RUN);
                    MoveTransform(Vector2.right);
                }
                else
                {
                    //move left
                    Animator animation = animator.GetComponent<Animator>();
                    animation.Play(GameController.GANGMAN_RUN_L);
                    MoveTransform(Vector2.left);
                }
            }
            else if (command == GangmanCommands.Idle 
                || GangmanState == GangmanAction.Idle 
                || GangmanState == GangmanAction.Cooldown)
            {
                //GangmanState = GangmanAction.Idle;
                Animator animation = animator.GetComponent<Animator>();
                if (facingRight)
                {
                    animation.Play(GameController.GANGMAN_IDLE);
                }
                else
                {
                    animation.Play(GameController.GANGMAN_IDLE_L);
                }
            }
        }

        private void Attack()
        {
            if (command == GangmanCommands.Punch)
            {
                GangmanState = GangmanAction.Punch;
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
                    animation.Play(GameController.GANGMAN_PUNCH);
                    if (hcontrollerR != null)
                    {
                        hcontrollerR.GetActionHit();
                    }
                }
                else
                {
                    animation.Play(GameController.GANGMAN_PUNCH_L);
                    if (hcontrollerL != null)
                    {
                        hcontrollerL.GetActionHit();
                    }
                }

                if (CanHit && AimHit!= null)
                {
                    HittableController hController = AimHit.GetComponent<HittableController>();
                    if (hController != null)
                    {
                        hController.GettingHit(GameController.ATTACK_POWER_2);
                    }
                }
                command = GangmanCommands.Idle;

            }
            else if (command == GangmanCommands.Kick)
            {
                GangmanState = GangmanAction.Kick;
                //TODO
                command = GangmanCommands.Idle;
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

        void OnCollisionEnter2D(Collision2D other)
        {
            //print(other.gameObject.tag);
            if (other.gameObject.tag == "Ground")
            {
                CalcMaxJumpHigh();
            } else if (other.gameObject.tag == "Player")
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

        public override void FaceLeftCommand()
        {
            facingRight = false;
            //print("Enemy Faced Left! ");
        }

        public override void FaceRightCommand()
        {
            //print("Enemy Faced Right! ");
            facingRight = true;
        }

        public override void IdleCommand()
        {
            command = GangmanCommands.Idle;
        }

        public override void MoveCommand()
        {
            command = GangmanCommands.Move;
        }

        public override void PunchCommand()
        {
            command = GangmanCommands.Punch;
        }

        public override bool IsHitting()
        {
            return GangmanState == GangmanAction.Punch || GangmanState == GangmanAction.Kick;
        }

        public override void GettingHit(float power)
        {
            print(gameObject.tag + " is getting Hit : " + power);
        }

        private bool CanHitPlayer()
        {
            return AimHit != null && AimHit.tag == "Player" && CanHit;
        }
    }
}
