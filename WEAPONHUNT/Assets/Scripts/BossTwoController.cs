using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class BossTwoController : EnemyController
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
        return GameController.BossTwoPoints;
    }

    public const float SPEED_CONSTANT = 1f;

    public const float TIME_ATTACK_1 = 1f;
    public const float TIME_ATTACK_2 = 1f;

    public const string BOSS_ONE_IDLE = "Idle";
    public const string BOSS_ONE_IDLE_L = "Idle_l";

    public const string BOSS_ONE_MOVE = "Walk";
    public const string BOSS_ONE_MOVE_L = "Walk_l";

    public const string BOSS_ONE_ATTACK = "Attack";
    public const string BOSS_ONE_ATTACK_L = "Attack_l";

    public const string BOSS_ONE_FALL = "Defeated";
    public const string BOSS_ONE_FALL_L = "Defeated_l";

    public override void Attack1Command()
    {
        if (EnemyState != EnemyAction.Cooldown)
        {
            command = EnemyCommands.Attack1;
            MoveTransform(FacingRight ? Vector2.right : Vector2.left, 20);
        }        
    }

    public override void Attack2Command()
    {
        if (EnemyState != EnemyAction.Cooldown)
        {
            command = EnemyCommands.Attack2;
        }
    }
}
