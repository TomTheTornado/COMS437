using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics;

namespace SpaceRace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont gameFont;
        
        private Space space = new Space();
        Spaceship ship;
        Ring r1, r2, r3, r4, r5, r6, r7, r8, r9;
        Skybox skybox;
        int countedRings, totalRings, missedRings;
        bool inSession, gameOver, restart;

        float timer = 0f;
        int currentScore = 0;
        int highScore = 0;
        float highScoreTime = 0f;

        private Camera camera
        {
            get;
            set;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            _graphics.PreferredBackBufferWidth = 1100; //width
            _graphics.PreferredBackBufferHeight = 890; //height
            _graphics.ApplyChanges(); //Changes the resolution

            Services.AddService<Space>(space);

            skybox = new Skybox(this);//skybox

            ship = new Spaceship(this, new Vector3(0, 0, 700), "A", 2, new Vector3(0, 0, 0), new Vector3(0, 0, 0));//spaceship

            //rings we use in the game, with positions and orientation
            r1 = new Ring(this, new Vector3(0, 0, -550), "B", 0, Vector3.Zero, Vector3.Zero);
            r2 = new Ring(this, new Vector3(400, 300, -2150), "C", 0, Vector3.Zero, Vector3.Zero);
            space.Entities[2].WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * -45 / 360f)) * space.Entities[2].WorldTransform;
            r3 = new Ring(this, new Vector3(2000, 500, -2650), "D", 0, Vector3.Zero, Vector3.Zero);
            space.Entities[3].WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * -90 / 360f)) * space.Entities[3].WorldTransform;
            r4 = new Ring(this, new Vector3(3600, 0, -2150), "E", 0, Vector3.Zero, Vector3.Zero);
            space.Entities[4].WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * 45 / 360f)) * space.Entities[4].WorldTransform;
            r5 = new Ring(this, new Vector3(4000, -300, -550), "F", 0, Vector3.Zero, Vector3.Zero);

            r6 = new Ring(this, new Vector3(4000, 0, 550), "G", 0, Vector3.Zero, Vector3.Zero);
            r7 = new Ring(this, new Vector3(3600, 200, 2150), "H", 0, Vector3.Zero, Vector3.Zero);
            space.Entities[7].WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * -45 / 360f)) * space.Entities[7].WorldTransform;
            r8 = new Ring(this, new Vector3(2000, 0, 2650), "I", 0, Vector3.Zero, Vector3.Zero);
            space.Entities[8].WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * 90 / 360f)) * space.Entities[8].WorldTransform;
            r9 = new Ring(this, new Vector3(400, -400, 2150), "J", 0, Vector3.Zero, Vector3.Zero);
            space.Entities[9].WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * 45 / 360f)) * space.Entities[9].WorldTransform;

            r1.activeRing = true;//setting the first ring to look different
            r9.finishRing = true;//setting the last ring to look like a checkerboard

            countedRings = 0;
            totalRings = 9;
            missedRings = 0;

            inSession = false;
            gameOver = true;
            restart = true;

            //camera for our scene
            camera = new Camera();

            Services.AddService<Camera>(camera);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFont = Content.Load<SpriteFont>("textFont");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            UpdateInput();
            //checks all rings and updates the timer
            if (!gameOver)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                LastRing(r9);
                UpdateRing(r8, r9);
                UpdateRing(r7, r8);
                UpdateRing(r6, r7);
                UpdateRing(r5, r6);
                UpdateRing(r4, r5);
                UpdateRing(r3, r4);
                UpdateRing(r2, r3);
                UpdateRing(r1, r2);
            }
            else
            {
                //calculates time
                if(timer > 0)
                {
                    currentScore = (int)Math.Ceiling((2500f / timer) + (countedRings * 100f * (30f/timer)));
                }
                //changes high score
                if(currentScore > highScore)
                {
                    int temp = currentScore;
                    highScore = currentScore;
                    highScoreTime = timer;
                }
            }


            // TODO: Add your update logic here
            Services.GetService<Space>().Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            //need to turn these off for 2d
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            
            _spriteBatch.Begin();
            _spriteBatch.DrawString(gameFont, "Rings: " + countedRings + " / " + totalRings + "   Missed Rings: " + missedRings, new Vector2(800, 3), Color.White);
            _spriteBatch.DrawString(gameFont, "Current Run Time: " + timer.ToString("0.00") + " seconds", new Vector2(800, 33), Color.White);
            _spriteBatch.DrawString(gameFont, "Best Score: " + highScore + "    Best Score Time: " + highScoreTime.ToString("0.00") + " seconds", new Vector2(3, 3), Color.White);
            if (gameOver)//displays last run score and time
            {
                _spriteBatch.DrawString(gameFont, "Last Run Score: " + currentScore + "  Time: " + timer.ToString("0.00") + " seconds", new Vector2(375, 600), Color.White);
                _spriteBatch.DrawString(gameFont, "Press 'r' at anytime to reset, then 'e' or 'space' to begin.", new Vector2(330, 640), Color.White);
            }
            _spriteBatch.End();

            //turn these back on for 3d
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;



        }

        protected void UpdateInput()
        {

            KeyboardState currentKeyState = Keyboard.GetState();

            float rotationSpeed = 1f;//Speed ship rotates at

            if (inSession)
            {
                //Yaw
                if (currentKeyState.IsKeyDown(Keys.Up))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.TwoPi * rotationSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currentKeyState.IsKeyDown(Keys.Down))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.TwoPi * -rotationSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }

                //Pitch
                if (currentKeyState.IsKeyDown(Keys.Right))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * -rotationSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currentKeyState.IsKeyDown(Keys.Left))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationY(Microsoft.Xna.Framework.MathHelper.TwoPi * rotationSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }

                //Roll
                if (currentKeyState.IsKeyDown(Keys.A))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationZ(Microsoft.Xna.Framework.MathHelper.TwoPi * rotationSpeed / 360f)) * ship.physicsObject.WorldTransform;

                }
                if (currentKeyState.IsKeyDown(Keys.D))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateRotationZ(Microsoft.Xna.Framework.MathHelper.TwoPi * -rotationSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }

                //Thrust
                if (currentKeyState.IsKeyDown(Keys.W) || currentKeyState.IsKeyDown(Keys.Space))
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateTranslation(new Vector3(0, 0, -15f))) * ship.physicsObject.WorldTransform;
                }
                if (currentKeyState.IsKeyDown(Keys.S)) //Moving backwards is slower
                {
                    ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.CreateTranslation(new Vector3(0, 0, 5f))) * ship.physicsObject.WorldTransform;
                }
            }
            if (restart)
            {
                if (currentKeyState.IsKeyDown(Keys.E) || currentKeyState.IsKeyDown(Keys.Space))//starts the game
                {
                    restart = false;
                    inSession = true;
                    gameOver = false;
                }
            }
            if (currentKeyState.IsKeyDown(Keys.R)) //resets all the variables
            {
                restart = true;
                inSession = false;
                gameOver = true;
                currentScore = 0;
                timer = 0f;

                countedRings = 0;
                totalRings = 9;
                missedRings = 0;
                r1.activeRing = true; r1.passedRing = false; r1.missedRing = false;
                r2.activeRing = false; r2.passedRing = false; r2.missedRing = false;
                r3.activeRing = false; r3.passedRing = false; r3.missedRing = false;
                r4.activeRing = false; r4.passedRing = false; r4.missedRing = false;
                r5.activeRing = false; r5.passedRing = false; r5.missedRing = false;
                r6.activeRing = false; r6.passedRing = false; r6.missedRing = false;
                r7.activeRing = false; r7.passedRing = false; r7.missedRing = false;
                r8.activeRing = false; r8.passedRing = false; r8.missedRing = false;
                r9.activeRing = false; r9.passedRing = false; r9.missedRing = false;



                //reset ship and rotation to initial position
                ship.physicsObject.Position = ConversionHelper.MathConverter.Convert(new Vector3(0,0,700));
                ship.physicsObject.WorldTransform = ConversionHelper.MathConverter.Convert(Matrix.Identity);
            }

            //Calculates the view of the camera
            camera.CameraPosition = ConversionHelper.MathConverter.Convert(ship.physicsObject.Position) + (ConversionHelper.MathConverter.Convert(ship.physicsObject.WorldTransform.Backward) * 530f) + ConversionHelper.MathConverter.Convert(ship.physicsObject.WorldTransform.Up);
            camera.CameraDirection = ConversionHelper.MathConverter.Convert(ship.physicsObject.Position);
            camera.CameraUp = ConversionHelper.MathConverter.Convert(ship.physicsObject.WorldTransform.Up);



        }

        //Checks rings and updates the ring information
        private void UpdateRing(Ring current, Ring next)
        {
            float dist = (ship.physicsObject.Position - current.physicsObject.Position).Length();
            //if passing through ring
            if(dist < 155 && !current.passedRing)
            {
                current.passedRing = true;
                countedRings++;
                if (current.activeRing)//for changing next ring in sequence
                {
                    next.activeRing = true;
                    current.activeRing = false;
                }
                if (current.missedRing)//for missed rings
                {
                    current.missedRing = false;
                    missedRings--;
                }
            }

            //checking missed rings
            if (!current.passedRing && !current.missedRing && (next.missedRing || next.passedRing)) 
            {
                current.missedRing = true;
                missedRings++;
            }

            //changing ring if active ring has been passed through beforehand
            if(current.passedRing && current.activeRing)
            {
                next.activeRing = true;
                current.activeRing = false;
            }
        }

        //Checking last ring to end game
        private void LastRing(Ring last)
        {
            float dist = (ship.physicsObject.Position - last.physicsObject.Position).Length();
            if (dist < 155)
            {
                last.passedRing = true;
                countedRings++;
                gameOver = true;
            }
        }
    }
}
