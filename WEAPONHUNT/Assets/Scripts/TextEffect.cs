using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextEffect : MonoBehaviour {

    public TextMeshProUGUI TextMeshPro;
    private float time;
    private float timeTowait = 7;

    public string TextToExhibit = "";
    //int lenghtText;

	// Use this for initialization
	void Start () {
        TextMeshPro.text = "";
        //lenghtText = textToExhibit.Length;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time >= timeTowait * Time.deltaTime)
        {
            if (TextToExhibit.Length > TextMeshPro.text.Length)
            {
                TextMeshPro.text = TextMeshPro.text + TextToExhibit[TextMeshPro.text.Length];
            }
            time = 0;
        }
	}

    public void ContinueEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
