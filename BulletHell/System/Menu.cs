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
using BulletHell.System;
using BulletHell.Game;
using BulletHell.Game.Object;
using BulletHell.Game.Interface;
using System.Diagnostics;

namespace BulletHell.System
{
    public class Menu : GameState
    {
        #region Fields
        private MenuNode[] menuNodes;
        private string[] titles;
        private bool[] actives;
        private Texture2D arrowTexture;
        //private Texture2D background, arrowTexture, dialogTexture;
        private Obj arrow;
        private Vector2 titlePos;
        private Color activeTitleColor, inactiveTitleColor;
        private int mouseOverIndex;

        #endregion

        #region Initialization
        public Menu()
        {
            mouseOverIndex = 0; // decides which option has the current focus
            titles = new string[] { "Start Game", "Continue", "Credits", "Options", "Exit" };
            actives = new bool[] { true, false, true, true, true };

            Debug.Assert(titles.Length == actives.Length);

            menuNodes = new MenuNode[titles.Length];
            for (int i = 0; i < menuNodes.Length; ++i)
                menuNodes[i] = new MenuNode(titles[i], actives[i]);
        }

        protected override void Initialize()
        {
            titlePos = new Vector2(_GLOBAL.Viewport.Width - 180, 100);
            activeTitleColor = new Color(0, 0, 0, 200);
            inactiveTitleColor = new Color(0, 0, 0, 100);

            LoadContent();
            arrow = new Obj(arrowTexture, titlePos.X - 70, 100);
        }
        #endregion

        #region Contents
        internal override void LoadContent()
        {
            arrowTexture = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\arrow");
            //dialogTexture = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\dialogBox");
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            NeedUpdate = true;
            if (_GLOBAL.inGameState)
                return;
            
            NeedDraw = true;
            if (_GLOBAL.InputHandler.isKeyUp())
            {
                /* If the option is inactive, jumps to the next options */
                while (true)
                {
                    mouseOverIndex = (mouseOverIndex - 1) % menuNodes.Length;

                    /* Note: C# Mod, -1 % N = -1, it will not wrap around! */
                    if (mouseOverIndex == -1) mouseOverIndex = menuNodes.Length - 1;
                    if (menuNodes[mouseOverIndex].Active) break; // skips inactive options
                }
                _GLOBAL.SoundBank.PlayCue(_GLOBAL.onSelectOption);
            }
            
            if (_GLOBAL.InputHandler.isKeyDown())
            {
                while (true)
                {
                    mouseOverIndex = (mouseOverIndex + 1) % menuNodes.Length;
                    if (menuNodes[mouseOverIndex].Active) break;
                }

                _GLOBAL.SoundBank.PlayCue(_GLOBAL.onSelectOption);
            }
            
            if (_GLOBAL.InputHandler.isKeyEnter())
                selectOption(mouseOverIndex);
            
            if (_GLOBAL.InputHandler.isKeyEscape())
                selectOption(4); // select exit
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //_GLOBAL.SpriteBatch.Draw(background, _GLOBAL.ViewportRect, Color.White);
            for (int i = 0; i < menuNodes.Length; ++i)
            {
                titlePos.Y = i * 30 + 100;

                if (menuNodes[i].Active)
                {
                    _GLOBAL.SpriteBatch.DrawString(_GLOBAL.HuakanFont,
                        menuNodes[i].Title, titlePos,
                        activeTitleColor, 0.0f, Vector2.Zero,
                        (mouseOverIndex == i) ? 1.0f + (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds * 3) / 10 : 1.0f,
                        SpriteEffects.None, 0.0f);
                }
                else
                    _GLOBAL.SpriteBatch.DrawString(_GLOBAL.HuakanFont, menuNodes[i].Title, titlePos, inactiveTitleColor);

                if (mouseOverIndex == i)
                {
                    arrow.position.Y = i * 30 + 100;
                    _GLOBAL.SpriteBatch.Draw(arrow.sprite, arrow.position, Color.White);
                }
            }
            _GLOBAL.SpriteBatch.End();
        }
        #endregion

        #region Methods

        private void selectOption(int index)
        {
            if (menuNodes[index].GameState != null)
            {
                _GLOBAL.GameStateManager.activate(menuNodes[index].GameState);
                _GLOBAL.inGameState = true;
                _GLOBAL.SoundBank.PlayCue(_GLOBAL.onConfirm);
                return;
            }
            Debug.Assert(index >= 0 && index <= menuNodes.Length);

            switch (index)
            {
                case 0: // Start Game
                    menuNodes[index].GameState = new StartGame();
                    NeedDraw = false;
                    break;
                case 1:
                    // Continue;
                    NeedDraw = true;
                    return;
                case 2: // Credits
                    menuNodes[index].GameState = new Credits();
                    NeedDraw = false;
                    break;
                case 3: // Settings
                    NeedDraw = true;
                    return;
                case 4: // Exit
                    menuNodes[index].GameState = new Dialog("Exit", "Are you sure you want to quit?",
                                        new Vector2(_GLOBAL.Viewport.Width, _GLOBAL.Viewport.Height));
                    NeedDraw = true;
                    break;
            }
            selectOption(index);
        }
        #endregion
    }
}