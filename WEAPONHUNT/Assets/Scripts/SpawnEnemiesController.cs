using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesController : MonoBehaviour {

    public Transform EnemyPosition;
    private GameController GameController;
    public bool KeepGenerating;
    private bool Generated;
    public bool IsBoss;

    private void FindGameBarInScene()
    {
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        if (gObj != null)
        {
            GameController = gObj.GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (GameController == null)
        {
            FindGameBarInScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!Generated)
            {
                if (IsBoss)
                {
                    GameController.GenerateBoss(EnemyPosition);
                } else
                {
                    GameController.GenerateGangMan(EnemyPosition);
                }
                Generated = true;
            }

            if (KeepGenerating)
            {
                Generated = false;
            }
        }
    }
}
