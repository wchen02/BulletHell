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
    public class Event
    {
        public int Milliseconds { get; private set; }
        public string Obj { get; private set; }

        public Event(int milliseconds, string obj)
        {
            Milliseconds = milliseconds;
            Obj = obj;
        }
    }
}
