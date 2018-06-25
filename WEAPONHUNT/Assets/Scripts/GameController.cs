using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public GameObject Menuposition;
        public GameObject Itemsposition;
        public GameObject LifeBarposition;
        public Canvas LifeBarCanvas;
        public Image LifeBar;
        public int LifeAmount = 10;

        public float MenuOffset = 2.5f;
        public float ItemOffset = 0.7f;
        public float CameraTop;

        public int Level = 1;

        public GameObject GangMan;
        public List<GameObject> Gangmen;
        public int qtdGangmen = 5;
        private int qtdGangmenGenerated = 0;

        private void Start()
        {
            playerItems = new List<GameObject>();
            Gangmen = new List<GameObject>();

            //Add items
            playerItems.Add(CreateLife());
            playerItems.Add(CreateLife());
            playerItems.Add(CreateLife());

            LifeBarCanvas.transform.parent = LifeBarposition.transform;
            LifeBarCanvas.transform.localPosition = new Vector3(-7.25f + MenuOffset, LifeBarposition.transform.localPosition.y, LifeBarposition.transform.localPosition.z);

            GenerateGangMan(true);
        }

        void Update()
        {
            PlayerScore.text = "" +PlayerScoreN;
            GangmanScore.text = "" + GangmanScoreN;
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

            switch (Level)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }            
        }

        public void EliminateGangMan(GameObject gangman)
        {
            Gangmen.Remove(gangman);
            gangman.SetActive(false);
            Destroy(gangman);

            if (qtdGangmenGenerated < qtdGangmen)
            {
                GenerateGangMan(true);
            } else
            {
                GenerateBoss(true);
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
                item.transform.parent = Menuposition.transform;
                item.transform.localPosition = new Vector3(var + MenuOffset, Menuposition.transform.localPosition.y, Menuposition.transform.localPosition.z);
                item.SetActive(true);
                var = var + ItemOffset;
            }

            foreach (GameObject item in ItemList)
            {
                item.transform.parent = Itemsposition.transform;
                item.transform.localPosition = new Vector3(var + MenuOffset, Itemsposition.transform.localPosition.y, Itemsposition.transform.localPosition.z);
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

        public Text PlayerScore;
        public Text GangmanScore;

        public int PlayerScoreN { get; set; }
        public int GangmanScoreN { get; set; }

        public const float ATTACK_PUNCH = 1f;
        public const float ATTACK_KICK = 2f;

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

        public const string IDLE = "CharIdle";
        public const string IDLE_L = "CharIdle_l";

        public const string RUN = "CharRun";
        public const string RUN_L = "CharRun_l";

        public const string JUMP_AN = "CharJump";
        public const string JUMP_AN_L = "CharJump_l";

        public const string PUNCH = "CharPunch";
        public const string PUNCH_L = "CharPunch_l";

        public const string KICK = "CharKick";
        public const string KICK_L = "CharKick_l";

        public const string CHAR_FALL = "CharDefeated";
        public const string CHAR_FALL_L = "CharDefeated_l";

        public const string CHAR_GET_UP = "CharGetUp";
        public const string CHAR_GET_UP_L = "CharGetUp_l";

        public const string GANGMAN_IDLE = "GangmanIdle";
        public const string GANGMAN_IDLE_L = "GangmanIdle_l";

        public const string GANGMAN_RUN = "GangmanRun";
        public const string GANGMAN_RUN_L = "GangmanRun_l";

        public const string GANGMAN_JUMP_AN = "GangmanJump";
        public const string GANGMAN_JUMP_AN_L = "GangmanJump_l";

        public const string GANGMAN_PUNCH = "GangmanPunch";
        public const string GANGMAN_PUNCH_L = "GangmanPunch_l";

        public const string GANGMAN_FALL = "GangmanFall";
        public const string GANGMAN_FALL_L = "GangmanFall_l";

        public const string HITTING = "Hitting";

        public const float TIME_PUNCH = 0.5f;
        public const float TIME_KICK = 0.5f;
        public const float TIME_JUMP = 1.2f;
        public const float COOL_DOWN_TIME_PUNCH = 1f;
        public const float COOL_DOWN_TIME = 0.3f;
        public const float TIME_DEFEATED = 2f;
        public const float TIME_GET_UP = 0.6f;
    }
}
