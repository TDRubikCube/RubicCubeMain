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
        public string AllTimeAlgOrder = "";
        public string YAlgOrder = "";
        //bool stopAnim = false;
        public int rotationsLeft = 0;
        public bool shouldRotate;
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
            camera.RealRotate(cameraPos);
            if (cube.Angle <= -100)
            {
                if ((AlgOrder[0] == 'Z') || (AlgOrder[0] == 'z'))
                {
                    char lastCommnad = AllTimeAlgOrder[AllTimeAlgOrder.Length - 1];
                    if ((lastCommnad == 'I') || (lastCommnad == 'i'))
                    {
                        YAlgOrder = lastCommnad + YAlgOrder;
                        AllTimeAlgOrder = AllTimeAlgOrder.Substring(0, AllTimeAlgOrder.Length - 1);
                    }
                    YAlgOrder = lastCommnad + YAlgOrder;
                    AllTimeAlgOrder = AllTimeAlgOrder.Substring(0, AllTimeAlgOrder.Length - 1);
                }
                if ((AlgOrder[0] == 'Y') || (AlgOrder[0] == 'y'))
                {
                    Debug.WriteLine("HELLO 1");
                    AllTimeAlgOrder += YAlgOrder[0];
                    YAlgOrder = YAlgOrder.Substring(1);
                    if (YAlgOrder.Length > 0)
                    {
                        if ((YAlgOrder[0] == 'I') || (YAlgOrder[0] == 'i'))
                        {
                            Debug.WriteLine("HELLO 2");
                            AllTimeAlgOrder += YAlgOrder[0];
                            YAlgOrder = YAlgOrder.Substring(1);
                        }
                    }
                    Debug.WriteLine("HELLO 3");
                }
                //////////////////////////////////////////////////////
                Debug.WriteLine("Original=   " + AlgOrder);
                if (AlgOrder.Length > 1)
                {
                    if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                    {
                        AllTimeAlgOrder += "I";
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
                Debug.WriteLine("AllTimeAlgOrder " + AllTimeAlgOrder);
                Debug.WriteLine("YAlgOrder " + YAlgOrder);
                cube.Angle = 0;
            }
            /////////////
            if (keyboardState.IsKeyDown(Keys.Q) && oldKeyboardState.IsKeyUp(Keys.Q))
            {
                DebugBorders("");
            }
            if (keyboardState.IsKeyDown(Keys.T) && oldKeyboardState.IsKeyUp(Keys.T))//T is 4 tests!
            {

                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    DebugBorders("");
                    Debug.WriteLine("T was pressed, stating rotating R 30 times counter-clockWise for tests");
                    DebugBorders("");
                    for (int i = 0; i < 15; i++)
                    {
                        AlgOrder += (VectorToChar(camera.RealRight));
                        AlgOrder += ("I");
                        AllTimeAlgOrder += (VectorToChar(camera.RealRight));
                        AllTimeAlgOrder +=("I");
                    }
                }
                else
                {
                    DebugBorders("");
                    Debug.WriteLine("T was pressed, stating rotating R 30 times clockWise for tests");
                    DebugBorders("");
                    for (int i = 0; i < 15; i++)
                    {
                        AlgOrder += (VectorToChar(camera.RealRight));
                        AllTimeAlgOrder += (VectorToChar(camera.RealRight));
                    }
                }
            }
            if (keyboardState.IsKeyDown(Keys.W) && oldKeyboardState.IsKeyUp(Keys.W))
            {
                DebugBorders("w");
                Debug.WriteLine("AlgOrder is: " + AlgOrder);
                Debug.WriteLine("AllTimeAlgOrder is: " + AllTimeAlgOrder);
                Debug.WriteLine("YAlgOrder is: " + YAlgOrder);
                Debug.WriteLine("cameraPos.X is: "+cameraPos.X);
                Debug.WriteLine("cameraPos.Y is: " + cameraPos.Y);
                Debug.WriteLine("cameraPos.Z is: " + cameraPos.Z);
                Debug.WriteLine("Angle is: " + cube.Angle);
                DebugBorders("w");
            }
            CheckForClick(ref keyboardState, ref oldKeyboardState, Keys.R, camera.RealRight);
            CheckForClick(ref keyboardState, ref oldKeyboardState, Keys.L, camera.RealLeft);
            CheckForClick(ref keyboardState, ref oldKeyboardState, Keys.U, Vector3.Up);
            CheckForClick(ref keyboardState, ref oldKeyboardState, Keys.D, Vector3.Down);
            CheckForClick(ref keyboardState, ref oldKeyboardState, Keys.F, camera.RealForward);
            CheckForClick(ref keyboardState, ref oldKeyboardState, Keys.B, camera.RealBackward);
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)) && keyboardState.IsKeyUp(Keys.Z) && oldKeyboardState.IsKeyDown(Keys.Z))
            {
                if (AllTimeAlgOrder.Length > 0)
                {
                    AlgOrder += "Z";
                }
            }
            if ((keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl)) && keyboardState.IsKeyUp(Keys.Y) && oldKeyboardState.IsKeyDown(Keys.Y))
            {
                if (YAlgOrder.Length > 0)
                    AlgOrder += "y";
            }

            UpdateAlgo(AlgOrder);

            //here the fun starts
            //Debug.WriteLine("algOrder=    "+algOrder);

            if (true) //Because we're getting rid of this fucking function ayyy
            {
                if (AlgOrder.Length > 0)
                {
                    if ((AlgOrder[0] == 'Z') || (AlgOrder[0] == 'z')) //Check for control+Z
                    {
                        if (AllTimeAlgOrder.Length > 0)
                        {
                            ControlZ();
                        }
                        else
                        {
                            AlgOrder.Substring(1);
                            Console.Beep();
                        }
                    }
                    else if ((AlgOrder[0] == 'Y') || (AlgOrder[0] == 'y')) //Check for control+Y
                    {
                        if (YAlgOrder.Length > 0)
                        {
                            ControlY();
                        }
                        else
                        {
                            AlgOrder.Substring(1);
                            Console.Beep(1,100);
                            Console.Beep(5, 100);
                        }
                    }

                    else if ((AlgOrder[0] == 'L') || (AlgOrder[0] == 'l'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("L"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("L"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("L"), true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'R') || (AlgOrder[0] == 'r'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("R"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("R"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("R"), true, AlgOrder);
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
                                cube.Rotate(CharToVector("B"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("B"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("B"), true, AlgOrder);
                        }
                    }
                    else if ((AlgOrder[0] == 'F') || (AlgOrder[0] == 'f'))
                    {
                        if (AlgOrder.Length > 1)
                        {
                            if ((AlgOrder[1] == 'i') || (AlgOrder[1] == 'I') || (AlgOrder[1] == '\''))
                            {
                                cube.Rotate(CharToVector("F"), false, AlgOrder);
                            }
                            else
                            {
                                cube.Rotate(CharToVector("F"), true, AlgOrder);
                            }
                        }
                        else
                        {
                            cube.Rotate(CharToVector("F"), true, AlgOrder);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("AlgOrder unknown = " + AlgOrder);
                        AlgOrder = AlgOrder.Substring(1);
                    }
                }
            }
        }

        private void CheckForClick(ref KeyboardState keyboardState, ref KeyboardState oldKeyboardState, Keys key, Vector3 direction)
        {
            if (keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
            {
                AlgOrder += (VectorToChar(direction));
                AllTimeAlgOrder += (VectorToChar(direction));
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                    AlgOrder += "I";
                YAlgOrder = "";
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
                    if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.Options:
                    if (keyboardState.IsKeyDown(Keys.Right) && oldKeyboardState.IsKeyUp(Keys.Right)) MediaPlayer.Stop();
                    if (button.BtnRussian.IsClicked) lang.Russian();
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
                    button.BtnRussian.Update(false, gameTime);
                    if (keyboardState.IsKeyDown(Keys.Escape)) CurrentGameState = GameState.MainMenu;
                    break;
                case GameState.FreePlay:
                    if (keyboardState.IsKeyDown(Keys.Escape))
                    {
                        CurrentGameState = GameState.MainMenu;
                        DebugBorders("MainMenu");
                    }
                    if (button.BtnScramble.IsClicked) shouldRotate = true;
                    if (button.BtnSolve.IsClicked)
                    {
                        cube.Angle = 0;
                        shouldRotate = false;
                        AlgOrder = "";
                        AllTimeAlgOrder = "";
                        YAlgOrder = "";
                        cube.Solve();
                        DebugBorders("Reset!");
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
                    spriteBatch.DrawString(font, lang.FreePlayReset, new Vector2(graphicsDevice.Viewport.Width / 4f, graphicsDevice.Viewport.Height / 1.4f), Color.Black);
                    button.BtnScramble.Draw(spriteBatch);
                    button.BtnSolve.Draw(spriteBatch);
                    spriteBatch.End();
                    DrawModel(cube, world, view, projection, graphicsDevice);
                    break;
                case GameState.Options:
                    spriteBatch.Begin();
                    button.BtnRussian.Draw(spriteBatch);
                    button.BtnHebrew.Draw(spriteBatch);
                    button.BtnEnglish.Draw(spriteBatch);
                    spriteBatch.DrawString(font, lang.OptionsTitle, new Vector2(graphicsDevice.Viewport.Width / 3f, 10), Color.Black);
                    spriteBatch.DrawString(font, lang.OptionsFreeText, new Vector2(graphicsDevice.Viewport.Width / 3f, 40), Color.Black);
                    //spriteBatch.DrawString(text, "Choose Music Genre:         Rock       Classic", new Vector2(GraphicsDevice.Viewport.Width / 3f, GraphicsDevice.Viewport.Height / 3f), Color.Black);
                    spriteBatch.DrawString(font, "English", new Vector2(graphicsDevice.Viewport.Width / 2.5f, 440), Color.Black);
                    spriteBatch.DrawString(font, "ת י ר ב ע", new Vector2(graphicsDevice.Viewport.Width / 1.85f, 440), Color.Black);
                    spriteBatch.DrawString(font, "Russian", new Vector2(graphicsDevice.Viewport.Width / 1.55f, 440), Color.Black);
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
        public void ControlZ()
        {
            if (AllTimeAlgOrder.Length > 0)
            {
                int num = 0;
                int length = AllTimeAlgOrder.Length - 1;
                bool counterClockWise = false;
                if ((AllTimeAlgOrder[length - num] == 'I') || (AllTimeAlgOrder[length - num] == 'i'))
                {
                    num += 1;
                    counterClockWise = true;
                }

                if (AllTimeAlgOrder[length - num] == 'l')
                {
                    cube.Rotate(Vector3.Left, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'r')
                {
                    cube.Rotate(Vector3.Right, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'u')
                {
                    cube.Rotate(Vector3.Up, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'd')
                {
                    cube.Rotate(Vector3.Down, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'f')
                {
                    cube.Rotate(Vector3.Forward, counterClockWise, AlgOrder);
                }
                else if (AllTimeAlgOrder[length - num] == 'b')
                {
                    cube.Rotate(Vector3.Backward, counterClockWise, AlgOrder);
                }
            }
            else
            {
                Debug.WriteLine("AllTimeAlgOrder is 0!!!");
            }
        }
        public void ControlY()
        {
            if (YAlgOrder.Length > 0)
            {
                int num = 0;
                int length = YAlgOrder.Length - 1;
                bool counterClockWise = true;
                if (YAlgOrder.Length > 1)
                {
                    if ((YAlgOrder[1] == 'I') || (YAlgOrder[1] == 'i'))
                    {
                        num += 1;
                        counterClockWise = false;
                    }
                }
                if (YAlgOrder[num] == 'l')
                {
                    cube.Rotate(Vector3.Left, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'r')
                {
                    cube.Rotate(Vector3.Right, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'u')
                {
                    cube.Rotate(Vector3.Up, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'd')
                {
                    cube.Rotate(Vector3.Down, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'f')
                {
                    cube.Rotate(Vector3.Forward, counterClockWise, AlgOrder);
                }
                else if (YAlgOrder[num] == 'b')
                {
                    cube.Rotate(Vector3.Backward, counterClockWise, AlgOrder);
                }
            }
            else
            {
                Debug.WriteLine("YAlgOrder is 0!!!");
            }
        }
        public string VectorToChar(Vector3 real)
        {
            if (real == Vector3.Left)
            {
                return "l";
            }
            if (real == Vector3.Right)
            {
                return "r";
            }
            if (real == Vector3.Backward)
            {
                return "b";
            }
            if (real == Vector3.Forward)
            {
                return "f";
            }
            if (real == Vector3.Up)
            {
                return "u";
            }
            if (real == Vector3.Down)
            {
                return "d";
            }
            Debug.WriteLine("VectorToChar returned null");
            return "";

        }
        public Vector3 CharToVector(string real)
        {
            if ((real == "l") || (real == "L"))
            {
                return Vector3.Left;
            }
            if ((real == "r") || (real == "R"))
            {
                return Vector3.Right;
            }
            if ((real == "b") || (real == "B"))
            {
                return Vector3.Backward;
            }
            if ((real == "f") || (real == "F"))
            {
                return Vector3.Forward;
            }
            if ((real == "u") || (real == "U"))
            {
                return Vector3.Up;
            }
            if ((real == "d") || (real == "D"))
            {
                return Vector3.Down;
            }
            Debug.WriteLine("CharToVector returned null");
            return Vector3.Zero;
        }
        public void DebugBorders(string a)
        {
            string b = "~~~~~~~~~~~~~~";
            if (a.Length < (b.Length * 2))
            {
                for (int i = 0; i < (a.Length) / 2; i++)
                {
                    b = b.Substring(1);
                }
                if ((a.Length % 2) == 1)
                {
                    b = b.Substring(1) + a + b;
                }
                else
                {
                    b += a + b;
                }
                Debug.WriteLine(b);
              //Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            else
            {
                Debug.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            }
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
