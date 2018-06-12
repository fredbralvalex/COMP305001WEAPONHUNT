using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class GameController
    {
        public const float ATTACK_POWER_1 = 2f;
        public const float ATTACK_POWER_2 = 3f;

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

        public const string GANGMAN_IDLE = "GangmanIdle";
        public const string GANGMAN_IDLE_L = "GangmanIdle_l";

        public const string GANGMAN_RUN = "GangmanRun";
        public const string GANGMAN_RUN_L = "GangmanRun_l";

        public const string GANGMAN_JUMP_AN = "GangmanJump";
        public const string GANGMAN_JUMP_AN_L = "GangmanJump_l";

        public const string GANGMAN_PUNCH = "GangmanPunch";
        public const string GANGMAN_PUNCH_L = "GangmanPunch_l";

        public const float TIME_PUNCH = 0.5f;
        public const float TIME_JUMP = 1.2f;
        public const float COOL_DOWN_TIME_PUNCH = 1f;
        public const float COOL_DOWN_TIME = 0.3f;
    }
}
