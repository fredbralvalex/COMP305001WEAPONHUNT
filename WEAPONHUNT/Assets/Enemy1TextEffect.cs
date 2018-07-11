using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy1TextEffect : TextEffect {
    public string TextToEnemy = "";
    public TextMeshProUGUI TextMeshPro2;
    private float time;
    private float timeTowait = 7;

    // Use this for initialization
    void Start () {
        TextMeshPro2.text = "";
        //TextMeshPro2.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void ShowNextText()
    {
        time += Time.deltaTime;

        //TextMeshPro2.enabled = true;
        if (time >= timeTowait * Time.deltaTime)
        {
            
                TextMeshPro2.text = TextMeshPro2.text + TextToEnemy[TextMeshPro2.text.Length];
          
        }


    }
}
