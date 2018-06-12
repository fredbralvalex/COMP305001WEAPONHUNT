using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    abstract class EnemyController : HittableController
    {

        public abstract void FaceLeftCommand();
        public abstract void FaceRightCommand();

        public abstract void PunchCommand();
        public abstract void MoveCommand();
        public abstract void IdleCommand();

    }
}
