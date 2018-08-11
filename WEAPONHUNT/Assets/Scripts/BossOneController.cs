using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class BossOneController : EnemyController
{
    public Image LifeBar;
    SpriteRenderer sprite;

    Animator animator;
    Rigidbody2D enemyRB;

    void Start () {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        enemyRB = gameObject.GetComponent<Rigidbody2D>();
    }

    protected override Image GetLifeBar()
    {
        return LifeBar;
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
            animation.Play(BOSS_ONE_FALL);
        }
        else
        {
            animation.Play(BOSS_ONE_FALL_L);
        }
    }

    protected override void PlayMove()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (FacingRight)
        {
            animation.Play(BOSS_ONE_MOVE);
        }
        else
        {
            animation.Play(BOSS_ONE_MOVE_L);
        }
    }

    protected override void PlayMoveBack()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (FacingRight)
        {
            animation.Play(BOSS_ONE_MOVE);
        }
        else
        {
            animation.Play(BOSS_ONE_MOVE_L);
        }
    }

    protected override void PlayIdle()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (FacingRight)
        {
            animation.Play(BOSS_ONE_IDLE);
        }
        else
        {
            animation.Play(BOSS_ONE_IDLE_L);
        }
    }

    protected override void PlayAttack()
    {
        GameSaveStateController.GetInstance().GeneratePlayWeaponMoveAudio();
        Animator animation = animator.GetComponent<Animator>();
        if (FacingRight)
        {
            animation.Play(BOSS_ONE_ATTACK);
        }
        else
        {
            animation.Play(BOSS_ONE_ATTACK_L);
        }
    }

    protected override void PlayCoolDown()
    {
        PlayIdle();
    }

    protected override float GetTimeAttack()
    {
        if (EnemyState == EnemyAction.Attack1)
        {
            return TIME_ATTACK_1;
        }
        else
        {
            return TIME_ATTACK_2;
        }
    }

    protected override float GetPowerAttack()
    {
        if (EnemyState == EnemyAction.Attack1)
        {
            return GameController.POWER_ATTACK_3;
        }
        else
        {
            return GameController.POWER_ATTACK_3;
        }
    }

    protected override float GetSpeedMovement()
    {
        return SPEED_CONSTANT;
    }

    protected override int GetHitPoints()
    {
        return GameController.HitBossPoints;
    }

    public override int GetDefeatPoints()
    {
        return GameController.BossOnePoints;
    }

    public const float SPEED_CONSTANT = 1f;

    public const float TIME_ATTACK_1 = 0.4f;
    public const float TIME_ATTACK_2 = 0.4f;

    public const string BOSS_ONE_IDLE = "B1Idle";
    public const string BOSS_ONE_IDLE_L = "B1Idle_l";

    public const string BOSS_ONE_MOVE = "b1Walk";
    public const string BOSS_ONE_MOVE_L = "b1Walk_l";

    public const string BOSS_ONE_ATTACK = "B1Attack";
    public const string BOSS_ONE_ATTACK_L = "B1Attack_l";

    public const string BOSS_ONE_FALL = "b1Defeated";
    public const string BOSS_ONE_FALL_L = "b1Defeated_l";
}
