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

namespace KeepGrinding
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D grey;
        Texture2D blue;
        Model player1model;
        Model player2model;
        Model weapon1model;
        Model weapon2model;
        Player [] player = new Player[2];
        Vector3 thirdPersonReference = new Vector3(100, 0, 0);
        float avatarYaw = MathHelper.PiOver2;
        Matrix rotationMatrix;
        Vector3 transformedReference;
        Vector3 avatarPosition = new Vector3(2.5f, 0, 0);
        Vector3 cameraPosition;
        Vector3 p1location, p2location, w1location, w2location;
        float p1position, p2position, w1position, w2position;
        bool punch1, punch2, punch1Animation, punch2Animation, punch1out, punch2out;
        float SPEED_DIVISOR = 150f;
        float PUNCH_LENGTH = 3f;
        Vector2 fontPos;
        SpriteFont font;
        Vector2 FontOrigin;
        String output;
        bool gameIsOver;
        bool allocatingStatsStage;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();
            // mapScale = 1.5;
            // AllocConsole();
            IsMouseVisible = true;
            player[0] = new Player();
            //player[0].setStats(5000, 33, 30, 33);
            player[1] = new Player();
            //player[1].setStats(5000, 33, 1, 33);
            p1position = w1position = 0;
            p2position = w2position = 5;
            p1location = new Vector3(p1position, 0, 0);
            p2location = new Vector3(p2position, 0, 0);
            punch1 = punch2 = punch1Animation = punch2Animation = punch1out = punch2out = gameIsOver = false;
            allocatingStatsStage = true;
            fontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                graphics.GraphicsDevice.Viewport.Height / 4);
            base.Initialize();
        }

        public void calculateDamage(int defender)
        {
            if (defender == 1)
            {
                player[1].takeDamage(player[0].getSpeed() * player[0].getAttack() / player[1].getDefense());
            }
            else if (defender == 0)
            {
                player[0].takeDamage(player[1].getSpeed() * player[1].getAttack() / player[0].getDefense());
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            grey = Content.Load<Texture2D>("Hexes");
            blue = Content.Load<Texture2D>("HexesSpecular");
            player1model = Content.Load<Model>("Cube");
            player2model = Content.Load<Model>("Cube");
            weapon1model = Content.Load<Model>("Cube");
            weapon2model = Content.Load<Model>("Cube");
            font = Content.Load<SpriteFont>("CourierNew");
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
            //punch 1
            if (allocatingStatsStage == true)
            {
                output =  "Player 2                                         Player 1\n\n";
                output += "     Press and Hold the Corrosponding Buttons to Add\n\n";
                output += "Points Remaining: " + player[1].getPoints();
                for(int i = output.Length; i < 162; i++)
                {
                    output += " ";
                }
                output += "Points Remaining: " + player[0].getPoints();
                for (int i = output.Length; i < 185; i++)
                {
                    output += " ";
                }
                output += "\n      (A) Attack: " + (int)player[1].getAttack();
                for (int i = output.Length; i < 238; i++)
                {
                    output += " ";
                }
                output += "(Left) Attack: " + (int)player[0].getAttack();
                for (int i = output.Length; i < 258; i++)
                {
                    output += " ";
                }
                output += "\n     (D) Defense: " + (int)player[1].getDefense();
                for (int i = output.Length; i < 309; i++)
                {
                    output += " ";
                }
                output += "(Right) Defense: " + (int)player[0].getDefense();
                for (int i = output.Length; i < 331; i++)
                {
                    output += " ";
                }
                output += "\n       (W) Speed: " + (int)player[1].getSpeed();
                for (int i = output.Length; i < 387; i++)
                {
                    output += " ";
                }
                output += "(Up) Speed: " + (int)player[0].getSpeed();

                //adding stats
                //player 2
                if (player[1].getPoints() > 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        player[1].addAttack(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        player[1].addDefense(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                    {
                        player[1].addSpeed(0.1f);
                    }
                }
                //player 1
                if (player[0].getPoints() > 0)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        player[0].addAttack(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        player[0].addDefense(0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        player[0].addSpeed(0.1f);
                    }
                }

                //ending the stage
                if(player[0].getPoints() <= 0 && player[1].getPoints() <= 0 && Keyboard.GetState().GetPressedKeys().Length == 0)
                {
                    allocatingStatsStage = false;
                }

            } // end allocating stats stage
            else // start fighting stage
            {
                if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    punch1 = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && punch1 == false)
                {
                    punch1 = true;
                }
                if (punch1 == true && punch1Animation == false)
                {
                    punch1Animation = punch1out = true;
                }
                if (punch1Animation == true)
                {
                    if (punch1out == true)
                    {
                        w1position += player[0].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w1position, p1position) > PUNCH_LENGTH)
                    {
                        punch1out = false;
                    }
                    if (punch1out == false)
                    {
                        w1position -= player[0].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w1position, p1position) < 0.2 && punch1out == false)
                    {
                        w1position = p1position;
                        punch1Animation = false;
                    }
                }
                //punch 2
                if (Keyboard.GetState().IsKeyUp(Keys.W))
                {
                    punch2 = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) && punch2 == false)
                {
                    punch2 = true;
                }
                if (punch2 == true && punch2Animation == false)
                {
                    punch2Animation = punch2out = true;
                }
                if (punch2Animation == true)
                {
                    if (punch2out == true)
                    {
                        w2position -= player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w2position, p2position) > PUNCH_LENGTH)
                    {
                        punch2out = false;
                    }
                    if (punch2out == false)
                    {
                        w2position += player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    if (MathHelper.Distance(w2position, p2position) < 0.2 && punch2out == false)
                    {
                        w2position = p2position;
                        punch2Animation = false;
                    }
                }
                //player 2 movement
                if (punch2Animation == false && Keyboard.GetState().IsKeyDown(Keys.W) == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        p2position += player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        p2position -= player[1].getSpeed() / SPEED_DIVISOR;
                    }
                    w2position = p2position;
                }
                //player 1 movement
                if (punch1Animation == false && Keyboard.GetState().IsKeyDown(Keys.Up) == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        p1position += player[0].getSpeed() / SPEED_DIVISOR;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        p1position -= player[0].getSpeed() / SPEED_DIVISOR;
                    }
                    w1position = p1position;
                }
                // damage from punches
                if (gameIsOver == false)
                {
                    if (w1position > (p2position - 1))
                    {
                        calculateDamage(1);
                    }
                    if (w2position < (p1position + 1))
                    {
                        calculateDamage(0);
                    }
                }
                // TODO: Add your update logic here
                p1location = new Vector3(p1position, 0, 0);
                p2location = new Vector3(p2position, 0, 0);
                w1location = new Vector3(w1position, 0, 0);
                w2location = new Vector3(w2position, 0, 0);
                output = "Player 2 HP: " + player[1].getHealth();
                output += "                            ";
                output += "Player 1 HP: " + player[0].getHealth();
                if (player[0].getHealth() <= 0 && player[1].getHealth() <= 0)
                {
                    gameOver(2);
                }
                else if (player[0].getHealth() <= 0)
                {
                    gameOver(1);
                }
                else if (player[1].getHealth() <= 0)
                {
                    gameOver(0);
                }
            } // end fighting stage
            base.Update(gameTime);
        }
        
        void gameOver(int winner)
        {
            gameIsOver = true;
            if (winner == 2)
            {
                output = "Enough! Both Players Are Down!!!";
            }
            else
            {
                output = "Enough! Player " + (winner + 1) + " Wins!!!";
            }
            output += "\nPress Enter to play again...";
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                player[0].resetStats();
                player[1].resetStats();
                p1position = w1position = 0;
                p2position = w2position = 5;
                p1location = new Vector3(p1position, 0, 0);
                p2location = new Vector3(p2position, 0, 0);
                w1location = new Vector3(w1position, 0, 0);
                w2location = new Vector3(w2position, 0, 0);
                punch1 = punch2 = punch1Animation = punch2Animation = punch1out = punch2out = gameIsOver = false;
                allocatingStatsStage = true;
            }
        }

        void DrawModel(Model model, Texture2D texture, Vector3 scale, Vector3 rotation, Vector3 location)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(location);
                    effect.View = Matrix.CreateLookAt(cameraPosition, avatarPosition, new Vector3(0.0f, 0.0f, 1.0f));
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(0.1f, graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f);
                    effect.EnableDefaultLighting();
                    //effect.DiffuseColor = pipeColor[i];
                    effect.Texture = texture;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            rotationMatrix = Matrix.CreateRotationZ(avatarYaw);
            transformedReference = Vector3.Transform(thirdPersonReference, rotationMatrix);
            cameraPosition = transformedReference + avatarPosition;
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.End();
            DrawModel(weapon1model, grey, new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0), w1location);
            DrawModel(weapon2model, blue, new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0, 0, 0), w2location);
            DrawModel(player1model, grey, new Vector3(1, 1, 1), new Vector3(0, 0, 0), p1location);
            DrawModel(player2model, blue, new Vector3(1, 1, 1), new Vector3(0, 0, 0), p2location);

            spriteBatch.Begin();
            // Find the center of the string
            FontOrigin = font.MeasureString(output) / 2;
            // Draw the string
            spriteBatch.DrawString(font, output, fontPos, Color.Black,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
