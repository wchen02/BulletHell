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
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;

namespace BulletHell.Game.Library
{
    public class Path : Queue<Waypoint>
    {
        public Path() { }

        /* Create a mirror of this path, for more details, reference to Waypoint.createMirror */
        public Path createMirror(bool xReverse, bool yReverse) 
        {
            Path mirror = new Path();
            
            foreach (Waypoint waypoint in this)
                mirror.Enqueue(waypoint.createMirror(xReverse, yReverse,
                    new Vector2(_GLOBAL.ViewportRect.Width/2, _GLOBAL.ViewportRect.Height/2)));

            return mirror;
        }
    }
}
