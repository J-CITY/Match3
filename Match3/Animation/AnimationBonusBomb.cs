using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	class AnimationBonusBomb : AnimationBonus {
		public Vector2 position;
		public Vector2 offset = new Vector2(-55, -55);

		public AnimationBonusBomb(Vector2 pos) {
			position = pos;
			frames.Add(new Rectangle(40, 2487, 34, 236));
			frames.Add(new Rectangle(81, 2466, 85, 75));
			frames.Add(new Rectangle(178, 2455, 94, 97));
			frames.Add(new Rectangle(285, 2450, 100, 100));
			frames.Add(new Rectangle(400, 2432, 140, 140));
			frames.Add(new Rectangle(546, 2438, 115, 145));
			frames.Add(new Rectangle(685, 2441, 100, 135));
			frames.Add(new Rectangle(825, 2435, 110, 140));

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
		}
	}
}
