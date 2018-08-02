using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [Header("Game Menu Position Elements")]
        public GameObject GameElements;
        public GameObject MenuPosition;
        public GameObject ItemsPosition;
        public GameObject CoinsPosition;
        public GameObject LifeBarPosition;
        public GameObject LifeBarBossPosition;
        public GameObject WeaponPosition;
        public GameObject BarePosition;
        public GameObject PikePosition;
        public GameObject AxePosition;
        public GameObject Blood;
        public GameObject LevelTitlePosition;


        [Header("Game Menu Canvas Elements")]
        public Canvas PauseCanvas;
        public Canvas CanvasCoins;
        public Canvas LevelTitle;
        public Canvas LifeBarCanvas;
        public Canvas WeaponCanvas;
        public Canvas LifeBarBossOneCanvas;
        public Canvas LifeBarBossTwoCanvas;
        public Canvas LifeBarBossThreeCanvas;

        [Header("Game Images Elements")]
        public Image LifeBar;
        public Image LifeBarBossOneImage;
        public Image LifeBarBossTwoImage;
        public Image LifeBarBossThreeImage;


        public Image Bare;
        public Image Bare_Selected;

        public Image Pike;
        public Image Pike_Selected;

        public Image Axe;
        public Image Axe_Selected;

        public int lives = 0;
        public const int LifeAmount = 10;

        public float MenuOffset = 2.5f;
        public float ItemOffset = 0.7f;
        public float CameraTop;

        public float endPositionX;
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

            if (lives != 0)
            {
                GameSaveStateController.GetInstance().life = lives;
            }
            
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

            Text[] txts = LifeBarCanvas.GetComponentsInChildren<Text>();
            foreach (Text t in txts)
            {
                if (t.tag == "PlayerScore")
                {
                    PlayerScoresTxt = t;
                }
                if (t.tag == "EnemyScore")
                {
                    EnemyScoreTxt = t;
                }
            }
            Canvas clone = Instantiate(CanvasCoins, CoinsPosition.transform.position, CoinsPosition.transform.rotation) as Canvas;
            clone.transform.parent = CoinsPosition.transform;
            clone.transform.position = new Vector3(-8.1f + MenuOffset, CoinsPosition.transform.localPosition.y + 2.5f, CoinsPosition.transform.localPosition.z);
            CanvasCoins = clone;

            CoinsCountTxt = CanvasCoins.GetComponentInChildren<Text>();

            BuildWeaponMenu();
            BuildPauseAction();
            BuildLevelTitle();
            StartLevelGame();
        }

        private void BuildPauseAction()
        {
            Canvas clone = Instantiate(PauseCanvas) as Canvas;
            PauseCanvas = clone;
            
        }

        private void BuildLevelTitle()
        {
            Canvas clone = Instantiate(LevelTitle, LevelTitlePosition.transform.position, LevelTitlePosition.transform.rotation) as Canvas;
            clone.transform.parent = LevelTitlePosition.transform;
            clone.transform.position = new Vector3(LevelTitlePosition.transform.localPosition.x, LevelTitlePosition.transform.localPosition.y, LevelTitlePosition.transform.localPosition.z);
            LevelTitle = clone;

            LevelTitleText = LevelTitle.GetComponentInChildren<Text>();
            switch (Level)
            {
                case 1:
                    LevelTitleText.text = Level01_Title;
                    break;
                case 2:
                    LevelTitleText.text = Level02_Title;
                    break;
                case 3:
                    LevelTitleText.text = Level03_Title;
                    break;
            }
        }

        private void BuildWeaponMenu()
        {
            Canvas Weaponclone = Instantiate(WeaponCanvas, WeaponPosition.transform.position, WeaponPosition.transform.rotation) as Canvas;
            Weaponclone.transform.parent = WeaponPosition.transform;
            Weaponclone.transform.position = new Vector3(WeaponPosition.transform.localPosition.x, WeaponPosition.transform.localPosition.y, WeaponPosition.transform.localPosition.z);
            WeaponCanvas = Weaponclone;
            
            Image bareImage = Instantiate(Bare, BarePosition.transform.position, BarePosition.transform.rotation) as Image;
            Image bareImageS = Instantiate(Bare_Selected, BarePosition.transform.position, BarePosition.transform.rotation) as Image;
            Bare = bareImage;
            Bare_Selected = bareImageS;

            Bare.transform.parent = WeaponCanvas.transform;
            Bare_Selected.transform.parent = WeaponCanvas.transform;

            Image pikeImage = Instantiate(Pike, PikePosition.transform.position, PikePosition.transform.rotation) as Image;
            Image pikeImageS = Instantiate(Pike_Selected, PikePosition.transform.position, PikePosition.transform.rotation) as Image;
            Pike = pikeImage;
            Pike_Selected = pikeImageS;
            if (GameStateController.level > 1)
            {
                Pike.transform.parent = WeaponCanvas.transform;
                Pike_Selected.transform.parent = WeaponCanvas.transform;
            }

            Image axeImage = Instantiate(Axe, AxePosition.transform.position, AxePosition.transform.rotation) as Image;
            Image axeImageS = Instantiate(Axe_Selected, AxePosition.transform.position, AxePosition.transform.rotation) as Image;
            Axe = axeImage;
            Axe_Selected = axeImageS;
            if (GameStateController.level > 2)
            {
                Axe.transform.parent = WeaponCanvas.transform;
                Axe_Selected.transform.parent = WeaponCanvas.transform;
            }

            WeaponPosition.transform.position = new Vector3(WeaponPosition.transform.localPosition.x, WeaponPosition.transform.localPosition.y + 5, WeaponPosition.transform.localPosition.z);

            Bare.enabled = GameStateController.level >= 1;
            Bare_Selected.enabled = GameStateController.level >= 1;
            Pike.enabled = GameStateController.level > 1;
            Pike_Selected.enabled = false;//GameStateController.level > 1;
            Axe.enabled = GameStateController.level > 2;
            Axe_Selected.enabled = false; //GameStateController.level > 2;
            
            
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
            //GenerateGangMan(true);
        }

        private double time;
        private void VerifyWinning()
        {
            time += Time.deltaTime;

            if (Player != null && Player.GetComponent<PlayerController>() != null &&
               Player.GetComponent<PlayerController>().Dummy &&
               Player.GetComponent<PlayerController>().playerDummyState == PlayerController.PlayerDummyAction.Won)
            {
                //TODO
            }
        }

        void FixedUpdate()
        {

            PlayerScoresTxt.text = "" + PlayerScoreN;
            EnemyScoreTxt.text = "" + EnemiesScoreN;
            CoinsCountTxt.text = "" + Coins;

            MountMenu();

            if (Player != null && Player.GetComponent<PlayerController>() != null &&
               Player.GetComponent<PlayerController>().Dummy &&
               Player.GetComponent<PlayerController>().playerDummyState == PlayerController.PlayerDummyAction.MoveToCenter)
            {
                LevelTitle.enabled = true;
            } else
            {
                LevelTitle.enabled = false ;
            }

            if (Player!= null && Player.GetComponent<PlayerController>() != null &&
                Player.GetComponent<PlayerController>().Dummy &&
                Player.GetComponent<PlayerController>().playerDummyState == PlayerController.PlayerDummyAction.Won)
            {
                //updating hits for the next level
                GameStateController.hits = Player.GetComponent<PlayerController>().Hits;
                print("G - On Finishing Level Lives: " + GameSaveStateController.GetInstance().life);

                GameStateController.LoadNextStoryScreen();
            }

            if (Player != null && Player.GetComponent<PlayerController>() != null)
            {
                if (PlayerController.Weapon.Bare == Player.GetComponent<PlayerController>().chosenWeapon)
                {
                    Bare.enabled = false;
                    Bare_Selected.enabled = true;
                    Pike.enabled = true;
                    Pike_Selected.enabled = false;
                    Axe.enabled = true;
                    Axe_Selected.enabled = false;
                } else if (PlayerController.Weapon.Pike == Player.GetComponent<PlayerController>().chosenWeapon)
                {
                    Bare.enabled = true;
                    Bare_Selected.enabled = false;
                    Pike.enabled = false;
                    Pike_Selected.enabled = true;
                    Axe.enabled = true;
                    Axe_Selected.enabled = false;
                } else if (PlayerController.Weapon.Axe == Player.GetComponent<PlayerController>().chosenWeapon)
                {
                    Bare.enabled = true;
                    Bare_Selected.enabled = false;
                    Pike.enabled = true;
                    Pike_Selected.enabled = false;
                    Axe.enabled = false;
                    Axe_Selected.enabled = true;
                }
            }
        }

        public void GeneratePlayer()
        {
            GameObject PlayerTmp = GameObject.Find("/Player");
            if (PlayerTmp == null)
            {
                GameObject player = Instantiate(Player);
                player.transform.localPosition = new Vector3 (RespawnLeft.transform.position.x, 0, - 1.34f);
                player.GetComponent<PlayerController>().Dummy = true;
                player.GetComponent<PlayerController>().playerDummyState = PlayerController.PlayerDummyAction.MoveToCenter;
                player.GetComponent<PlayerController>().Hits = GameStateController.hits;
                player.GetComponent<PlayerController>().UpdateLifeBar();
                Player = player;

            } else
            {
                Player = PlayerTmp;
            }
            endPositionX = Player.transform.position.x;
        }

        [Obsolete("Not using fixed position")]
        private void GenerateGangMan(bool right)
        {            
            if(right)
            {
                GenerateGangMan(RespawnRight.transform);
            } else
            {
                GenerateGangMan(RespawnLeft.transform);
            }
        }


        public void GenerateGangMan(Transform transformPosition)
        {
            GameObject gangman = Instantiate(GangMan);
            qtdGangmenGenerated++;
            Gangmen.Add(gangman);
            gangman.transform.localPosition = new Vector3 (transformPosition.position.x, transformPosition.position.y, 0);
        }

        [Obsolete("Not using fixed position")]
        private void GenerateBoss(bool right)
        {
            if (right)
            {
                GenerateBoss(RespawnRight.transform);
            }
            else
            {
                GenerateBoss(RespawnLeft.transform);
            }
        }

        public void GenerateBoss(Transform transformPosition)
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
                boss.transform.localPosition = new Vector3(transformPosition.position.x, transformPosition.position.y, 0);                
            }

        }

        public GameObject CreateBlood()
        {
            GameObject blood = Instantiate(Blood);
            return blood;
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
                GameObject blood = CreateBlood();
                blood.transform.localPosition = new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
                /*
                if (qtdGangmenGenerated < qtdGangmen)
                {
                    GenerateGangMan(true);
                }
                else
                {
                    GenerateBoss(true);
                }*/
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

        public float GetEndPositionX()
        {
            return endPositionX;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "BossOne" || other.tag == "BossThree" || other.tag == "BossTwo" || other.tag == "Gangman")
            {
                FreezeCamera = true;
            }
        }

        public Text EnemyScoreTxt;
        public Text PlayerScoresTxt;
        public Text CoinsCountTxt;

        public Text LevelTitleText;

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

        public const float SPEED_CONSTANT = 1f;        
        public const float SPEED_JUMP_CONSTANT = 2f;
        public const float SPEED_FALL_CONSTANT = 4f;

        //public const KeyCode UP = KeyCode.W;
        //public const KeyCode DOWN = KeyCode.S;
        public const KeyCode LEFT = KeyCode.A;
        public const KeyCode RIGHT = KeyCode.D;

        public const KeyCode JUMP = KeyCode.J;
        public const KeyCode ATTACK_1 = KeyCode.K;
        public const KeyCode ATTACK_2 = KeyCode.L;

        public const KeyCode NO_WEAPON = KeyCode.U;
        public const KeyCode WEAPON_1 = KeyCode.I;
        public const KeyCode WEAPON_2 = KeyCode.O;

        public const string HITTING = "Hitting";

        public const float TIME_PUNCH = 0.5f;
        public const float TIME_KICK = 0.5f;
        public const float TIME_PIKE = 0.4f;
        public const float TIME_AXE = 0.5f;
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

        public const string Level01_Title = "The Suburb's Park";
        public const string Level02_Title = "Axel's Cave";
        public const string Level03_Title = "Frozen Hearts";
    }
}
