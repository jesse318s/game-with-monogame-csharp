using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// added for audio
using Microsoft.Xna.Framework.Media;

// for sound effects
using Microsoft.Xna.Framework.Audio;

// added for Lists
using System.Collections.Generic;

// for random numbers
using System;



namespace JSitesMastery2
{
    // bullet class
    public class bullet
    {
        // create bullet texture
        public Texture2D bulletTexture;

        // create bullet position
        public Vector2 bulletPosition;

        // create bullet speed
        public float bulletSpeed = 800;

        // create bool tracking if bullet is active
        public bool active = false;

    }


    // enemy class

    public class enemy
    {
        // create enemy texture
        public Texture2D enemyTexture;

        // create bullet position
        public Vector2 enemyPosition;

        // enemy health
        public int enemyHealth = 3;

        // enemy points
        public int enemyPoints = 300;

        // create enemy speed
        public int enemySpeed = 800;

        // create bool tracking if enemy is active
        public bool active = false;

    }


    public class Game1 : Game
    {

        // background stuff
        Texture2D backgroundTexture;
        Vector2 backgroundPosition;

        Texture2D backgroundTexture2;
        Vector2 backgroundPosition2;

        int backgroundSpeed;


        // player stuff
        Texture2D playerTexture;
        Vector2 playerPosition;
        int playerSpeed;
        int maxBullets;

        // player bullets
        List<bullet> playerBullets = new List<bullet>();
        // variable tracking if the player can fire
        bool canFire = true;
        // laser sound
        SoundEffect laser;

        // integer tracking the current game level
        int currentLevel = 1;
        int numberOfRocks = 1;

        // enemies list
        List<enemy> enemyList = new List<enemy>();

        // enemy textures
        Texture2D enemySmall, enemyMedium, enemyLarge;

        // random number
        Random rnd = new Random();

        // text font stuff
        private SpriteFont myFont;

        private int playerScore = 0;
        private int playerLives = 3;

        // laser sound
        SoundEffect explosion;


        // game state
        string gameState = "MAIN";


        // score needed to win the game
        int winningScore = 2000;



        // reset game
        public void ResetGame()
        {
            // reset player
            playerScore = 0;
            playerLives = 3;

            // reset level - 1 will be added when the resetlevel is called
            currentLevel = 0;

            // set player position and speed
            playerPosition = new Vector2((_graphics.PreferredBackBufferWidth / 2), 600);

            // "turn off" player bullets
            // update (MOVE) bullets
            foreach (bullet bullet in playerBullets)
            {
                // make bullet inactive
                bullet.active = false;

                // move bullet off screen
                bullet.bulletPosition = new Vector2(-2000, -2000);
            }


            // call Reset Level
            ResetLevel();

        }







        // method to reset the level
        public void ResetLevel()
        {
            // increase a level
            currentLevel++;

            // increase number of rocks'
            numberOfRocks = currentLevel;

            //clear enemyList
            enemyList = new List<enemy>();

            // initialize enemy list based on the current level
            for (int i = 0; i < currentLevel; i++)
            {
                // create a temporary enemy
                enemy tempEnemy = new enemy();

                // add to the enemy's list
                enemyList.Add(tempEnemy);

            }

            // load enemy's textures and set position
            foreach (enemy enemy in enemyList)
            {

                // resize enemy
                //random size
                int tempSize = rnd.Next(3);

                if (tempSize == 0)// if 0 make small enemy
                {
                    // load texture
                    enemy.enemyTexture = enemySmall;

                    // change health
                    enemy.enemyHealth = 1;

                    // change points
                    enemy.enemyPoints = 100;
                }
                else if (tempSize == 1)
                {
                    // load texture
                    enemy.enemyTexture = enemyMedium;

                    // change health
                    enemy.enemyHealth = 2;

                    // change points
                    enemy.enemyPoints = 200;
                }
                else// otherwise it's a large
                {
                    // load texture
                    enemy.enemyTexture = enemyLarge;
                }


                // setting the meteor's starting position
                Vector2 tempVector = new Vector2();

                // random horizontal position
                tempVector.X = rnd.Next(0, (_graphics.PreferredBackBufferWidth - enemy.enemyTexture.Width));

                // off the top of the screen
                tempVector.Y = (-enemy.enemyTexture.Height);

                // set new position
                enemy.enemyPosition = tempVector;

                // random speed
                enemy.enemySpeed = rnd.Next(enemy.enemySpeed / 2, enemy.enemySpeed);

                //active the enemy
                enemy.active = true;

            }


        }



        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // change window size
            _graphics.PreferredBackBufferWidth = 1280; // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 720; // set this value to the desired height of your window
            _graphics.ApplyChanges();

