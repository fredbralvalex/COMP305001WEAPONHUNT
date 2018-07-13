using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class GameSaveStateController
    {
        static GameSaveStateController instance;
        public int life;

        public static GameSaveStateController GetInstance()
        {
            if (instance == null)
            {
                instance = new GameSaveStateController();
            }
            return instance;
        }
    }
}
