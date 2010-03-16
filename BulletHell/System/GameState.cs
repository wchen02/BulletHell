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
    public abstract class GameState
    {
        internal bool NeedUpdate { get; set; }
        internal bool NeedDraw { get; set; }

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

#region template
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
        public ()
        {
        }

        protected override void Initialize()()
        {
        }

        internal override void LoadContent()
        {
        }

        protected override void UnloadContent() { }

        internal override void Update(GameTime gameTime) { }

        internal override void Draw(GameTime gameTime)
        {
        }
    }
}
*/
#endregion