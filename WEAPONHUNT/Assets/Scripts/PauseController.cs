using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {


    //public AudioMixerSnapshot Paused;
    //public AudioMixerSnapshot Unpaused;
    private Canvas canvas;
    public Scene ActualScene;

    // Use this for initialization
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace))
        {
            Pause();
        }
    }

    public void Pause()
    {
        //Toggle canvas enabled
        canvas.enabled = !canvas.enabled;

        //Pause Game
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        //change sound
    }

    public void ChangeSound()
    {/*
        if (Time.timeScale == 0)
        {
            //Paused
            Paused.TransitionTo(0.001f);
        }
        else
        {
            //unpaused
            Unpaused.TransitionTo(0.001f);
        }*/
    }

    public void ReturnToMenu()
    {
        //SceneManager.LoadScene("MainMenu");        
    }

    public Scene ReturnScene()
    {
        return ActualScene;
    }

    public void QuitGame()
    {
        //SceneManager.LoadScene("MainMenu");        
        Application.Quit();
    }

    public void RestartLevel()
    {
        //StopMenuMusic();
        //Application.Quit();
    }
}
