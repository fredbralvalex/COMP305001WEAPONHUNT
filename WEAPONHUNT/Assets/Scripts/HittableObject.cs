using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class HittableObject : HittableController
    {
        public int Life = 3;
        SpriteRenderer sprite;

        private void Start()
        {
            sprite = gameObject.GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {

            Blinking();
            if (Life - Hits <= 0)
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
        public override void GettingHit(float power)
        {
            Blink = true;
            Hits += Convert.ToInt16(power);
        }

        public override void GettingHit(int hits)
        {
            Blink = true;
            Hits ++;
        }

        public override bool IsHitting()
        {
            throw new NotImplementedException();
        }

        protected override SpriteRenderer GetSprite()
        {
            return sprite;
        }
    }
}
