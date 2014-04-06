using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Collections;
using GameName1.Interfaces;
using GameName1.NPCs;

namespace GameName1
{
	public class Camera
	{
		public Matrix transform;
		Vector2 center; 
		float cameraX, cameraY; 

		public Camera () {}

		public void Update(int numberOfPlayers, Player player, Rectangle LevelBounds) {
			int viewportWidth, viewportHeight; 

			float cameraWidth, cameraHeight, worldWidth, worldHeight; 

			if (numberOfPlayers == 1) {
				cameraWidth = Static.SCREEN_WIDTH;
				cameraHeight = Static.SCREEN_HEIGHT; 
				viewportWidth = Static.SCREEN_WIDTH;
				viewportHeight = Static.SCREEN_HEIGHT;
			}
			else if (numberOfPlayers == 2) {
				cameraWidth = Static.SCREEN_WIDTH / 2; 
				cameraHeight = Static.SCREEN_HEIGHT; 
				viewportWidth = Static.SCREEN_WIDTH / 2;	// add 80 because SCREEN_WIDTH is off for some reason
				viewportHeight = Static.SCREEN_HEIGHT;
			}
			else {
				cameraWidth = Static.SCREEN_WIDTH / 2; 
				cameraHeight = Static.SCREEN_HEIGHT / 2; 
				viewportWidth = Static.SCREEN_WIDTH / 2;
				viewportHeight = Static.SCREEN_HEIGHT / 2;
			}

			// keep player centered in camera
			center = new Vector2(
				player.x + (player.hitbox.Width / 2) - (viewportWidth / 2),		// camera in x dir
				player.y + (player.hitbox.Height / 2) - (viewportHeight / 2)	// camera in y dir 
				// 0
			);

			cameraWidth = viewportWidth;
			cameraHeight = viewportHeight; 

			cameraX = center.X;
			cameraY = center.Y; 
			worldWidth = LevelBounds.Width;
			worldHeight = LevelBounds.Height;

			// control how far the camera can scroll
			if (cameraX < 0)
				cameraX = 0;
			else if (cameraX + cameraWidth > worldWidth)	
				cameraX = worldWidth - cameraWidth;

			int reserved = 100; 

			if (cameraY < 0 - reserved) 
				cameraY = 0 - reserved;
			else if (cameraY + cameraHeight > worldHeight + reserved)	// add reserved here and player won't be able to move past center
				cameraY = worldHeight - cameraHeight + reserved;
				
			transform = Matrix.CreateScale(1f) * Matrix.CreateTranslation(new Vector3(-cameraX, -cameraY, 0)); 
		} 

		public float getScreenPositionX(float worldPosition) {
			return worldPosition - cameraX;
		}
		public float getScreenPositionY(float worldPosition) {
			return worldPosition - cameraY;
		}

		public float getWorldPositionX(float screenPosition) {
			return screenPosition + cameraX; 
		}
		public float getWorldPositionY(float screenPosition) {
			return screenPosition + cameraY; 
		}

	}

}

//public void Update(Vector2 position) {	// position is the player position
//	center = new Vector2(position.X, position.Y);
//	transform = Matrix.CreateScale(new Vector3(1,1,0)) * Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0));
//}

