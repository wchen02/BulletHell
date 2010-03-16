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
        private SpriteFont huakanFont;
        private MenuNode[] menuNodes;
        private string[] titles;
        private bool[] actives;
        private Texture2D background, arrowTexture, dialogTexture;
        private Obj arrow, dialogBox;
        private Vector2 titlePos;
        private Color activeTitleColor, inactiveTitleColor;
        private int mouseOverIndex, selected;
        private float fontEffect = 0;
        //private bool /*inGame, */songStart = false;
        
        #endregion

        #region Initialization
        public Menu(GameStateManager gameStateManager)
        {
            mouseOverIndex = 0; // decides which option has the current focus
            selected = -1; // not selecting any options
            //inGame = false;
            titles = new string[] { "Start Game", "Continue", "Credits", "Options", "Exit" };
            actives = new bool[] { true, false, true, true, true };

            Debug.Assert(titles.Length == actives.Length);

            menuNodes = new MenuNode[titles.Length];
            for (int i = 0; i < menuNodes.Length; ++i)
                menuNodes[i] = new MenuNode(titles[i], actives[i]);
        }

        protected override void Initialize() {
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
            background = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\background");
            dialogTexture = _GLOBAL.ContentManager.Load<Texture2D>(@"sprites\dialogBox");
            huakanFont = _GLOBAL.ContentManager.Load<SpriteFont>(@"fonts\DFShaoNvW5-GB");
        }

        protected override void UnloadContent() { }
        #endregion

        #region Update and Draw
        internal override void Update(GameTime gameTime)
        {
            if (selected == -1)
            {
                if (_GLOBAL.InputHandler.isKeyUp())
                {
                    for (int i = mouseOverIndex; mouseOverIndex != i + 1;)
                    {
                        mouseOverIndex = (mouseOverIndex > 0) ? mouseOverIndex - 1 : menuNodes.Length - 1;
                        if (menuNodes[mouseOverIndex].Active) break;
                        if (i == menuNodes.Length && mouseOverIndex == 0) break;
                    }

                    //BulletHell.audioManager.play("cursorMove");
                }
                else if (_GLOBAL.InputHandler.isKeyDown())
                {
                    for (int i = mouseOverIndex; mouseOverIndex != i - 1; )
                    {
                        mouseOverIndex = (mouseOverIndex < menuNodes.Length - 1) ? mouseOverIndex + 1 : 0;
                        if (menuNodes[mouseOverIndex].Active) break;
                        if (i == 0 && mouseOverIndex == menuNodes.Length) break;
                    }

                    //BulletHell.audioManager.play("cursorMove");
                }
                else if (_GLOBAL.InputHandler.isKeyEnter())
                {
                    selected = mouseOverIndex;
                    //BulletHell.audioManager.play("confrim");
                }

                if (/*!inGame && */_GLOBAL.GameStateSelect != 0 && _GLOBAL.InputHandler.isKeyEscape())
                {
                    if (/*!inGame && */NeedDraw)
                    {
                        selected = 4;
                        menuChoice();
                    }
                }

                if (_GLOBAL.GameStateSelect == 0)
                {
                    _GLOBAL.GameStateSelect = 1;
                    //inGame = false;
                    NeedDraw = true;
                }
            }
            else
            {
                menuChoice();
            }

            NeedUpdate = true;
        }

        internal override void Draw(GameTime gameTime)
        {
            _GLOBAL.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            _GLOBAL.SpriteBatch.Draw(background, _GLOBAL.ViewportRect, Color.White);
            for (int i = 0; i < menuNodes.Length; ++i)
            {
                titlePos.Y = i*30 + 100;

                if (menuNodes[i].Active)
                {
                    _GLOBAL.SpriteBatch.DrawString(huakanFont,
                        menuNodes[i].Title, titlePos,
                        activeTitleColor, 0.0f, Vector2.Zero,
                        (mouseOverIndex == i) ? 1.0f + (float)Math.Cos(fontEffect += 0.075f) / 12 : 1.0f,
                        SpriteEffects.None, 0.0f);
                }
                else
                    _GLOBAL.SpriteBatch.DrawString(huakanFont, menuNodes[i].Title, titlePos, inactiveTitleColor);

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

        private void select(MenuNode node)
        {
            //inGame = true;
            _GLOBAL.GameStateManager.activate(node.GameState);
        }

        private void menuChoice()
        {
            GameState tmpState;
            switch (selected)
            {
                case 0:
                    if (menuNodes[selected].GameState == null)
                    {
                        tmpState = new StartGame(_GLOBAL.GameStateManager);
                        changeState(tmpState);
                    }
                    else changeState(menuNodes[selected].GameState);
                    NeedDraw = false;
                    break;
                case 1:
                   // Continue;
                    selected = -1;
                    NeedDraw = true; 
                    break;
                case 2:
                    if (menuNodes[selected].GameState == null)
                    {
                        tmpState = new Credits(_GLOBAL.GameStateManager);
                        changeState(tmpState);
                    }
                    else changeState(menuNodes[selected].GameState);

                    NeedDraw = false;
                    break;
                case 3:
                    //Setting;
                    selected = -1;
                    NeedDraw = true;
                    break;
                case 4:

                    if (menuNodes[selected].GameState == null)
                    {
                        dialogBox = new Obj(dialogTexture, _GLOBAL.Viewport.Width, _GLOBAL.Viewport.Height);

                        tmpState = new Dialog(_GLOBAL.GameStateManager, dialogBox.sprite,
                                           "Exit", "Are you sure you want to quit?",
                                            dialogBox.position);
                        changeState(tmpState);
                    }
                    else changeState(menuNodes[selected].GameState);
                    NeedDraw = true;
                    break;
                default:
                    _GLOBAL.Quit = true;
                    break;
            }
        }

        private void changeState(GameState newState) {
            if (_GLOBAL.GameStateSelect == 0)
            {
                selected = -1;
                return;
            }

            if (menuNodes[selected].GameState == null)
            {
                menuNodes[selected].GameState = newState;
                select(menuNodes[selected]);
            }
            else
                menuNodes[selected].action();
        }
        #endregion
    }
}