using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	class AnimationStep {
		private Vector2 xFrom = new Vector2();
		public Vector2 XFrom {
			get { return xFrom; }
			set { xFrom = value; }
		}

		private Vector2 xTo = new Vector2();
		public Vector2 XTo {
			get { return xTo; }
			set { xTo = value; }
		}

		private float time = 0f;
		public float Time {
			get { return time; }
			set { time = value; }
		}

		private AnimationType type;
		public AnimationType Type {
			get { return type; }
			set { type = value; }
		}

		public AnimationStep() { }
		public AnimationStep(Vector2 _X0, Vector2 _X1, float _T, AnimationType _type) {
			xFrom = _X0;
			xTo = _X1;
			time = _T;
			type = _type;

		}
	}
}
