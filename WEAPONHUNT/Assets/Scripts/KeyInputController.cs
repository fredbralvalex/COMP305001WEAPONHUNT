using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputController : MonoBehaviour {

    public KeyCode KeyCommand = KeyCode.Alpha0;

	void FixedUpdate () {

        if (Input.GetKeyDown(GameController.LEFT))
        {
            KeyCommand = GameController.LEFT;
        }
        else if (Input.GetKeyDown(GameController.RIGHT))
        {
            KeyCommand = GameController.RIGHT;
        }
        else if (Input.GetKeyDown(GameController.JUMP))
        {
            KeyCommand = GameController.JUMP;
        }
        else if (Input.GetKeyDown(GameController.ATTACK_1))
        {
            KeyCommand = GameController.ATTACK_1;
        }
        else if (Input.GetKeyDown(GameController.ATTACK_2))
        {
            KeyCommand = GameController.ATTACK_2;
        }
        else if (Input.GetKeyUp(GameController.LEFT))
        {
            KeyCommand = KeyCode.Alpha0;
        }
        else if (Input.GetKeyUp(GameController.RIGHT))
        {
            KeyCommand = KeyCode.Alpha0;
        }
        else if (Input.GetKeyUp(GameController.JUMP))
        {
            KeyCommand = KeyCode.Alpha0;
        }
        else if (Input.GetKeyUp(GameController.ATTACK_1))
        {
            KeyCommand = KeyCode.Alpha0;
        }
        else if (Input.GetKeyUp(GameController.ATTACK_2))
        {
            KeyCommand = KeyCode.Alpha0;
        }
        else
        {
            KeyCommand = KeyCode.Alpha0;
        }

    }
}
