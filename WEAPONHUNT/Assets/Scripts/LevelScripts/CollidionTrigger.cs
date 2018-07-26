using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidionTrigger : MonoBehaviour {

    private GameObject player;
    private BoxCollider2D playerCollider;

    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private BoxCollider2D platformTrigger;
	

	void Start () {
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerCollider = player.GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(playerCollider, platformTrigger, true);
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
            if (player != null)
            {
                playerCollider = player.GetComponent<BoxCollider2D>();
                Physics2D.IgnoreCollision(playerCollider, platformTrigger, true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name=="Player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }
}
