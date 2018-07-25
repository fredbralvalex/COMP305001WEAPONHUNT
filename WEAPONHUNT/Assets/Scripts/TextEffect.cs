using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour {

    public Text Text;
    private float time;
    private float timeTowait = 7;

    public string TextToExhibit = "";
  
    // Use this for initialization
    void Start () {
        Text.text = "";
      
        //lenghtText = textToExhibit.Length;
    }
    


    // Update is called once per frame
    void FixedUpdate() {
        time += Time.deltaTime;
        if (time >= timeTowait * Time.deltaTime)
        {
            if (TextToExhibit.Length > Text.text.Length)
            {
                Text.text = Text.text + TextToExhibit[Text.text.Length];
            } else
            {
                ShowNextText();
            }
            
            time = 0;
        }
	}

    protected virtual void ShowNextText()
    { }

    public void ContinueEvent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
