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

    public GameObject player;
    private PlayerController playerController;
    private GameController gameController;
    private Vector3 offset;
    private float fixedY;

    // Use this for initialization
    void Start () {

        //player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
        //Debug.Log(offset);
        fixedY = player.transform.localPosition.y;

        playerController = player.GetComponent<PlayerController>();
        gameController = GetComponentInChildren<GameController>();
    }
	
	// Update is called once per frame
	void LateUpdate () {
        /*
        float x = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float y = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        gameObject.transform.position = new Vector3(x, y, gameObject.transform.position.z);
        */
        //transform.position = player.transform.position + offset;
        if (!gameController.FreezesCamera())
        {
            transform.localPosition = new Vector3(player.transform.position.x, fixedY, player.transform.localPosition.z) + offset;
        }
	}
}