            // set backgrounds position and speed
            // Background texture #1
            backgroundPosition = new Vector2(0, 0);

            // Background Texture #2
            backgroundPosition2 = new Vector2(0, -720);

            // Background Speed
            backgroundSpeed = 100;

            // background music - load, play, repeat
            Song song = Content.Load<Song>("SpaceyMusic"); // Put the name of your song here instead of "song_title"
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            // set player poition and speed
            playerPosition = new Vector2((_graphics.PreferredBackBufferWidth / 2), 600);
            playerSpeed = 500;


            // player's max # of bullets
            maxBullets = 15;

            // initialize player bullet queue
            // add the maximum number of bullets
            for (int i = 0; i < maxBullets; i++)
            {
                // create a temporary bullet
                bullet tempBullet = new bullet();

                // position off screen
                tempBullet.bulletPosition = new Vector2(-2000, -2000);

                // add to the player's bullet queue
                playerBullets.Add(tempBullet);

            }

            // intialize enemy list based on current level
            for (int i = 0; i < currentLevel; i++)
            {
                // create a temporary enemy
                enemy tempEnemy = new enemy();

                // add to the enemy's list
                enemyList.Add(tempEnemy);
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // load the background texture
            backgroundTexture = Content.Load<Texture2D>("background");
            backgroundTexture2 = Content.Load<Texture2D>("background");

            // load the player texture
            playerTexture = Content.Load<Texture2D>("player");

            // load bullet textures
            foreach (bullet bullet in playerBullets)
            {
                bullet.bulletTexture = Content.Load<Texture2D>("laser");
            }

            // load the laser sound effect
            laser = Content.Load<SoundEffect>("laserSoundEffect");


            // load enemy textures
            enemySmall = Content.Load<Texture2D>("enemySmall");

            enemyMedium = Content.Load<Texture2D>("enemyMedium");

            enemyLarge = Content.Load<Texture2D>("enemyLarge");

            // load enemy's textures and set position
            foreach (enemy enemy in enemyList)
            {
                // resize enemy
                //random size
                int tempSize = rnd.Next(3);

                if (tempSize == 0)// if 0 make small enemy
                {
                    // load texture
                    enemy.enemyTexture = enemySmall;

                    //change health
                    enemy.enemyHealth = 1;

                    // change points
                    enemy.enemyPoints = 100;
                }
                else if (tempSize == 1)// if 1 make medium enemy
                {
                    // load texture
                    enemy.enemyTexture = enemyMedium;

                    //change health
                    enemy.enemyHealth = 2;

                    // change points
                    enemy.enemyPoints = 200;
                }
                else
                {
                    // load texture
                    enemy.enemyTexture = enemyLarge;
                }

                // setting the meteor's starting position
                Vector2 tempVector = new Vector2();

                // random horizontal position
                tempVector.X = rnd.Next(0, (_graphics.PreferredBackBufferWidth - enemy.enemyTexture.Width));

                // off the top of the screen
                tempVector.Y = (-enemy.enemyTexture.Height);

                // set new position
                enemy.enemyPosition = tempVector;

                // random speed
                enemy.enemySpeed = rnd.Next(enemy.enemySpeed / 2, enemy.enemySpeed);

                //active the enemy
                enemy.active = true;

            }

            // load font
            myFont = Content.Load<SpriteFont>("myFont");

            // load the explosion sound effect
            explosion = Content.Load<SoundEffect>("explosionSoundEffect");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // update background #1 and background #2
            backgroundPosition.Y += backgroundSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            backgroundPosition2.Y += backgroundSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // check background #1 to see if it is past the bottom and set back to the top of the screen
            // also position background #2 at 0 so there is never a gap
            if (backgroundPosition.Y > backgroundTexture.Height)
            {
                backgroundPosition.Y = -backgroundTexture.Height;
                backgroundPosition2.Y = 0;

            }

            // check background #2 to see if it is past the bottom and set back to the top of the screen
            // also position background #1 at 0 so there is never a gap
            if (backgroundPosition2.Y > backgroundTexture2.Height)
            {
                backgroundPosition2.Y = -backgroundTexture2.Height;
                backgroundPosition.Y = 0;
            }


            // set variable to get the keyboard state
            var kstate = Keyboard.GetState();


            // check for MAIN game state
            if (gameState == "MAIN")
            {
                // check to see if the "I" key is pressed and open instructions
                if (kstate.IsKeyDown(Keys.I))
                    gameState = "INSTRUCTIONS";

                // check to see if the "G" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.G))
                    gameState = "GAME";
            }

            // check for INSTRUCTIONS game state
            if (gameState == "INSTRUCTIONS")
            {
                // check to see if the "M" key is pressed and open main menu
                if (kstate.IsKeyDown(Keys.M))
                    gameState = "MAIN";

                // check to see if the "G" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.G))
                    gameState = "GAME";
            }


            // GAME STATE
            if (gameState == "GAME")
            {

                // if W key - subtract from the player's Y position (vertical) to move up
                if (kstate.IsKeyDown(Keys.W))
                    playerPosition.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // if S key - add to the player's Y position (vertical) to move down
                if (kstate.IsKeyDown(Keys.S))
                    playerPosition.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // if A key - substract from the player's X position (horizontal) to move left
                if (kstate.IsKeyDown(Keys.A))
                    playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // if D key - add to the player's X position (horizontal) to move right
                if (kstate.IsKeyDown(Keys.D))
                    playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;


                // keep the player within the designated positions on the Y - vertical - up and down
                if (playerPosition.Y > (_graphics.PreferredBackBufferHeight - playerTexture.Height))
                {
                    playerPosition.Y = (_graphics.PreferredBackBufferHeight - playerTexture.Height);
                }
                else if (playerPosition.Y < 0)
                {
                    playerPosition.Y = 0;
                }

                // keep the player within the designated positions on the X - horizontal - left and right
                if (playerPosition.X > (_graphics.PreferredBackBufferWidth - playerTexture.Width))
                {
                    playerPosition.X = (_graphics.PreferredBackBufferWidth - playerTexture.Width);
                }
                else if (playerPosition.X < 0)
                {
                    playerPosition.X = 0;
                }

                // check if the space bar is up
                // and if needed, reset the player's ability to fire
                if (kstate.IsKeyUp(Keys.Space) && (canFire == false))
                {
                    canFire = true;
                }

                // check the fire button - space down
                if (kstate.IsKeyDown(Keys.Space) && canFire)
                {
                    // if space pressed, set canfire to false
                    canFire = false;

                    // cycle through bullets to find an inactive one
                    for (int i = 0; i < playerBullets.Count; i++)
                    {
                        // if inactive
                        if (playerBullets[i].active == false)
                        {
                            // player laser sound
                            laser.Play();

                            // make active
                            playerBullets[i].active = true;

                            // place on screen at player's upper left position
                            playerBullets[i].bulletPosition = playerPosition;

                            // move up
                            playerBullets[i].bulletPosition.Y -= playerTexture.Height;

                            //move right
                            playerBullets[i].bulletPosition.X = (playerPosition.X + (playerTexture.Width / 2)) - (playerBullets[i].bulletTexture.Width / 2);

                            // bullet fired, break out of loop
                            break;

                        }
                    }
                }

                // update (MOVE) bullets
                foreach (bullet bullet in playerBullets)
                {
                    // take bullet pos Y and subtract the bullet speed from it
                    bullet.bulletPosition.Y -= bullet.bulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // check if the bullet is off the screen
                    if (bullet.bulletPosition.Y < -_graphics.PreferredBackBufferHeight)
                    {
                        // set bullet to inactive
                        bullet.active = false;

                        // move off screen
                        bullet.bulletPosition = new Vector2(-2000, -2000);
                    }

                }


                // update the enemies
                foreach (enemy enemy in enemyList)
                {
                    // is enemy active
                    if (enemy.active == true)
                    {
                        enemy.enemyPosition.Y += enemy.enemySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                        // reset enemy if off screen
                        if (enemy.enemyPosition.Y >= (_graphics.PreferredBackBufferHeight + enemy.enemyTexture.Height))
                        {
                            // setting the meteor's starting position
                            Vector2 tempVector = new Vector2();

                            // random horizontal position
                            tempVector.X = rnd.Next(0, (_graphics.PreferredBackBufferWidth - enemy.enemyTexture.Width));

                            // off the top of the screen
                            tempVector.Y = (-enemy.enemyTexture.Height);

                            // set new position
                            enemy.enemyPosition = tempVector;

                            // random speed
                            enemy.enemySpeed = rnd.Next(enemy.enemySpeed / 2, enemy.enemySpeed);
                        }


                    }
                }



                // check enemy collision with bullets
                // this takes each enemy in the enemy list
                // and cycles through each active bullet
                foreach (enemy enemy in enemyList)
                {

                    if (enemy.active == true)
                    {

                        // Get the bounding rectangle of this enemy
                        Rectangle enemyRectangle = new Rectangle((int)enemy.enemyPosition.X, (int)enemy.enemyPosition.Y, enemy.enemyTexture.Width, enemy.enemyTexture.Height);

                        //look at bullets
                        foreach (bullet bullet in playerBullets)
                        {
                            // Get the bounding rectangle of this bullet
                            Rectangle bulletRectangle = new Rectangle((int)bullet.bulletPosition.X, (int)bullet.bulletPosition.Y, bullet.bulletTexture.Width, bullet.bulletTexture.Height);

                            // check for collision
                            if (enemyRectangle.Intersects(bulletRectangle))
                            {

                                // play explosion sound
                                explosion.Play();

                                //reset and deactivate the bullet
                                bullet.active = false;

                                bullet.bulletPosition = new Vector2(-2000, -2000);

                                // remove 1 point from enemy health
                                enemy.enemyHealth--;

                                // if enemy health less than zero, "destroy"
                                if (enemy.enemyHealth <= 0)
                                {
                                    // award player points
                                    playerScore += enemy.enemyPoints;

                                    // check for winning score
                                    // check to see if the player has won
                                    if (playerScore >= winningScore)
                                    {
                                        gameState = "WIN";
                                    }

                                    // deactive enemy
                                    enemy.active = false;

                                    // move off screen
                                    enemy.enemyPosition = new Vector2(-4000, -4000);

                                    // subtract from number of rocks
                                    numberOfRocks--;

                                    // if no rocks - reset our currentlevel
                                    if (numberOfRocks <= 0)
                                    {
                                        ResetLevel();
                                    }

                                }

                            }

                        }

                    }
                }



                // check player/enemy collision with enemies
                // this takes each enemy in the enmy list
                // and cycles through each checkin to see if they collide with the player
                foreach (enemy enemy in enemyList)
                {
                    if (enemy.active == true)
                    {
                        // Get the bounding rectangle of this enemy
                        Rectangle enemyRectangle = new Rectangle((int)enemy.enemyPosition.X, (int)enemy.enemyPosition.Y, enemy.enemyTexture.Width, enemy.enemyTexture.Height);

                        // Get the bounding rectangle of the player
                        Rectangle playerRectangle = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, playerTexture.Width, playerTexture.Height);

                        // check for collision
                        if (enemyRectangle.Intersects(playerRectangle))
                        {
                            // reset the enemy off screen - not dead just back to the right side of the screen
                            // setting the meteor's new position
                            Vector2 tempVector = new Vector2();

                            // random horizontal position
                            tempVector.X = rnd.Next(0, (_graphics.PreferredBackBufferWidth - enemy.enemyTexture.Width));

                            // off the top of the screen
                            tempVector.Y = (-enemy.enemyTexture.Height);

                            // set new position
                            enemy.enemyPosition = tempVector;

                            // play explosion sound
                            explosion.Play();

                            // remove a life
                            playerLives--;

                            // check if the player is dead
                            if (playerLives <= 0)
                            {
                                //load lose screen
                                gameState = "LOSE";
                            }


                        }

                    }
                }



            }



