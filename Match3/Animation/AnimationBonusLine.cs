using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	class AnimationBonusLine : AnimationBonus {
		public Vector2 pos1, pos2;
		private Vector2 from, to1, to2;

		public AnimationBonusLine(Vector2 _from, Vector2 _to1, Vector2 _to2) {
			from = _from;
			to1 = _to1;
			to2 = _to2;
			pos1 = from;
			pos2 = from;
			//frames.Add(new Rectangle(269, 409, 68, 63));
			frames.Add(new Rectangle(89, 1231, 54, 54));
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
			pos1 = moveQuadratic(from, to1, speed, timeStart);
			pos2 = moveQuadratic(from, to2, speed, timeStart);
			Frame = frames[f];
		}

		private Vector2 moveQuadratic(Vector2 X0, Vector2 X1, float T, float t) {
			var f = t / T;
			if (f < 0) { f = 0f; isDone = true; }
			if (f > 1) { f = 1f; isDone = true; }
			f = f * f;
			return X0 * (1 - f) + X1 * f;
		}
	}
}
