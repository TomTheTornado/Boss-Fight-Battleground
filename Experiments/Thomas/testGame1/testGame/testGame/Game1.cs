﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace testGame
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D target_Sprite;
        Texture2D crosshair_Sprite;
        Texture2D background_Sprite;

        //Constructor for the game
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
        }

        //Starts when the game starts
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        //Load images and sounds, etc.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            target_Sprite = Content.Load<Texture2D>("target");
            crosshair_Sprite = Content.Load<Texture2D>("circle");
            background_Sprite = Content.Load<Texture2D>("clouds");

            // TODO: use this.Content to load your game content here
        }

        //To remove assets not present in certain level or game
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //Game loop, runs every frame
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        //Anything that involves drawing images or text to the screen
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(target_Sprite, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
