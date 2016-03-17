using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NanozinProject
{

    public class Nanozin : Microsoft.Xna.Framework.Game
    {
        static public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public const int SCREEN_WIDTH = 1280,
                         SCREEN_HEIGHT = 768,
                         SPRITE_LENGTH = 64,
                         NUM_LEVELS = 11;
        static public Vector2 SCREEN_MID = new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2);

        Effect fade;
        static public EffectParameter fadeLevel;

        static public MouseState theMouse;

        //Resources             //Particle textures
        static public Texture2D dustTexture,
                                chargeTexture,
                                starTexture,
                                cloudsTexture,

                                //Other textures
                                buttonTexture,
                                editorTexture,
                                salvagerRifleTexture,
                                salvagerTexture,
                                fuelCellTexture,
                                ammoTexture,
                                plasmaRifleTexture,
                                plasmaProjectileTexture,
                                plasmaFilmTexture,
                                pressurePlateTexture,
                                wallsTexture,
                                detectorTexture,
                                mirrorTexture,
                                plasmaLancerTexture,
                                furnaceTexture,
                                iceTexture,
                                wasteTexture,
                                ground1Texture,
                                ground2Texture,
                                ground3Texture,
                                ground4Texture;

        //Array for particle textures
        static public Texture2D[] particleTextures = new Texture2D[15];

        //Sounds
        static public SoundEffect soundBombWall,
                                  soundPlasmaCharge,
                                  soundPlasmaFire,
                                  soundPlasmaFadeOut,
                                  soundPlasmaHardWall,
                                  soundSizzle,
                                  soundFootstep2,
                                  soundFootstep3,
                                  soundFootstep4,
                                  soundSwitchOn,
                                  soundSwitchOff,
                                  soundScream,
                                  soundFuelGet,
                                  soundPlasmaAbsorb,
                                  soundAmmo,
                                  soundFuelCellBroken,
                                  soundBombCharge,
                                  soundNoAmmo,
                                  soundSplash,
                                  soundHumming,
                                  soundPickUp,
                                  soundDrop,
                                  soundLevelLoad;

        //Fonts
        static public SpriteFont fontMenu,
                                 fontEditor,
                                 fontButtonName,
                                 fontFloater;

        //Temp rendering target
        static public RenderTarget2D tempTarget;

        //Initialize template particles
        static public Particle dustP,
                               chargeP,
                               trailP,
                               powerP,
                               burstP,
                               deathP,
                               fireP,
                               fuelP,
                               splashP,
                               explosionP,
                               cloudP,
                               oozeP;

        //Random
        static public Random rand = new Random();
        
        //Control variables
        
        //Screens:
        //0 - Loading
        //1 - Menu
        //2 - Play
        //3 - Transition

        //Menus:
        //0 - Main
        //1 - Credits
        //2 - Options
        //3 - Controls
        //4 - Level Select

        static public int currentScreen = 0,
                          currentLevel = 1,
                          currentMenu = 0,
                          editorLevelAmmo = 3;

        static public float currentScreenTimer = 0,
                            timePaused = -1f,
                            transitionTime = 2f,
                            timeTransitionStarted = -1f,
                            buttonAlpha = 0,
                            buttonScrollY = 0,
                            titleHeight,
                            titleAlpha,
                            menuLastChanged = -2f,
                            menuSlow = .2f,
                            editorCursorAni = 0f,
                            salvEditRot = 0f,
                            loreAlpha = 0f,
                            cloudChance = 700f;
        
        bool menuExplosion = false;
        static public int pulseIds = 0,
                          plasmaIds = 0,
                          buttonIds = 0,
                          editorSelection = 1,
                          numFuelCells,
                          levelWidth = 1280,
                          levelHeight = 768,
                          particlesPerDeath = 50,
                          currentBackground = 1;
        static public bool transitioning = false,
                           paused = false,
                           beatLevel = false,
                           exitingGame = false,
                           levelEditing = false,
                           playTesting = false,
                           muted = false,
                           mustRenameLevel = false;

        static public string currentLanguage = "English",
                             levelName = "";
        static public string[] theText = new string[100];

        static public Vector2 levelCenter = new Vector2(levelWidth / 2, levelHeight / 2),
                              cameraPosition = levelCenter,
                              cloudVelocity,
                              salvagerEdit = new Vector2(-64, -64);

        //Object lists
        static public Salvager theSalvager;
        static public Rifle theRifle;
        static public TextInput theTextInput = null;
        static public List<Button> buttons = new List<Button>();
        static public List<FuelCell> fuelCells = new List<FuelCell>();
        static public List<Plasma> plasmas = new List<Plasma>();
        static public List<Ammo> ammos = new List<Ammo>();
        static public List<PlasmaFilm> plasmaFilms = new List<PlasmaFilm>();
        static public List<Node> nodes = new List<Node>();
        static public List<Receiver> receivers = new List<Receiver>();
        static public List<Toggler> togglers = new List<Toggler>();
        static public List<Detector> detectors = new List<Detector>();
        static public List<PressurePlate> pressurePlates = new List<PressurePlate>();
        static public List<Mirror> mirrors = new List<Mirror>();
        static public List<PlasmaLancer> plasmaLancers = new List<PlasmaLancer>();
        static public List<Furnace> furnaces = new List<Furnace>();
        static public List<Wall> walls = new List<Wall>();
        static public List<HardWall> hardWalls = new List<HardWall>();
        static public List<CrackedWall> crackedWalls = new List<CrackedWall>();
        static public List<BombWall> bombWalls = new List<BombWall>();
        static public List<WallExplosion> wallExplosions = new List<WallExplosion>();
        static public List<Ice> icePatches = new List<Ice>();
        static public List<Waste> wastes = new List<Waste>();
        static public List<ExplosionArea> explosionAreas = new List<ExplosionArea>();

        static public List<Particle> particles = new List<Particle>();
        static public List<Particle> deathParticles = new List<Particle>();
        static public List<Floater> floaters = new List<Floater>();

        public Nanozin()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Mouse visible
            IsMouseVisible = false;

            //Set backbuffer size
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();

            //Template particle definitions
            //Key                  texture      origin               position                   velocity               acceleration     friction  rot  sscale  escale        scolor                           ecolor          duration  salpha  ealpha  depth
            dustP = new Particle(      0,       new Vector2(32, 32), new Vector2(0, 0),     new Vector2(0, 0),      new Vector2(0, 0),    1f,     0f,   3.5f,   3.5f,   new Color(255, 255, 255),         new Color(0, 0, 0),     20,     1f,    1f,   .9f);
            chargeP = new Particle(    1,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0),  .96f,     0f,    .1f,    .1f,    new Color(255, 255, 255),    new Color(0, 255, 255),      1,    .3f,   .9f,   .91f);
            trailP = new Particle(     2,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0),    1f,     0f,    .2f,     0f,    new Color(125, 255, 255),    new Color(0, 255, 255),    .5f,    .9f,    0f,   .91f);
            powerP = new Particle(     2,       new Vector2(32, 32), new Vector2(0, 0),  new Vector2(0, -2.5f),     new Vector2(0, 0),    1f,     0f,    .2f,   .05f,      new Color(120, 0, 255),    new Color(120, 0, 255),   .15f,    .9f,   .8f,   .7f);
            burstP = new Particle(     2,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0), 1.01f,     0f,    .1f,    .1f,      new Color(0, 255, 255),    new Color(0, 255, 255),     1f,    .9f,    0f,   .91f);
            deathP = new Particle(     3,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0),    1f,     0f,    .3f,    .3f,     new Color(220, 175, 50),   new Color(220, 175, 59),     5f,    .7f,   .7f,   .7f);
            fireP = new Particle(      5,       new Vector2(32, 32), new Vector2(0, 0),   new Vector2(0, -.8f),     new Vector2(0, 0),    1f,     0f,    .3f,    .7f,        new Color(160, 0, 0),     new Color(20, 20, 20),     3f,     1f,    1f,   .91f);
            fuelP = new Particle(      6,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0),    1f,    .2f,    .4f,    .6f,    new Color(140, 140, 140),  new Color(140, 140, 140),     1f,     1f,    0f,   .7f);
            splashP = new Particle(    7,       new Vector2(32, 32), new Vector2(0, 0),  new Vector2(0, -1.2f),     new Vector2(0, 0),    1f,     0f,    .05f,    0f,     new Color(50, 120, 255),  new Color(255, 255, 255),    .3f,    .5f,   .5f,   .55f);
            explosionP = new Particle( 8,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0),    1f,     0f,    .5f,     3f,        new Color(255, 0, 0),        new Color(0, 0, 0),   2.5f,     1f,    0f,   .85f);
            cloudP = new Particle(     9,       new Vector2(32, 32), new Vector2(0, 0),      new Vector2(0, 0),     new Vector2(0, 0),    1f,     0f,     3f,     3f,          new Color(0, 0, 0),        new Color(0, 0, 0),  9999f,     1f,    1f,   .9f);
            oozeP = new Particle(     10,       new Vector2(32, 32), new Vector2(0, 0), new Vector2(0, -0.03f),     new Vector2(0, 0),    1f,     0f,   .09f,  .025f,     new Color(170, 200, 70),  new Color(255, 255, 255),     4f,     1f,   .4f,   .9f);

            //Load English
            Functions.loadLanguage("English");

            //Intro particles
            for (int i = 0; i < 110; i++ )
                Functions.addTemplateParticle(dustP, new Vector2((float)((rand.Next() % SCREEN_WIDTH)), SCREEN_HEIGHT + 20 + (rand.Next() % 90)));
            for (int i = 0; i < 110; i++ )
                Functions.addTemplateParticle(dustP, new Vector2((float)((rand.Next() % SCREEN_WIDTH)), -20 - (rand.Next() % 90)));

            titleHeight = 800;
            titleAlpha = 0;

            base.Initialize();
        }

        //Load the content
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            dustTexture = Content.Load<Texture2D>("dust");
            chargeTexture = Content.Load<Texture2D>("charge");
            starTexture = Content.Load<Texture2D>("star");
            cloudsTexture = Content.Load<Texture2D>("clouds");

            buttonTexture = Content.Load<Texture2D>("Button");
            editorTexture = Content.Load<Texture2D>("Editor");
            salvagerRifleTexture = Content.Load<Texture2D>("SalvagerWithRifle");
            salvagerTexture = Content.Load<Texture2D>("SalvagerWithoutRifle");
            fuelCellTexture = Content.Load<Texture2D>("FuelCell");
            plasmaRifleTexture = Content.Load<Texture2D>("PlasmaRifle");
            ammoTexture = Content.Load<Texture2D>("Ammo");
            plasmaProjectileTexture = Content.Load<Texture2D>("PlasmaProjectile");
            plasmaFilmTexture = Content.Load<Texture2D>("PlasmaFilm");
            pressurePlateTexture = Content.Load<Texture2D>("PressurePlate");
            wallsTexture = Content.Load<Texture2D>("Walls");
            detectorTexture = Content.Load<Texture2D>("Detector");
            mirrorTexture = Content.Load<Texture2D>("Mirror");
            plasmaLancerTexture = Content.Load<Texture2D>("PlasmaLancer");
            furnaceTexture = Content.Load<Texture2D>("Furnace");
            iceTexture = Content.Load<Texture2D>("Ice");
            wasteTexture = Content.Load<Texture2D>("Waste");
            ground1Texture = Content.Load<Texture2D>("Ground1");
            ground2Texture = Content.Load<Texture2D>("Ground2");
            ground3Texture = Content.Load<Texture2D>("Ground3");
            ground4Texture = Content.Load<Texture2D>("Ground4");

            soundBombWall = Content.Load<SoundEffect>("Bombwall");
            soundPlasmaCharge = Content.Load<SoundEffect>("PlasmaCharge");
            soundPlasmaFire = Content.Load<SoundEffect>("PlasmaFire");
            soundPlasmaFadeOut = Content.Load<SoundEffect>("PlasmaFadeOut");
            soundPlasmaHardWall = Content.Load<SoundEffect>("PlasmaHardWall");
            soundSizzle = Content.Load<SoundEffect>("Sizzle");
            soundFootstep2 = Content.Load<SoundEffect>("Footstep2");
            soundFootstep3 = Content.Load<SoundEffect>("Footstep3");
            soundFootstep4 = Content.Load<SoundEffect>("Footstep4");
            soundSwitchOn = Content.Load<SoundEffect>("SwitchOn");
            soundSwitchOff = Content.Load<SoundEffect>("SwitchOff");
            soundScream = Content.Load<SoundEffect>("Scream");
            soundFuelGet = Content.Load<SoundEffect>("FuelGet");
            soundPlasmaAbsorb = Content.Load<SoundEffect>("PlasmaAbsorb");
            soundAmmo = Content.Load<SoundEffect>("AmmoGain");
            soundFuelCellBroken = Content.Load<SoundEffect>("FuelCellBroken");
            soundBombCharge = Content.Load<SoundEffect>("BombCharge");
            soundNoAmmo = Content.Load<SoundEffect>("NoAmmo");
            soundSplash = Content.Load<SoundEffect>("Splash");
            soundHumming = Content.Load<SoundEffect>("Humming");
            soundPickUp = Content.Load<SoundEffect>("PickUp");
            soundDrop = Content.Load<SoundEffect>("Drop");
            soundLevelLoad = Content.Load<SoundEffect>("LevelLoad");

            fontMenu = Content.Load<SpriteFont>("fontMenu");
            fontEditor = Content.Load<SpriteFont>("fontEditor");
            fontButtonName = Content.Load<SpriteFont>("fontButtonName");
            fontFloater = Content.Load<SpriteFont>("fontFloater");

            //Create temp rendering target
            tempTarget = new RenderTarget2D(GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);

            //Shader
            fade = Content.Load<Effect>("Fade");
            fadeLevel = fade.Parameters["fadeLevel"];

            //Define particle texture array
            particleTextures[0] = dustTexture;
            particleTextures[1] = chargeTexture;
            particleTextures[2] = chargeTexture;
            particleTextures[3] = chargeTexture;
            particleTextures[4] = starTexture;
            particleTextures[5] = dustTexture;
            particleTextures[6] = starTexture;
            particleTextures[7] = chargeTexture;
            particleTextures[8] = dustTexture;
            particleTextures[9] = cloudsTexture;
            particleTextures[10] = chargeTexture;
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
            if (currentScreen == -1)
            {
                //GraphicsDevice.Clear(Color.Black);
                //base.Draw(gameTime);

                currentScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentScreenTimer > 7)
                {
                    currentScreenTimer = 0;
                    currentScreen = 0;
                }
            }
            else
            {
                theMouse = Mouse.GetState();

                //Add time in current screen
                currentScreenTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Particles
                if (particles.Count > 0 && !paused)
                {
                    //Update and Remove particles
                    for (int i = 0; i < particles.Count; i++)
                    {
                        particles[i].mAge += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        particles[i].update();

                        if (particles[i].isTrash)
                            particles.RemoveAt(i);
                    }
                }

                //Load Up Screen
                if (currentScreen == 0)
                {
                    IsMouseVisible = false;

                    //Go to Menu
                    if (currentScreenTimer >= 7)
                    {
                        currentScreen = 1;
                    }

                    //Title intro
                    if (titleHeight > 90)
                        titleHeight -= 2f;

                    if (titleAlpha < 1)
                    {
                        titleAlpha += .002f;
                        if (titleAlpha > 1)
                            titleAlpha = 1;
                    }

                    //Skip to menu
                    if ((Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Space)) && menuLastChanged < currentScreenTimer - menuSlow)
                    {
                        currentScreen = 1;
                        menuLastChanged = currentScreenTimer;
                    }
                }
                else if (currentScreen == 1)
                {
                    IsMouseVisible = true;

                    //Spawn buttons
                    if (buttons.Count == 0)
                    {
                        Mouse.SetPosition((int)levelCenter.X - 10, (int)levelCenter.Y - 2);
                        buttons.Add(new Button(new Vector2(250, 190)));
                        buttons.Add(new Button(new Vector2(650, 190)));
                        buttons.Add(new Button(new Vector2(250, 400)));
                        buttons.Add(new Button(new Vector2(450, 500)));
                        buttons.Add(new Button(new Vector2(650, 400)));
                    }

                    //Button Alpha
                    if (buttonAlpha < .4f)
                    {
                        buttonAlpha += .0025f;
                    }

                    //Particle explosion
                    if (!menuExplosion && buttonAlpha >= .2f)
                    {
                        if (!muted)
                            soundPlasmaCharge.Play();

                        menuExplosion = true;

                        for (int i = 0; i < 500; i++)
                        {
                            Functions.addTemplateParticle(chargeP, new Vector2(SCREEN_WIDTH / 2, SCREEN_HEIGHT / 2));
                        }
                    }

                    //Update Buttons
                    for (int i = 0; i < buttons.Count; i++ )
                    {
                        buttons[i].update();
                    }

                    if (exitingGame)
                        this.Exit();

                }
                else if (currentScreen == 2)
                {
                    if (paused)
                    {
                        if (fadeLevel.GetValueSingle() > .25f)
                            fadeLevel.SetValue(fadeLevel.GetValueSingle() - .01f);

                        //Scroll editor buttons
                        if (levelEditing && theMouse.X > 800)
                        {
                            int maxY = (buttonIds * 74);

                            if (theMouse.Y <= SCREEN_HEIGHT * .25 && buttonScrollY > 0)
                            {
                                buttonScrollY -= 12;
                                if (buttonScrollY < 0)
                                    buttonScrollY = 0;
                            }
                            else if (theMouse.Y >= SCREEN_HEIGHT * .75 && buttonScrollY < maxY)
                            {
                                buttonScrollY += 12;
                                if (buttonScrollY > maxY)
                                    buttonScrollY = maxY;
                            }
                        }

                        //Unpause
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape) && menuLastChanged < currentScreenTimer - menuSlow)
                        {
                            paused = false;
                            currentMenu = 0;
                            currentScreenTimer = timePaused;
                            menuLastChanged = currentScreenTimer;
                        }
                    }
                    else if (!paused)
                    {
                        IsMouseVisible = false;

                        //Pause
                        if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !transitioning && menuLastChanged < currentScreenTimer - menuSlow && theTextInput == null)
                        {
                            paused = true;
                            Nanozin.currentMenu = 0;
                            timePaused = currentScreenTimer;
                            menuLastChanged = currentScreenTimer;
                            IsMouseVisible = true;
                            fadeLevel.SetValue(.5f);
                        }
                        if (levelEditing)
                        {
                            Rectangle mousePos = new Rectangle(theMouse.X + (int)(cameraPosition.X - Nanozin.SCREEN_MID.X), theMouse.Y + (int)(cameraPosition.Y - Nanozin.SCREEN_MID.Y), 1, 1);

                            if (mustRenameLevel)
                            {
                                if (theTextInput == null)
                                    theTextInput = new TextInput("getFileName", "Must Rename Level:", "", 0);
                            }

                            //Remove Objects
                            if (theMouse.RightButton == ButtonState.Pressed)
                            {
                                if (editorSelection != 0)
                                {
                                    int index;

                                    if (new Rectangle((int)salvagerEdit.X, (int)salvagerEdit.Y, 64, 64).Intersects(mousePos))
                                        salvagerEdit = new Vector2(-64, -64);

                                    if (theRifle.mBoundingBox.Intersects(mousePos))
                                        theRifle.mPosition = new Vector2(-64, -64);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "walls", 0);
                                    if (index != -1)
                                        walls.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "ammos", 0);
                                    if (index != -1)
                                        ammos.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "plasmaFilms", 0);
                                    if (index != -1)
                                        plasmaFilms.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "nodes", 0);
                                    if (index != -1)
                                        nodes.RemoveAt(index);
                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "receivers", 0);
                                    if (index != -1)
                                        receivers.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "togglers", 0);
                                    if (index != -1)
                                        togglers.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "detectors", 0);
                                    if (index != -1)
                                        detectors.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "pressurePlates", 0);
                                    if (index != -1)
                                        pressurePlates.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "mirrors", 0);
                                    if (index != -1)
                                        mirrors.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "plasmaLancers", 0);
                                    if (index != -1)
                                        plasmaLancers.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "furnaces", 0);
                                    if (index != -1)
                                        furnaces.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "hardWalls", 0);
                                    if (index != -1)
                                        hardWalls.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "crackedWalls", 0);
                                    if (index != -1)
                                        crackedWalls.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "bombWalls", 0);
                                    if (index != -1)
                                        bombWalls.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "icePatches", 0);
                                    if (index != -1)
                                        icePatches.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "fuelCells", 0);
                                    if (index != -1)
                                        fuelCells.RemoveAt(index);

                                    index = Functions.checkObjectCollision(mousePos, 0, 0, "wastes", 0);
                                    if (index != -1)
                                        wastes.RemoveAt(index);
                                }
                                //Change second object attribute
                                else
                                {
                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                    {
                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;

                                        //Player ammo
                                        if (new Rectangle((int)salvagerEdit.X, (int)salvagerEdit.Y, 64, 64).Intersects(mousePos))
                                        {
                                            theTextInput = new TextInput("setSalvagerAmmo", "Starting Ammo:", editorLevelAmmo.ToString(), 0);
                                        }
                                    
                                        //Ammo supply
                                        for(int i = 0; i < ammos.Count; i++)
                                        {
                                            if (ammos[i].mBoundingBox.Intersects(mousePos))
                                            {
                                                theTextInput = new TextInput("setAmmoSupply", "  Ammo Count:", ammos[i].supply.ToString(), i);
                                            }
                                        }

                                        //Lancer CD
                                        for (int i = 0; i < plasmaLancers.Count; i++)
                                        {
                                            if (plasmaLancers[i].mBoundingBox.Intersects(mousePos))
                                            {
                                                theTextInput = new TextInput("setLancerCD", "Lancer Cooldown:", plasmaLancers[i].coolDown.ToString(), i);
                                            }
                                        }

                                        //Toggler CD
                                        for (int i = 0; i < togglers.Count; i++)
                                        {
                                            if (togglers[i].mBoundingBox.Intersects(mousePos))
                                            {
                                                theTextInput = new TextInput("setTogglerCD", "Toggler Cooldown:", togglers[i].toggleCD.ToString(), i);
                                            }
                                        }

                                        if (theTextInput == null
                                            && theMouse.X >= 0
                                            && theMouse.Y >= 0
                                            && theMouse.X < SCREEN_WIDTH
                                            && theMouse.Y < SCREEN_HEIGHT
                                            && (!(new Rectangle((int)salvagerEdit.X, (int)salvagerEdit.Y, 64, 64).Intersects(mousePos)) || editorSelection == 14)
                                            && (!theRifle.mBoundingBox.Intersects(mousePos) || editorSelection == 1)
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "plasmas", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "ammos", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "plasmaFilms", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "nodes", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "receivers", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "togglers", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "detectors", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "pressurePlates", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "mirrors", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "plasmaLancers", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "furnaces", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "walls", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "hardWalls", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "crackedWalls", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "bombWalls", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "icePatches", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "fuelCells", 0) == -1
                                            && Functions.checkObjectCollision(mousePos, 0, 0, "wastes", 0) == -1)
                                        {
                                            Nanozin.currentBackground += 1;
                                            if (Nanozin.currentBackground > 4)
                                                Nanozin.currentBackground = 1;
                                        }
                                    }
                                }
                            }
                            //Add objects
                            else if (theMouse.LeftButton == ButtonState.Pressed)
                            {
                                if (editorSelection != 0)
                                {
                                    if (theTextInput == null
                                        && theMouse.X >= 0
                                        && theMouse.Y >= 0
                                        && theMouse.X < SCREEN_WIDTH
                                        && theMouse.Y < SCREEN_HEIGHT
                                        && (!(new Rectangle((int)salvagerEdit.X, (int)salvagerEdit.Y, 64, 64).Intersects(mousePos)) || editorSelection == 14)
                                        && (!theRifle.mBoundingBox.Intersects(mousePos) || editorSelection == 1)
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "plasmas", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "ammos", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "plasmaFilms", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "nodes", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "receivers", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "togglers", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "detectors", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "pressurePlates", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "mirrors", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "plasmaLancers", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "furnaces", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "walls", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "hardWalls", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "crackedWalls", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "bombWalls", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "icePatches", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "fuelCells", 0) == -1
                                        && Functions.checkObjectCollision(mousePos, 0, 0, "wastes", 0) == -1)
                                    {

                                        switch (editorSelection)
                                        {
                                            case 1:
                                                salvagerEdit = new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64));
                                                break;
                                            case 2:
                                                walls.Add(new Wall(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 3:
                                                crackedWalls.Add(new CrackedWall(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 4:
                                                hardWalls.Add(new HardWall(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 5:
                                                nodes.Add(new Node(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 6:
                                                receivers.Add(new Receiver(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 7:
                                                mirrors.Add(new Mirror(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 8:
                                                pressurePlates.Add(new PressurePlate(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 9:
                                                plasmaFilms.Add(new PlasmaFilm(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 10:
                                                ammos.Add(new Ammo(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 11:
                                                icePatches.Add(new Ice(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 12:
                                                bombWalls.Add(new BombWall(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 13:
                                                fuelCells.Add(new FuelCell(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 14:
                                                theRifle.mPosition = new Vector2(mousePos.X - (mousePos.X % 64) + 32, mousePos.Y - (mousePos.Y % 64) + 32);
                                                break;
                                            case 15:
                                                plasmaLancers.Add(new PlasmaLancer(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 16:
                                                togglers.Add(new Toggler(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 17:
                                                furnaces.Add(new Furnace(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 18:
                                                detectors.Add(new Detector(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                            case 19:
                                                wastes.Add(new Waste(new Vector2(mousePos.X - (mousePos.X % 64), mousePos.Y - (mousePos.Y % 64))));
                                                break;
                                        }
                                    }
                                }
                                //Change object rotations
                                else
                                {
                                    if (Nanozin.menuLastChanged < Nanozin.currentScreenTimer - Nanozin.menuSlow)
                                    {
                                        Nanozin.menuLastChanged = Nanozin.currentScreenTimer;

                                        //Player
                                        if (new Rectangle((int)salvagerEdit.X, (int)salvagerEdit.Y, 64, 64).Intersects(mousePos))
                                        {
                                            salvEditRot += ((float)Math.PI / 2f);
                                            if (salvEditRot >= 2f * Math.PI)
                                                salvEditRot -= 2f * (float)Math.PI;
                                        }

                                        //Rifle
                                        if (theRifle.mBoundingBox.Intersects(mousePos))
                                        {
                                            theRifle.mRotation += ((float)Math.PI / 2f);
                                            if (theRifle.mRotation >= 2f * Math.PI)
                                                theRifle.mRotation -= 2f * (float)Math.PI;
                                        }

                                        //Mirrors
                                        for (int i = 0; i < mirrors.Count; i++)
                                        {
                                            if (mirrors[i].mBoundingBox.Intersects(mousePos) && mirrors[i].rotateAmount <= 0)
                                            {
                                                mirrors[i].rotateAmount += (float)(Math.PI / 2f);
                                            }
                                        }

                                        //Lancers
                                        for (int i = 0; i < plasmaLancers.Count; i++)
                                        {
                                            if (plasmaLancers[i].mBoundingBox.Intersects(mousePos) && plasmaLancers[i].rotateAmount <= 0)
                                            {
                                                plasmaLancers[i].rotateAmount += (float)(Math.PI / 2f);
                                            }
                                        }

                                        //Toggler beginning state
                                        for (int i = 0; i < togglers.Count; i++)
                                        {
                                            if (togglers[i].mBoundingBox.Intersects(mousePos))
                                            {
                                                if (togglers[i].powering)
                                                    togglers[i].powering = false;
                                                else
                                                    togglers[i].powering = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Update Camera
                        if (theSalvager != null)
                            cameraPosition = new Vector2(theSalvager.mPosition.X - 32, theSalvager.mPosition.Y - 32);
                        else if (levelEditing && theTextInput == null)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.W) || (theMouse.Y < SCREEN_HEIGHT * .15 && theMouse.Y > 0 && fadeLevel.GetValueSingle() > .7f && theMouse.LeftButton != ButtonState.Pressed && theMouse.RightButton != ButtonState.Pressed))
                                cameraPosition.Y -= 8;
                            if (Keyboard.GetState().IsKeyDown(Keys.A) || (theMouse.X < SCREEN_WIDTH * .15 && theMouse.X > 0 && fadeLevel.GetValueSingle() > .7f && theMouse.LeftButton != ButtonState.Pressed && theMouse.RightButton != ButtonState.Pressed))
                                cameraPosition.X -= 8;
                            if (Keyboard.GetState().IsKeyDown(Keys.S) || (theMouse.Y > SCREEN_HEIGHT * .85 && theMouse.Y < SCREEN_HEIGHT && fadeLevel.GetValueSingle() > .7f && theMouse.LeftButton != ButtonState.Pressed && theMouse.RightButton != ButtonState.Pressed))
                                cameraPosition.Y += 8;
                            if (Keyboard.GetState().IsKeyDown(Keys.D) || (theMouse.X > SCREEN_WIDTH * .85 && theMouse.X < SCREEN_WIDTH && fadeLevel.GetValueSingle() > .7f && theMouse.LeftButton != ButtonState.Pressed && theMouse.RightButton != ButtonState.Pressed))
                                cameraPosition.X += 8;
                        }

                        if (cameraPosition.X < SCREEN_MID.X)
                            cameraPosition.X = SCREEN_MID.X;
                        else if (cameraPosition.X > levelWidth - SCREEN_MID.X)
                            cameraPosition.X = levelWidth - SCREEN_MID.X;
                        if (cameraPosition.Y < SCREEN_MID.Y)
                            cameraPosition.Y = SCREEN_MID.Y;
                        else if (cameraPosition.Y > levelHeight - SCREEN_MID.Y)
                            cameraPosition.Y = levelHeight - SCREEN_MID.Y;

                        //Don't allow Id's to go near variable max value
                        if (pulseIds > Int32.MaxValue / 2)
                            pulseIds = 0;
                        if (plasmaIds > Int32.MaxValue / 2)
                            plasmaIds = 0;

                        //Dev cheat
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !transitioning && menuLastChanged < currentScreenTimer - menuSlow && !levelEditing)
                        {
                            currentScreenTimer = 0f;
                            //Instant:
                            //menuLastChanged = 0f;
                            //Functions.endPlay();
                            //Nanozin.currentLevel++;
                            //Functions.startPlay();

                            //Transition:
                            transitioning = false;
                            menuLastChanged = 0f;
                            currentScreen = 3;
                            loreAlpha = 0f;

                            if (currentLevel < NUM_LEVELS)
                                currentLevel++;
                            else
                                currentLevel = 1;
                            Functions.endPlay();
                        }

                        //Add cloud particles
                        if (Nanozin.rand.Next() % cloudChance == 0)
                        {
                            float posX,
                                  posY;

                            cloudChance = 700f;
                        
                            if (cloudVelocity.X > 0)
                            {
                                if (cloudVelocity.Y > 0)
                                {
                                    if (rand.Next() % 2 == 0)
                                    {
                                        posX = (rand.Next() % levelWidth + 512) - 256;
                                        posY = -512;
                                    }
                                    else
                                    {
                                        posX = -512;
                                        posY = (rand.Next() % levelHeight + 512) - 256;
                                    }
                                }
                                else
                                {
                                    if (rand.Next() % 2 == 0)
                                    {
                                        posX = (rand.Next() % levelWidth + 512) - 256;
                                        posY = levelHeight + 512;
                                    }
                                    else
                                    {
                                        posX = -512;
                                        posY = (rand.Next() % levelHeight + 512) - 256;
                                    }
                                }
                            }
                            else
                            {
                                if (cloudVelocity.Y > 0)
                                {
                                    if (rand.Next() % 2 == 0)
                                    {
                                        posX = (rand.Next() % levelWidth + 512) - 256;
                                        posY = -512;
                                    }
                                    else
                                    {
                                        posX = levelWidth + 512;
                                        posY = (rand.Next() % levelHeight + 512) - 256;
                                    }
                                }
                                else
                                {
                                    if (rand.Next() % 2 == 0)
                                    {
                                        posX = (rand.Next() % levelWidth + 512) - 256;
                                        posY = levelHeight + 512;
                                    }
                                    else
                                    {
                                        posX = levelWidth + 512;
                                        posY = (rand.Next() % levelHeight + 512) - 256;
                                    }
                                }
                            }
                        
                            Functions.addTemplateParticle(Nanozin.cloudP, new Vector2(posX, posY));
                        }
                        else
                        {
                            if (cloudChance > 50f && rand.Next() % 4 == 0)
                                cloudChance -= 1f;
                        }

                        //Update objects
                        bool delete;

                        if (theSalvager != null)
                            theSalvager.update();

                        if (theRifle != null)
                            theRifle.update();

                        for (int i = 0; i < plasmas.Count; i++)
                        {
                            delete = plasmas[i].update();
                            if (delete)
                            {
                                plasmas.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < bombWalls.Count; i++)
                        {
                            delete = bombWalls[i].update();
                            if (delete)
                            {
                                bombWalls.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < fuelCells.Count; i++)
                        {
                            delete = fuelCells[i].update();
                            if (delete)
                            {
                                fuelCells.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < icePatches.Count; i++)
                        {
                            icePatches[i].update();
                        }

                        for (int i = 0; i < plasmaFilms.Count; i++)
                        {
                            delete = plasmaFilms[i].update();
                            if (delete)
                            {
                                plasmaFilms.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < detectors.Count; i++)
                        {
                            detectors[i].update();
                        }

                        for (int i = 0; i < nodes.Count; i++)
                        {
                            nodes[i].update();
                        }

                        for (int i = 0; i < receivers.Count; i++)
                        {
                            receivers[i].update();
                        }

                        for (int i = 0; i < togglers.Count; i++)
                        {
                            togglers[i].update();
                        }

                        for (int i = 0; i < pressurePlates.Count; i++)
                        {
                            pressurePlates[i].update();
                        }

                        for (int i = 0; i < wastes.Count; i++)
                        {
                            wastes[i].update();
                        }

                        for (int i = 0; i < mirrors.Count; i++)
                        {
                            mirrors[i].update();
                        }

                        for (int i = 0; i < plasmaLancers.Count; i++)
                        {
                            delete = plasmaLancers[i].update();
                            if (delete)
                            {
                                plasmaLancers.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < furnaces.Count; i++)
                        {
                            furnaces[i].update();
                        }

                        for (int i = 0; i < crackedWalls.Count; i++)
                        {
                            delete = crackedWalls[i].update();
                            if (delete)
                            {
                                plasmaFilms.Add(new PlasmaFilm(crackedWalls[i].mPosition));

                                crackedWalls.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < wallExplosions.Count; i++)
                        {
                            delete = wallExplosions[i].update();
                            if (delete)
                            {
                                plasmaFilms.Add(new PlasmaFilm(wallExplosions[i].mPosition));

                                wallExplosions.RemoveAt(i);
                                i--;
                            }
                        }

                        for (int i = 0; i < explosionAreas.Count; i++)
                        {
                            explosionAreas[i].update();
                            explosionAreas.RemoveAt(i);
                            i--;
                        }

                        for (int i = 0; i < floaters.Count; i++)
                        {
                            delete = floaters[i].update();
                            if (delete)
                            {
                                floaters.RemoveAt(i);
                                i--;
                            }
                        }

                        //Update textInput
                        if (theTextInput != null)
                        {
                            delete = theTextInput.update();
                            if (delete)
                                theTextInput = null;
                        }

                        //Set Fade level
                        if (transitioning)
                        {
                            if (currentScreenTimer <= 1.2f)
                                fadeLevel.SetValue(currentScreenTimer / 1f);
                            else
                                fadeLevel.SetValue(((timeTransitionStarted + transitionTime) - currentScreenTimer) / transitionTime);
                        }
                        else if (!paused)
                        {
                            if (fadeLevel.GetValueSingle() > 1f)
                            {
                                fadeLevel.SetValue(fadeLevel.GetValueSingle() - .01f);
                                if (fadeLevel.GetValueSingle() < 1f)
                                    fadeLevel.SetValue(1f);
                            }
                            else if (fadeLevel.GetValueSingle() < 1f)
                            {
                                fadeLevel.SetValue(fadeLevel.GetValueSingle() + .01f);
                                if (fadeLevel.GetValueSingle() > 1f)
                                    fadeLevel.SetValue(1f);
                            }
                        }

                        //Transition out of level
                        if (timeTransitionStarted != -1 && currentScreenTimer >= timeTransitionStarted + transitionTime)
                        {
                            transitioning = false;
                            menuLastChanged = 0f;
                            currentScreen = 3;
                            loreAlpha = 0f;
                            if (!theSalvager.dead && numFuelCells == 0 && !playTesting && currentLevel < NUM_LEVELS)
                                currentLevel++;
                            Functions.endPlay();
                        }

                        //Stop transitioning into level
                        if (transitioning && currentScreenTimer >= 1 && timeTransitionStarted == -1)
                        {
                            transitioning = false;
                        }
                    }

                    //Update Buttons
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].update();
                    }
                }
                else if (currentScreen == 3)
                {
                    IsMouseVisible = false;

                    if (loreAlpha == 0f)
                    {
                        if (!muted)
                            soundLevelLoad.Play();
                    }

                    if (loreAlpha < 1f)
                    {
                        loreAlpha += .0075f;
                        if (loreAlpha > 1f)
                            loreAlpha = 1f;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || Nanozin.playTesting)
                    {
                        currentScreen = 2;
                        menuLastChanged = 0f;
                        Functions.startPlay();

                        //Set continue level
                        if (currentLevel != 1) 
                            Functions.setSave(currentLevel);
                    }
                    else if (Nanozin.particles.Count == 0 && Nanozin.deathParticles.Count > 0 && currentScreenTimer < 3f)
                    {
                        for (int i = 0; i < Nanozin.particlesPerDeath; i++)
                        {
                            Nanozin.particles.Add(new Particle());
                            Nanozin.particles[i].replicate(Nanozin.deathParticles[i]);
                        }
                    }
                }

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (currentScreen != -1)
            {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            //Screen Specific Things
            switch (currentScreen)
            {
                //Load Screen
                case 0:
                    float tmp;
                    if (titleHeight <= 350)
                        tmp = titleHeight;
                    else
                        tmp = 350;

                    Color titleColor = new Color(titleAlpha, titleAlpha, titleAlpha);

                    spriteBatch.DrawString(fontMenu, "Salvager of Nanozin", new Vector2(400, tmp), titleColor);

                    //Particles
                    if (particles.Count > 0)
                    {
                        for (int i = 0; i < particles.Count; i++)
                        {
                            particles[i].draw(spriteBatch);
                        }
                    }

                    spriteBatch.End();

                    break;
                //Menu Screen
                case 1:
                    string title = "";
                    
                    switch(currentMenu)
                    {
                        case 0:
                            title = theText[0];
                            break;
                        case 1:
                            title = theText[1];
                            spriteBatch.DrawString(fontMenu, theText[23] + "\n"
                                                             + theText[24] + "\n"
                                                             + theText[25] + "\n"
                                                             + theText[26] + "\n",
                                                             new Vector2(100, 190), Color.White);
                            break;
                        case 2:
                            title = theText[2];
                            break;
                        case 3:
                            title = theText[3];
                            spriteBatch.DrawString(fontMenu, theText[18] + "\n"
                                                           + theText[19] + "\n"
                                                           + theText[20] + "\n"
                                                           + theText[21] + "\n"
                                                           + theText[22] + "\n", 
                                                    new Vector2(300, 160), Color.White);
                            break;
                        case 4:
                            title = theText[0];
                            break;
                    }

                    spriteBatch.DrawString(fontMenu, title, new Vector2(400, 90), Color.White);

                    //Draw Buttons
                    for (int i = 0; i < buttons.Count; i++)
                    {
                        buttons[i].draw(spriteBatch);
                    }

                    //Particles
                    if (particles.Count > 0)
                    {
                        for (int i = 0; i < particles.Count; i++)
                        {
                            particles[i].draw(spriteBatch);
                        }
                    }

                    spriteBatch.End();

                    break;

                //Play Screen
                case 2:
                    //Save the active target binding and set the render target to a temp target
                    RenderTargetBinding[] activeTarget = GraphicsDevice.GetRenderTargets();
                    GraphicsDevice.SetRenderTarget(tempTarget);

                    GraphicsDevice.Clear(Color.Black);

                    Texture2D theBackground;
                    
                    switch (currentBackground)
                    {
                        case 1:
                            theBackground = ground1Texture;
                            break;
                        case 2:
                            theBackground = ground2Texture;
                            break;
                        case 3:
                            theBackground = ground3Texture;
                            break;
                        case 4:
                            theBackground = ground4Texture;
                            break;
                        default:
                            theBackground = ground1Texture;
                            break;
                    }

                    for (int i = 0; i < levelWidth; i += 256)
                        for (int j = 0; j < levelHeight; j += 256)
                            spriteBatch.Draw(theBackground, new Vector2(i - (cameraPosition.X - SCREEN_MID.X), j - (cameraPosition.Y - SCREEN_MID.Y)), Color.White);

                    //Draw Ammo
                    for (int i = 0; i < ammos.Count; i++)
                    {
                        ammos[i].draw(spriteBatch);
                    }

                    //Draw FuelCells
                    for (int i = 0; i < fuelCells.Count; i++)
                    {
                        fuelCells[i].draw(spriteBatch);
                    }

                    //Draw Films
                    for (int i = 0; i < plasmaFilms.Count; i++)
                    {
                       plasmaFilms[i].draw(spriteBatch);
                    }

                    //Draw PressurePlates
                    for (int i = 0; i < pressurePlates.Count; i++)
                    {
                        pressurePlates[i].draw( spriteBatch );
                    }

                    //Draw ice patches
                    for (int i = 0; i < icePatches.Count; i++)
                    {
                        icePatches[i].draw(spriteBatch);
                    }

                    //Draw Wastes
                    for (int i = 0; i < wastes.Count; i++)
                    {
                        wastes[i].draw(spriteBatch);
                    }

                    //Draw Plasmas
                    for (int i = 0; i < plasmas.Count; i++)
                    {
                        plasmas[i].draw(spriteBatch);
                    }

                    //Draw Walls
                    for (int i = 0; i < walls.Count; i++)
                    {
                        walls[i].draw(spriteBatch);
                    }

                    //Draw HardWalls
                    for (int i = 0; i < hardWalls.Count; i++)
                    {
                        hardWalls[i].draw(spriteBatch);
                    }

                    //Draw CrackedWalls
                    for (int i = 0; i < crackedWalls.Count; i++)
                    {
                        crackedWalls[i].draw(spriteBatch);
                    }

                    //Draw BombWalls
                    for (int i = 0; i < bombWalls.Count; i++)
                    {
                        bombWalls[i].draw(spriteBatch);
                    }

                    //Draw Detectors
                    for (int i = 0; i < detectors.Count; i++)
                    {
                        detectors[i].draw(spriteBatch);
                    }

                    //Draw Nodes
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        nodes[i].draw(spriteBatch);
                    }

                    //Draw Receivers
                    for (int i = 0; i < receivers.Count; i++)
                    {
                        receivers[i].draw(spriteBatch);
                    }

                    //Draw Receivers
                    for (int i = 0; i < togglers.Count; i++)
                    {
                        togglers[i].draw(spriteBatch);
                    }

                    //Draw Mirrors
                    for (int i = 0; i < mirrors.Count; i++)
                    {
                        mirrors[i].draw(spriteBatch);
                    }

                    //Draw plasmaLancers
                    for (int i = 0; i < plasmaLancers.Count; i++)
                    {
                        plasmaLancers[i].draw(spriteBatch);
                    }

                    //Draw furnaces
                    for (int i = 0; i < furnaces.Count; i++)
                    {
                        furnaces[i].draw(spriteBatch);
                    }

                    //Draw WallExplosions
                    for (int i = 0; i < wallExplosions.Count; i++)
                    {
                        wallExplosions[i].draw(spriteBatch);
                    }

                    //Draw Rifle
                    if (theRifle != null)
                        theRifle.draw(spriteBatch);

                    //Draw Player
                    if (theSalvager != null && !theSalvager.dead)
                        theSalvager.draw(spriteBatch);

                    if (levelEditing)
                    {
                        //Draw text inputs
                        if (theTextInput != null)
                            theTextInput.draw(spriteBatch);

                        //Draw player in editor mode
                        Texture2D salvLook = salvagerTexture;
                        if (theRifle.mBoundingBox.Intersects(new Rectangle((int)salvagerEdit.X, (int)salvagerEdit.Y, 64, 64)))
                            salvLook = salvagerRifleTexture;

                        spriteBatch.Draw(salvLook,
                                         new Vector2(salvagerEdit.X - (Nanozin.cameraPosition.X - Nanozin.SCREEN_MID.X) + 32, salvagerEdit.Y - (Nanozin.cameraPosition.Y - Nanozin.SCREEN_MID.Y) + 32),
                                         new Rectangle(0, 0, 64, 64),
                                         Color.White,
                                         salvEditRot,
                                         new Vector2(32, 32),
                                         .8f,
                                         SpriteEffects.None,
                                         .5f);

                        if (!paused)
                        {
                            //Draw animated cursor
                            Rectangle theSource = new Rectangle(0, 0, 64, 64);

                            editorCursorAni += (float)gameTime.ElapsedGameTime.TotalSeconds;

                            float waitTime = .7f;
                            if (editorCursorAni < waitTime)
                                theSource.X = 0;
                            else if (editorCursorAni < waitTime + .07f)
                                theSource.X = 64;
                            else if (editorCursorAni < waitTime + .14f)
                                theSource.X = 128;
                            else if (editorCursorAni < waitTime + .21f)
                                theSource.X = 192;
                            else if (editorCursorAni < waitTime + .28f)
                                theSource.X = 256;
                            else
                                editorCursorAni = 0f;

                            Color theTint = Color.White;
                            if (editorSelection == 0)
                                theTint = Color.Lime;
                            else if (theMouse.RightButton == ButtonState.Pressed)
                                theTint = Color.Red;

                            Vector2 mousePos = new Vector2(theMouse.X + (int)(cameraPosition.X - Nanozin.SCREEN_MID.X), theMouse.Y + (int)(cameraPosition.Y - Nanozin.SCREEN_MID.Y));

                            spriteBatch.Draw(chargeTexture,
                                             new Vector2(theMouse.X, theMouse.Y),
                                             new Rectangle(0, 0, 64, 64),
                                             theTint * .8f,
                                             0f,
                                             new Vector2(32, 32),
                                             .15f,
                                             SpriteEffects.None,
                                             .81f);

                            spriteBatch.Draw(editorTexture,
                                             new Vector2(theMouse.X - (mousePos.X % 64), theMouse.Y - (mousePos.Y % 64)),
                                             theSource,
                                             theTint * .75f,
                                             0f,
                                             new Vector2(0, 0),
                                             1f,
                                             SpriteEffects.None,
                                             .8f);
                        }
                    }

                    //Particles
                    if (particles.Count > 0)
                    {
                        for (int i = 0; i < particles.Count; i++)
                        {
                            particles[i].draw(spriteBatch);
                        }
                    }

                    spriteBatch.End();

                    GraphicsDevice.SetRenderTargets(activeTarget);

                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    fade.CurrentTechnique.Passes[0].Apply();

                    spriteBatch.Draw(tempTarget, Vector2.Zero, Color.White);

                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                    //Death Particles
                    if (particles.Count > 0)
                    {
                        for (int i = 0; i < particles.Count; i++)
                        {
                            if (particles[i].mTextureIndex == 3)
                            particles[i].draw(spriteBatch);
                        }
                    }

                    //Floaters
                    if (floaters.Count > 0)
                    {
                        for (int i = 0; i < floaters.Count; i++)
                        {
                            floaters[i].draw(spriteBatch);
                        }
                    }

                    if (!transitioning)
                    {
                        //Draw player ammo
                        if (!levelEditing)
                        {
                            spriteBatch.Draw(plasmaRifleTexture, new Vector2(0, SCREEN_HEIGHT - SPRITE_LENGTH), new Rectangle(64, 0, 64, 64), Color.White);
                            spriteBatch.DrawString(fontMenu, theSalvager.mAmmo.ToString(), new Vector2(70, SCREEN_HEIGHT - 74), Color.Cyan);
                        }
                    }

                    if (paused)
                    {
                        if (currentMenu == 0)
                        {
                            if (!levelEditing)
                                spriteBatch.DrawString(fontMenu, Nanozin.theText[17], new Vector2(400, 90), Color.White);
                            else
                            {
                                spriteBatch.DrawString(fontMenu, Nanozin.theText[27], new Vector2(515, 90), Color.White);
                                spriteBatch.DrawString(fontMenu, levelName, new Vector2(46, 90), Color.White);
                                spriteBatch.DrawString(fontMenu, "__________________", new Vector2(46, 100), Color.White);
                            }
                        }
                        else
                        {
                            //Draw controls
                            spriteBatch.DrawString(fontMenu, Nanozin.theText[3], new Vector2(400, 90), Color.White);
                            spriteBatch.DrawString(fontMenu, theText[18] + "\n"
                                                             + theText[19] + "\n"
                                                             + theText[20] + "\n"
                                                             + theText[21] + "\n"
                                                             + theText[22] + "\n",
                                                             new Vector2(300, 160), Color.White);
                        }
                        
                        for (int i = 0; i < buttons.Count; i++)
                        {
                            buttons[i].draw(spriteBatch);
                        }
                    }

                    spriteBatch.End();
                    
                    break;

                case 3:
                    GraphicsDevice.Clear(Color.Black);

                    //Particles
                    if (particles.Count > 0)
                    {
                        for (int i = 0; i < particles.Count; i++)
                        {
                            particles[i].draw(spriteBatch);
                        }
                    }

                    if (currentLevel != -1)
                    {
                        spriteBatch.DrawString(fontMenu, "Level   " + currentLevel, new Vector2(555, 180), Color.White);
                        spriteBatch.DrawString(fontEditor, Nanozin.theText[27 + (currentLevel * 2)], new Vector2(0, 320), Color.White * loreAlpha);
                        spriteBatch.DrawString(fontEditor, Nanozin.theText[28 + (currentLevel * 2)], new Vector2(0, 380), Color.White * loreAlpha);
                    }

                    switch(currentLevel)
                    {
                        case 1:
                            spriteBatch.Draw(fuelCellTexture, 
                                             new Vector2(638, 396), 
                                             new Rectangle(0, 0, 64, 64),
                                             Color.White * loreAlpha,
                                             0f,
                                             new Vector2(32, 32),
                                             .8f,
                                             SpriteEffects.None,
                                             1f);
                            break;
                        case 2:
                            spriteBatch.Draw(plasmaLancerTexture,
                                             new Vector2(638 - 124, 464),
                                             new Rectangle(0, 0, 64, 64),
                                             Color.White * loreAlpha,
                                             0f,
                                             new Vector2(32, 32),
                                             .8f,
                                             SpriteEffects.None,
                                             1f);
                            spriteBatch.Draw(plasmaLancerTexture,
                                             new Vector2(638 - 124, 464),
                                             new Rectangle(128, 0, 64, 64),
                                             Color.White * loreAlpha,
                                             0f,
                                             new Vector2(32, 32),
                                             .8f,
                                             SpriteEffects.None,
                                             1f);
                            spriteBatch.Draw(wallsTexture,
                                             new Vector2(644, 464),
                                             new Rectangle(0, 0, 64, 64),
                                             Color.White * loreAlpha,
                                             0f,
                                             new Vector2(32, 32),
                                             .8f,
                                             SpriteEffects.None,
                                             1f);
                            spriteBatch.Draw(plasmaFilmTexture,
                                             new Vector2(638 + 132, 464),
                                             new Rectangle(0, 0, 64, 64),
                                             Color.White * loreAlpha,
                                             0f,
                                             new Vector2(32, 32),
                                             .8f,
                                             SpriteEffects.None,
                                             1f);
                            break;
                    }

                    if (currentScreenTimer >= 3f)
                    {
                        spriteBatch.DrawString(fontEditor, Nanozin.theText[28], new Vector2(575, 600), Color.White);
                    }

                    spriteBatch.End();

                    break;
            }
            
            base.Draw(gameTime);
        }
    }
    }
}