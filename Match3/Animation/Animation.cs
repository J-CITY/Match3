using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	enum AnimationType { eLiner = 1, eQuadric, eQuadricInv, eCubic };

	class Animation {
		private bool isAllDone = false;
		public bool IsAllDone {
			get { return isAllDone; }
			set { isAllDone = value; }
		}

		private List<AnimationStep> steps;
		private int stepsI = 0;
		private bool isDone = false;
		private float startTime = 0;

		public Animation(List<AnimationStep> _steps) {
			steps = _steps;
			stepsI = 0;
			isDone = false;
			isAllDone = false;

		}

		public void Restart() {
			stepsI = 0;
			isDone = false;
			isAllDone = false;

		}

		public Vector2? DoStep(float _time) {
			if (isAllDone) {
				return null;
			}
			var res = new Vector2();

			var s = steps[stepsI];
			startTime += _time;

			switch (s.Type) {
				case AnimationType.eLiner:
				res = MoveLinear(s.XFrom, s.XTo, s.Time, startTime);
				break;
				case AnimationType.eQuadric:
				res = MoveQuadratic(s.XFrom, s.XTo, s.Time, startTime);
				break;
				case AnimationType.eQuadricInv:
				res = MoveQuadraticInv(s.XFrom, s.XTo, s.Time, startTime);
				break;
				case AnimationType.eCubic:
				res = MoveCubic(s.XFrom, s.XTo, s.Time, startTime);
				break;
			}

			if (isDone) {
				isDone = !isDone;
				stepsI++;
				if (stepsI == steps.Count) {
					isAllDone = true;
				}

			}
			return res;
		}

		// fom X0 в X1 for T
		private Vector2 MoveLinear(Vector2 X0, Vector2 X1, float T, float t) {
			var f = t / T;
			if (f < 0) { f = 0f; isDone = true; }
			if (f > 1) { f = 1f; isDone = true; }
			return X0 * (1f - f) + X1 * f;
		}
		
		private Vector2 MoveQuadratic(Vector2 X0, Vector2 X1, float T, float t) {
			var f = t / T;
			if (f < 0) { f = 0f; isDone = true; }
			if (f > 1) { f = 1f; isDone = true; }
			f = f * f;
			return X0 * (1 - f) + X1 * f;
		}

		private Vector2 MoveQuadraticInv(Vector2 X0, Vector2 X1, float T, float t) {
			var f = t / T;
			if (f < 0) { f = 0f; isDone = true; }
			if (f > 1) { f = 1f; isDone = true; }
			f = 1 - f;
			f = f * f;
			return X0 * f + X1 * (1 - f);
		}
		
		private Vector2 MoveCubic(Vector2 X0, Vector2 X1, float T, float t) {
			var f = t / T;
			if (f < 0) { f = 0f; isDone = true; }
			if (f > 1) { f = 1f; isDone = true; }
			f = f * f * (3 - 2 * f);
			return X0 * (1 - f) + X1 * f;
		}
	}
}
