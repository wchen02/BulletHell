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
    /* To use this class, the keyboardState and previousKeyboardState has to be updated in the game state. */
    public class InputHandler
    {
        #region Fields
        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;
        //private GamePadState gamepadState;

        public Keys up, down, left, right, okay, escape, pause;
        #endregion

        #region Initialization
        public InputHandler()
        {
            keyboardState = new KeyboardState();
            previousKeyboardState = new KeyboardState();

            up = Keys.Up;
            down = Keys.Down;
            left = Keys.Left;
            right = Keys.Right;
            okay = Keys.Enter;
            escape = Keys.Escape;
            pause = Keys.P;
        }

        #endregion

        #region Methods
        public void Update(GameTime gameTime) {
            /* These two statement has been disable because they have proven themselves to be not efficient */

            //_GLOBAL.InputHandler.keyboardState = Keyboard.GetState(); 
            //_GLOBAL.InputHandler.previousKeyboardState = _GLOBAL.InputHandler.keyboardState;
        }

        public bool isKeyDown()
        {
            return previousKeyboardState.IsKeyDown(down)
                && keyboardState.IsKeyUp(down);
        }

        public bool isKeyDownPressed()
        {
            return keyboardState.IsKeyDown(down);
        }

        public bool isKeyUp()
        {
            return previousKeyboardState.IsKeyDown(up)
                && keyboardState.IsKeyUp(up);
        }

        public bool isKeyUpPressed()
        {
            return keyboardState.IsKeyDown(up);
        }

        public bool isKeyLeft()
        {
            return previousKeyboardState.IsKeyDown(left)
                && keyboardState.IsKeyUp(left);
        }

        public bool isKeyLeftPressed()
        {
            return keyboardState.IsKeyDown(left);
        }

        public bool isKeyRight()
        {
            return previousKeyboardState.IsKeyDown(right)
                && keyboardState.IsKeyUp(right);
        }

        public bool isKeyRightPressed()
        {
            return keyboardState.IsKeyDown(right);
        }

        public bool isKeyEnter()
        {
            return keyboardState.IsKeyDown(Keys.Enter) && previousKeyboardState.IsKeyUp(Keys.Enter);
        }

        public bool isKeyEnterPressed()
        {
            return keyboardState.IsKeyDown(okay);
        }

        public bool isKeyEscape()
        {
            return previousKeyboardState.IsKeyDown(escape)
                && keyboardState.IsKeyUp(escape);
        }

        public bool isKeyPause()
        {
            return previousKeyboardState.IsKeyDown(pause)
                && keyboardState.IsKeyUp(pause);
        }
        #endregion
    }
}
