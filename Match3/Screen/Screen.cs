using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3
{
	public abstract class Screen {
		public Screen() { }
		public abstract void Update(float delta);
		public abstract void Draw(Game1 game);
		public abstract void MouseClick(Vector2 pos);
	}
}
