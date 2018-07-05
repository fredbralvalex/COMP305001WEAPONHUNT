using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class GameController :MonoBehaviour
    {
        public List<GameObject> playerItems;
        public GameObject Life;
        public GameObject RespawnLeft;
        public GameObject RespawnRight;

        public GameObject GameElements;
        public GameObject MenuPosition;
        public GameObject ItemsPosition;
        public GameObject LifeBarPosition;
        public GameObject LifeBarBossPosition;

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

        public int Level = 1;

        public GameObject GangMan;
        public GameObject BossOne;
        public GameObject BossTwo;
        public GameObject BossThree;
        public List<GameObject> Gangmen;
        public int qtdGangmen = 5;
        private int qtdGangmenGenerated = 0;

        public bool FreezeCamera;

        private void Start()
        {
            playerItems = new List<GameObject>();
            Gangmen = new List<GameObject>();

            //Add items
            playerItems.Add(CreateLife());
            playerItems.Add(CreateLife());
            playerItems.Add(CreateLife());

            LifeBarCanvas.transform.parent = LifeBarPosition.transform;
            LifeBarCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, LifeBarPosition.transform.localPosition.y, LifeBarPosition.transform.localPosition.z);
            //GenerateBoss(true);
            //GenerateGangMan(true);
        }

        void FixedUpdate()
        {           
            PlayerScores.text = "" + PlayerScoreN;
            EnemyScore.text = "" + EnemiesScoreN;
            MountMenu();
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
                    LifeBarBossOneCanvas.transform.parent = LifeBarBossPosition.transform;
                    LifeBarBossOneCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, 0, LifeBarBossPosition.transform.localPosition.z);
                    boss = Instantiate(BossOne);
                    BossOneController controllerB1 = boss.GetComponent<BossOneController>();
                    controllerB1.LifeBar = LifeBarBossOneImage;
                    break;
                case 2:
                    LifeBarBossTwoCanvas.transform.parent = LifeBarBossPosition.transform;
                    LifeBarBossTwoCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, 0, LifeBarBossPosition.transform.localPosition.z);
                    boss = Instantiate(BossTwo);
                    BossTwoController controllerB2 = boss.GetComponent<BossTwoController>();
                    controllerB2.LifeBar = LifeBarBossTwoImage;
                    break;
                case 3:
                    LifeBarBossThreeCanvas.transform.parent = LifeBarBossPosition.transform;
                    LifeBarBossThreeCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, 0, LifeBarBossPosition.transform.localPosition.z);
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
            }
        }

        GameObject CreateLife()
        {
            GameObject clone;
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
                return false;
            }
            else
            {
                playerItems.Remove(life);
                life.SetActive(false);
                Destroy(life);
                return true;
            }
        }

        public bool FreezesCamera ()
        {
            return FreezeCamera;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "BossOne" || other.tag == "BossThree" || other.tag == "BossTwo")
            {
                FreezeCamera = true;
            }
        }

        public TextMeshProUGUI EnemyScore;

        public TextMeshProUGUI PlayerScores;

        public int PlayerScoreN { get; set; }
        public int EnemiesScoreN { get; set; }

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
    }
}
