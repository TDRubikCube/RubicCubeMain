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
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace RubikCube
{
    public enum GameState
    {
        MainMenu, Tutorial, FreePlay, Options
    }
    public class SwitchGameState
    {
        #region classes & xna.vars declaration

        readonly Cube cube;
        readonly ButtonSetUp button;
        readonly Text lang;
        readonly Matrix world;
        private Camera camera;
        Matrix view;
        readonly Matrix projection;
        private Vector3 cameraPos;
        KeyboardState oldKeyboardState;
        readonly SpriteFont font;
        //String firstText;
        //String typedText;
        Point mousePos;

        #endregion

        #region normal vars
        private float currentScale;
        public bool JustSwitched = false;
        public string AlgOrder = "";
        //bool stopAnim = false;
        int rotationsLeft = 0;
        bool shouldRotate;
        //double typedTextLength;
        //int delayInMilliseconds;
        //bool isDoneDrawing;
        //bool lockScreen = true;
        //string whichGenre = "default";
        public GameState CurrentGameState;

        #endregion

        public SwitchGameState(GraphicsDevice graphicsDevice, GraphicsDeviceManager graphics, ContentManager content)
        {
            //class initialize
            cube = new Cube();
            lang = new Text();
            camera = new Camera();
            button = new ButtonSetUp(graphics, graphicsDevice, content)
            {
                ClassicBound =
                    new Rectangle((int)(graphicsDevice.Viewport.Width / 1.32f), graphicsDevice.Viewport.Height / 3, 60, 40),
                RockBound =
                    new Rectangle((int)(graphicsDevice.Viewport.Width / 1.55f), graphicsDevice.Viewport.Height / 3, 50, 40)
            };
            cube.Model = content.Load<Model>("rubik");

            //text
            font = content.Load<SpriteFont>("font");

            //matrixes
            world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            view = camera.View;
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), graphicsDevice.Viewport.AspectRatio, 10f, 200f);
        }

        #region private methods

        /// <summary>
        /// the main rotation function
        /// </summary>
        /// <param name="keyboardState"></param>
        /// <param name="oldKeyboardState"></param>
        /// <param name="cameraPos"></param>
        private void RotateWhichSide(KeyboardState keyboardState, KeyboardState oldKeyboardState, Vector3 cameraPos)
        {
            if (cube.Angle <= -100)
            {
                Debug.WriteLine("Original=   " + AlgOrder);
                if (AlgOrder.Length > 1)
                {
                    if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                    {
                        AlgOrder = AlgOrder.Substring(2);
                    }
                    else
                    {
                        AlgOrder = AlgOrder.Substring(1);
                    }
                    Debug.WriteLine("After Change" + AlgOrder);
                }
                else
                {
                    AlgOrder = "";
                }
                camera.RealRotate(cameraPos);
                rotationsLeft = AlgOrder.Length;
                if (AlgOrder.Split('i').Length != -1)
                {
                    rotationsLeft -= AlgOrder.Split('i').Length - 1;

                }
                if (AlgOrder.Split('I').Length != -1)
                {
                    rotationsLeft -= AlgOrder.Split('I').Length - 1;

                }
                if (AlgOrder.Split('\'').Length != -1)
                {
                    rotationsLeft -= AlgOrder.Split('\'').Length - 1;

                }
                Debug.WriteLine("Number of rotations left:" + rotationsLeft);
                cube.Angle = 0;
            }
            if (keyboardState.IsKeyDown(Keys.L) && oldKeyboardState.IsKeyUp(Keys.L))
            {
                AlgOrder += "L";
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
            }
            if (keyboardState.IsKeyDown(Keys.R) && oldKeyboardState.IsKeyUp(Keys.R))
            {
                AlgOrder += "R";
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
            }
            if (keyboardState.IsKeyDown(Keys.U) && oldKeyboardState.IsKeyUp(Keys.U))
            {
                AlgOrder += "U";
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
            }
            if (keyboardState.IsKeyDown(Keys.D) && oldKeyboardState.IsKeyUp(Keys.D))
            {
                AlgOrder += "D";
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
            }
            if (keyboardState.IsKeyDown(Keys.B) && oldKeyboardState.IsKeyUp(Keys.B))
            {
                AlgOrder += "B";
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
            }
            if (keyboardState.IsKeyDown(Keys.F) && oldKeyboardState.IsKeyUp(Keys.F))
            {
                AlgOrder += "F";
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
            }
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || oldKeyboardState.IsKeyUp(Keys.RightControl)) && keyboardState.IsKeyUp(Keys.Z) && oldKeyboardState.IsKeyDown(Keys.Z))
            {
                AlgOrder += "Z";
            }
            UpdateAlgo(AlgOrder);

            //here the fun starts
            //Debug.WriteLine("algOrder=    "+algOrder);
            if (cube.ShouldAddScrambleToOrder)
            {
                AlgOrder += cube.ScramblingVectors;
                cube.ShouldAddScrambleToOrder = false;
            }
            if (AlgOrder.Length > 0)
            {
                if ((AlgOrder[0] == 'L') || (AlgOrder[0] == 'l'))
                {
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            cube.Rotate(camera.RealLeft, false, AlgOrder);
                        }
                        else
                        {
                            cube.Rotate(camera.RealLeft, true, AlgOrder);
                        }
                    }
                    else
                        cube.Rotate(camera.RealLeft, true, AlgOrder);
                }
                else if ((AlgOrder[0] == 'R') || (AlgOrder[0] == 'r'))
                {
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            cube.Rotate(camera.RealRight, false, AlgOrder);
                        }
                        else
                        {
                            cube.Rotate(camera.RealRight, true, AlgOrder);
                        }
                    }
                    else
                    {
                        cube.Rotate(camera.RealRight, true, AlgOrder);
                    }
                }
                else if ((AlgOrder[0] == 'U') || (AlgOrder[0] == 'u'))
                {
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            cube.Rotate(Vector3.Up, false, AlgOrder);
                        }
                        else
                        {
                            cube.Rotate(Vector3.Up, true, AlgOrder);
                        }
                    }
                    else
                    {
                        cube.Rotate(Vector3.Up, true, AlgOrder);
                    }
                }
                else if ((AlgOrder[0] == 'D') || (AlgOrder[0] == 'd'))
                {
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            cube.Rotate(Vector3.Down, false, AlgOrder);
                        }
                        else
                        {
                            cube.Rotate(Vector3.Down, true, AlgOrder);
                        }
                    }
                    else
                    {
                        cube.Rotate(Vector3.Down, true, AlgOrder);
                    }
                }
                else if ((AlgOrder[0] == 'B') || (AlgOrder[0] == 'b'))
                {
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            cube.Rotate(camera.RealBackward, false, AlgOrder);
                        }
                        else
                        {
                            cube.Rotate(camera.RealBackward, true, AlgOrder);
                        }
                    }
                    else
                    {
                        cube.Rotate(camera.RealBackward, true, AlgOrder);
                    }
                }
                else if ((AlgOrder[0] == 'F') || (AlgOrder[0] == 'f'))
                {
                    if (AlgOrder.Length > 1)
                    {
                        if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                        {
                            cube.Rotate(camera.RealForward, false, AlgOrder);
                        }
                        else
                        {
                            cube.Rotate(camera.RealForward, true, AlgOrder);
                        }
                    }
                    else
                    {
                        cube.Rotate(camera.RealForward, true, AlgOrder);
                    }
                }

                else
                {
                    AlgOrder = AlgOrder.Substring(1);
                    Debug.WriteLine("AlgOrder unknown = " + AlgOrder);
                }
            }
        }

        /// <summary>
        /// sets the oldState vars
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="keyboardState"></param>
        private void OldState(ref MouseState mouseState, ref KeyboardState keyboardState)
        {
            oldKeyboardState = keyboardState;
        }

        /// <summary>
        /// updates everything that's specific for a GameState
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="keyboardState"></param>
        /// <param name="gameTime"></param>
        private void SwitchUpdate(MouseState mouseState, KeyboardState keyboardState, GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (button.BtnFreePlay.IsClicked) CurrentGameState = GameState.FreePlay;
                    if (button.BtnTutorial.IsClicked) CurrentGameState = GameState.Tutorial;
                    if (button.BtnOptions.IsClicked) CurrentGameState = GameState.Options;
                    button.BtnOptions.Update(false, gameTime);
                    button.BtnTutorial.Update(false, gameTime);
                    button.BtnFreePlay.Update(false, gameTime);
                    break;
                case GameState.Tutorial:
                    if (keyboardState.IsKeyDown(Keys.Back)) CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.Options:
                    if (keyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right)) MediaPlayer.Stop();
                    if (button.BtnHebrew.IsClicked) lang.Hebrew();
                    if (button.BtnEnglish.IsClicked) lang.English();
                    //if (button.ClassicBound.Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && whichGenre != "classic")
                    //{
                    //    whichGenre = "classic";
                    //    justSwitched = true;
                    //    MediaPlayer.Stop();
                    //}
                    //else if (whichGenre == "classic") justSwitched = false;
                    //if (button.RockBound.Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released && whichGenre != "rock")
                    //{
                    //    whichGenre = "rock";
                    //    justSwitched = true;
                    //    MediaPlayer.Stop();
                    //}

                    //else if (whichGenre == "rock") justSwitched = false;
                    //music.Update(mouseState, whichGenre, justSwitched);
                    button.BtnEnglish.Update(false, gameTime);
                    button.BtnHebrew.Update(false, gameTime);
                    if (keyboardState.IsKeyDown(Keys.Back)) CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.FreePlay:
                    if (keyboardState.IsKeyDown(Keys.Back)) CurrentGameState = GameState.MainMenu;
                    if (button.BtnScramble.IsClicked) shouldRotate = true;
                    if (button.BtnSolve.IsClicked)
                    {
                        cube.Solve();
                        shouldRotate = false;
                    }
                    cube.Update(gameTime, shouldRotate, cube.ScramblingVectors, false);
                    if (cube.ScrambleIndex >= 25)
                    {
                        shouldRotate = false;
                        cube.ScrambleIndex = 0;
                    }
                    button.BtnScramble.Update(false, gameTime);
                    button.BtnSolve.Update(false, gameTime);
                    break;

            }
        }

        /// <summary>
        /// draws everything that is specific for a GameState
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphicsDevice"></param>
        private void SwitchDraw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    //if (typedText != null)
                    //{
                    //    spriteBatch.DrawString(font, typedText, new Vector2(GraphicsDevice.Viewport.Width / 3, GraphicsDevice.Viewport.Height / 3), Color.Black,0,Vector2.Zero,1.0f,SpriteEffects.None,1.0f);
                    //}
                    if (!System.Windows.Forms.Application.OpenForms.OfType<FirstPopup>().Any())
                        spriteBatch.DrawString(font, lang.MainTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    button.BtnTutorial.Draw(spriteBatch);
                    button.BtnOptions.Draw(spriteBatch);
                    button.BtnFreePlay.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.FreePlay:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, lang.FreePlayTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlayScramble, new Vector2(graphicsDevice.Viewport.Width / 13f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
                    spriteBatch.DrawString(font, lang.FreePlaySolve, new Vector2(graphicsDevice.Viewport.Width / 4f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
                    button.BtnScramble.Draw(spriteBatch);
                    button.BtnSolve.Draw(spriteBatch);
                    spriteBatch.End();
                    DrawModel(cube, world, view, projection, graphicsDevice);
                    break;
                case GameState.Options:
                    spriteBatch.Begin();
                    button.BtnHebrew.Draw(spriteBatch);
                    button.BtnEnglish.Draw(spriteBatch);
                    spriteBatch.DrawString(font, lang.OptionsTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.OptionsFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 40), Color.Black);
                    //spriteBatch.DrawString(text, "Choose Music Genre:         Rock       Classic", new Vector2(GraphicsDevice.Viewport.Width / 3f, GraphicsDevice.Viewport.Height / 3f), Color.Black);
                    spriteBatch.DrawString(font, "English", new Vector2(graphicsDevice.Viewport.Width / 2.5f, 440), Color.Black);
                    spriteBatch.DrawString(font, "ת י ר ב ע", new Vector2(graphicsDevice.Viewport.Width / 1.85f, 440), Color.Black);
                    spriteBatch.End();
                    break;
                case GameState.Tutorial:
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, lang.TutorialTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.TutorialFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 50), Color.Black);
                    spriteBatch.DrawString(font, lang.TutorialFreeText2, new Vector2(graphicsDevice.Viewport.Width / 3f, 90), Color.Black);
                    spriteBatch.End();
                    break;
            }
        }

        public void UpdateAlgo(string algo)
        {
            AlgOrder = algo;
        }
        #endregion

        #region public methods

        /// <summary>
        /// the main draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="graphicsDevice"></param>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            SwitchDraw(spriteBatch, graphicsDevice);
        }

        /// <summary>
        /// the main update
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphics"></param>
        public void Update(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            view = camera.View;
            cameraPos = Matrix.Invert(view).Translation;
            if (CurrentGameState == GameState.FreePlay)
            {
                camera.Update();
                RotateWhichSide(keyboardState, oldKeyboardState, cameraPos);
            }
            mousePos = new Point(mouseState.X, mouseState.Y);
            if (!Application.OpenForms.OfType<FirstPopup>().Any())
                SwitchUpdate(mouseState, keyboardState, gameTime);
            OldState(ref mouseState, ref keyboardState);
            currentScale = Game1.CubieSize * 3 / graphics.GraphicsDevice.Viewport.AspectRatio / Game1.OriginalScale;
        }

        /// <summary>
        /// switches the GameState to tutorial
        /// </summary>
        public void SwitchToTutorial()
        {
            CurrentGameState = GameState.Tutorial;
        }

        /// <summary>
        /// draws the model given
        /// </summary>
        /// <param name="cube"></param>
        /// <param name="objectWorldMatrix"></param>
        /// <param name="view"></param>
        /// <param name="projection"></param>
        /// <param name="graphicsDevice"></param>
        public void DrawModel(Cube cube, Matrix objectWorldMatrix, Matrix view, Matrix projection, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
            for (int index = 0; index < cube.Model.Meshes.Count; index++)
            {
                ModelMesh mesh = cube.Model.Meshes[index];
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateScale(currentScale) * mesh.ParentBone.Transform * Matrix.CreateTranslation(Game1.CubieSize, -Game1.CubieSize, -Game1.CubieSize) * cube.MeshTransforms[index] * objectWorldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();

            }
        }

        #endregion
    }
}
