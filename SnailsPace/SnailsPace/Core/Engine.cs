using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SnailsPace.Core
{
    class Engine
    {
        // Engine state
        Boolean enginePaused = false;

        // Game font
        public SpriteFont gameFont;
#if DEBUG
        public SpriteFont debugFont;
#endif

        // Game map
        public Objects.Map map;

        // Player
        public Objects.Helix helix;

        // Bullets
        public List<Objects.Bullet> bullets;

		// Pause Screen
		public Objects.GameObject pause;

		// Crosshair
		public Objects.GameObject crosshair;

		// Renderer
		public Renderer gameRenderer;

        // Constructors
        public Engine(String map)
        {
            GameLua lua = new GameLua();

			bullets = new List<Objects.Bullet>();
			
			// TODO: Load the map object from Lua
			this.map = new Objects.Map(map);
			
			// TODO: Initialize Helix;
            helix = new Objects.Helix();
            helix.sprites = new Dictionary<string, Objects.Sprite>();
            
			Objects.Sprite helSprite = new Objects.Sprite();
            helSprite.image = new Objects.Image();
            helSprite.image.filename = "Resources/Textures/HelixTable";
            helSprite.image.blocks = new Vector2(4.0f, 4.0f);
            helSprite.image.size = new Vector2(128.0f, 128.0f);
            helSprite.visible = true;
            helSprite.effect = "Resources/Effects/effects";

			Objects.Sprite gun = new Objects.Sprite();
			gun.image = helSprite.image;
			gun.visible = true;
			gun.effect = helSprite.effect;

			helix.sprites.Add("Snail", helSprite);
			helix.sprites["Snail"].animationStart = 0;
			helix.sprites["Snail"].animationEnd = 11;
			helix.sprites["Snail"].frame = 0;
			helix.sprites["Snail"].animationDelay = 1.0f / 15.0f;
			helix.sprites["Snail"].timer = 0f;

			helix.sprites.Add("Gun", gun);
			helix.sprites["Gun"].animationStart = 12;
			helix.sprites["Gun"].animationEnd = 15;
			helix.sprites["Gun"].frame = 12;
			helix.sprites["Gun"].animationDelay = 1.0f / 15.0f;
			helix.sprites["Gun"].timer = 0f;

            
            helix.velocity = new Vector2(3.0f, 2.0f);
			helix.layer = 0;

            loadFonts();
            setupPauseOverlay();
			setupCrosshair();
			setupGameRenderer();
        }

        private void setupPauseOverlay()
        {
            Objects.Sprite pauseSprite = new Objects.Sprite();
            pauseSprite.image = new Objects.Image();
            pauseSprite.image.filename = "Resources/Textures/PauseScreen";
            pauseSprite.image.blocks = new Vector2(1.0f, 1.0f);
            pauseSprite.image.size = new Vector2(800.0f, 600.0f);
            pauseSprite.visible = false;
            pauseSprite.effect = "Resources/Effects/effects";
            pause = new Objects.GameObject();
            pause.sprites = new Dictionary<string, Objects.Sprite>();
            pause.sprites.Add("Pause", pauseSprite);
            pause.position = new Vector2(0.0f, 0.0f);
            pause.layer = -3;
        }

		private void setupCrosshair()
		{
			Objects.Sprite crosshairSprite = new Objects.Sprite();
			crosshairSprite.image = new Objects.Image();
			crosshairSprite.image.filename = "Resources/Textures/Crosshair";
			crosshairSprite.image.blocks = new Vector2(1.0f, 1.0f);
			crosshairSprite.image.size = new Vector2(64.0f, 64.0f);
			crosshairSprite.visible = true;
			crosshairSprite.effect = "Resources/Effects/effects";
			crosshair = new Objects.GameObject();
			crosshair.sprites = new Dictionary<string, Objects.Sprite>();
			crosshair.sprites.Add("Crosshair", crosshairSprite);
			crosshair.position = new Vector2(0.0f, 0.0f);
			crosshair.layer = 0;
		}

		private void loadFonts()
		{
			gameFont = SnailsPace.getInstance().Content.Load<SpriteFont>("Resources/Fonts/Menu");
#if DEBUG
			debugFont = SnailsPace.getInstance().Content.Load<SpriteFont>("Resources/Fonts/Debug");
#endif
		}

		private void setupGameRenderer()
		{
			gameRenderer = new Renderer();
//            gameRenderer.createTexturesAndEffects(allObjects());

			Vector2 offsetPosition = new Vector2(50, 25);
			gameRenderer.cameraPosition = new Vector3( helix.position + offsetPosition, gameRenderer.cameraTargetOffset.Z * 1.5f);

			gameRenderer.cameraTarget = helix;
			gameRenderer.cameraTargetOffset.X = -2;
			gameRenderer.cameraTargetOffset.Y = 6;
		}

        public void think(GameTime gameTime)
        {
            Input input = SnailsPace.inputManager;

            if (input.inputPressed("Pause"))
            {
                enginePaused = !enginePaused;
            }
            if (input.inputPressed("MenuToggle"))
            {
                enginePaused = true;
                SnailsPace.getInstance().changeState(SnailsPace.GameStates.MainMenu);
            }

			pause.sprites["Pause"].visible = enginePaused;

			if (enginePaused)
			{
				pause.position.X = gameRenderer.cameraPosition.X;
				pause.position.Y = gameRenderer.cameraPosition.Y;
				return;
			}


            // TODO: iterate through map.characters calling think() on each one.
			List<Objects.Character>.Enumerator charEnum = map.characters.GetEnumerator();
			while (charEnum.MoveNext())
			{
				charEnum.Current.think(gameTime);
			}


			if (input.inputDown("Left") && input.inputDown("Right"))
			{
				// do nothing
			} else if (input.inputDown("Left"))
			{
                float movement = helix.velocity.X * Math.Min((float)gameTime.ElapsedRealTime.TotalSeconds, 1);
				helix.position.X -= movement;
				helix.sprites["Snail"].animate(gameTime);

			} else if (input.inputDown("Right"))
            {
                float movement = helix.velocity.X * Math.Min((float)gameTime.ElapsedRealTime.TotalSeconds, 1);
                helix.position.X += movement;
				helix.sprites["Snail"].animate(gameTime);
            }

			if (input.inputDown("Up") && input.inputDown("Down"))
			{
				//do nothing
			} else if (input.inputDown("Up"))
            {
                float movement = helix.velocity.Y * Math.Min((float)gameTime.ElapsedRealTime.TotalSeconds, 1);
                helix.position.Y += movement;

            } else if (input.inputDown("Down"))
            {
                float movement = helix.velocity.Y * Math.Min((float)gameTime.ElapsedRealTime.TotalSeconds, 1);
                helix.position.Y -= movement;
            }

			// Update things that depend on mouse position
			crosshair.position.X = mouseToScreenX(input.MouseX);
			crosshair.position.Y = mouseToScreenY(input.MouseY);
			helix.sprites["Gun"].rotation = ((crosshair.position.X - helix.position.X) < 0 ? MathHelper.Pi : 0 ) + (float)Math.Atan((crosshair.position.Y - helix.position.Y) / (crosshair.position.X - helix.position.X));

            // TODO: handle player inputs to change Helix's attributes.
			helix.think(gameTime);
        }

		private float mouseToScreenX(int mouseX)
		{
			return mouseX / gameRenderer.cameraPosition.Z + gameRenderer.cameraPosition.X - 16;
		}

		private float mouseToScreenY(int mouseY)
		{
			return -mouseY / gameRenderer.cameraPosition.Z + gameRenderer.cameraPosition.Y + 12;
		}

		public void physics(GameTime gameTime)
        {
            if (enginePaused)
            {
                return;
            }

			List<Objects.GameObject>.Enumerator objEnumerator = this.map.objects.GetEnumerator();
			while (objEnumerator.MoveNext())
			{
				Dictionary<string, Objects.Sprite>.ValueCollection.Enumerator sprtEnumerator = objEnumerator.Current.sprites.Values.GetEnumerator();
				while (sprtEnumerator.MoveNext())
				{
					sprtEnumerator.Current.animate(gameTime);
				}
			}

            // TODO: iterate through map.characters and this.bullets using collision detection to move everything.

            // TODO: iterate through map.triggers and map.characters to find which triggers to execute
        }

		public void render(GameTime gameTime)
        {
            // TODO: iterate through map.objects, map.characters, and this.bullets to gather all visible sprites
            // and then send the list of sprites to the rendering system.
			// TODO: add bullets
            List<Objects.Text> strings = new List<Objects.Text>();

#if DEBUG
            int numDebugStrings = 0;
            if (SnailsPace.debugHelixPosition)
            {
                Objects.Text debugString = new Objects.Text();
                debugString.color = Color.Yellow;
                debugString.content = "Helix: (" + helix.position.X + ", " + helix.position.Y + ")";
                debugString.font = debugFont;
                debugString.position = new Vector2(2 * debugFont.Spacing, debugFont.Spacing + numDebugStrings++ * debugFont.LineSpacing);
                debugString.rotation = 0;
                debugString.scale = Vector2.One;
                strings.Add(debugString);
            }
            if (SnailsPace.debugCameraPosition)
            {
                Objects.Text debugString = new Objects.Text();
                debugString.color = Color.Yellow;
                debugString.content = "Camera: (" + gameRenderer.cameraPosition.X + ", " + gameRenderer.cameraPosition.Y + ", " + gameRenderer.cameraPosition.Z + ")";
                debugString.font = debugFont;
                debugString.position = new Vector2(2 * debugFont.Spacing, debugFont.Spacing + numDebugStrings++ * debugFont.LineSpacing);
                debugString.rotation = 0;
                debugString.scale = Vector2.One;
                strings.Add(debugString);

                debugString = new Objects.Text();
                debugString.color = Color.Yellow;
				Vector3 cameraTargetPosition = gameRenderer.getCameraTargetPosition();
				debugString.content = "Target: (" + cameraTargetPosition.X + ", " + cameraTargetPosition.Y + ", " + cameraTargetPosition.Z + ")";
                debugString.font = debugFont;
                debugString.position = new Vector2(2 * debugFont.Spacing, debugFont.Spacing + numDebugStrings++ * debugFont.LineSpacing);
                debugString.rotation = 0;
                debugString.scale = Vector2.One;
                strings.Add(debugString);

                debugString = new Objects.Text();
                debugString.color = Color.Yellow;
				Vector3 distance = cameraTargetPosition - gameRenderer.cameraPosition;
                debugString.content = "Distance: (" + distance.X + ", " + distance.Y + ", " + distance.Z + ")";
                debugString.font = debugFont;
                debugString.position = new Vector2(2 * debugFont.Spacing, debugFont.Spacing + numDebugStrings++ * debugFont.LineSpacing);
                debugString.rotation = 0;
                debugString.scale = Vector2.One;
                strings.Add(debugString);

				debugString = new Objects.Text();
				debugString.color = Color.Yellow;
				debugString.content = "Crosshair: (" + crosshair.position.X + ", " + crosshair.position.Y + ")";
				debugString.font = debugFont;
				debugString.position = new Vector2(2 * debugFont.Spacing, debugFont.Spacing + numDebugStrings++ * debugFont.LineSpacing);
				debugString.rotation = 0;
				debugString.scale = Vector2.One;
				strings.Add(debugString);
            }
#endif
			gameRenderer.render(allObjects(), strings, gameTime);
        }

		private List<Objects.GameObject> allObjects()
		{
			List<Objects.GameObject> objects = new List<Objects.GameObject>(map.objects);
			objects.Add(helix);
			objects.Add(pause);
			objects.Add(crosshair);
			return objects;
		}
    }
}