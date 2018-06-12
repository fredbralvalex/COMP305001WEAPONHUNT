using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class HittableController : MonoBehaviour
    {
        public bool CanHit { get; set; }
        public GameObject AimHit { get; set; }
        public GameObject Hit { get; set; }

        public abstract bool IsHitting();
        public abstract void GettingHit(float power);
    }
}
