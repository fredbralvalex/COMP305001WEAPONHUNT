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
        public enum GangmanAction { Move, Idle, Jump, Punch, Kick, Cooldown, Defeated, End};
        public enum GangmanCommands { Move, Idle, Punch, Kick };
        private GangmanAction GangmanState = GangmanAction.Idle;
        private GangmanAction GangmanNextState = GangmanAction.Idle;
        private double time;       

        float maxJumpHigh;
        GangmanCommands command;

        public int Life = 5;
        public new int Hits;

        public Image LifeBar;

        public bool facingRight = true;
        bool stateMovement = true;
        Animator animator;
        SpriteRenderer sprite;
        Rigidbody2D enemyRB;

        void Start()
        {
            //LifeBar.type = Image.Type.Filled;
            //LifeBar.fillAmount = 0.5f;
            animator = gameObject.GetComponent<Animator>();
            sprite = gameObject.GetComponent<SpriteRenderer>();
            enemyRB = gameObject.GetComponent<Rigidbody2D>();
            command = GangmanCommands.Idle;            
        }

        void Update()
        {
            //enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            Blinking();
            time += Time.deltaTime;            

            if (ValidateTimeToWait())
            {
                return;
            }
            //CanHitPlayer();

            if (Life - Hits <= 0)
            {
                if (GangmanState == GangmanAction.End)
                {
                    GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
                    GameController gameController = gObj.GetComponent<GameController>();
                    gameController.EliminateGangMan(gameObject);
                } else
                {
                    GangmanState = GangmanAction.Defeated;
                    PlayDefeated();
                }
            }
            else
            {
                Action();
                Attack();
            }
        }

        bool ValidateTimeToWait()
        {
            bool wait = false;
            if (GangmanState == GangmanAction.Punch && Life - Hits > 0)
            {
                wait = time <= GameController.TIME_PUNCH;
                GangmanNextState = GangmanAction.Cooldown;
            }
            else if (GangmanState == GangmanAction.Cooldown && command != GangmanCommands.Move && Life - Hits > 0)
            {
                wait = time <= GameController.COOL_DOWN_TIME_PUNCH + GameController.TIME_PUNCH;
                GangmanNextState = GangmanAction.Idle;
                //print("waiting cooldown");
            } else if (GangmanState == GangmanAction.Defeated)
            {
                wait = time <= GameController.TIME_DEFEATED;
                GangmanNextState = GangmanAction.End;
            }

            //when the time to wait finishes
            if (!wait)
            {
                //print("not waiting: "+ GangmanNextState);
                time = 0;
                GangmanState = GangmanNextState;
                //print(GangmanState);
            }
            return wait;

        }

        private void Action()
        {            
            if (command == GangmanCommands.Move && !CanHit)
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
                }
                else
                {
                    animation.Play(GameController.GANGMAN_PUNCH_L);
                }

                if (CanHit && AimHit!= null)
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
                        hController.GettingHit(GameController.ATTACK_KICK);
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
                //print("state movement: " + transform.localPosition + ":: " + transform.localPosition + (Vector3)nextPosition);
                transform.localPosition += (Vector3)nextPosition;
            }
            else
            {
                //print("NOT state movement");
                transform.localPosition -= (Vector3)nextPosition;
                //gameObject.SetActive(false);
                stateMovement = true;
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            //print(other.gameObject.tag);
            if (other.gameObject.tag == "Ground")
            {
            } else if (other.gameObject.tag == "Player")
            {
                command = GangmanCommands.Idle;
            }
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
            if (GangmanState != GangmanAction.Cooldown)
            {
                command = GangmanCommands.Punch;
            }
        }

        public override bool IsHitting()
        {
            return GangmanState == GangmanAction.Punch || GangmanState == GangmanAction.Kick;
        }

        public override void GettingHit(float power)
        {
            GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
            GameController gController = gObj.GetComponent<GameController>();
            gController.PlayerScoreN++;
            Hits++;
            //print(gameObject.tag + " is getting Hit : " + power);
            Blink = true;

            if ((float)Hits / Life <= 1)
            {
                LifeBar.fillAmount = 1.0f - (float)Hits/Life;
                if (LifeBar.fillAmount > 0.75)
                {
                    LifeBar.color = Color.green;
                } else if (LifeBar.fillAmount > 0.25 && LifeBar.fillAmount <= 0.75)
                {
                    LifeBar.color = Color.yellow;
                }
                else if (LifeBar.fillAmount > 0 && LifeBar.fillAmount <= 0.25)
                {
                    LifeBar.color = Color.red;
                }
            }
            PushedBack(power);
        }


        private void PushedBack(float power)
        {
            Vector2 direction;
            if (facingRight)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }

            Vector2 nextPosition = direction * power * 0.10f;
            transform.localPosition += (Vector3)nextPosition;
        }

        private bool CanHitPlayer()
            {
                return AimHit != null && AimHit.tag == "Player" && CanHit;
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
                animation.Play(GameController.GANGMAN_FALL);
            } else
            {
                animation.Play(GameController.GANGMAN_FALL_L);
            }
        }
    }


}
