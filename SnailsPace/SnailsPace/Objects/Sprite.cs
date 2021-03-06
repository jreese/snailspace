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
        public bool visible;
		public bool horizontalFlip;

        // The sprite's image and effect.
        public Image image;
        public String effect;
		public float layerOffset;

        // Sprite animation information.
        public int animationStart;
        public int animationEnd;
        public int frame;
		public float animationDelay;
		public float timer;

        // The sprite's position relative to the parent object.
        public Vector2 position;
		public float rotation;
        
        /// <summary>
        /// Animate the sprite.
        /// </summary>
        /// <param name="gameTime">The current time.</param>
        public void animate(GameTime gameTime)
        {
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (timer >= animationDelay)
			{
				timer = timer - animationDelay;
				if (frame == animationEnd)
				{
					frame = animationStart;
				}
				else
				{
					frame++;
				}
			}
        }

        /// <summary>
        /// Reset the sprite to the first frame of animation.
        /// </summary>
        public void reset()
        {
            frame = animationStart;
        }

		/// <summary>
		/// Perform a deep clone of the sprite.
		/// </summary>
		public Sprite clone()
		{
			Sprite clonedSprite = new Sprite();
			clonedSprite.visible = visible;
			clonedSprite.animationDelay = animationDelay;
			clonedSprite.animationEnd = animationEnd;
			clonedSprite.animationStart = animationStart;
			clonedSprite.effect = effect;
			clonedSprite.frame = frame;
            clonedSprite.horizontalFlip = horizontalFlip;
			clonedSprite.image = image.clone();
            clonedSprite.layerOffset = layerOffset;
            clonedSprite.position = new Vector2(position.X, position.Y);
            clonedSprite.rotation = rotation;
            clonedSprite.timer = timer;
			clonedSprite.visible = visible;
			return clonedSprite;
		}
    }
}
