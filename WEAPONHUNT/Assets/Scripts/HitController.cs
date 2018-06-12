using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        //print("Hit Enter: " + other.gameObject.tag);
        HitReacheable(other, true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //print("Hit Stay: " + other.gameObject.tag);
        //HitReacheable(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //print("Hit Exit: " + other.gameObject.tag);
        HitReacheable(other, false);
    }

    private void HitReacheable(Collider2D other, bool canHit)
    {
        GameObject obj = transform.parent.gameObject;
        HittableController objController = obj.GetComponent<HittableController>();
        HittableController oController = other.gameObject.GetComponent<HittableController>();
        if (oController == null)
        {
            canHit = false;
        } else
        {
            //print("Can Hit: " + obj.gameObject.tag + " :: " + other.gameObject.tag + " > " + canHit);
        }
        objController.CanHit = canHit;
        objController.AimHit = other.gameObject;
    }

    /*
    private void ActionHitting (Collider2D other)
    {
        GameObject obj = transform.parent.gameObject;
        HittableController objController = obj.GetComponent<HittableController>();
        print("Hit: " + obj.gameObject.tag + " :: " + other.gameObject.tag);
        if (objController.IsHitting())
        {
            HittableController oController = other.gameObject.GetComponent<HittableController>();
            if (oController != null)
            {
                if (obj.gameObject.tag == "Gangman" && other.gameObject.tag == "Player")
                {
                    oController.GettingHit(GameController.ATTACK_POWER_1);
                }
                else if (obj.gameObject.tag == "Player" && other.gameObject.tag == "Gangman")
                {
                    oController.GettingHit(GameController.ATTACK_POWER_2);
                }
            }
        }
    }
    */
}
