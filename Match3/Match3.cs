using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Match3 {
	class Match3 {
		public static int ColSize = 10;
		public static int RowSize = 10;
		public static int cellSize = Cell.cellSize;

		private int gameScore = 0;
		public int GameScore {
			get { return gameScore; }
			set { gameScore = value; }
		}

		private float gameTime = 60000;
		public float GameTime {
			get { return gameTime; }
			set { gameTime = value; }
		}

		private Cell[,] map = new Cell[ColSize, RowSize];
		private Random random = new Random();

		private bool isSwap = false;
		private bool isMove = false;
		private bool isStep = true;
		private int score = 0;
		private bool isGameOver = false;

		private Vector2 mousePos;
		private int mouseClick = 0;

		private Vector2 cell1 = new Vector2(0, 0);
		private Vector2 cell1Pos = new Vector2(0, 0);
		private Vector2 cell2 = new Vector2(0, 0);
		private Vector2 cell2Pos = new Vector2(0, 0);
		// for bonus animation
		private float delay = 0;

		public Match3() {
			for (int i = 1; i < ColSize - 1; i++) {
				for (int j = 1; j < RowSize - 1; j++) {
					Cell c = new Cell();
					c.Position = new Microsoft.Xna.Framework.Vector2(i * Cell.cellSize, j * Cell.cellSize);
					c.Col = i;
					c.Row = j;
					c.SetType(GetRandomCellType(), GetRandomCellTypeBonus());
					map[i, j] = c;
				}
			}
		}

		public void MouseClick(Vector2 pos) {
			if (!isSwap && !isMove) {
				mouseClick++;
			}
			mousePos = pos;

			if ((int)(mousePos.X / cellSize) >= ColSize-1 || (int)(mousePos.X / cellSize) < 1 ||
				(int)(mousePos.Y / cellSize) >= RowSize-1 || (int)(mousePos.Y / cellSize) < 1) {
				return;
			}

			if (mouseClick == 1) {
				cell1Pos = new Vector2(mousePos.X / cellSize,
					mousePos.Y / cellSize);
			}
			if (mouseClick == 2) {
				cell2Pos = new Vector2(mousePos.X / cellSize,
					mousePos.Y / cellSize);
				var _x = Math.Abs((int)cell2Pos.X - (int)cell1Pos.X);
				var _y = Math.Abs((int)cell2Pos.Y - (int)cell1Pos.Y);
				if (_x + _y == 1) {
					Swap(map[(int)cell1Pos.X, (int)cell1Pos.Y], map[(int)cell2Pos.X, (int)cell2Pos.Y]);
					isSwap = true;
					mouseClick = 0;
					isStep = true;
				} else {
					mouseClick = 0;
					cell1Pos = new Vector2(0, 0);
					cell2Pos = new Vector2(0, 0);
				}
			}
		}

		public List<Vector2> GetActivePositions() {
			return new List<Vector2>() { cell1Pos, cell2Pos };
		}

		public Cell GetCell(int x, int y) {
			return (x < ColSize && y < RowSize) ? map[x, y] : null;
		}

		public void Swap(Cell p1, Cell p2) {

			var col = p1.Col;
			p1.Col = p2.Col;
			p2.Col = col;

			var row = p1.Row;
			p1.Row = p2.Row;
			p2.Row = row;

			map[p1.Col, p1.Row] = p1;
			map[p2.Col, p2.Row] = p2;

			List<AnimationStep> _steps = new List<AnimationStep>();
			var posTo = new Vector2(p1.Col, p1.Row);
			_steps.Add(new AnimationStep(p1.Position, posTo * Cell.cellSize, 100f, AnimationType.eCubic));
			map[p1.Col, p1.Row].move = new Animation(_steps);

			List<AnimationStep> _steps2 = new List<AnimationStep>();
			var posTo2 = new Vector2(p2.Col, p2.Row);
			_steps2.Add(new AnimationStep(p2.Position, posTo2 * Cell.cellSize, 100f, AnimationType.eCubic));
			map[p2.Col, p2.Row].move = new Animation(_steps2);

		}

		public void Update(float time) {
			gameTime = gameTime < 0 ? 0 : gameTime - time;
			if (gameTime <= 0 && !isSwap && !isMove) {
				isGameOver = true;
			}
			if (isStep && !isMove) {
				FindMatch();
				isStep = false;
				score = GetScore();
				gameScore += score;
			}
			AnimationStep(time);

			if (isSwap && !isMove) {
				if (score == 0) {
					Swap(map[(int)cell1Pos.X, (int)cell1Pos.Y], map[(int)cell2Pos.X, (int)cell2Pos.Y]);
					cell1Pos = new Vector2(0, 0);
					cell2Pos = new Vector2(0, 0);
				} else {
					cell1Pos = new Vector2(0, 0);
					cell2Pos = new Vector2(0, 0);
				}
				isSwap = false;
			}
			UpdateMap();
		}

		private Type GetRandomCellType() {
			return (Type)random.Next(1, 5);
		}

		private TypeBonus GetRandomCellTypeBonus() {
			var r = random.Next(1, 100);
			if (r == 97) {
				return TypeBonus.Hline;
			}
			if (r == 98) {
				return TypeBonus.Vline;
			}
			if (r == 99) {
				return TypeBonus.Bomb;
			}
			return TypeBonus.Normal;
		}

		private void UpdateMap() {
			if (!isMove) {
				for (int i = RowSize-2; i >= 1; i--) {
					for (int j = 1; j <= ColSize-2; j++) {
						if (map[j, i].Match) {
							for (int n = i; n >= 1; n--) {
								if (!map[j, n].Match) {
									Swap(map[j, n], map[j, i]);
									break;
								}
							}
						}
					}
				}
				for (int j = 1; j <= ColSize-2; j++) {
					int n = 1;
					for (int i = RowSize-2; i > 0; i--) {
						if (map[j, i].Match) {
							map[j, i].Position = new Vector2(j * Cell.cellSize, 0);
							map[j, i].SetType(GetRandomCellType(), GetRandomCellTypeBonus());

							map[j, i].Col = j;
							map[j, i].Row = i;
							n++;
							map[j, i].Match = false;
							map[j, i].Transparency = 255;
						}
					}
				}
				isStep = true;
			}
			if (!isMove) {
				for (int i = 1; i <= ColSize - 2; i++) {
					for (int j = 1; j <= RowSize - 2; j++) {
						if ((int)(map[i, j].Position.X / Cell.cellSize) != map[i, j].Col ||
							(int)(map[i, j].Position.Y / Cell.cellSize) != map[i, j].Row) {
							List<AnimationStep> _steps = new List<AnimationStep>();
							_steps.Add(new AnimationStep(map[i, j].Position,
								new Vector2(map[i, j].Col * Cell.cellSize, map[i, j].Row * Cell.cellSize), 100f, AnimationType.eCubic));
							map[i, j].move = new Animation(_steps);
						}
					}
				}
			}
		}

		private int GetScore() {
			int score = 0;
			int x = 1;
			for (int i = 1; i <= ColSize-2; i++) {
				for (int j = 1; j <= RowSize-2; j++) {
					score += map[i, j].Match ? x : 0;
				}
			}
			x *= 2;
			return score;
		}

		public void ApplyBonus(int x, int y, TypeBonus type) {
			if (type == TypeBonus.Bomb) {
				for (int i = -1; i <= 1; ++i) {
					for (int j = -1; j <= 1; ++j) {
						if (map[x + i, y + j] != null && !map[x + i, y + j].Match) {
							map[x + i, y + j].Match = true;
							ApplyBonusAnimation(x + i, y + j);
							ApplyMatchAnimation(x + i, y + j);
							ApplyBonus(x + i, y + j, map[x + i, y + j].TypeBonus);
						}
					}
				}
			} else if (type == TypeBonus.Hline) {
				for (int i = 1; i <= ColSize-2; ++i) {
					if (map[i, y] != null && !map[i, y].Match) {
						map[i, y].Match = true;
						ApplyBonusAnimation(i, y);
						ApplyMatchAnimation(i, y);
						ApplyBonus(i, y, map[i, y].TypeBonus);
					}
				}
			} else if (type == TypeBonus.Vline) {
				for (int i = 1; i <= RowSize-2; ++i) {
					if (map[x, i] != null && !map[x, i].Match) {
						map[x, i].Match = true;
						ApplyBonusAnimation(x, i);
						ApplyMatchAnimation(x, i);
						ApplyBonus(x, i, map[x, i].TypeBonus);
					}
				}
			}
		}
		
		private void ApplyBonusAnimation(int i, int j) {
			switch (map[i, j].TypeBonus) {
				case TypeBonus.Bomb: {
					map[i, j].animationBonus = new AnimationBonusBomb(new Vector2(map[i, j].Col * Cell.cellSize,
						map[i, j].Row * Cell.cellSize));
					map[i, j].animationBonus.Delay = delay;
					delay += 250;
					break;
				}
				case TypeBonus.Hline: {
					map[i, j].animationBonus = new AnimationBonusLine(new Vector2(map[i, j].Col * Cell.cellSize,
						map[i, j].Row * Cell.cellSize),
						new Vector2(Cell.cellSize, map[i, j].Row * Cell.cellSize),
						new Vector2(Cell.cellSize * 8, map[i, j].Row * Cell.cellSize));
					map[i, j].animationBonus.Delay = delay;
					delay += 250;
					break;
				}
				case TypeBonus.Vline: {
					map[i, j].animationBonus = new AnimationBonusLine(new Vector2(map[i, j].Col * Cell.cellSize,
						map[i, j].Row * Cell.cellSize),
						new Vector2(map[i, j].Col * Cell.cellSize, Cell.cellSize),
						new Vector2(map[i, j].Col * Cell.cellSize, Cell.cellSize * 8));
					map[i, j].animationBonus.Delay = delay;
					delay += 250;
					break;
				}
			}
		}

		private void ApplyMatchAnimation(int i, int j) {
			if (map[i, j].delmove == null && map[i, j].alpha == null) {
				List<AnimationStep> _steps3 = new List<AnimationStep>();
				var posTo3 = new Vector2(Game1.screenWidth - 10, 10);
				_steps3.Add(new AnimationStep(map[i, j].Position, posTo3, 1000f, AnimationType.eCubic));
				map[i, j].delmove = new Animation(_steps3);
				List<AnimationStep> _steps4 = new List<AnimationStep>();
				var posTo4 = new Vector2(0, 0);
				_steps4.Add(new AnimationStep(new Vector2(map[i, j].Transparency, 0), posTo4, 1000f, AnimationType.eCubic));
				map[i, j].alpha = new Animation(_steps4);
			}
		}

		private void FindMatch() {
			delay = 0;
			for (int i = 1; i <= ColSize-2; i++) {
				for (int j = 1; j <= RowSize-2; j++) {
					int vCount = 0, hCount = 0;
					HashSet<Vector2> matchItems = new HashSet<Vector2>();

					if (map[i, j] != null && map[i, j].Match) {
						continue;
					}

					if (map[i, j]?.Type == map[i + 1, j]?.Type) {
						if (map[i, j]?.Type == map[i - 1, j]?.Type) {
							map[i, j].Match = true;
							matchItems.Add(new Vector2(map[i, j].Col, map[i, j].Row));
							hCount++;
							if (map[i, j].TypeBonus != TypeBonus.Normal) {
								ApplyBonus(i, j, map[i, j].TypeBonus);
								ApplyBonusAnimation(i, j);
							}
							ApplyMatchAnimation(i, j);

							for (int n = i - 1; n >= 1; n--) {
								if (map[i, j].Type != map[n, j].Type)
									break;
								map[n, j].Match = true;
								matchItems.Add(new Vector2(map[n, j].Col, map[n, j].Row));

								if (map[n, j].TypeBonus != TypeBonus.Normal) {
									ApplyBonus(n, j, map[n, j].TypeBonus);
									ApplyBonusAnimation(n, j);
								}
								ApplyMatchAnimation(n, j);
								hCount++;
							}
							for (int n = i + 1; n <= 8; n++) {
								if (map[i, j].Type != map[n, j].Type)
									break;
								map[n, j].Match = true;
								matchItems.Add(new Vector2(map[n, j].Col, map[n, j].Row));

								if (map[n, j].TypeBonus != TypeBonus.Normal) {
									ApplyBonus(n, j, map[n, j].TypeBonus);
									ApplyBonusAnimation(n, j);
								}
								ApplyMatchAnimation(n, j);
								hCount++;
							}
						}
					}
					if (map[i, j]?.Type == map[i, j + 1]?.Type) {
						if (map[i, j]?.Type == map[i, j - 1]?.Type) {
							vCount++;
							map[i, j].Match = true;
							matchItems.Add(new Vector2(map[i, j].Col, map[i, j].Row));

							if (map[i, j].TypeBonus != TypeBonus.Normal) {
								ApplyBonus(i, j, map[i, j].TypeBonus);
								ApplyBonusAnimation(i, j);
							}
							ApplyMatchAnimation(i, j);

							for (int n = j - 1; n >= 1; n--) {
								if (map[i, j].Type != map[i, n].Type) {
									break;
								}
								map[i, n].Match = true;
								matchItems.Add(new Vector2(map[i, n].Col, map[i, n].Row));

								if (map[i, n].TypeBonus != TypeBonus.Normal) {
									ApplyBonus(i, n, map[i, n].TypeBonus);
									ApplyBonusAnimation(i, n);
								}
								ApplyMatchAnimation(i, n);
								vCount++;
							}
							for (int n = j + 1; n <= 8; n++) {
								if (map[i, j].Type != map[i, n].Type) {
									break;
								}
								map[i, n].Match = true;
								matchItems.Add(new Vector2(map[i, n].Col, map[i, n].Row));

								if (map[i, n].TypeBonus != TypeBonus.Normal) {
									ApplyBonus(i, n, map[i, n].TypeBonus);
									ApplyBonusAnimation(i, n);
								}
								ApplyMatchAnimation(i, n);
								vCount++;
							}
						}
					}
					if (hCount == 4 && vCount == 0) {
						AddCombo(i, j, TypeBonus.Hline, matchItems);
					}
					if (hCount == 0 && vCount == 4) {
						AddCombo(i, j, TypeBonus.Vline, matchItems);
					}
					if (hCount + vCount >= 5) {
						AddCombo(i, j, TypeBonus.Bomb, matchItems);
					}
				}
			}
		}

		private void AddCombo(int i, int j, TypeBonus type, HashSet<Vector2> matchItems) {
			int x = i, y = j;
			if (matchItems.Contains(new Vector2((int)(cell1Pos.X), (int)(cell1Pos.Y)))) {
				x = (int)(cell1Pos.X);
				y = (int)(cell1Pos.Y);
			} else if (matchItems.Contains(new Vector2((int)(cell2Pos.X), (int)(cell2Pos.Y)))) {
				x = (int)(cell2Pos.X);
				y = (int)(cell2Pos.Y);
			}

			map[x, y].SetType(map[x, y].Type, type);
			
			ApplyBonusAnimation(x, y);
			ApplyMatchAnimation(x, y);
			ApplyBonus(x, y, map[x, y].TypeBonus);
		}


		private void AnimationStep(float time) {
			isMove = false;
			bool isM = false;
			for (int i = 1; i <= 8; i++) {
				for (int j = 1; j <= 8; j++) {
					if (map[i, j].move != null) {
						Vector2 res = (Vector2)map[i, j].move.DoStep(time);
						map[i, j].Position = res;
						if (!map[i, j].move.IsAllDone) {
							isMove = true;
							isM = true;

						} else {
							map[i, j].move = null;
						}
					}
				}
			}

			if (isM) {
				return;
			}

			isMove = false;
			bool isBonus = false;
			for (int i = 1; i <= 8; i++) {
				for (int j = 1; j <= 8; j++) {
					if (map[i, j].animationBonus != null) {
						map[i, j].animationBonus.Update(time);
						if (!map[i, j].animationBonus.isDone) {
							isMove = true;
							isBonus = true;

						} else {
							map[i, j].animationBonus = null;
						}
					}
				}
			}
			if (isBonus) {
				return;
			}

			//Deleting amimation
			if (!isMove) {
				for (int i = 1; i <= 8; i++) {
					for (int j = 1; j <= 8; j++) {
						if (map[i, j].Match) {
							if (map[i, j].alpha != null) {
								Vector2 res = (Vector2)map[i, j].alpha.DoStep(time);
								map[i, j].Transparency = res.X;

								if (!map[i, j].alpha.IsAllDone) {
									isMove = true;
								} else {
									map[i, j].alpha = null;
								}
							}
							if (map[i, j].delmove != null) {
								Vector2 res = (Vector2)map[i, j].delmove.DoStep(time);
								map[i, j].Position = res;
								if (!map[i, j].delmove.IsAllDone) {
									isMove = true;
								} else {
									map[i, j].delmove = null;
								}
							}
						}
					}
				}
			}
		}
	}
}
