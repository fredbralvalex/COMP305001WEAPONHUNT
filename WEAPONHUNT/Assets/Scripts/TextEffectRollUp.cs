﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffectRollUp : MonoBehaviour {

    public List<string> CreditsList = new List<string> { "Group 2",
                            "KasFreSanKasGur",
                            "Weapon Hunt",
                            "Members",
                            "Alexandre, Frederico",
                            "Gururaja, Megha",
                            "Jagdeep Kaur, Kashdeep Kaur",
                            "Rao, Kashish",
                            "Singh, Sandeep"};

    public Transform BeginPosition;
    public Transform EndPosition;
    public Text TextToGo;
    public List<Text> CreditsListText;
    public Canvas CanvasToShow;
    private float time;
    private float timeTowait = 20;
    int Position = 0;

    // Use this for initialization
    void Start () {
        CreditsListText = new List<Text>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        time += Time.deltaTime;
        if (time >= timeTowait * Time.deltaTime)
        {
            if (Position < CreditsList.Count)
            {
                Text newT = CloneText(CreditsList[Position]);
                CreditsListText.Add(newT);
                Position++;
            }
            time = 0;

            foreach (Text txt in CreditsListText)
            {
                //Time.deltaTime*10
                if (txt.transform.position.y < EndPosition.localPosition.y)
                {
                    txt.transform.position = new Vector3(txt.transform.position.x, txt.transform.position.y + 1, txt.transform.position.z);
                }
                if (txt.transform.position.y > BeginPosition.localPosition.y - 1 && txt.transform.position.y < EndPosition.localPosition.y - 1)
                {
                    txt.enabled = true;
                } else
                {
                    txt.enabled = false;
                }
            }
        }
    }

    private Text CloneText (string text)
    {
        Text textToGo;
        textToGo = Instantiate(TextToGo, TextToGo.transform.position, TextToGo.transform.rotation) as Text;

        textToGo.transform.parent = CanvasToShow.transform;
        //textToGo.transform.localPosition = new Vector3(BeginPosition.localPosition.y, BeginPosition.localPosition.y, BeginPosition.localPosition.z);
        textToGo.text = text;
        textToGo.transform.localScale = new Vector3(1, 1, 1);
        return textToGo;
    }

    public void ExitGame()
    {
        //print("Exit Game");
        Application.Quit();
    }
}
