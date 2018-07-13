using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController :MonoBehaviour
    {
        GameStateController gameState;        
        public GameObject Life;
        [Header ("Game Position Elements")]
        public GameObject RespawnLeft;
        public GameObject RespawnRight;
        private GameObject PlayerInitialPosition;

        public GameObject GameElements;
        public GameObject MenuPosition;
        public GameObject ItemsPosition;
        public GameObject CoinsPosition;
        public GameObject LifeBarPosition;
        public GameObject LifeBarBossPosition;

        public Canvas CanvasCoins;
        public Canvas LifeBarCanvas;
        public Canvas LifeBarBossOneCanvas;
        public Canvas LifeBarBossTwoCanvas;
        public Canvas LifeBarBossThreeCanvas;

        public Image LifeBar;
        public Image LifeBarBossOneImage;
        public Image LifeBarBossTwoImage;
        public Image LifeBarBossThreeImage;


        public int LifeAmount = 10;

        public float MenuOffset = 2.5f;
        public float ItemOffset = 0.7f;
        public float CameraTop;

        public int Level
        {
            get
            {
                return GameStateController.level;
            }
            set {
                GameStateController.level = value;
            }
        }

        public GameObject Player;
        public GameObject GangMan;
        public GameObject BossOne;
        public GameObject BossTwo;
        public GameObject BossThree;
        public List<GameObject> Gangmen;
        public int qtdGangmen = 5;
        private int qtdGangmenGenerated = 0;

        public bool FreezeCamera;

        public List<GameObject> playerItems
        {
            get
            {
                return GameStateController.playerItems;
            }
            set
            {
                GameStateController.playerItems = value;
            }
        }

        private void Start()
        {            
            playerItems = new List<GameObject>();
            gameState = new GameStateController();
            Gangmen = new List<GameObject>();

            PlayerInitialPosition = GetComponentInParent<CameraController>().gameObject;

            Canvas LifeBarCanvasClone;
            LifeBarCanvasClone = Instantiate(LifeBarCanvas, LifeBarCanvas.transform.position, LifeBarCanvas.transform.rotation) as Canvas;

            LifeBarCanvasClone.transform.parent = LifeBarPosition.transform;
            LifeBarCanvasClone.transform.localPosition = new Vector3(-7.25f + MenuOffset, LifeBarPosition.transform.localPosition.y, LifeBarPosition.transform.localPosition.z);
            LifeBarCanvas = LifeBarCanvasClone;            

            Image[] Images = LifeBarCanvas.GetComponentsInChildren<Image>();
            foreach (Image i in Images)
            {
                if (i.tag == "LifeBar")
                {
                    LifeBar = i;
                    break;
                }
            }

            TextMeshProUGUI[] txts = LifeBarCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI t in txts)
            {
                if (t.tag == "PlayerScore")
                {
                    PlayerScores = t;
                }
                if (t.tag == "EnemyScore")
                {
                    EnemyScore = t;
                }
            }
            Canvas clone = Instantiate(CanvasCoins, CoinsPosition.transform.position, CoinsPosition.transform.rotation) as Canvas;
            clone.transform.parent = CoinsPosition.transform;
            clone.transform.position = new Vector3(-7.75f + MenuOffset, CoinsPosition.transform.localPosition.y + 2, CoinsPosition.transform.localPosition.z);
            CanvasCoins = clone;

            CoinsCount = CanvasCoins.GetComponentInChildren<TextMeshProUGUI>();

            StartLevelGame();
        }

        public void StartLevelGame()
        {
            //Add items
            for(int i = 0; i < GameSaveStateController.GetInstance().life; i++)
            {
                playerItems.Add(CreateLife());
            }
            
            print("On Load Level Lives: " + GameSaveStateController.GetInstance().life);
            //GenerateBoss(true);
            GeneratePlayer();
            GenerateGangMan(true);
        }

        void FixedUpdate()
        {           
            PlayerScores.text = "" + PlayerScoreN;
            EnemyScore.text = "" + EnemiesScoreN;
            CoinsCount.text = "" + Coins;
            MountMenu();

            if (Player!= null && Player.GetComponent<PlayerController>() != null &&
                Player.GetComponent<PlayerController>().Dummy &&
                Player.GetComponent<PlayerController>().playerDummyState == PlayerController.PlayerDummyAction.Won)
            {
                //updating hits for the next level
                GameStateController.hits = Player.GetComponent<PlayerController>().Hits;
                print("G - On Finishing Level Lives: " + GameSaveStateController.GetInstance().life);

                GameStateController.LoadNextStoryScreen();
            }
        }

        public void GeneratePlayer()
        {
            GameObject PlayerTmp = GameObject.Find("/Player");
            if (PlayerTmp == null)
            {
                GameObject player = Instantiate(Player);
                player.transform.localPosition = new Vector3 (RespawnLeft.transform.position.x, PlayerInitialPosition.transform.position.y, - 1.34f);
                player.GetComponent<PlayerController>().Dummy = true;
                player.GetComponent<PlayerController>().playerDummyState = PlayerController.PlayerDummyAction.MoveToCenter;
                player.GetComponent<PlayerController>().Hits = GameStateController.hits;
                player.GetComponent<PlayerController>().UpdateLifeBar();
                Player = player;

            } else
            {
                Player = PlayerTmp;
            }
        }

        public void GenerateGangMan(bool right)
        {
            GameObject gangman = Instantiate(GangMan);
            qtdGangmenGenerated++;
            Gangmen.Add(gangman);
            if(right)
            {
                gangman.transform.localPosition = RespawnRight.transform.position;
            } else
            {
                gangman.transform.localPosition = RespawnLeft.transform.position;
            }
        }

        public void GenerateBoss(bool right)
        {
            GameObject boss = null;
            switch (Level)
            {
                case 1:
                    Canvas LifeBarCanvasBossOneClone;
                    LifeBarCanvasBossOneClone = Instantiate(LifeBarBossOneCanvas, LifeBarBossOneCanvas.transform.position, LifeBarBossOneCanvas.transform.rotation) as Canvas;

                    LifeBarCanvasBossOneClone.transform.parent = LifeBarPosition.transform;
                    LifeBarCanvasBossOneClone.transform.localPosition = new Vector3(-7.25f + MenuOffset, LifeBarBossPosition.transform.localPosition.y, LifeBarBossPosition.transform.localPosition.z);
                    LifeBarBossOneCanvas = LifeBarCanvasBossOneClone;

                    foreach (Image i in LifeBarBossOneCanvas.GetComponentsInChildren<Image>())
                    {
                        if (i.tag == "LifeBar")
                        {
                            LifeBarBossOneImage = i;
                            break;
                        }
                    }

                    boss = Instantiate(BossOne);
                    BossOneController controllerB1 = boss.GetComponent<BossOneController>();
                    controllerB1.LifeBar = LifeBarBossOneImage;
                    break;
                case 2:
                    Canvas LifeBarCanvasBossTwoClone;
                    LifeBarCanvasBossTwoClone = Instantiate(LifeBarBossTwoCanvas, LifeBarBossTwoCanvas.transform.position, LifeBarBossTwoCanvas.transform.rotation) as Canvas;

                    LifeBarCanvasBossTwoClone.transform.parent = LifeBarPosition.transform;
                    LifeBarCanvasBossTwoClone.transform.localPosition = new Vector3(-7.25f + MenuOffset, LifeBarBossPosition.transform.localPosition.y, LifeBarBossPosition.transform.localPosition.z);
                    LifeBarBossTwoCanvas = LifeBarCanvasBossTwoClone;

                    foreach (Image i in LifeBarBossTwoCanvas.GetComponentsInChildren<Image>())
                    {
                        if (i.tag == "LifeBar")
                        {
                            LifeBarBossTwoImage = i;
                            break;
                        }
                    }
                    /*
                        LifeBarBossTwoCanvas.transform.parent = LifeBarBossPosition.transform;
                        LifeBarBossTwoCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, 0, LifeBarBossPosition.transform.localPosition.z);
                    */
                    boss = Instantiate(BossTwo);
                    BossTwoController controllerB2 = boss.GetComponent<BossTwoController>();
                    controllerB2.LifeBar = LifeBarBossTwoImage;
                    break;
                case 3:
                    Canvas LifeBarCanvasBossThreeClone;
                    LifeBarCanvasBossThreeClone = Instantiate(LifeBarBossThreeCanvas, LifeBarBossThreeCanvas.transform.position, LifeBarBossThreeCanvas.transform.rotation) as Canvas;

                    LifeBarCanvasBossThreeClone.transform.parent = LifeBarPosition.transform;
                    LifeBarCanvasBossThreeClone.transform.localPosition = new Vector3(-7.25f + MenuOffset, LifeBarBossPosition.transform.localPosition.y, LifeBarBossPosition.transform.localPosition.z);
                    LifeBarBossThreeCanvas = LifeBarCanvasBossThreeClone;

                    foreach (Image i in LifeBarBossThreeCanvas.GetComponentsInChildren<Image>())
                    {
                        if (i.tag == "LifeBar")
                        {
                            LifeBarBossThreeImage = i;
                            break;
                        }
                    }
                    /*
                        LifeBarBossThreeCanvas.transform.parent = LifeBarBossPosition.transform;
                        LifeBarBossThreeCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, 0, LifeBarBossPosition.transform.localPosition.z);
                    */
                    boss = Instantiate(BossThree);
                    BossThreeController controllerB3 = boss.GetComponent<BossThreeController>();
                    controllerB3.LifeBar = LifeBarBossThreeImage;
                    break;
            }

            if (boss != null)
            {
                if (right)
                {
                    boss.transform.localPosition = RespawnRight.transform.position;
                }
                else
                {
                    boss.transform.localPosition = RespawnLeft.transform.position;
                }
            }
        }

        public void EliminateEnemy(GameObject enemy)
        {
            PlayerScoreN+=enemy.GetComponent<EnemyController>().GetDefeatPoints();
            if (enemy.tag == "Gangman")
            {
                Gangmen.Remove(enemy);
                enemy.SetActive(false);
                Destroy(enemy);
                FreezeCamera = false;

                if (qtdGangmenGenerated < qtdGangmen)
                {
                    GenerateGangMan(true);
                }
                else
                {
                    GenerateBoss(true);
                }
            }
            else if (enemy.tag == "BossOne")
            {
                enemy.SetActive(false);
                Destroy(enemy);
                Destroy(LifeBarBossOneCanvas.gameObject);
                FreezeCamera = false;
                Level = 2;
                Player.GetComponent<PlayerController>().PlayWinning();
                //GenerateBoss(true);
            }
            else if (enemy.tag == "BossTwo")
            {
                enemy.SetActive(false);
                Destroy(enemy);
                Destroy(LifeBarBossTwoCanvas.gameObject);
                FreezeCamera = false;
                Level = 3;
                Player.GetComponent<PlayerController>().PlayWinning();
                //GenerateBoss(true);
            }
            else if (enemy.tag == "BossThree")
            {
                enemy.SetActive(false);
                Destroy(enemy);
                Destroy(LifeBarBossThreeCanvas.gameObject);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                Level = 4;
                Player.GetComponent<PlayerController>().PlayWinning();
                //GenerateBoss(true);
            }
        }

        GameObject CreateLife()
        {
            GameObject clone = null;
            clone = Instantiate(Life, Life.transform.position, Life.transform.rotation) as GameObject;
            return clone;
        }

        public void AddItem(GameObject item)
        {
            playerItems.Add(item);
        }

        public void AddBood()
        {
            LifeAmount++;
        }

        void MountMenu()
        {

            List<GameObject> lifeList = new List<GameObject>();
            List<GameObject> ItemList = new List<GameObject>();

            float var = -8;


            foreach (GameObject item in playerItems)
            {
                //Debug.Log(item.tag);
                if (item.tag == "Life")
                {
                    lifeList.Add(item);
                }
            }

            foreach (GameObject item in lifeList)
            {
                item.transform.parent = MenuPosition.transform;
                item.transform.localPosition = new Vector3(var + MenuOffset, MenuPosition.transform.localPosition.y, MenuPosition.transform.localPosition.z);
                item.SetActive(true);
                var = var + ItemOffset;
            }

            foreach (GameObject item in ItemList)
            {
                item.transform.parent = ItemsPosition.transform;
                item.transform.localPosition = new Vector3(var + MenuOffset, ItemsPosition.transform.localPosition.y, ItemsPosition.transform.localPosition.z);
                item.SetActive(true);
                var = var + ItemOffset;
            }
        }

        public GameObject GetLife()
        {
            foreach (GameObject item in playerItems)
            {
                if (item.tag == "Life")
                {
                    return item;
                }
            }
            return null;
        }

        public bool NewChancePlayer()
        {
            return LostLife();
        }

        public bool LostLife()
        {
            GameObject life = GetLife();
            if (life == null)
            {
                //endGame();
                gameState.LoadGameOverTryAgainScreen();
                return false;
            }
            else
            {
                playerItems.Remove(life);
                life.SetActive(false);
                Destroy(life);
                GameSaveStateController.GetInstance().life--;
                return true;
            }
        }

        public bool FreezesCamera ()
        {
            return FreezeCamera;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "BossOne" || other.tag == "BossThree" || other.tag == "BossTwo" || other.tag == "Gangman")
            {
                FreezeCamera = true;
            }
        }

        public TextMeshProUGUI EnemyScore;

        public TextMeshProUGUI PlayerScores;

        public TextMeshProUGUI CoinsCount;

        public int PlayerScoreN {
            get
            {
                return GameStateController.playerScoreN;
            }
            set
            {
                GameStateController.playerScoreN = value;
            }
        }
        public int EnemiesScoreN {
            get
            {
                return GameStateController.enemiesScoreN;
            }
            set
            {
                GameStateController.enemiesScoreN = value;
            }
        }

        public int Coins
        {
            get
            {
                return GameStateController.coins;
            }
            set
            {
                GameStateController.coins = value;
            }
        }

        public const float POWER_ATTACK_1 = 1f;
        public const float POWER_ATTACK_2 = 2f;
        public const float POWER_ATTACK_3 = 3f;

        public const float SPEED_CONSTANT = 2f;        
        public const float SPEED_JUMP_CONSTANT = 4f;
        public const float SPEED_FALL_CONSTANT = 4f;

        //public const KeyCode UP = KeyCode.W;
        //public const KeyCode DOWN = KeyCode.S;
        public const KeyCode LEFT = KeyCode.A;
        public const KeyCode RIGHT = KeyCode.D;

        public const KeyCode JUMP = KeyCode.J;
        public const KeyCode ATTACK_1 = KeyCode.K;
        public const KeyCode ATTACK_2 = KeyCode.L;                

        public const string HITTING = "Hitting";

        public const float TIME_PUNCH = 0.5f;
        public const float TIME_KICK = 0.5f;
        public const float TIME_JUMP = 1.2f;
        public const float COOL_DOWN_TIME_ATTACK1 = 1f;
        public const float COOL_DOWN_TIME = 0.3f;
        public const float TIME_DEFEATED = 2f;
        public const float TIME_GET_UP = 0.6f;

        public const int HitPoints = 10;
        public const int HitBossPoints = 15;
        public const int BossOnePoints = 150;
        public const int BossTwoPoints = 250;
        public const int BossThreePoints = 350;
    }
}
