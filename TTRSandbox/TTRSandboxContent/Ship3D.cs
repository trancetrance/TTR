<<<<<<< HEAD
// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
?
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
using TTengine;
using TTengine.Core;
using TTengine.Util;

namespace TTR
{
    public class TTRSandbox : Game
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

        private Model test;
        private Vector3 Position = Vector3.One; 
        private float Zoom = 2500;
        private float RotationY = 0.0f;
        private float RotationX = 0.0f;
        private Matrix gameWorldRotation;

        public TTRSandbox()
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
            //physicsModel.Add(new TTRStateMachine());
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

            test = Content.Load<Model>("Ship");
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

            gameWorldRotation =
         Matrix.CreateRotationX(MathHelper.ToRadians(RotationX)) *
         Matrix.CreateRotationY(MathHelper.ToRadians(RotationY));
  
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

        private void DrawModel(Model m)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            m.CopyAbsoluteBoneTransformsTo(transforms);
            Matrix projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                aspectRatio, 1.0f, 10000.0f);
            Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, Zoom),
                Vector3.Zero, Vector3.Up);

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = gameWorldRotation *
                        transforms[mesh.ParentBone.Index] *
                        Matrix.CreateTranslation(Position);
                }
                mesh.Draw();
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw all my gamelet items
            //GraphicsDevice.SetRenderTarget(null); // TODO
            //TTengineMaster.Draw(gameTime, treeRoot);

            // then buffer drawing on screen at right positions                        
            /*
            GraphicsDevice.SetRenderTarget(null); // TODO
            //
            Rectangle destRect = new Rectangle(0, 0, toplevelScreen.RenderTarget.Width, toplevelScreen.RenderTarget.Height);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(toplevelScreen.RenderTarget, destRect, Color.White);
            spriteBatch.End();
            */
            DrawModel(test);

            // then draw other (if any) game components on the screen
            base.Draw(gameTime);

        }

    }
}
=======
// (c) 2010-2011 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt
?
// ------------------------------------------------------------------
// defines for global settings (debug etc)
// -> defines set in Visual Studio Profiles: DEBUG, RELEASE, PROFILE
//#define MUSIC_ENABLED
//#define TIMELOGGING_ENABLED

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

using TTMusicEngine;
using TTR.level;
using TTR.gameobj;
using TTR.main;
using TTengine;
using TTengine.Core;
using TTengine.Util;

namespace TTR
{
    public class TTRSandbox : Game
    {
        
        public GraphicsDeviceManager graphics;
        public int preferredWindowWidth = 1366; //1280; //1440; //1280;
        public int preferredWindowHeight = 768; //720; //900; //720;
        MusicEngine musicEngine = null;
        public Level level;
        public Screenlet toplevelScreen;
        // treeRoot is a pointer, set to the top-level Gamelet to render
        public Gamelet treeRoot;
        //public Gamelet titleScreen;
        public Gamelet gameletsRoot;
        public SpriteBatch spriteBatch;

        public TTRSandbox()
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
            //physicsModel.Add(new TTRStateMachine());
            treeRoot = toplevelScreen;
            gameletsRoot = physicsModel;

            // finally call base to enumnerate all (gfx) Game components to init
            base.Initialize();
        }

        protected override void LoadContent()
        {                        
            base.LoadContent();            

            if (musicEngine != null && !musicEngine.Initialized)
            {
                MsgBox.Show("TTR", "Error - FMOD DLL not found or unable to initialize");
                this.Exit();
                return;
            }

            // HERE TEST CONTENT FOR SANDBOX
            TestGOLLogo();
            TestTimewarpLogo();

            // ends with engine init
            TTengineMaster.Initialize(treeRoot);


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

        }

        protected void TestGOLLogo()
        {
            GoLEffect gol = new GoLEffect("ttlogo-gol");
            gol.LayerDepth = 0f;
            gol.Position = new Vector2(0.6f, 0.4f);
            gol.Scale = 1.0f;
            gol.Rotate = 0.0f;
            gol.Add(new PeriodicPulsingBehavior(0.05f, 140f / 60f / 16f)); // 140 / 60 * 1/16
            gameletsRoot.Add(gol);

        }

        protected void TestTimewarpLogo()
        {
            TimewarpLogo l = new TimewarpLogo("timewarp_logo_bw");
            l.Position = new Vector2(0.7f, 0.5f);
            l.LayerDepth = 0f;
            l.Add(new SineWaveModifier(delegate(float val) { l.ScaleModifier = val; }, 0.1f, 0.189f, 1f));
            gameletsRoot.Add(l);
        }
    }
}
>>>>>>> 6603ac9475fdc9367e0d47a8654e38aeec37cf26
