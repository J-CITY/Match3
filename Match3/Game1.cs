using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using System.Diagnostics;

namespace Match3 {
	public class Game1 : Game {
		public static int screenWidth = 800, screenHeight = 600;
		public static Stack<Screen> screens = new Stack<Screen>();

		public SpriteBatch spriteBatch;
		public Texture2D textureBg, textureJelly,
			textureCell1, textureCell2, textureCellActive, textureBonus, texturePlayBtn, textureOkBtn;
		public SpriteFont font;

		private GraphicsDeviceManager graphics;
		private MouseState oldState = Mouse.GetState();

		public Game1() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize() {
			//screens.Push(new ScreenGame());
			screens.Push(new ScreenStartMenu(Game1.screenWidth, Game1.screenHeight));

			graphics.PreferredBackBufferWidth = screenWidth;
			graphics.PreferredBackBufferHeight = screenHeight;
			graphics.ApplyChanges();
			this.IsMouseVisible = true;
			base.Initialize();
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice);

			font = Content.Load<SpriteFont>("Font");

			textureBg = Content.Load<Texture2D>("sprite/bg/bg1");
			textureJelly = Content.Load<Texture2D>("sprite/jellys/Jelly");
			textureCell1 = Content.Load<Texture2D>("sprite/cell/cell1");
			textureCell2 = Content.Load<Texture2D>("sprite/cell/cell2");
			textureCellActive = Content.Load<Texture2D>("sprite/cell/cellActive");
			textureBonus = Content.Load<Texture2D>("sprite/jellys/BonusEffect");

			textureOkBtn = Content.Load<Texture2D>("sprite/ui/Ok");
			texturePlayBtn = Content.Load<Texture2D>("sprite/ui/Play");
		}

		protected override void UnloadContent() {
			Content.Unload();
		}

		protected override void Update(GameTime gameTime) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
				Exit();
			}

			MouseState newState = Mouse.GetState();
			if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released) {
				screens.Peek().MouseClick(new Vector2(newState.X, newState.Y));
			}
			oldState = newState;
			var delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			screens.Peek().Update(delta);
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);
			screens.Peek().Draw(this);
			base.Draw(gameTime);
		}
	}
}
