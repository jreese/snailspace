using System;
using System.Collections.Generic;
using System.Text;
using SnailsPace.Core;
using Microsoft.Xna.Framework;

namespace SnailsPace.Objects
{
	class Explosion : GameObject
    {
        public float damage = 0;
        public List<Character> damaged;
        public string cue = "";

		/// <summary>
		/// Create a new explosion.
		/// </summary>
		public Explosion() : base()
		{
			Image image = new Image();
			image.filename = "Resources/Textures/ExplosionTable";
			image.blocks = new Vector2(4, 2);
			image.size = new Vector2(48, 48);
			Sprite sprite = new Sprite();
			sprite.image = image;
			sprite.visible = true;
			sprite.effect = "Resources/Effects/effects";
			sprite.animationStart = 0;
			sprite.animationEnd = 7;
			sprite.frame = 0;
			sprite.animationDelay = 0.035f;
			sprite.timer = 0;
			sprites.Add("explosion", sprite);
            collidable = false;
            damaged = new List<Character>();
		}

		/// <summary>
		/// Run the explosion's animation.
		/// </summary>
		/// <param name="gameTime">The current time.</param>
		/// <returns>False, if the animation has ended.</returns>
		public bool DoAnimation( GameTime gameTime )
		{
			int currentFrame = sprites["explosion"].frame;
			sprites["explosion"].animate( gameTime );
			if (sprites["explosion"].frame < currentFrame)
			{
				return false;
			}
			return true;
		}
	}
}
