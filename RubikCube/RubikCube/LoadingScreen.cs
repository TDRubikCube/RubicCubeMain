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
using System.Diagnostics;

namespace RubikCube
{
    class LoadingScreen
    {
        readonly SpriteFont font;
        int totalDots = 1;
        public bool ShouldStart;
        float timer = 500;

        public LoadingScreen(ContentManager content)
        {
            font = content.Load<SpriteFont>("font");
            ShouldStart = false;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "loading.", new Vector2(graphicsDevice.Viewport.Width / 2f, graphicsDevice.Viewport.Height / 14f), Color.Black);
            ShouldStart = true;
            for (int i = 1; i < totalDots; i++)
            {
                int j = 3;
                if (i == 2)
                    j -= 4;
                spriteBatch.DrawString(font, ".", new Vector2(graphicsDevice.Viewport.Width / 1.68f - j, graphicsDevice.Viewport.Height / 14f), Color.Black);
            }
            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            float elpased = gameTime.ElapsedGameTime.Milliseconds;
            timer -= elpased;
            if (timer < 0)
            {
                if (totalDots < 3)
                {
                    totalDots++;
                }
                else
                    totalDots = 1;
                timer = 500;
            }
        }

    }
}
