using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnailsPace.Objects
{
    class Sprite
    {
        // Whether to display the sprite.
        public Boolean visible;

        // The sprite's image and effect.
        public Image image;
        public Effect effect;

        // Sprite animation information.
        public int animationStart;
        public int animationEnd;
        public int frame;

        // The sprite's position relative to the parent object.
        public Vector2 position;
        
        // Animate the sprite.
        public void animate()
        {
            if (frame == animationEnd)
            {
                frame = animationStart;
            }
            else
            {
                frame++;
            }
        }

        // Reset the sprite.
        public void reset()
        {
            frame = animationStart;
        }
    }
}
