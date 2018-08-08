using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneBlock : MonoBehaviour {

    public Rigidbody2D rb;

    [SerializeField]
    public float speed = 1f;
	void FixedUpdate () {

      
        rb.MovePosition(rb.position + Vector2.down * Time.fixedDeltaTime * speed);
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        //print(other.collider.gameObject.tag);
        if (other.collider.gameObject.tag == "Ground" || other.collider.gameObject.tag == "Water")
        {
            enabled = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //print("Trigger :: " + other.gameObject.tag);
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Water")
        {
            enabled = false;
            Destroy(gameObject);
        }
    }
}
