using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class HittableController : MonoBehaviour
    {

        public Transform HitPosition;

        public int Hits { get; set; }
        public bool CanHit { get; set; }
        public GameObject AimHit { get; set; }
        public GameObject Hit { get; set; }

        public abstract bool IsHitting();
        public abstract void GettingHit(float power);
        public abstract void GettingHit(int hits);

        private double timeToBlink;
        private double timeToColor;
        protected bool Blink { get; set; }

        protected abstract SpriteRenderer GetSprite();

        protected void Blinking()
        {
            if (Blink)
            {
                SpriteRenderer sprite = GetSprite();
                if (timeToBlink == 0)
                {
                    sprite.color = Color.yellow;
                }

                timeToBlink += Time.deltaTime;
                timeToColor += Time.deltaTime;

                if (timeToColor > 7 * Time.deltaTime)
                {
                    if (sprite.color == Color.yellow)
                    {
                        //print("yellow " + timeToBlink / Time.deltaTime);
                        sprite.color = Color.red;
                    }
                    else if (sprite.color == Color.red)
                    {
                        //print("red " + timeToBlink / Time.deltaTime);
                        sprite.color = Color.white;
                    }
                    else if (sprite.color == Color.white)
                    {
                        //print("white " + timeToBlink / Time.deltaTime);
                        sprite.color = Color.yellow;
                    }
                    //print(sprite.color +" " + time2 / Time.deltaTime);                    
                    timeToColor = 0;
                } else
                {/*
                    if (sprite.color == Color.yellow)
                    {
                        print("yellow " + timeToBlink / Time.deltaTime);
                    }
                    else if (sprite.color == Color.white)
                    {
                        print("white " + timeToBlink / Time.deltaTime);
                    }*/
                }

                if (timeToBlink > 36 * Time.deltaTime)
                {
                    sprite.color = Color.white;
                    Blink = false;
                    timeToBlink = 0;
                    timeToColor = 0;
                }
            }
        }

    }
}
