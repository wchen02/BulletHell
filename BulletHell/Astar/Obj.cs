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

namespace BulletHell
{
    public class Obj {
        private Texture2D sprite;
        private Vector2 position;
        private Rectangle srcRect;
        private bool visibility;

        public Obj(Texture2D sprite, Vector2 position, bool visibility, Vector2 srcRectSize) {
            this.visibility = visibility;
            this.position = position;
            this.sprite = sprite;
            this.srcRect = new Rectangle(0, 0, (int)srcRectSize.X, (int)srcRectSize.Y);
        }

        public Obj(Texture2D sprite, Vector2 position, bool visibility) 
            : this(sprite, position, visibility, new Vector2(sprite.Width, sprite.Height)){}
        
        public Obj(Texture2D sprite, Vector2 position) 
            : this(sprite, position, true) {}

        public bool Visibility {
            get { return visibility; }
            protected set { visibility = value; }
        }

        public Rectangle SrcRect {
            get { return srcRect; }
            protected set { srcRect = value; }
        }

        public Texture2D Sprite {
            get { return sprite; }
            protected set { sprite = value; }
        }

        public Vector2 Position {
            get { return position; }
            protected set { position = value; }
        }

        public float XPosition {
            get { return position.X; }
            /*protected*/ set { position.X = value; }
        }

        public float YPosition {
            get { return position.Y; }
            protected set { position.Y = value; }
        }
    }
}
