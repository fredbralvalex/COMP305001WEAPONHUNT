using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class GangmanController : EnemyController
    {
        public Image LifeBar;
        SpriteRenderer sprite;

        
        Animator animator;
        Rigidbody2D enemyRB;

        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
            sprite = gameObject.GetComponent<SpriteRenderer>();
            enemyRB = gameObject.GetComponent<Rigidbody2D>();
            command = EnemyCommands.Idle;            
        }


        protected override Image GetLifeBar()
        {
            return LifeBar;
        }

        protected override void Action()
        {            
            if (command == EnemyCommands.Move && !CanHit)
            {
                //GangmanState = GangmanAction.Move;
                if (FacingRight)
                {
                    //move right
                    Animator animation = animator.GetComponent<Animator>();
                    animation.Play(GANGMAN_RUN);
                    MoveTransform(Vector2.right);
                }
                else
                {
                    //move left
                    Animator animation = animator.GetComponent<Animator>();
                    animation.Play(GANGMAN_RUN_L);
                    MoveTransform(Vector2.left);
                }
            }
            else if (command == EnemyCommands.Idle 
                || EnemyState == EnemyAction.Idle 
                || EnemyState == EnemyAction.Cooldown)
            {
                //GangmanState = GangmanAction.Idle;
                Animator animation = animator.GetComponent<Animator>();
                if (FacingRight)
                {
                    animation.Play(GANGMAN_IDLE);
                }
                else
                {
                    animation.Play(GANGMAN_IDLE_L);
                }
            }
        }

        protected override void Attack()
        {
            if (command == EnemyCommands.Attack1)
            {
                EnemyState = EnemyAction.Attack1;
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
                if (FacingRight)
                {
                    animation.Play(GANGMAN_PUNCH);
                }
                else
                {
                    animation.Play(GANGMAN_PUNCH_L);
                }

                if (CanHit && AimHit!= null)
                {
                    HittableController hController = AimHit.GetComponent<HittableController>();
                    if (hController != null)
                    {
                        if (FacingRight)
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
                        hController.GettingHit(GameController.ATTACK_KICK);
                    }
                }
                command = EnemyCommands.Idle;

            }
            else if (command == EnemyCommands.Attack2)
            {
                EnemyState = EnemyAction.Attack2;
                //TODO
                command = EnemyCommands.Idle;
            }
        }

        private void MoveTransform(Vector2 direction)
        {
            float var = 1;

            Vector2 nextPosition = direction * var * GameController.SPEED_CONSTANT * Time.deltaTime;
            if (StateMovement)
            {
                //print("state movement: " + transform.localPosition + ":: " + transform.localPosition + (Vector3)nextPosition);
                transform.localPosition += (Vector3)nextPosition;
            }
            else
            {
                //print("NOT state movement");
                transform.localPosition -= (Vector3)nextPosition;
                //gameObject.SetActive(false);
                StateMovement = true;
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            //print(other.gameObject.tag);
            if (other.gameObject.tag == "Ground")
            {
            } else if (other.gameObject.tag == "Player")
            {
                command = EnemyCommands.Idle;
            }
        }

        protected override SpriteRenderer GetSprite()
        {
            return sprite;
        }

        protected override void PlayDefeated()
        {
            Animator animation = animator.GetComponent<Animator>();
            if (FacingRight)
            {
                animation.Play(GANGMAN_FALL);
            } else
            {
                animation.Play(GANGMAN_FALL_L);
            }
        }

        public const string GANGMAN_IDLE = "GangmanIdle";
        public const string GANGMAN_IDLE_L = "GangmanIdle_l";

        public const string GANGMAN_RUN = "GangmanRun";
        public const string GANGMAN_RUN_L = "GangmanRun_l";

        public const string GANGMAN_JUMP_AN = "GangmanJump";
        public const string GANGMAN_JUMP_AN_L = "GangmanJump_l";

        public const string GANGMAN_PUNCH = "GangmanPunch";
        public const string GANGMAN_PUNCH_L = "GangmanPunch_l";

        public const string GANGMAN_FALL = "GangmanFall";
        public const string GANGMAN_FALL_L = "GangmanFall_l";
    }



}
