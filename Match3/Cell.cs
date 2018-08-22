using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using System.Diagnostics;

namespace Match3 {
	enum Type { Yellow = 1, Green, Red, Blue, Pink };
	enum TypeBonus { Normal = 1, Hline, Vline, Bomb };

	class Cell {
		public static int cellSize = 55;
		//move
		public Animation move = null;
		//delete
		public Animation delmove = null;
		public Animation alpha = null;
		//bonus
		public AnimationBonus animationBonus = null;

		private float transparency = 1;
		public float Transparency {
			get { return transparency; }
			set { transparency = value; }
		}

		private Vector2 position;
		public Vector2 Position {
			get { return position; }
			set { position = value; }
		}

		private Vector2 offset;
		public Vector2 Offset {
			get { return offset; }
			set { offset = value; }
		}

		private int row;
		public int Row {
			get { return row; }
			set { row = value; }
		}

		private int col;
		public int Col {
			get { return col; }
			set { col = value; }
		}

		private Type type;
		public Type Type {
			get { return type; }
			set { type = value; }
		}

		private TypeBonus typeBonus;
		public TypeBonus TypeBonus {
			get { return typeBonus; }
			set { typeBonus = value; }
		}

		private bool match;
		public bool Match {
			get { return match; }
			set { match = value; }
		}

		public Cell() {
			match = false;
			offset = new Vector2(3, 3);
		}

		public void SetType(Type t, TypeBonus tb) {
			type = t;
			typeBonus = tb;
			if (typeBonus != TypeBonus.Normal) {
				offset = new Vector2(8, 8);
			} else {
				offset = new Vector2(3, 3);
			}
		}
	}
}
