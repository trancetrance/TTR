// (c) 2010-2014 IndiegameGarden.com. Distributed under the FreeBSD license in LICENSE.txt
﻿
// ------------------------------------------------------------------
// defines for global settings (debug etc)
// -> defines set in Visual Studio Profiles: DEBUG, RELEASE, PROFILE
//#define MUSIC_ENABLED
//#define TIMELOGGING_ENABLED

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using TTMusicEngine;
using TTR.level;
using TTR.gameobj;
using TTR.main;

using TTengine.Core;
using TTengine.Util;

namespace TTR
{
    public class TTRGame : Game
    {
        
        public GraphicsDeviceManager graphics;
        public int preferredWindowWidth = 1366; //1280; //1440; //1280;
        public int preferredWindowHeight = 768; //720; //900; //720;
        MusicEngine musicEngine = null;
        public Level level;
        public Screenlet toplevelScreen;
        // treeRoot is a pointer, set to the top-level Gamelet to render
        public Gamelet treeRoot;
        public Gamelet titleScreen;
        public SpriteBatch spriteBatch;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

        public TTRGame()
        {
            Content.RootDirectory = "Content";

            // create the TTengine for this game
            TTengineMaster.Create(this);

            // basic XNA graphics init here (before Initialize() and LoadContent() )
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = preferredWindowWidth;
            graphics.PreferredBackBufferHeight = preferredWindowHeight;
#if RELEASE
            graphics.IsFullScreen = true;
#else
            graphics.IsFullScreen = false;
#endif
            //this.TargetElapsedTime = TimeSpan.FromMilliseconds(10);
#if PROFILE
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
#else
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = true;
#endif
        }

        protected override void Initialize()
        {
            RunningGameState.IsXNAHiDef = (GraphicsDevice.GraphicsProfile == GraphicsProfile.HiDef);
            spriteBatch = new SpriteBatch(GraphicsDevice);

#if MUSIC_ENABLED
            // create music engine
            musicEngine = MusicEngine.GetInstance(); // TODO check for Initialized property
            musicEngine.AudioPath = "..\\..\\..\\..\\Audio";
#endif
            RunningGameState.musicEngine = musicEngine;

            toplevelScreen = new Screenlet(1280, 768);
            Gamelet physicsModel = new FixedTimestepPhysics();
            
            toplevelScreen.Add(physicsModel);
            toplevelScreen.Add(new FrameRateCounter(1.0f, 0f));
            physicsModel.Add(new TTRStateMachine());
            treeRoot = toplevelScreen;

            TTengineMaster.Initialize(treeRoot);

            // finally call base to enumnerate all (gfx) Game components to init
            base.Initialize();
        }

        protected override void LoadContent()
        {                        
            base.LoadContent();            

            if (musicEngine != null && !musicEngine.Initialized)
            {
                MessageBox(new IntPtr(0), "Error - FMOD DLL not found or unable to initialize", "TTR", 0); // TODO name of window set
                this.Exit();
                return;
            }            
        }

        protected override void Update(GameTime gameTime)
        {

#if TIMELOGGING
            double dtms = gameTime.ElapsedGameTime.TotalMilliseconds;
            Util.Log("Updt() gt.tot.ts= " + String.Format("{0,7:0.000}",gameTime.TotalGameTime.TotalSeconds) + "  gt.elap.tms= " +
                String.Format("{0,5:0.00}",dtms) + "\n");
#endif
            // Allows the game to exit instantly
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) )
            {
                this.Exit();
            }

            // update params, and call the root gamelet to do all.
            TTengineMaster.Update(gameTime, treeRoot);

            // update any other XNA components
            base.Update(gameTime);
        }

        protected override bool BeginDraw()
        {
            return base.BeginDraw();
        }

        protected override void EndDraw()
        {
            base.EndDraw();
        }

        protected override void Draw(GameTime gameTime)
        {
            // draw all my gamelet items
            GraphicsDevice.SetRenderTarget(null); // TODO
            TTengineMaster.Draw(gameTime, treeRoot);

            // then buffer drawing on screen at right positions                        
            GraphicsDevice.SetRenderTarget(null); // TODO
            //GraphicsDevice.Clear(Color.Black);
            Rectangle destRect = new Rectangle(0, 0, toplevelScreen.RenderTarget.Width, toplevelScreen.RenderTarget.Height);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(toplevelScreen.RenderTarget, destRect, Color.White);
            spriteBatch.End();
            
            // then draw other (if any) game components on the screen
            base.Draw(gameTime);

            // log
#if TIMELOGGING_ENABLED
            double dt = gameTime.ElapsedGameTime.TotalMilliseconds;
            Util.Log("Draw() gt.tot.ts= " + String.Format("{0,7:0.000}", gameTime.TotalGameTime.TotalSeconds) + "                        gt.elap.tms= " +
                String.Format("{0,5:0.00}", dt) + "\n");
#endif

        }

    }
}
