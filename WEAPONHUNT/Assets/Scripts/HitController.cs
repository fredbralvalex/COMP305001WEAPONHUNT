using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {

    public GameObject hit;
    public float power = 1f;
    public bool Destroyed { get; set; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        HittableController objController = other.GetComponent<HittableController>();
        if (objController != null)
        {
            //print("Hit Enter: " + other.gameObject.tag);
            //GameObject obj = transform.parent.gameObject;
            objController.Hit = GetActionHit(objController.HitPosition);
            objController.GettingHit(power);
        }
            //HitReacheable(other, true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //print("Hit Stay: " + other.gameObject.tag);
        //HitReacheable(other, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //print("Hit Exit: " + other.gameObject.tag);
        //HitReacheable(other, false);
    }
    /*
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
    }*/

    public GameObject GetActionHit(Transform hitPosition)
    {
        GameObject clone;
        clone = Instantiate(hit, hit.transform.position, hit.transform.rotation) as GameObject;
        clone.transform.position = new Vector3(hitPosition.position.x, hitPosition.position.y, hitPosition.position.z - 1);
        return clone;
    }
}
