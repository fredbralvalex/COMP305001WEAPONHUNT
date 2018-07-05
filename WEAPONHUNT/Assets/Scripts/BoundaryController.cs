using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour {

	void Start () {
		
	}
	
	void FixedUpdate () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boundary")
        {
            IBoundaryElementController controller = other.GetComponentInParent<IBoundaryElementController>();
            controller.TouchesBoundaries();
            print("dont move");
        }
    }
}
