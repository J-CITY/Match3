using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	public class ScreenGameOver : Screen {
		private Rectangle btn;
		private bool isBtnPress = false;
		private int gameScore = 0;
		private int btnWidth = 306, btnHeight = 148;

		public ScreenGameOver(int w, int h, int score) {
			gameScore = score;
			int x = w / 2 - btnWidth / 2;
			int y = h / 2 - btnHeight / 2;
			btn = new Rectangle(x, y, btnWidth, btnHeight);
		}

		public override void Draw(Game1 game) {
			game.spriteBatch.Begin();

			game.spriteBatch.Draw(game.textureBg,
				new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight), Color.White);
			game.spriteBatch.Draw(game.textureOkBtn,
				btn, Color.White);

			string scoreText = "GAME OVER! Your score: " + gameScore;
			Vector2 size = game.font.MeasureString(scoreText);
			game.spriteBatch.DrawString(game.font, scoreText,
				new Vector2(Game1.ScreenWidth / 2 - size.X / 2, 10), Color.White);

			game.spriteBatch.End();
		}

		public override void MouseClick(Vector2 pos) {
			if (pos.X >= btn.X && pos.X <= btn.X + btn.Width &&
				pos.Y >= btn.Y && pos.Y <= btn.Y + btn.Height) {
				isBtnPress = true;
			}
		}

		public override void Update(float delta) {
			if (isBtnPress) {
				Game1.Screens.Pop();
				Game1.Screens.Push(new ScreenStartMenu(Game1.ScreenWidth, Game1.ScreenHeight));
			}
		}
	}
}
