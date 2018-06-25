using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class HittableController : MonoBehaviour
    {

        public int Hits { get; set; }
        public bool CanHit { get; set; }
        public GameObject AimHit { get; set; }
        public GameObject Hit { get; set; }

        public abstract bool IsHitting();
        public abstract void GettingHit(float power);

        private double time2;
        protected bool Blink { get; set; }

        protected abstract SpriteRenderer GetSprite();

        protected void Blinking()
        {
            if (Blink)
            {
                SpriteRenderer sprite = GetSprite();
                time2 += Time.deltaTime;
                sprite.color = Color.yellow;
                if (time2 > 5 * Time.deltaTime)
                {
                    time2 = 0;
                    sprite.color = Color.white;
                    Blink = false;
                }
            }
        }
    }
}
