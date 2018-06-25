using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangmanHitBoxController : MonoBehaviour {

    private double time = 0;
    private double timeCoolDown = 2;
    private bool cooldown = false;

    void Start () {
		
	}
	
	void Update () {
        if (cooldown)
        {
            time += Time.deltaTime;
            if (time < timeCoolDown)
            {
                return;
            }
            time = 0;
            cooldown = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CommandPunch(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CommandPunch(other);
    }

    private void CommandPunch(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            PlayerController pController = other.gameObject.GetComponent<PlayerController>();
            if (!cooldown 
                && pController.playerState != PlayerController.PlayerAction.Defeated 
                && pController.playerState != PlayerController.PlayerAction.End)
            {
                eController.PunchCommand();
                cooldown = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            eController.MoveCommand();
            cooldown = false;
        }
    }
}
