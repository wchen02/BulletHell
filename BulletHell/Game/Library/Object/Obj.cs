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

namespace BulletHell.Game.Object
{
    public class Obj
    {
        #region Fields
        internal Texture2D sprite;
        internal Vector2 position, origin;
        internal Rectangle srcRect, // The Rectange in the sprite.
            posRect; // The  Rectange of the obj in the Viewport.
        internal bool visibility;
        internal float scale = 1.0f, rotation = 0.0f, layerDepth = 0.0f;
        #endregion


        #region Properties

        internal float X
        {
            get { return position.X; }
        }

        internal float Y
        {
            get { return position.Y; }
        }

        internal int Width
        {
            get { return srcRect.Width; }
        }

        internal int Height
        {
            get { return srcRect.Height; }
        }

        internal string ID { get; set; }

        #endregion


        #region Initialization
        internal Obj(Texture2D sprite, int x, int y, int width, int height, float scale)
            : this(sprite, x, y, width, height)
        {
            this.scale = scale;
        }

        internal Obj(Texture2D sprite, float x, float y, int width, int height)
        {
            this.sprite = sprite;
            position = new Vector2(x, y);
            visibility = true;
            srcRect = new Rectangle(0, 0, width, height);
            posRect = new Rectangle((int)x + width / 3, (int)y + height / 3, (int)((float)2 / 3 * width), (int)((float)2 / 3 * height));
            origin = Vector2.Zero;
            ID = this.ToString() + "_" + new Random().Next().ToString(); // Unique ID of the object.
        }

        internal Obj(Texture2D sprite, float x, float y)
            : this(sprite, x, y, sprite.Width, sprite.Height) { }

        #endregion


        internal void dispose()
        {
            sprite.Dispose();
        }

        internal bool collision(Obj anotherObj)
        {
            return posRect.Intersects(anotherObj.posRect);
        }

        internal virtual void Update(GameTime gameTime) { }

        internal virtual void Draw(GameTime gameTime)
        {
            if (visibility)
                _GLOBAL.SpriteBatch.Draw(sprite, position, srcRect, Color.White, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }
    }
}
