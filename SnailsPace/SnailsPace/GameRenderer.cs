using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnailsPace
{
    class GameRenderer
    {
        // Camera information
        public Vector3 cameraPosition;
        public Vector3 cameraTargetPosition;
        public Matrix cameraView;
		public Matrix cameraProjection;
		public const float normalCameraDistance = 5.0f;
		public const float minimumCameraMovement = 0.5f;
		public const float cameraSpeed = 2.5f;

		// Set distance from the camera of the near and far clipping planes.
		static float nearClip = 0.1f;
		static float farClip = 2000.0f;

		VertexPositionTexture[] vertices;

		private Dictionary<String, Texture2D> texture;

        public GameRenderer()
        {
			cameraPosition = new Vector3(-10, 0, normalCameraDistance);
			cameraTargetPosition = new Vector3(0, 0, normalCameraDistance);
			cameraView = Matrix.CreateLookAt(cameraPosition, cameraPosition + new Vector3(0,0,-1), Vector3.Up);
			setUpVertices();

			//TODO: Iterate through game objects, take filename from sprite image, create texture 2D for it, put into dictionary, maps image filename to texture2D

        }

		public void createTextures(List<Objects.GameObject> objects)
		{
			texture = new Dictionary<string, Texture2D>();
			List<Objects.GameObject>.Enumerator objectEnumerator = objects.GetEnumerator();
			while (objectEnumerator.MoveNext())
			{
				Dictionary<String, Objects.Sprite>.ValueCollection.Enumerator spriteEnumerator = objectEnumerator.Current.sprites.Values.GetEnumerator();
				while (spriteEnumerator.MoveNext())
				{
					ContentManager aLoader = SnailsPace.getInstance().Content;
					Texture2D temp = aLoader.Load<Texture2D>(spriteEnumerator.Current.image.filename) as Texture2D;
					texture.Add(spriteEnumerator.Current.image.filename, temp);
				}
			}
		}

        public void render(List<Objects.GameObject> objects, List<Objects.Text> strings, GameTime gameTime)
        {
			SnailsPace.getInstance().GraphicsDevice.Clear(Color.CornflowerBlue);
			if (!cameraPosition.Equals(cameraTargetPosition))
			{
                float elapsedTime = (float)Math.Min(gameTime.ElapsedRealTime.TotalSeconds, 1);
                float minMovement = elapsedTime * minimumCameraMovement;
                Vector3 cameraDifference = cameraTargetPosition - cameraPosition;
                Vector3 cameraPositionMovement = cameraDifference * cameraDifference.Length() * elapsedTime * cameraSpeed;
                if (minMovement > 0)
                {
                    if (Math.Abs(cameraPositionMovement.X) < minMovement)
                    {
                        cameraPositionMovement.X = minMovement;
                    }
                    if (Math.Abs(cameraPositionMovement.Y) < minMovement)
                    {
                        cameraPositionMovement.Y = minMovement;
                    }
                    if (Math.Abs(cameraPositionMovement.Z) < minMovement)
                    {
                        cameraPositionMovement.Z = minMovement;
                    }
                }
                cameraPosition = cameraPosition + cameraPositionMovement;
                if (minMovement > 0)
                {
                    if (Math.Abs(cameraDifference.X) < minMovement)
                    {
                        cameraPosition.X = cameraTargetPosition.X;
                    }
                    if (Math.Abs(cameraDifference.Y) < minMovement)
                    {
                        cameraPosition.Y = cameraTargetPosition.Y;
                    }
                    if (Math.Abs(cameraDifference.Z) < minMovement)
                    {
                        cameraPosition.Z = cameraTargetPosition.Z;
                    }
                }
            }
			Viewport viewport = SnailsPace.getInstance().GraphicsDevice.Viewport;
			float aspectRatio = (float)viewport.Width / (float)viewport.Height;
			cameraProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, nearClip, farClip);
			cameraView = Matrix.CreateLookAt(cameraPosition, cameraPosition + new Vector3(0, 0, -1), Vector3.Up);

			if (objects != null)
			{
				List<Objects.GameObject>.Enumerator objectEnumerator = objects.GetEnumerator();
				while (objectEnumerator.MoveNext())
				{
					Dictionary<String, Objects.Sprite>.ValueCollection.Enumerator spriteEnumerator = objectEnumerator.Current.sprites.Values.GetEnumerator();
					while (spriteEnumerator.MoveNext())
					{
						if (spriteEnumerator.Current.visible)
						{
							Vector3 objectPosition = new Vector3(objectEnumerator.Current.position, -objectEnumerator.Current.layer);
							Vector3 objectScale = new Vector3(1, 1, 1);

							Matrix worldMatrix = Matrix.CreateScale(objectScale) * Matrix.CreateRotationZ(objectEnumerator.Current.rotation) *
								Matrix.CreateTranslation(objectPosition);
							spriteEnumerator.Current.effect.CurrentTechnique = spriteEnumerator.Current.effect.Techniques["Textured"];
							spriteEnumerator.Current.effect.Parameters["xView"].SetValue(cameraView);
							spriteEnumerator.Current.effect.Parameters["xProjection"].SetValue(cameraProjection);
							spriteEnumerator.Current.effect.Parameters["xWorld"].SetValue(worldMatrix);

							// TODO: pull the appropraite part of the texture
							spriteEnumerator.Current.effect.Parameters["xTexture"].SetValue(texture[spriteEnumerator.Current.image.filename]);

							spriteEnumerator.Current.effect.Begin();
							foreach (EffectPass pass in spriteEnumerator.Current.effect.CurrentTechnique.Passes)
							{
								pass.Begin();
								SnailsPace.getInstance().GraphicsDevice.VertexDeclaration = new VertexDeclaration(SnailsPace.getInstance().GraphicsDevice, VertexPositionTexture.VertexElements);
								SnailsPace.getInstance().GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleStrip, vertices, 0, 2);
								pass.End();
							}
							spriteEnumerator.Current.effect.End();
						}
					}
				}
			}

			if (strings != null)
			{
				SpriteBatch batch = new SpriteBatch(SnailsPace.getInstance().GraphicsDevice);
				List<Objects.Text>.Enumerator textEnumerator = strings.GetEnumerator();
				batch.Begin();
				while (textEnumerator.MoveNext())
				{
					batch.DrawString(textEnumerator.Current.font, textEnumerator.Current.content, textEnumerator.Current.position, textEnumerator.Current.color, textEnumerator.Current.rotation, Vector2.Zero, textEnumerator.Current.scale, SpriteEffects.None, 0);
				}
				batch.End();
			}
        }

		private void setUpVertices()
		{
			vertices = new VertexPositionTexture[4];
			vertices[3].Position = new Vector3(-1.0f, 1.0f, 0.0f);
			vertices[3].TextureCoordinate.X = 0;
			vertices[3].TextureCoordinate.Y = 0;

			vertices[2].Position = new Vector3(1.0f, 1.0f, 0f);
			vertices[2].TextureCoordinate.X = 1;
			vertices[2].TextureCoordinate.Y = 0;

			vertices[1].Position = new Vector3(-1.0f, -1.0f, 0.0f);
			vertices[1].TextureCoordinate.X = 0;
			vertices[1].TextureCoordinate.Y = 1;

			vertices[0].Position = new Vector3(1.0f, -1.0f, 0.0f);
			vertices[0].TextureCoordinate.X = 1;
			vertices[0].TextureCoordinate.Y = 1;
		}

    }
}
