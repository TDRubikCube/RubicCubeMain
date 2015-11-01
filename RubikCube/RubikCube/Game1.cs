using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Xml;

namespace RubikCube
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region classes & xna.vars declaration

        readonly GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Music music;
        //save save;
        private Timer timer;
        FirstPopup popup;
        LoadingScreen loading;
        Thread loadingThread;
        ButtonSetUp button;
        SwitchGameState gameState;
        private SaveGame save;
        #endregion

        #region normal vars

        bool justFinshed;
        private bool isFirstTime = true;
        public const float OriginalScale = 3.4524f;
        public const float CubieSize = 1.918f;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            Window.AllowUserResizing = true;
            this.IsFixedTimeStep = false;
            loading = new LoadingScreen(Content);
            loadingThread = new Thread(Load);
            loadingThread.Start();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //sets the mouse handle
            Mouse.WindowHandle = Window.Handle;

            //loading logic
            if (!loadingThread.IsAlive)
            {
                //load bools from save file
                foreach (Tuple<string, string> b in save.LoadBools())
                {
                    if (b.Item1 == "isFirstTime")
                        isFirstTime = Convert.ToBoolean(b.Item2);
                }

                // checks if the loading & the timer are done
                if (justFinshed && timer.CallTimer(gameTime))
                {
                    MediaPlayer.Resume();
                    if (isFirstTime)
                    {
                        popup.Show();
                        save.AddBool("isFirstTime", "false");
                    }
                    justFinshed = false;
                }

                //neccesary updates
                button.BtnMute.Update(true, gameTime);
                button.BtnUnMute.Update(true, gameTime);
                MuteEvent();
                gameState.Update(gameTime, graphics);
            }
            else
            {
                //loading animation
                loading.Update(gameTime);
                //pause media player to prevent bugs
                if (MediaPlayer.State != MediaState.Paused)
                    MediaPlayer.Pause();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            if (!loadingThread.IsAlive)
            {
                //draw main game components
                gameState.Draw(spriteBatch, GraphicsDevice);
                spriteBatch.Begin();
                //draw mute/unmute button
                if (music.IsMuted)
                    button.BtnMute.Draw(spriteBatch);
                else
                    button.BtnUnMute.Draw(spriteBatch);
                spriteBatch.End();
            }
            //draw loading animation
            else
                loading.Draw(spriteBatch, GraphicsDevice);
            base.Draw(gameTime);
        }

        #region methods

        /// <summary>
        /// load all files
        /// </summary>
        private void Load()
        {
            gameState = new SwitchGameState(GraphicsDevice, graphics, Content);
            music = new Music(graphics, GraphicsDevice, Content);
            button = new ButtonSetUp(graphics, GraphicsDevice, Content);
            popup = new FirstPopup(gameState);
            timer = new Timer(200);
            save = new SaveGame("..\\..\\..\\save.xml", "root");
            justFinshed = true;
        }

        /// <summary>
        /// mute/unMute button logic
        /// </summary>
        private void MuteEvent()
        {
            if (button.BtnUnMute.IsClicked && MediaPlayer.State == MediaState.Playing)
            {
                music.IsMuted = true;
                MediaPlayer.Pause();
            }
            else if (button.BtnMute.IsClicked && MediaPlayer.State == MediaState.Paused)
            {
                music.IsMuted = false;
                MediaPlayer.Resume();
            }
        }

        #endregion
    }
}
