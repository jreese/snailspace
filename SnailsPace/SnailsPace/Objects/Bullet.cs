using System;
using System.Collections.Generic;
using System.Text;
using SnailsPace.Core;
using Microsoft.Xna.Framework;

namespace SnailsPace.Objects
{
    class Bullet : GameObject
	{
		#region Bullet properties
		public float damage;
        public Explosion explosion;
        public bool destroy = true;
        public int hits = 0;
        public Vector2 createPosition;
        public float range = 1000;

		// Was this bullet shot by a Player Character?
		public bool isPCBullet = false;
		#endregion

		/// <summary>
		/// Create a new bullet.
		/// </summary>
		public Bullet() : base()
		{
			horizontalFriction = 0;
            createPosition = Player.helix.position;
		}

		/// <summary>
		/// Decide if a collision can occur between this bullet and another object.
		/// </summary>
		/// <param name="otherObject">The object that this bullet collided with.</param>
		/// <returns>Whether or not a collision should occur.</returns>
		public override bool canCollideWith(GameObject otherObject)
		{
			// Note: This should be in the order of most specific to least specific (ie: Helix before character), otherwise more specific cases will be missed
			if (otherObject == null)
			{
				return true;
			}
			else if (otherObject is Helix)
			{
				// Helix's own bullets can't hurt him.
				if (isPCBullet)
				{
					return false;
				}
			}
			else if (otherObject is Character)
			{
				// Enemies' own bullets can't hurt other enemies.
				if (!isPCBullet)
				{
					return false;
				}
			}
			else if (otherObject is Objects.Bullet)
			{
				// Bullets can't collide with each other.
				return false;
			}

			return base.canCollideWith(otherObject);
		}

		/// <summary>
		/// Action performed when this bullet collides with another object.
		/// </summary>
		/// <param name="otherObject">The object that this bullet collided with.</param>
		public override void collidedWith(GameObject otherObject)
		{
			// Assumes canCollideWith
            if (otherObject is Character)
			{
				hits++;
                if (hits == 1)
                {
                    ((Character)otherObject).takeDamage(damage);
                }
                else
                {
                    ((Character)otherObject).takeDamage(damage / ( hits * 3 ), false);
                }
			}
			else if (otherObject is GameObject && otherObject.name == "fallingPlatform")
			{
				// Special case for falling platforms in Garden.lua.
				// Make platforms drop when shot.

				((GameObject)otherObject).affectedByGravity = true;
				if (((GameObject)otherObject).sprites.ContainsKey("Pour"))
				{
					((GameObject)otherObject).sprites["Pour"].visible = false;
				}
			}
		}
	}
}
