using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameSaveStateController : MonoBehaviour
    {
        static GameSaveStateController instance;
        public static int Life{ get; set; }

        public static GameSaveStateController GetInstance()
        {
            if (instance == null)
            {
                instance = new GameSaveStateController();
            }
            return instance;
        }

        public void GeneratePlayJumpAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioJump") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }

        public void GeneratePlayPunchAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioPunch") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
        public void GeneratePlayKickAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioKick") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
        public void GeneratePlaySwordAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioSword") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
        public void GeneratePlayPikeAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioPike") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
        public void GeneratePlayAxeAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioAxe") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }

        public void GeneratePlayWinAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioWin") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
        public void GeneratePlayWeaponMoveAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioMov") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
        public void GeneratePlayBrickAudio()
        {
            GameObject prefab = Resources.Load<GameObject>("Audio/audioBrick") as GameObject;
            Instantiate(prefab).GetComponent<PlaySoundController>().PlayAudioSource();
        }
    }
}
