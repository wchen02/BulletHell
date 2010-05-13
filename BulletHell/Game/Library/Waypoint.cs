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
    public class Waypoint
    {
        public Vector2 Point { get; private set; }

        public Waypoint(Vector2 point)
        {
            Point = point;
        }

        public Waypoint(float x, float y)
        {
            Point = new Vector2(x, y);
        }

        public bool withinRange(Vector2 pos, float range)
        {
            return Vector2.Distance(pos, Point) <= range;
        }

        /* Create a mirror of this waypoint around the mirrorPosition, 
         * such that the created waypoint has the effect of reflection from the mirroir in mirror position */
        public Waypoint createMirror(bool xReverse, bool yReverse, Vector2 mirrorPosition)
        {
            return new Waypoint((xReverse) ? mirrorPosition.X + (mirrorPosition.X - Point.X) : Point.X,
                (yReverse) ? mirrorPosition.Y + (mirrorPosition.Y - Point.Y) : Point.Y);
        }
    }
}
