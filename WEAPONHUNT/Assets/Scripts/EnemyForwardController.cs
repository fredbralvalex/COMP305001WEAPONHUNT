using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyForwardController : MonoBehaviour {

    public bool enable = true;

    void Start()
    {

    }

    void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        MoveForward(other, true);
    }
    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        MoveForward(other, true);
    }*/

    private void OnTriggerExit2D(Collider2D other)
    {
         MoveForward(other, false);
    }

    private void MoveForward(Collider2D other, bool entered)
    {
        if (other.gameObject.tag == "Player" && enable)
        {
            GameObject obj = transform.parent.gameObject;
            EnemyController objController = obj.GetComponent<EnemyController>();
            if (entered)// && (controller!= null && !controller.CanHitPlayer)
            {
                objController.MoveCommand();
            } else
            {
                objController.IdleCommand();
            }
        }
    }
}
