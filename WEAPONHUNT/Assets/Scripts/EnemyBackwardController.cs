using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBackwardController : MonoBehaviour {

	void Start () {
		
	}
	
	void FixedUpdate () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        KeepDistance(other, true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        KeepDistance(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        KeepDistance(other, false);
    }

    private void KeepDistance(Collider2D other, bool entered)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject obj = transform.parent.gameObject;
            EnemyController objController = obj.GetComponent<EnemyController>();
            if (entered)
            {
                objController.MoveBackCommand();
            } else
            {
                objController.IdleCommand();
            }
        }
    }
}
