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
        command = EnemyCommands.Idle;
    }

    public override bool IsHitting()
    {
        throw new System.NotImplementedException();
    }

    protected override Image GetLifeBar()
    {
        return LifeBar;
    }

    public override void GettingHit(float power)
    {
        throw new System.NotImplementedException();
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

    protected override void Action()
    {
        throw new System.NotImplementedException();
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public const string BOSS_ONE_IDLE = "BossOneIdle";
    public const string BOSS_ONE_IDLE_L = "BossOneIdle_l";

    public const string BOSS_ONE_MOVE = "BossOneMove";
    public const string BOSS_ONE_MOVE_L = "BossOneMove_l";

    public const string BOSS_ONE_ATTACK = "BossOneAttack";
    public const string BOSS_ONE_ATTACK_L = "BossOneAttack_l";

    public const string BOSS_ONE_FALL = "BossOneFall";
    public const string BOSS_ONE_FALL_L = "BossOneFall_l";
}
