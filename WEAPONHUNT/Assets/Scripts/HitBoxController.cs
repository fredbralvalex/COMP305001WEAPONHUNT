using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour {
    private double time = 0;
    private double timeCoolDown = 2;
    public bool Cooldown = false;
    public bool CanHitPlayer = false;

    public bool enable = true;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (Cooldown)
        {
            time += Time.deltaTime;
            if (time < timeCoolDown)
            {
                return;
            }
            time = 0;
            Cooldown = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enable)
        {
            CommandAttack(other);

        }
        CanHitPlayer = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (enable)
        {
            CommandAttack(other);

        }
        CanHitPlayer = true;

    }

    private void CommandAttack(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            PlayerController pController = other.gameObject.GetComponent<PlayerController>();
            if (!Cooldown
                && pController.playerState != PlayerController.PlayerAction.Defeated
                && pController.playerState != PlayerController.PlayerAction.End)
            {
                //Use random to determine the attack command
                eController.Attack1Command();
                Cooldown = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (enable)
        {
            GameObject enemy = transform.parent.gameObject;
            EnemyController eController = enemy.GetComponent<EnemyController>();
            if (other.gameObject.tag == "Player")
            {
                //eController.MoveCommand();
                Cooldown = false;
            }
        }
        CanHitPlayer = false;
    }
}