            // update for win state
            if (gameState == "WIN")
            {
                // check to see if the "M" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.M))
                {

                    // reset the game
                    ResetGame();

                    // set game state to "MAIN"
                    gameState = "MAIN";
                }

                // check to see if the "I" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.I))
                {

                    // reset the game
                    ResetGame();

                    // set game state to "INSTRUCTIONS"
                    gameState = "INSTRUCTIONS";
                }


                // check to see if the "G" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.G))
                {

                    // reset the game
                    ResetGame();

                    // set to game
                    gameState = "GAME";
                }


            }


            // update for lose state
            if (gameState == "LOSE")
            {
                // check to see if the "M" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.M))
                {

                    // reset the game
                    ResetGame();

                    // set game state to "MAIN"
                    gameState = "MAIN";
                }

                // check to see if the "I" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.I))
                {

                    // reset the game
                    ResetGame();

                    // set game state to "INSTRUCTIONS"
                    gameState = "INSTRUCTIONS";
                }

                // check to see if the "G" key is pressed and start the game
                if (kstate.IsKeyDown(Keys.G))
                {

                    // reset the game
                    ResetGame();

                    // set to game
                    gameState = "GAME";
                }


            }






            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            // Draw the Background
            _spriteBatch.Draw(backgroundTexture, backgroundPosition, Color.White);
            _spriteBatch.Draw(backgroundTexture2, backgroundPosition2, Color.White);

