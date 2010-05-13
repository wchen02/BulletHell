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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace BulletHell.System
{
    public class TimeEventHandler
    {
        internal TimeSpan timeElapsed;

        public TimeEventHandler()
        {
            reset();
        }


        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime;
        }

        public double elapsedSeconds() {
            return timeElapsed.TotalSeconds;
        }

        public double elapsedMilliseconds()
        {
            return timeElapsed.TotalMilliseconds;
        }

        public double elapsedMinutes()
        {
            return timeElapsed.TotalMinutes;
        }

        public void reset()
        {
            timeElapsed = TimeSpan.Zero;
        }
    }
}
