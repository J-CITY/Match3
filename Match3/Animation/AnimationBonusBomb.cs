using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	class AnimationBonusBomb : AnimationBonus {
		public Vector2 position;
		public Vector2 offset = new Vector2(-Cell.cellSize, -Cell.cellSize);

		public AnimationBonusBomb(Vector2 pos) {
			position = pos + new Vector2(Cell.cellSize / 4, Cell.cellSize / 4);
			frames.Add(new Rectangle(41, 2489, 33, 33));
			frames.Add(new Rectangle(81, 2470, 85, 71));
			frames.Add(new Rectangle(179, 2461, 95, 89));
			frames.Add(new Rectangle(288, 2459, 99, 93));
			frames.Add(new Rectangle(404, 2441, 129, 129));
			frames.Add(new Rectangle(547, 2442, 123, 127));
			frames.Add(new Rectangle(687, 2445, 121, 120));
			frames.Add(new Rectangle(828, 2439, 132, 132));

			Frame = frames[0];
		}

		public override void Update(float time) {
			if (delay > 0) {
				delay -= time;
				Frame = delayFrame;
				return;
			}
			timeStart += time;
			float fs = (speed / frames.Count);
			int f = 0;
			while (f * fs < timeStart) {
				f++;
			}
			if (f >= frames.Count) {
				f = frames.Count - 1;
			}
			if (timeStart >= speed) {
				isDone = true;
			}
			Frame = frames[f];

			offset = new Vector2(-Frame.Width / 2, 
				 -Frame.Height / 2);
		}
	}
}
