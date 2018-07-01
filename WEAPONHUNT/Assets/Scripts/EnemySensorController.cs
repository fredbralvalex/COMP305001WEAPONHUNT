using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensorController : MonoBehaviour {

	void Start () {
		
	}
	
	void FixedUpdate() {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTrigger(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTrigger(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        if (other.gameObject.tag == "Player")
        {
            eController.IdleCommand();
        }
    }

    private void OnTrigger(Collider2D other)
    {
        GameObject enemy = transform.parent.gameObject;
        EnemyController eController = enemy.GetComponent<EnemyController>();
        Vector2 ePos = transform.parent.transform.position;
        if (other.gameObject.tag == "Player")
        {
            Vector2 pPos = other.transform.position;
            //print("Facing Player : " + ePos.x + " <> "+ pPos.x);
            if (ePos.x < pPos.x)
            {
                eController.FaceRightCommand();
                //print("Enemy Facing Right! ");
            }
            else
            {
                //print("Enemy Facing Left! ");
                eController.FaceLeftCommand();
            }
        }
    }
}
