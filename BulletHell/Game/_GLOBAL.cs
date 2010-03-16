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
    public static class _GLOBAL
    {
        public static GameStateManager GameStateManager;
        public static ContentManager ContentManager;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static InputHandler InputHandler;
        public static Viewport Viewport;
        public static Rectangle ViewportRect;
        public static SpriteFont SpriteFont;
        public static AudioEngine AudioEngine;
        public static WaveBank WaveBank;
        public static SoundBank SoundBank;

        public static int GameStateSelect = -1;
        public static bool Quit;
    }
}
