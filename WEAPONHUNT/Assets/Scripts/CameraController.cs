using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;
    private PlayerController playerController;
    public GameObject gameBarPrefab;
    private GameObject gameBarInstatiated;
    private GameController gameController;
    private Vector3 offset;
    private float fixedY;

    float horizontalPosition = 0;

    // Use this for initialization
    void Start () {
        gameBarInstatiated = Instantiate(gameBarPrefab, GetComponent<Camera>().transform);
        gameController = gameBarInstatiated.GetComponent<GameController>();
        //gameBarInstatiated.transform.parent = GetComponent<Camera>().transform;
        gameController.transform.parent = gameBarInstatiated.transform;
        //gameBarInstatiated.transform.position = new Vector3(0, 2.55f, 9);
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
            fixedY = player.transform.localPosition.y - 1.22f;

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
	}
}
