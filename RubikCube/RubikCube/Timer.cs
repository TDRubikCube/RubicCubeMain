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

namespace RubikCube
{
    class Timer
    {
        private float timer = 0;
        public Timer(int intervel)
        {
            timer = intervel;
        }
        public bool CallTimer(GameTime gameTime)
        {
            float elpased = gameTime.ElapsedGameTime.Milliseconds;
            timer -= elpased;
            if (timer < 0)
            {
                timer = 500;
                return true;
            }
            return false;
        }
    }
}
