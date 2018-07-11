using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    /*
    [SerializeField]
    private float xMax;

    [SerializeField]
    private float yMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMin;
    */

    private GameObject player;
    private PlayerController playerController;
    private GameController gameController;
    private Vector3 offset;
    private float fixedY;

    private Transform TmpTransform; 
    float horizontalPosition = 0;

    // Use this for initialization
    void Start () {
        TmpTransform = transform;
        gameController = GetComponentInChildren<GameController>();
        UpdateOffSetPlayer();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                UpdateOffSetPlayer();
            }
        }        
    }

    private void UpdateOffSetPlayer()
    {
        Vector3 playerPosition = new Vector3();
        if (player != null)
        {
            playerPosition = player.transform.position;
            fixedY = player.transform.localPosition.y;

            playerController = player.GetComponent<PlayerController>();
        }        
        offset = transform.position - playerPosition;
        //Debug.Log(offset);

    }

    // Update is called once per frame
    void LateUpdate () {

        if (!gameController.FreezesCamera() && player != null && !playerController.isMovingBack())
        {
            if (player.transform.position.x + offset.x >= transform.localPosition.x )
            {
                horizontalPosition = player.transform.position.x;
            }
            transform.localPosition = new Vector3(horizontalPosition, fixedY, player.transform.localPosition.z) + offset;

        }
        TmpTransform = transform;
	}
}
