using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangmanHitBoxController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            eController.PunchCommand();
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            eController.PunchCommand();

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            eController.MoveCommand();
        }
    }
}
