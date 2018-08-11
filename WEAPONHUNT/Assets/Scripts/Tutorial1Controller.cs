using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial1Controller : MonoBehaviour {

    public Text Tutorial;
    public Text Intructions;
    public Text ButtonToPress;
    public Transform transformCenterPosition;
    public Transform transformFirePosition;
    public Transform transformCrate1Position;
    public Transform transformCrate2Position;
    public Transform CameraPosition;

    public GameObject fire;
    public GameObject Crate1;
    public GameObject Crate2;

    private GameObject fireClone;
    private GameObject Crate1Clone;
    private GameObject Crate2Clone;

    public CameraController cameraController;

    private string tutorialText = "Prince must run!";
    private string tutorialButton = "Press 'A' to go Left or 'D' to go Right!";

    private string tutorial1Text = "There are some hurdle in the way and Prince must Jump!";
    private string tutorial1Button = "Press 'J' to Jump!";

    private string tutorial2Text = "During the fight Prince can use his punches!";
    private string tutorial2Button = "Press 'K' to Punch!";

    private string tutorial3Text = "Prince can use his kicks as well!";
    private string tutorial3Button = "Press 'L' to Kick!";

    int tutorialNumber = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //print(Mathf.Round(CameraPosition.position.x * 10) +" :: " + Mathf.Round(transformFirePosition.position.x * 10));
        if (Mathf.Round(CameraPosition.position.x * 10) == Mathf.Round(transformCenterPosition.position.x * 10) && tutorialNumber == 0)
        {
            Intructions.text = tutorialText;
            ButtonToPress.text = tutorialButton;
            cameraController.StopCamera = true;
            tutorialNumber = 1;
        }
        else if (Mathf.Round(CameraPosition.position.x * 10)  == Mathf.Round(transformFirePosition.position.x * 10) && tutorialNumber == 1)
        {
            Intructions.text = tutorial1Text;
            ButtonToPress.text = tutorial1Button;
            cameraController.StopCamera = true;
            tutorialNumber = 2;
        } else if (Mathf.Round(CameraPosition.position.x * 10) == Mathf.Round(transformCrate1Position.position.x * 10) && tutorialNumber == 2)
        {
            Intructions.text = tutorial2Text;
            ButtonToPress.text = tutorial2Button;
            cameraController.StopCamera = true;
            tutorialNumber = 3;
        }
        else if (Mathf.Round(CameraPosition.position.x * 10) == Mathf.Round(transformCrate2Position.position.x * 10) && tutorialNumber == 3)
        {
            Intructions.text = tutorial3Text;
            ButtonToPress.text = tutorial3Button;
            cameraController.StopCamera = true;
            tutorialNumber = 4;
        }

        if ((Input.GetKeyDown(GameController.RIGHT) || Input.GetKeyDown(GameController.LEFT)) && tutorialNumber == 1)
        {
            cameraController.StopCamera = false;
            Intructions.text = "";
            ButtonToPress.text = "";
        }
        else if (Input.GetKeyDown(GameController.JUMP) && tutorialNumber == 2)
        {
            cameraController.StopCamera = false;
            Intructions.text = "";
            ButtonToPress.text = "";
        } else if (Input.GetKeyDown(GameController.ATTACK_1) && tutorialNumber == 3)
        {
            cameraController.StopCamera = false;
            Intructions.text = "";
            ButtonToPress.text = "";
        } else if (Input.GetKeyDown(GameController.ATTACK_2) && tutorialNumber == 4)
        {
            cameraController.StopCamera = false;
            Intructions.text = "";
            ButtonToPress.text = "";
            Tutorial.text = "";
        }
    }
}
