using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3 {
	public abstract class AnimationBonus {
		private Rectangle frame;
		public Rectangle Frame {
			get { return frame; }
			set { frame = value; }
		}

		public bool isDone = false;
		public bool IsDone {
			get { return isDone; }
			set { isDone = value; }
		}

		protected List<Rectangle> frames = new List<Rectangle>();
		protected float speed = 1000;
		protected float timeStart = 0;
		protected float delay = 0;
		public float Delay {
			get { return delay; }
			set { delay = value; }
		}

		public AnimationBonus() { }
		public abstract void Update(float time);

		protected Rectangle delayFrame = new Rectangle(0, 0, 1, 1);
	}
}
