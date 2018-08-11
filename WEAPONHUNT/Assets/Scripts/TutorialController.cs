using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {
    public Text Tutorial;
    public Text Intructions;
    public Text ButtonToPress;

    public BoxCollider2D collider;
    public BoxCollider2D colliderPreCondition;
    public BoxCollider2D colliderPosCondition;
    public int TimeTowait = 0;

    public Transform transformPosition;

    public GameObject Aim;
    public KeyCode [] keys;

    private GameObject Clone;

    //public CameraController cameraController;

    public string tutorialTitle = "Tutorial!";
    public string tutorialText = "Prince must run!";
    public string tutorialButton = "Press 'A' to go Left or 'D' to go Right!";
    bool tutorialDone =  false;
    bool tutorialStarted = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        bool any = false;
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                any = true;
            }
        }
        if (tutorialStarted && !tutorialDone && any)
        {
            
            Intructions.text = "";
            ButtonToPress.text = "";
            Tutorial.text = "";

            callBack.Invoke();
            tutorialDone = true;
            collider.enabled = false;
            if (colliderPosCondition != null)
            {
                //yield return new WaitForSeconds(3);                                
                colliderPosCondition.enabled = true;        
            } else
            {
                ReturnPlayerState(playerController);
            }
            //Destroy(gameObject);
        } 
    }

    IEnumerator WaitTimeSecond()
    {        
        yield return new WaitForSeconds(TimeTowait);
    }

    delegate void CallBack();
    CallBack callBack;
    PlayerController playerController;

    //delegate void ReturnState(PlayerController playerController);
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (colliderPreCondition==null || (colliderPreCondition != null && !colliderPreCondition.enabled))
        {
            if (other.gameObject.tag == "Player")
            {
                //StartCoroutine(WaitTimeSecond());
                tutorialStarted = true;
                playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.TutorialController = this;
                playerController.Dummy = true;
                playerController.playerDummyState = PlayerController.PlayerDummyAction.Tutorial;

                //ReturnState r = ReturnPlayerState;

                callBack = playerController.ActionsTutorial;
                Tutorial.text = tutorialTitle;
                Intructions.text = tutorialText;
                ButtonToPress.text = tutorialButton;
            }
        }
    }

    private void ReturnPlayerState(PlayerController playerController) {
        playerController.Dummy = false;
        playerController.playerDummyState = PlayerController.PlayerDummyAction.Nothing;
    }

}
