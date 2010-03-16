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

        public int elapsedSeconds() {
            return timeElapsed.Seconds;
        }

        public int elapsedMilliseconds()
        {
            return timeElapsed.Milliseconds;
        }

        public int elapsedMinutes()
        {
            return timeElapsed.Minutes;
        }

        public void reset()
        {
            timeElapsed = TimeSpan.Zero;
        }
    }
}
