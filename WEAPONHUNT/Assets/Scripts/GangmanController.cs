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
        }


        protected override Image GetLifeBar()
        {
            return LifeBar;
        }

       

        protected override void PlayAttack()
        {
            Animator animation = animator.GetComponent<Animator>();
            if (FacingRight)
            {
                animation.Play(GANGMAN_PUNCH);
            }
            else
            {
                animation.Play(GANGMAN_PUNCH_L);
            }
        }

        protected override void PlayCoolDown()
        {
            PlayIdle();
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

        protected override void PlayMove()
        {
            Animator animation = animator.GetComponent<Animator>();
            if (FacingRight)
            {
                animation.Play(GANGMAN_RUN);
            }
            else
            {
                animation.Play(GANGMAN_RUN_L);
            }
        }

        protected override void PlayMoveBack()
        {
            Animator animation = animator.GetComponent<Animator>();
            if (FacingRight)
            {
                animation.Play(GANGMAN_RUN_L);
            }
            else
            {
                animation.Play(GANGMAN_RUN);
            }
        }

        protected override void PlayIdle()
        {
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

        protected override float GetPowerAttack()
        {
            if (EnemyState == EnemyAction.Attack1)
            {
                return GameController.POWER_ATTACK_1;
            }
            else
            {
                return GameController.POWER_ATTACK_2;
            }
        }

        protected override float GetTimeAttack()
        {
            if (EnemyState == EnemyAction.Attack1) {
                return TIME_ATTACK_1;
            }
            else
            {
                return TIME_ATTACK_2;
            }
        }

        protected override float GetSpeedMovement()
        {
            return SPEED_CONSTANT;
        }

        public const float SPEED_CONSTANT = 1.5f;

        public const float TIME_ATTACK_1 = 0.5f;
        public const float TIME_ATTACK_2 = 0.5f;

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
