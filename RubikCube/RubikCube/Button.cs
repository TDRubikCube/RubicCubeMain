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
using System.Threading;
using System.Diagnostics;

namespace RubikCube
{
    class Button
    {
        readonly Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        public Vector2 Size;
        public bool IsClicked;
        private Timer normalTimer;
        private Timer muteTimer;
        private bool clickDetected;
        private MouseState oldMouseState;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            normalTimer = new Timer(500);
            muteTimer = new Timer(40);
            texture = newTexture;
            Size = new Vector2(graphics.Viewport.Width / 5f, graphics.Viewport.Height / 7f);
        }

        public void Update(bool isMuteButton, GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            rectangle = new Rectangle((int)(position.X), (int)(position.Y), (int)(Size.X), (int)(Size.Y));
            Point mousePos = new Point(mouse.X, mouse.Y);
            if ((rectangle.Contains(mousePos) && mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)|| clickDetected)
            {
                clickDetected = true;
                if (normalTimer.CallTimer(gameTime) && !isMuteButton)
                {
                    IsClicked = true;
                    clickDetected = false;
                }
                else
                {
                    if (isMuteButton && muteTimer.CallTimer(gameTime))
                    {
                        IsClicked = true;
                        clickDetected = false;
                    }
                }
            }
            else IsClicked = false;
            oldMouseState = mouse;
        }

        public void SetPosition(Vector2 newPositon)
        {
            position = newPositon;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
                spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
