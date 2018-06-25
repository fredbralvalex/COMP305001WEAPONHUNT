using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {

    public GameObject hit;
    public bool Destroyed { get; set; }

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
        HitReacheable(other, true);
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
        if (canHit)
        {
            HittableController oController = other.gameObject.GetComponent<HittableController>();
            if (oController == null)
            {
                canHit = false;
            } else
            {
                //print("Can Hit: " + obj.gameObject.tag + " :: " + other.gameObject.tag + " > " + canHit);
                //objController.Hit = GetActionHit();
            }
        }
        //canHit = true;
        objController.CanHit = canHit;
        objController.AimHit = other.gameObject;
    }

    public GameObject GetActionHit()
    {
        GameObject clone;
        clone = Instantiate(hit, hit.transform.position, hit.transform.rotation) as GameObject;
        clone.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        return clone;
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
