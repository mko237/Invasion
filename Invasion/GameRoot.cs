using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using BloomPostprocess;

namespace Invasion
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameRoot : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        

        public static GameRoot Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static Vector2 DisplaySize { get { return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); } }
        //private static InputServer inputserver = new InputServer();
        public static ParticleManager<ParticleState> ParticleManager { get; private set; }
        private static Thread ServerThread = new Thread(InputServer.StartListening);
        public static bool resetClock = false;
        Tuple<int,int> timer = new Tuple<int,int>(0,0);
        public static int minutes = 0;
        public static double seconds = 0;
        
        public static BloomComponent bloom;
        AudioEngine engine;
        public static SoundBank soundBank;
        WaveBank waveBank;
        AudioCategory effectsCategory;
        AudioCategory musicCategory;

        

                       
        public GameRoot()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Instance = this;
            //http://msdn.microsoft.com/en-us/library/dd231915%28v=xnagamestudio.31%29.aspx notes on how to load audio engine. had to add xact.dll reference located in programfiles/microsoftxna/.../win/xact.dll 
            //http://xboxforums.create.msdn.com/forums/p/102228/608489.aspx how to find other audio devices.
            
            
            // Initialize audio objects.
            engine = new AudioEngine(@"Content\Audio\Xact.xgs", TimeSpan.Zero, "{0.0.0.00000000}.{c2f67847-1f14-477e-8a90-197b0f2f1715}");
            soundBank = new SoundBank(engine, @"Content\Audio\Sound Bank.xsb");
            waveBank = new WaveBank(engine, @"Content\Audio\Wave Bank.xwb");
            
            
            //Console.WriteLine("SOUND ENGINE: " + engine.RendererDetails.ToString()); used to determine the redndererID
            //foreach (var r in engine.RendererDetails)
            //{
            //    Console.WriteLine(r.FriendlyName +","+ r.RendererId);
            //}
            
            graphics.PreferredBackBufferWidth = 1600;//(int)DisplaySize.X-150;
            graphics.PreferredBackBufferHeight = 900;//(int)DisplaySize.Y -350;

            bloom = new BloomComponent(this);
            Components.Add(bloom);
            bloom.Settings = new BloomSettings(null, .25f, 4, 2, 1, 1.5f, 1);

            IsFixedTimeStep = true;
            //TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 120);
            
            

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
           

            base.Initialize();

            ParticleManager = new ParticleManager<ParticleState>(1024 * 20, ParticleState.UpdateParticle);
            
            //EntityManager.Add(Planet.Instance);
            Background Background = new Background();
            EntityManager.Add(Background);

            // spawns the level and the planets
            LevelSpawner Level = new LevelSpawner(38);
            TeamManager.GenerateTeams(Level, 2);
            Level.Spawn();

            

            //Console.WriteLine("in main starting thread");
            ServerThread.Start();
            //Console.WriteLine("back to main");
            //inputserver.StartListening();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Art.Load(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            InputDisplay.UpdateInput();
            InputParser.Update();
            EntityManager.Update();
            TeamManager.Update();
            ParticleManager.Update();
            //engine.Update();
            effectsCategory = engine.GetCategory("Default");
            musicCategory = engine.GetCategory("Music");
            effectsCategory.SetVolume(.2f);//adjust volume here
            musicCategory.SetVolume(2f);
            seconds += 0.0166667;
            if(seconds >=60)                
            {
                minutes += 1;
                seconds = 0;
                
            }
            timer = new Tuple<int, int>(minutes, (int)seconds);
            ;
            HUD.Update(timer);
            if (resetClock)
            {
                minutes = 0;
                seconds = 0;
                resetClock = false;

            }

            base.Update(gameTime);

            


            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            bloom.BeginDraw();
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            EntityManager.Draw(spriteBatch);
            TestInputDraw.Draw(spriteBatch);
            InputDisplay.Draw(spriteBatch);
            ParticleManager.Draw(spriteBatch);
            WinScreen.Draw(spriteBatch);
            HUD.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            WinScreen.DrawText(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);

        }
    }
}
