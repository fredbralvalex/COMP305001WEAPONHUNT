using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour {

    public static int level = 1;
    public static int playerScoreN = 0;
    public static int enemiesScoreN = 0;
    public static int life = 0;
    public static List<GameObject> playerItems;


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
        SceneManager.LoadScene("lv01_city");
    }

    public void LoadLevelTwo()
    {
        SceneManager.LoadScene("lv02");
    }

    public void LoadLevelThree()
    {
        SceneManager.LoadScene("lv03");
    }

    //
    public void StartPlayFromMenuscreen()
    {        
        LoadStoryScreenOne();
    }

    public void PlayCommand() {
        life = 0;
        LoadLevelOne();
    }

    public void TryAgainCommand()
    {
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
        SceneManager.LoadScene("StoryScene02");
    }

    public void LoadStoryScreenThree()
    {
        SceneManager.LoadScene("StoryScene03");
    }
}
