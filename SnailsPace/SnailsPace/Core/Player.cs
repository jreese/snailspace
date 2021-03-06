using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SnailsPace.Objects;

namespace SnailsPace.Core
{
    class Player
    {
        private int points = 0;
		public static double time = 0;
		public static double timeLimit = 500;
		public static bool allowLevelProgression = false;
        public static Helix helix;
        public static GameObject crosshair;
        public static GameObject weapon;
        public static GameObject saveObject;

        public static bool dead = false;
        public static bool showgun = true;
        public double deathTimer = 0;

		private Text pointsText;
		private List<Text> strings;

		// Statistics
		private int shotsFired = 0;
		private int enemiesHit = 0;
		private int timesHit = 0;
		private int enemiesKilled = 0;
		public int deaths = 0;
		public String nextLevel = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
        public Player()
            : this(new Vector2(0, 0))
        {
        }

		/// <summary>
		/// Create a new player with a specified starting position and generic gun.
		/// </summary>
		/// <param name="startPosition">Coordinates at which to start the player.</param>
        public Player(Vector2 startPosition)
            : this(startPosition, "generic")
        {
        }

		/// <summary>
		/// Create a new player with a specified starting position and specified gun.
		/// </summary>
		/// <param name="startPosition">Coordinates at which to start the player.</param>
		/// <param name="weaponName">Name of the player's default weapon.</param>
        public Player(Vector2 startPosition, String weaponName)
			: this( startPosition, weaponName, null )
        {

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="startPosition"></param>
		/// <param name="weaponName"></param>
		public Player(Vector2 startPosition, String weaponName, String nextLevel)
			: base()
		{
			if (allowLevelProgression)
			{
				this.nextLevel = nextLevel;
			}
			else
			{
				this.nextLevel = null;
			}
			saveObject = new GameObject();
			saveObject.affectedByGravity = false;
			saveObject.collidable = false;

			save(startPosition);
			Weapon[] oldInventory = new Weapon[0];
			if (helix != null)
			{
				oldInventory = helix.inventory;
			}
			helix = new Helix(startPosition, weaponName);
			if (SnailsPace.cheatAllWeapons)
			{
				helix.AddWeapon(Weapon.load("stinger"));
				helix.weapon.ammunition = 50;
				helix.weapon.cooldown *= 0.25;
				helix.AddWeapon(Weapon.load("grenadelauncher"));
				helix.weapon.ammunition = 20;
				helix.weapon.cooldown *= 1;
				helix.AddWeapon(Weapon.load("minigun"));
				helix.weapon.ammunition = 200;
				helix.weapon.cooldown *= 4;
				helix.AddWeapon(Weapon.load("flamethrower"));
				helix.weapon.ammunition = 20;
				helix.weapon.cooldown *= 1;
				helix.AddWeapon(Weapon.load("generic"));
			}
			load();

			// Crosshair creation
			Sprite crosshairSprite = new Sprite();
			crosshairSprite.image = new Image();
			crosshairSprite.image.filename = "Resources/Textures/Crosshair";
			crosshairSprite.image.blocks = new Vector2(1.0f, 1.0f);
			crosshairSprite.image.size = new Vector2(64.0f, 64.0f);
			crosshairSprite.visible = true;
			crosshairSprite.effect = "Resources/Effects/effects";
			crosshair = new GameObject();
			crosshair.sprites = new Dictionary<string, Sprite>();
			crosshair.sprites.Add("Crosshair", crosshairSprite);
			crosshair.position = new Vector2(0.0f, 0.0f);
			crosshair.layer = 0;
			crosshair.collidable = false;

			// Weapon
			weapon = new GameObject();
			weapon.sprites = new Dictionary<string, Sprite>();
			weapon.sprites.Add("Weapon", helix.weapon.sprite);
			weapon.position = helix.position;
			weapon.layer = -5;
			weapon.collidable = false;

			strings = new List<Text>();
			pointsText = new Text();
			pointsText.position = new Vector2(400, 0);
			pointsText.font = SnailsPace.getInstance().Content.Load<SpriteFont>("Resources/Fonts/Score");
			pointsText.color = Color.White;
			pointsText.scale = Vector2.One;
			pointsText.content = points.ToString();
			strings.Add(pointsText);
			recalculatePoints();
			Engine.player = this;
			Renderer.cameraTarget = helix;
		}

		/// <summary>
		/// Run player logic. Check for death and update crosshair location.
		/// </summary>
		/// <param name="gameTime">The current time.</param>
        public void think(GameTime gameTime)
        {
			// Handle cheats.
			if (SnailsPace.cheatInfiniteHealth)
				helix.health = helix.maxHealth;

			if (SnailsPace.cheatInfiniteFuel)
				helix.fuel = helix.maxFuel;

            // Update things that depend on mouse position
			crosshair.position = Engine.mouseToGame(SnailsPace.inputManager.mousePosition);

            // Handle life and death
            if (helix.health <= 0)
            {
                if (!dead)
                {
                    dead = true;
                    deaths++;
                    deathTimer = 0;
                    helix.health = 0;
                    helix.fuel = 0;
                    helix.collidable = false;

                    helix.sprites["NoFuel"].visible = false;

					weapon.affectedByGravity = true;
					weapon.collidable = true;
					weapon.direction.Y = -1;
					weapon.acceleration = 384;
					weapon.maxVelocity = 1024;

                    Engine.sound.stop("alarm");
                    Engine.sound.stop("jetpack");

                    Engine.sound.play("death");
                }
                else
                {
                    deathTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
					weapon.rotation += .3f;

                    if (deathTimer > 3000)
                    {
                        dead = false;
						weapon.rotation = 0;
                        SnailsPace.inputManager.reset();

                        load();
                    }
                    else if (deathTimer > 2000)
                    {
						if (Renderer.cameraTarget == helix)
						{
							Renderer.cameraTarget = saveObject;
						}
                    }
                    else if (deathTimer > 1500)
                    {
                        Dictionary<String, Sprite>.Enumerator sprites = helix.sprites.GetEnumerator();
                        while (sprites.MoveNext())
                        {
                            sprites.Current.Value.visible = false;
                        }
                        showgun = false;
                    }
                }
            }
            else
            {
				dead = false;
                helix.think(gameTime);
            }
        }

		/// <summary>
		/// Return a list of all the player GameObjects.
		/// </summary>
		/// <returns>A list of all the player GameObjects.</returns>
        public List<GameObject> gameObjects()
        {
            List<GameObject> objects = new List<GameObject>();

            Sprite weaponSprite = helix.weapon.sprite.clone();

            weapon.horizontalFlip = weaponSprite.horizontalFlip = helix.horizontalFlip;
            weaponSprite.visible = showgun;
            weapon.sprites["Weapon"] = weaponSprite;
			if (!dead)
			{
				weapon.position = helix.position;
				weaponSprite.rotation = ((crosshair.position.X - helix.position.X) < 0 ? MathHelper.Pi : 0) + (float)Math.Atan((crosshair.position.Y - helix.position.Y) / (crosshair.position.X - helix.position.X));
			}

            objects.Add(helix);
            objects.Add(weapon);
            objects.Add(crosshair);

            return objects;
        }

        public List<Text> textStrings()
        {
            return strings;
        }

		/// <summary>
		/// Update this statistic and recalculate score.
		/// </summary>
        public void shotBullet()
        {
            shotsFired++;
            recalculatePoints();
        }

		/// <summary>
		/// Update this statistic and recalculate score.
		/// </summary>
        public void enemyHit()
        {
            enemiesHit++;
            recalculatePoints();
        }

		/// <summary>
		/// Update this statistic and recalculate score.
		/// </summary>
        public void gotHit()
        {
            timesHit++;
            recalculatePoints();
        }

		/// <summary>
		/// Update this statistic and recalculate score.
		/// </summary>
        public void killedEnemy()
        {
            enemiesKilled++;
            recalculatePoints();
        }

		/// <summary>
		/// Recalculate the player's score.
		/// </summary>
        public void recalculatePoints()
        {
            points = enemiesKilled * 100 + enemiesHit * 10;
            pointsText.content = "Score: " + points.ToString();
        }

		/// <summary>
		/// Generate the player's final score.
		/// </summary>
		/// <returns>The player's final score.</returns>
        public String GetFinalPoints(GameTime gameTime)
        {
			recalculatePoints();
            float accuracy = 0;
            if (shotsFired > 0)
            {
                accuracy = enemiesHit / (float)shotsFired;
            }
            float accuracyBonus = (float)Math.Floor( (0.5f + accuracy) * 100.0f ) / 100.0f;
			double timeBonus = 0;
			if((time / 1000) < timeLimit)
			{
				timeBonus = (timeLimit - time / 1000) * 50;
			}
			float deathPenalty = 0;
            if (deaths > 0)
            {
                deathPenalty = deaths * 500;
            }
            String pointsString = "Base points: " + points;
            pointsString +=     "\n   Accuracy: " + Math.Floor(accuracy * 100) + "% (x " + accuracyBonus + ")";
            pointsString +=     "\n     Deaths: " + deaths + " (- " + deathPenalty + ")";
			pointsString +=		"\n       Time: " + Math.Floor(time / 1000);
			pointsString +=		"\n Time Bonus: " + Math.Floor(timeBonus);
            pointsString +=     "\n  -------------------------";
            pointsString +=     "\n       Total: " + Math.Ceiling(points * accuracyBonus - deathPenalty + timeBonus);
            return pointsString;
        }

		/// <summary>
		/// Save the player's current position, for when they hit a checkpoint.
		/// </summary>
		/// <param name="position">The player's current position.</param>
        public void save(Vector2 position)
        {
            saveObject.position = position;
        }

		/// <summary>
		/// Load the player's last saved position. Called when the player dies.
		/// </summary>
        public void load()
        {
            helix.position = saveObject.position;
            helix.velocity = new Vector2();

            helix.health = helix.maxHealth;
            helix.fuel = helix.maxFuel;

            helix.collidable = true;
            helix.horizontalFlip = false;
            showgun = true;

            Engine.sound.play("ready");
			if (Renderer.cameraTarget == saveObject)
			{
				Renderer.cameraTarget = helix;
			}
		}
    }
}
