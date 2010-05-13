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
    /* Abstract game state
     * Possible improvement of a solid gamestate is to exclude NeedUpdate and NeedDraw from the abstract class; 
     * Using stack to keep game states for the game state manager.
     * These two variables has proven more trouble than help. */

    public abstract class GameState 
    {
        internal bool NeedUpdate { get; set; } // Determines if a game state needs to be updated
        internal bool NeedDraw { get; set; } // Determines if a game state needs to be drawn

        public GameState()
        {
            Initialize();
        }

        protected abstract void Initialize();

        internal abstract void LoadContent();

        protected abstract void UnloadContent();

        internal abstract void Update(GameTime gameTime);

        internal abstract void Draw(GameTime gameTime);
    }
}

/* Template for derived classes */
#region Template
/*
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

namespace BulletHell.Game
{
    public class : GameState
    {
        public () { }

        protected override void Initialize() { }

        internal override void LoadContent() { }

        protected override void UnloadContent() { }

        internal override void Update(GameTime gameTime) { }

        internal override void Draw(GameTime gameTime) { }
    }
}
*/
#endregion