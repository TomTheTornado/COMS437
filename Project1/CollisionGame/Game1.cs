using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont gameFont;

        Rectangle northWall, southWall, eastWall, westWall, wallColor; //walls for our game
        Vector2 upperLeft, upperRight, lowerLeft, lowerRight; //corners of the walls
        float eastWallAngle, westWallAngle;
        int lineThickness = 6;
        int curScore = 0;
        int highScore = 0;

        Texture2D rainbowStrip;

        Sprite player = new Sprite();
        Sprite target1 = new Sprite();
        Sprite target2 = new Sprite();
        Sprite target3 = new Sprite();

        bool inSession = false;
        bool newBest = false;
        DateTime time = new DateTime();

        float timer = 60f;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = true;//Making sure game runs the same speed
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1100; //width
            _graphics.PreferredBackBufferHeight = 890; //height
            _graphics.ApplyChanges(); //Changes the resolution

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFont = Content.Load<SpriteFont>("collisionFont");//Font for Game Text
            rainbowStrip = Content.Load<Texture2D>("rainbowStrip");//Texture for Walls

            /*The following code for the walls are modified from LO3.pdf*/
            upperLeft = new Vector2(30, 40);
            upperRight = new Vector2(GraphicsDevice.Viewport.Width - 30, 40);
            lowerLeft = new Vector2(30, GraphicsDevice.Viewport.Height - 80);
            lowerRight = new Vector2(GraphicsDevice.Viewport.Width - 30, GraphicsDevice.Viewport.Height - 80);

            northWall = new Rectangle((int)(upperLeft.X - lineThickness), (int)(upperLeft.Y - lineThickness), (int)(upperRight.X - upperLeft.X), lineThickness);
            southWall = new Rectangle((int)lowerLeft.X, (int)lowerLeft.Y, (int)(lowerRight.X - lowerLeft.X), lineThickness);

            Vector2 eastWallVectorAngle = Vector2.Subtract(upperLeft, lowerLeft);
            eastWall = new Rectangle((int)upperLeft.X, (int)upperLeft.Y, (int)eastWallVectorAngle.Length() + lineThickness, lineThickness);
            eastWallAngle = (float)Math.Atan2(eastWallVectorAngle.Y, eastWallVectorAngle.X);

            Vector2 westWallVectorAngle = Vector2.Subtract(upperRight, lowerRight);
            westWall = new Rectangle((int)upperRight.X, (int)(upperRight.Y - lineThickness), (int)westWallVectorAngle.Length() + lineThickness, lineThickness);
            westWallAngle = (float)Math.Atan2(westWallVectorAngle.Y, westWallVectorAngle.X);
            wallColor = new Rectangle(0, 0, 9, 1);//Wall Texture


            /*Targets for Game*/
            target1.texture = Content.Load<Texture2D>("target");
            target1.position = new Vector2(0, 0);
            target1.velocity = new Vector2(0, 0);
            target1.rectangle = new Rectangle(0, 0, target1.texture.Width, target1.texture.Height);
            target1.origin = new Vector2(0, 0);
            target1.setColor();

            target2.texture = Content.Load<Texture2D>("target");
            target2.position = new Vector2(0, 0);
            target2.velocity = new Vector2(0, 0);
            target2.rectangle = new Rectangle(0, 0, target2.texture.Width, target2.texture.Height);
            target2.origin = new Vector2(0, 0);
            target2.setColor();

            target3.texture = Content.Load<Texture2D>("target");
            target3.position = new Vector2(0, 0);
            target3.velocity = new Vector2(0, 0);
            target3.rectangle = new Rectangle(0, 0, target3.texture.Width, target3.texture.Height);
            target3.origin = new Vector2(0, 0);
            target3.setColor();

            /*Initializes player variables*/
            player.texture = Content.Load<Texture2D>("player");//Player
            player.position = new Vector2(480, 300);
            player.velocity = new Vector2(0, 0);
            player.rectangle = new Rectangle(0, 0, player.texture.Width, player.texture.Height);
            player.origin = new Vector2(0, 0);
            player.setColor();



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if ((upperRight.X - upperLeft.X) < 1024 || (lowerLeft.Y - upperLeft.Y) < 768){ //Exception if resolution is too low.
                Exit();
                throw new InvalidOperationException("Resolution too low, make sure you can play on a screen with around 1100 pixels by 900 pixels. This will help make the play area at least 1024 pixels by 768 pixels.");
            }
            
            /*Collisions with Targets*/
            if(HandleCollision(player, target1)){ 
                spawnTarget(target1); 
                curScore += 1;}
            if (HandleCollision(player, target2)){ 
                spawnTarget(target2); 
                curScore += 1;}
            if (HandleCollision(player, target3)){ 
                spawnTarget(target3); 
                curScore += 1;
            }

            /*Ongoing Game*/
            if (inSession){
                handleMovement();
                if (timer > 0){//Countdown on Timer
                    timer -= (float)gameTime.ElapsedGameTime.TotalSeconds; 
                }
                else{ //Resets Game Info
                    timer = 0;
                    player.velocity = new Vector2(0, 0);
                    player.position = new Vector2(480, 300);
                    if(curScore > highScore)
                    {
                        highScore = curScore;
                        newBest = true;
                    }
                    inSession = false;
                }
            }
            else{//When game is not started
                if (Keyboard.GetState().IsKeyDown(Keys.Space)){

                    /*Initializes target positions*/
                    spawnTarget(target1);
                    spawnTarget(target2);
                    spawnTarget(target3);

                    newBest = false;
                    curScore = 0;
                    inSession = true;
                    timer = 60;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();


            /*Targets only seen when game is going*/
            if (inSession) {
                _spriteBatch.Draw(target1.texture, target1.position, target1.rectangle, Color.LightSlateGray, 0, target1.origin, 1f, SpriteEffects.None, 1);//1
                _spriteBatch.Draw(target2.texture, target2.position, target2.rectangle, Color.Orange, 0, target2.origin, 1f, SpriteEffects.None, 1);//2
                _spriteBatch.Draw(target3.texture, target3.position, target3.rectangle, Color.Aqua, 0, target3.origin, 1f, SpriteEffects.None, 1);//3
            }

            /*Player*/
            _spriteBatch.Draw(player.texture, player.position, player.rectangle, Color.White, 0, player.origin, 1f, SpriteEffects.None, 1); //(float)-Math.PI/4
            
            /*The following code for drawing the wall is modified from LO3.pdf*/
            _spriteBatch.Draw(rainbowStrip, northWall, wallColor, Color.White);
            _spriteBatch.Draw(rainbowStrip, southWall, wallColor, Color.White);
            _spriteBatch.Draw(rainbowStrip, eastWall, wallColor, Color.White, (float)(eastWallAngle + Math.PI), Vector2.Zero, SpriteEffects.None, 0.0f);
            _spriteBatch.Draw(rainbowStrip, westWall, wallColor, Color.White, (float)(westWallAngle + Math.PI), Vector2.Zero, SpriteEffects.None, 0.0f);

            /*Onscreen Text*/
            _spriteBatch.DrawString(gameFont, "Time Left: " + Math.Ceiling(timer).ToString() + " Seconds", new Vector2(27, 3), Color.White);
            _spriteBatch.DrawString(gameFont, "Current Score: " + curScore, new Vector2(400, 3), Color.White);
            _spriteBatch.DrawString(gameFont, "High Score: " + highScore, new Vector2(800, 3), Color.White);

            _spriteBatch.DrawString(gameFont, "Controls:    w/up arrow - UP    s/down arrow - DOWN      a/left arrow - LEFT     d/right arrow - RIGHT ", new Vector2(27, GraphicsDevice.Viewport.Height - 60), Color.White);
            _spriteBatch.DrawString(gameFont, "    Esc to quit            Spacebar to start", new Vector2(105, GraphicsDevice.Viewport.Height - 30), Color.White);

            /*Text when game is not started or is finished*/
            if (!inSession)
            {
                _spriteBatch.DrawString(gameFont, "Press SPACEBAR to begin!", new Vector2(400, 400), Color.White);
                if(curScore > 0)
                {
                    _spriteBatch.DrawString(gameFont, "Score: " + curScore, new Vector2(400, 450), Color.White);
                    if(newBest)
                    {
                        _spriteBatch.DrawString(gameFont, "New Best!", new Vector2(400, 470), Color.White);
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }


        /*The movement of the player is handled*/
        void handleMovement()
        {
            /*Character Movement*/
            int seed = (int)time.Ticks;
            Random rnd = new Random(seed);

            /*Velocity formula was from L03.pdf*/
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) { player.velocity += new Vector2(0, (float)(-(rnd.NextDouble() - 0.5) * 1.1)); }
            if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) { player.velocity += new Vector2(0, (float)((rnd.NextDouble() - 0.5) * 1.1)); }
            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) { player.velocity += new Vector2((float)(-(rnd.NextDouble() - 0.5) * 1.1), 0); }
            if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) { player.velocity += new Vector2((float)((rnd.NextDouble() - 0.5) * 1.1), 0); }

            float speedLimit = 8f;//Max speed we can go
            if (player.velocity.X > speedLimit) { player.velocity.X = speedLimit; }
            if (player.velocity.Y > speedLimit) { player.velocity.Y = speedLimit; }
            if (player.velocity.X < -speedLimit) { player.velocity.X = -speedLimit; }
            if (player.velocity.Y < -speedLimit) { player.velocity.Y = -speedLimit; }


            /*Handles wall collisions (Also reflect was found from lecture)*/
            if (player.position.X <= upperLeft.X) { 
                player.position.X = upperLeft.X + 1; //Ensure not stuck in the wall
                player.velocity = Vector2.Reflect(player.velocity, Vector2.UnitX); 
            }
            if (player.position.X + (player.texture.Width) >= upperRight.X - lineThickness) { 
                player.position.X = upperRight.X - lineThickness - (player.texture.Width) - 1; //Ensure not stuck in the wall
                player.velocity = Vector2.Reflect(player.velocity, Vector2.UnitX); 
            }
            if (player.position.Y <= upperLeft.Y + lineThickness) { 
                player.position.Y = upperLeft.Y + lineThickness + 1; //Ensure not stuck in the wall
                player.velocity = Vector2.Reflect(player.velocity, Vector2.UnitY); 
            }
            if (player.position.Y + (player.texture.Height) >= lowerLeft.Y) { 
                player.position.Y = lowerLeft.Y- (player.texture.Height) - 1; //Ensure not stuck in the wall
                player.velocity = Vector2.Reflect(player.velocity, Vector2.UnitY); 
            }

            player.position += player.velocity;

            /*Force acting against velocity so the player does not move forever*/
            float friction = 0.1f;
            if (player.velocity.X > 0f)
            {
                player.velocity -= new Vector2(friction, 0f);
                if (player.velocity.X < 0) { player.velocity.X = 0; }
            }
            if (player.velocity.Y > 0f)
            {
                player.velocity -= new Vector2(0f, friction);
                if (player.velocity.Y < 0) { player.velocity.Y = 0; }
            }
            if (player.velocity.X < 0f)
            {
                player.velocity += new Vector2(friction, 0f);
                if (player.velocity.X > 0) { player.velocity.X = 0; }
            }
            if (player.velocity.Y < 0f)
            {
                player.velocity += new Vector2(0f, friction);
                if (player.velocity.Y > 0) { player.velocity.Y = 0; }
            }
        }


        /*This will check to see if collisions happen between 2 sprites*/
        bool HandleCollision(Sprite s1, Sprite s2)
        {
            /*Code Modified from L03.pdf*/
            Rectangle r1 = new Rectangle((int)s1.position.X, (int)s1.position.Y, (int)s1.texture.Width, (int)s1.texture.Height);
            Rectangle r2 = new Rectangle((int)s2.position.X, (int)s2.position.Y, (int)s2.texture.Width, (int)s2.texture.Height);

            if (r1.Intersects(r2))
            {
                Rectangle intersect = Rectangle.Intersect(r1, r2);
                for (int x = intersect.X; x < intersect.X + intersect.Width; x++)
                {
                    for (int y = intersect.Y; y < intersect.Y + intersect.Height; y++)
                    {
                        int a1 = s1.spriteColor[x - (int)s1.position.X, y - (int)s1.position.Y].A;
                        int a2 = s2.spriteColor[x - (int)s2.position.X, y - (int)s2.position.Y].A;

                        if (a1 != 0 && a2 != 0){ return true; }
                    }
                }
            }
            return false;
        }

        /*This function will randomly change the position of a sprite. Used with targets.*/
        void spawnTarget(Sprite s1)
        {
            Random rnd = new Random();

            s1.position.X = rnd.Next((int)upperLeft.X, (int)upperRight.X - s1.texture.Width);
            s1.position.Y = rnd.Next((int)upperLeft.Y, (int)lowerLeft.Y - s1.texture.Height);
        }
    }
}
