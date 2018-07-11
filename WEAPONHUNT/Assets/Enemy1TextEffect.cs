using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy1TextEffect : TextEffect {
    public string TextToEnemy = "";
    public TextMeshProUGUI TextMeshPro2;
    private float _time;
    private float _timeTowait = 7;

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
        _time += Time.deltaTime;

        //TextMeshPro2.enabled = true;
        if (_time >= _timeTowait * Time.deltaTime)
        {
            if (TextToEnemy.Length > TextMeshPro2.text.Length)
            {
                TextMeshPro2.text = TextMeshPro2.text + TextToEnemy[TextMeshPro2.text.Length];
            }
          
        }


    }
}
