using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    abstract class EnemyController : HittableController, IBoundaryElementController
    {
        public enum EnemyAction { Move, MoveBack, Idle, Jump, Attack1, Attack2, Cooldown, Defeated, End };
        public enum EnemyCommands { Move, MoveBack, Idle, Attack1, Attack2 };
        public EnemyAction EnemyState = EnemyAction.Idle;
        protected EnemyAction GangmanNextState = EnemyAction.Idle;
        protected bool FacingRight = true;
        protected EnemyCommands command = EnemyCommands.Idle;

        public int Life = 0;
        private double time;
        protected bool StateMovement = true;

        void FixedUpdate()
        {
            //enemyRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            Blinking();
            time += Time.deltaTime;

            if (ValidateTimeToWait())
            {
                return;
            }

            if (Life - Hits <= 0)
            {
                if (EnemyState == EnemyAction.End)
                {
                    GameObject gObject = GameObject.Find("GameBar");
                    GameController gameController = gObject.GetComponent<GameController>();
                    gameController.EliminateEnemy(gameObject);
                }
                else
                {
                    EnemyState = EnemyAction.Defeated;
                    PlayDefeated();
                }
            }
            else
            {
                Action();
                Attack();
            }
        }

        protected abstract void PlayDefeated();
        

        protected abstract void PlayAttack();
        protected abstract void PlayMove();
        protected abstract void PlayMoveBack();
        protected abstract void PlayIdle();
        protected abstract void PlayCoolDown();

        protected void Action()
        {
            if (command == EnemyCommands.Move && !CanHit)
            {
                //GangmanState = GangmanAction.Move;
                if (FacingRight)
                {
                    //move right
                    MoveTransform(Vector2.right);
                }
                else
                {
                    //move left
                    MoveTransform(Vector2.left);
                }
                PlayMove();
            } else if (command == EnemyCommands.MoveBack && !CanHit)
            {
                if (FacingRight)
                {
                    //move right
                    MoveBackTransform(Vector2.left);
                }
                else
                {
                    //move left
                    MoveBackTransform(Vector2.right);
                }
                PlayMoveBack();
            }
            else if (command == EnemyCommands.Idle
                || EnemyState == EnemyAction.Idle)
            {
                //GangmanState = GangmanAction.Idle;
                PlayIdle();
            } else if (EnemyState == EnemyAction.Cooldown)
            {
                PlayCoolDown();
            }
        }

        private void MoveTransform(Vector2 direction)
        {
            float var = 1;

            Vector2 nextPosition = direction * var * GetSpeedMovement() * Time.deltaTime;
            Move(nextPosition);
        }

        private void MoveBackTransform(Vector2 direction)
        {
            float var = 1;

            Vector2 nextPosition = direction * var * GetSpeedMovement() * Time.deltaTime;
            Move(nextPosition);            
        }

        private void Move(Vector3 nextPosition)
        {
            if (StateMovement)
            {
                transform.localPosition += (Vector3)nextPosition;
            }
            else
            {
                print("NOT state movement");
                transform.localPosition -= (Vector3)nextPosition;
                //gameObject.SetActive(false);
                StateMovement = true;
            }
        }

        protected void Attack()
        {
            if (command == EnemyCommands.Attack1)
            {
                EnemyState = EnemyAction.Attack1;
                Attack(GetPowerAttack());

            }
            else if (command == EnemyCommands.Attack2)
            {
                EnemyState = EnemyAction.Attack2;
                Attack(GetPowerAttack());
            }
        }

        protected void Attack(float power)
        {
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

            PlayAttack();

            if (CanHit && AimHit != null)
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
                    hController.GettingHit(power);
                }
            }
            command = EnemyCommands.Idle;
        }

        protected abstract float GetTimeAttack();
        protected abstract float GetPowerAttack();
        protected abstract float GetSpeedMovement();

        bool ValidateTimeToWait()
        {
            bool wait = false;
            if ((EnemyState == EnemyAction.Attack1 || EnemyState == EnemyAction.Attack2) && Life - Hits > 0)
            {
                wait = time <= GetTimeAttack();
                GangmanNextState = EnemyAction.Cooldown;
            }
            else if (EnemyState == EnemyAction.Cooldown && command != EnemyCommands.Move && Life - Hits > 0)
            {
                wait = time <= GameController.COOL_DOWN_TIME_ATTACK1 + GetTimeAttack();
                GangmanNextState = EnemyAction.Idle;
                //print("waiting cooldown");
            }
            else if (EnemyState == EnemyAction.Defeated)
            {
                wait = time <= GameController.TIME_DEFEATED;
                GangmanNextState = EnemyAction.End;
            }

            //when the time to wait finishes
            if (!wait)
            {
                //print("not waiting: "+ GangmanNextState);
                time = 0;
                EnemyState = GangmanNextState;
                //print(GangmanState);
            }
            return wait;

        }

        public void FaceLeftCommand()
        {
            FacingRight = false;
        }

        public void FaceRightCommand()
        {
            FacingRight = true;
        }

        public void IdleCommand()
        {
            command = EnemyCommands.Idle;
        }

        public void MoveCommand()
        {
            command = EnemyCommands.Move;
        }
        public void MoveBackCommand()
        {
            command = EnemyCommands.MoveBack;
        }

        public void Attack1Command()
        {
            if (EnemyState != EnemyAction.Cooldown)
            {
                command = EnemyCommands.Attack1;
            }
        }

        public void Attack2Command()
        {
            if (EnemyState != EnemyAction.Cooldown)
            {
                command = EnemyCommands.Attack2;
            }
        }

        public override bool IsHitting()
        {
            return EnemyState == EnemyAction.Attack1 || EnemyState == EnemyAction.Attack2;
        }

        protected abstract Image GetLifeBar();

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
                GetLifeBar().fillAmount = 1.0f - (float)Hits / Life;
                if (GetLifeBar().fillAmount > 0.75)
                {
                    GetLifeBar().color = Color.green;
                }
                else if (GetLifeBar().fillAmount > 0.25 && GetLifeBar().fillAmount <= 0.75)
                {
                    GetLifeBar().color = Color.yellow;
                }
                else if (GetLifeBar().fillAmount > 0 && GetLifeBar().fillAmount <= 0.25)
                {
                    GetLifeBar().color = Color.red;
                }
            }
            PushedBack(power);
        }

        private void PushedBack(float power)
        {
            Vector2 direction;
            if (FacingRight)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }

            Vector2 nextPosition = direction * (power*power*power) * 0.10f;
            Move(nextPosition);            
        }

        void IBoundaryElementController.TouchesBoundaries()
        {
            StateMovement  = false;
        }
    }
}
