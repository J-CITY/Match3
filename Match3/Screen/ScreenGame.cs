using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	public class ScreenGame : Screen {
		private Match3 match3;

		public ScreenGame() {
			match3 = new Match3();
		}

		public override void Draw(Game1 game) {
			game.spriteBatch.Begin();
			// draw background
			game.spriteBatch.Draw(game.textureBg,
				new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);

			for (int i = 1; i < Match3.ColSize - 1; i++) {
				for (int j = 1; j < Match3.RowSize - 1; j++) {
					if ((i + j) % 2 == 0) {
						game.spriteBatch.Draw(game.textureCell1,
							new Rectangle(i * Cell.cellSize, j * Cell.cellSize, Cell.cellSize, Cell.cellSize), Color.White);
					} else {
						game.spriteBatch.Draw(game.textureCell2,
							new Rectangle(i * Cell.cellSize, j * Cell.cellSize, Cell.cellSize, Cell.cellSize), Color.White);
					}
				}
			}

			var activeCell = match3.GetActivePositions();
			foreach (Vector2 v in activeCell) {
				if (v.X != 0 && v.Y != 0) {
					game.spriteBatch.Draw(game.textureCellActive,
						new Rectangle((int)v.X * Cell.cellSize, (int)v.Y * Cell.cellSize, Cell.cellSize, Cell.cellSize), Color.White);
				}
			}

			for (int i = 1; i < Match3.ColSize - 1; i++) {
				for (int j = 1; j < Match3.RowSize - 1; j++) {
					var c = match3.GetCell(i, j);
					if (c != null) {
						var r = GetSprite(c.Type, c.TypeBonus);
						game.spriteBatch.Draw(game.textureJelly, c.Position + c.Offset,
							r,
							Color.White * c.Transparency, 0, Vector2.Zero,
							new Vector2(0.15f, 0.15f), SpriteEffects.None, 0);
					}
				}
			}

			//draw bonus if need
			for (int i = 1; i < Match3.ColSize - 1; i++) {
				for (int j = 1; j < Match3.RowSize - 1; j++) {
					var c = match3.GetCell(i, j);
					if (c != null && c.animationBonus != null) {
						if (c.TypeBonus == TypeBonus.Bomb) {
							var pos = ((AnimationBonusBomb)c.animationBonus).position;
							var offset = ((AnimationBonusBomb)c.animationBonus).offset;
							var frame = c.animationBonus.Frame;
							game.spriteBatch.Draw(game.textureBonus, pos + offset,
								frame,
								Color.White, 0, Vector2.Zero,
								new Vector2(1.2f, 1.2f), SpriteEffects.None, 0);
						} else {
							var pos1 = ((AnimationBonusLine)c.animationBonus).pos1;
							var pos2 = ((AnimationBonusLine)c.animationBonus).pos2;

							var frame = c.animationBonus.Frame;
							game.spriteBatch.Draw(game.textureBonus, pos1,
								frame,
								Color.White, 0, Vector2.Zero,
								new Vector2(1f, 1f), SpriteEffects.None, 0);
							game.spriteBatch.Draw(game.textureBonus, pos2,
								frame,
								Color.White, 0, Vector2.Zero,
								new Vector2(1f, 1f), SpriteEffects.None, 0);
						}
					}
				}
			}

			//draw score & time
			string scoreText = "Score: " + match3.GameScore;
			Vector2 size = game.font.MeasureString(scoreText);
			game.spriteBatch.DrawString(game.font, scoreText,
				new Vector2(Game1.screenWidth - size.X - 10, 10), Color.White);

			string timeText = "" + (int)(match3.GameTime / 1000);
			size = game.font.MeasureString(timeText);
			game.spriteBatch.DrawString(game.font, timeText,
				new Vector2(Game1.screenWidth - 200, 200), Color.White, 0, Vector2.Zero,
				new Vector2(2f, 2f), SpriteEffects.None, 0);

			game.spriteBatch.End();
		}

		public override void MouseClick(Vector2 pos) {
			match3.MouseClick(pos);
		}

		public override void Update(float delta) {
			if (match3.GameTime <= 0) {
				Game1.screens.Pop();
				Game1.screens.Push(new ScreenGameOver(Game1.screenWidth, Game1.screenHeight, match3.GameScore));
			}
			match3.Update(delta);
		}

		private Rectangle GetSprite(Type type, TypeBonus typeBonus) {
			if (typeBonus == TypeBonus.Normal) {
				Dictionary<Type, Rectangle> dict = new Dictionary<Type, Rectangle>
				{
					{ Type.Yellow, new Rectangle(0, 10, 334, 340) },
					{ Type.Green, new Rectangle(339, 10, 334, 340) },
					{ Type.Red, new Rectangle(682, 10, 334, 340) },
					{ Type.Blue, new Rectangle(1040, 10, 334, 340) },
					{ Type.Pink, new Rectangle(1393, 10, 334, 340) }
				};
				return dict[type];
			} else if (typeBonus == TypeBonus.Hline) {
				Dictionary<Type, Rectangle> dict = new Dictionary<Type, Rectangle>
				{
					{ Type.Yellow, new Rectangle(32, 2380, 231, 221) },
					{ Type.Green, new Rectangle(362, 2380, 231, 221) },
					{ Type.Red, new Rectangle(731, 2380, 231, 221) },
					{ Type.Blue, new Rectangle(1095, 2380, 231, 221) },
					{ Type.Pink, new Rectangle(1425, 2380, 231, 221) }
				};
				return dict[type];
			} else if (typeBonus == TypeBonus.Vline) {
				Dictionary<Type, Rectangle> dict = new Dictionary<Type, Rectangle>
				{
					{ Type.Yellow, new Rectangle(34, 2090, 221, 231) },
					{ Type.Green, new Rectangle(359, 2090, 221, 231) },
					{ Type.Red, new Rectangle(731, 2090, 221, 231) },
					{ Type.Blue, new Rectangle(1095, 2090, 221, 231) },
					{ Type.Pink, new Rectangle(1425, 2090, 221, 231) }
				};
				return dict[type];
			} else {
				Dictionary<Type, Rectangle> dict = new Dictionary<Type, Rectangle>
				{
					{ Type.Yellow, new Rectangle(34, 1760, 230, 280) },
					{ Type.Green, new Rectangle(365, 1760, 230, 280) },
					{ Type.Red, new Rectangle(730, 1760, 230, 280) },
					{ Type.Blue, new Rectangle(1097, 1760, 230, 280) },
					{ Type.Pink, new Rectangle(1425, 1760, 230, 280) }
				};
				return dict[type];
			}
		}
	}
}
