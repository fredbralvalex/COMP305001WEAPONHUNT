﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy1TextEffect : TextEffect {
    public string TextToEnemy = "";
    public Text Text2;
    private float _time;
    private float _timeTowait = 7;

    // Use this for initialization
    void Start () {
        Text2.text = "";
        //TextMeshPro2.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void ShowNextText()
    {
        _time += Time.deltaTime;

        //TextMeshPro2.enabled = true;
        if (_time >= _timeTowait * Time.deltaTime)
        {
            if (TextToEnemy.Length > Text2.text.Length)
            {
                Text2.text = Text2.text + TextToEnemy[Text2.text.Length];
            }
          
        }


    }
}
