﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour {

    public static int level = 1;
    public static int playerScoreN = 0;
    public static int enemiesScoreN = 0;
    public static int life = 0;
    public static int coins = 0;
    public static List<GameObject> playerItems;

    public static int playerScoreN_0 = 0;
    public static int enemiesScoreN_0 = 0;
    public static int coins_0 = 0;



    public AudioClip Audio1;
    private AudioSource source;
    public Button btn1;
    public int var;

    private void Start()
    {
        playerItems = new List<GameObject>();
        //Add items        
    }   

    //Funtions
    public void QuitCommand()
    {
        Application.Quit();
    }

    public void LoadGameOverTryAgainScreen()
    {        
        SceneManager.LoadScene("GameOverTryAgain");
    }

    public void LoadGameOverCreditsScreen()
    {
        SceneManager.LoadScene("CreditsGameOver");
    }

    public void LaodNextScreen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadLevelOne()
    {
        level = 1;
        playerScoreN_0 = playerScoreN;
        enemiesScoreN_0 = enemiesScoreN;
        coins_0 = coins;
        SceneManager.LoadScene("lv01_city");
    }

    public void LoadLevelTwo()
    {
        playerScoreN_0 = playerScoreN;
        enemiesScoreN_0 = enemiesScoreN;
        coins_0 = coins;
        level = 2;
        SceneManager.LoadScene("lv02_cave");
    }

    public void LoadLevelThree()
    {
        level = 3;
        playerScoreN_0 = playerScoreN;
        enemiesScoreN_0 = enemiesScoreN;
        coins_0 = coins;
        SceneManager.LoadScene("lv03_snow");
    }

    //
    public void StartPlayFromMenuscreen()
    {        
        LoadStoryScreenOne();
    }

    public void PlayCommand() {
        life = 3;
        LoadLevelOne();
    }

    public void TryAgainCommand()
    {
        playerScoreN = playerScoreN_0;
        enemiesScoreN = enemiesScoreN_0;
        coins = coins_0;

        life = 0;
        if (level == 1)
        {
            LoadLevelOne();
        } else if (level == 2)
        {
            LoadLevelTwo();
        } else if (level == 3)
        {
            LoadLevelThree();
        } else
        {
            LoadGameOverCreditsScreen();
        }

    }

    public void LoadStoryScreenOne()
    {

        SceneManager.LoadScene("StoryScene");
    }

    public void LoadStoryScreenTwo()
    {
        /*gameObject.AddComponent<AudioSource>();
        source.clip = Audio1;
        source.playOnAwake = false;
        btn1.onClick.AddListener(() => playSound());*/
        SceneManager.LoadScene("StoryScene02");

    }

    public void LoadStoryScreenThree()
    {
        SceneManager.LoadScene("StoryScene03");
    }

    void playSound()
    {
        source.PlayOneShot(Audio1);
    }

    public void LoadStoryScreenFour()
    {
        SceneManager.LoadScene("StoryScene04");
    }

    public void LoadStoryScreenFive()
    {
        SceneManager.LoadScene("StoryScene05");
    }

    public static void LoadNextStoryScreen()
    {
        if (level == 2)
        {
            new GameStateController().LoadStoryScreenThree();        
        }
        else if (level == 3)
        {
            new GameStateController().LoadStoryScreenFour();
        }
        else if (level == 4)
        {
            new GameStateController().LoadStoryScreenFive();
        }
    }
}