            // in MAIN state
            if (gameState == "MAIN")
            {
                _spriteBatch.DrawString(myFont, "Main Menu", new Vector2(525, 240), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'I' for Instructions", new Vector2(385, 390), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'G' to Start the Game", new Vector2(375, 490), Color.Yellow);
            }

            // in INSTRUCTIONS state
            if (gameState == "INSTRUCTIONS")
            {
                _spriteBatch.DrawString(myFont, "Instructions", new Vector2(500, 10), Color.Yellow);
                _spriteBatch.DrawString(myFont, "WASD Keys Move the Player", new Vector2(375, 110), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Space Bar Fires Laser", new Vector2(400, 185), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Destroy Enemies", new Vector2(450, 260), Color.Yellow);
                _spriteBatch.DrawString(myFont, "2000 Points Wins the Game", new Vector2(385, 335), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Lose All 3 Lives = Game Over", new Vector2(365, 410), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'M' for the Main Menu", new Vector2(375, 485), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'G' to Start the Game", new Vector2(375, 560), Color.Yellow);
            }

            // GAME STATE
            if (gameState == "GAME")
            {
                // Draw bullets
                foreach (bullet bullet in playerBullets)
                {
                    _spriteBatch.Draw(bullet.bulletTexture, bullet.bulletPosition, Color.White);
                }

                // Draw player
                _spriteBatch.Draw(playerTexture, playerPosition, Color.White);

                foreach (enemy enemy in enemyList)
                {
                    _spriteBatch.Draw(enemy.enemyTexture, enemy.enemyPosition, Color.White);
                }


                // draw player's score
                _spriteBatch.DrawString(myFont, "Score: " + playerScore, new Vector2(10, 10), Color.Yellow);

                //draw currentlevel
                _spriteBatch.DrawString(myFont, "Current Level: " + currentLevel, new Vector2(425, 10), Color.Yellow);

                //draw player's lives
                _spriteBatch.DrawString(myFont, "Lives: " + playerLives, new Vector2(1100, 10), Color.Yellow);

            }


            // win screen
            if (gameState == "WIN")
            {
                _spriteBatch.DrawString(myFont, "Congratulations! You have won the game!", new Vector2(275, 140), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'M' for the Main Menu", new Vector2(375, 290), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'I' for Instructions", new Vector2(385, 390), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'G' to Start the Game", new Vector2(375, 490), Color.Yellow);
            }

            // lose screen
            if (gameState == "LOSE")
            {
                _spriteBatch.DrawString(myFont, "Game over! You have lost the game!", new Vector2(325, 140), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'M' for the Main Menu", new Vector2(375, 290), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'I' for Instructions", new Vector2(385, 390), Color.Yellow);
                _spriteBatch.DrawString(myFont, "Press 'G' to Start the Game", new Vector2(375, 490), Color.Yellow);
            }




            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}