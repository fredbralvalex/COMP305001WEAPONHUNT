using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    abstract class EnemyController : HittableController
    {
        public enum EnemyAction { Move, Idle, Jump, Attack1, Attack2, Cooldown, Defeated, End };
        public enum EnemyCommands { Move, Idle, Attack1, Attack2 };
        protected EnemyAction EnemyState = EnemyAction.Idle;
        protected EnemyAction GangmanNextState = EnemyAction.Idle;
        protected bool FacingRight = true;
        protected EnemyCommands command;

        public int Life = 0;
        private double time;
        protected bool StateMovement = true;

        void Update()
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
        protected abstract void Action();
        protected abstract void Attack();


        bool ValidateTimeToWait()
        {
            bool wait = false;
            if (EnemyState == EnemyAction.Attack1 && Life - Hits > 0)
            {
                wait = time <= GameController.TIME_PUNCH;
                GangmanNextState = EnemyAction.Cooldown;
            }
            else if (EnemyState == EnemyAction.Cooldown && command != EnemyCommands.Move && Life - Hits > 0)
            {
                wait = time <= GameController.COOL_DOWN_TIME_ATTACK1 + GameController.TIME_PUNCH;
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
                command = EnemyCommands.Attack1;
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

            Vector2 nextPosition = direction * power * 0.10f;
            transform.localPosition += (Vector3)nextPosition;
        }

    }
}
