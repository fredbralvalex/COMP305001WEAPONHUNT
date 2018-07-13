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
}
