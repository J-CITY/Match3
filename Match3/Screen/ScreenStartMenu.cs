using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	public class ScreenStartMenu : Screen {
		private Rectangle btn;
		private bool isBtnPress = false;
		private int btnWidth = 306, btnHeight = 148;

		public ScreenStartMenu(int w, int h) {
			int x = w / 2 - btnWidth / 2;
			int y = h / 2 - btnHeight / 2;
			btn = new Rectangle(x, y, btnWidth, btnHeight);
		}

		public override void Draw(Game1 game) {
			game.spriteBatch.Begin();

			game.spriteBatch.Draw(game.textureBg,
				new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);
			game.spriteBatch.Draw(game.texturePlayBtn,
				btn, Color.White);

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
				Game1.screens.Pop();
				Game1.screens.Push(new ScreenGame());
			}
		}
	}
}
