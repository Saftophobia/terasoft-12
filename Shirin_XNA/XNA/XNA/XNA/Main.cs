using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace XNA
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Ball gameBall;
        //Number of goals is changed from here, max 60
        Goal[] goals = new Goal[35];
        int minX = 50;
        int minY = 30;
        Controller controller;
        int goalCounter = 0;
        Win win;
        Boolean[] hit;
        String[] goalTypes;
        List<float> storage = new List<float>();
        int globalCounter = 0;
        Loss loss;
        Boolean over;
        Score[] scores;
        Boolean[] score;
        

        public Main()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            gameBall = new Ball(Content.Load<Texture2D>("Images/ball"), new Vector2(100, 100),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            scores = new Score[goals.Length];

            win = new Win(Content.Load<Texture2D>("Images/win"), new Vector2(735, 150),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            loss = new Loss(Content.Load<Texture2D>("Images/Loss"), new Vector2(735, 90),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            over = false;

            hit = new Boolean[goals.Length];
            goalTypes = new String[goals.Length];
            score = new Boolean [goals.Length];




            for (int i = 0; i <= hit.Length - 1; i++)
            {
                hit[i] = false;
                score[i] = false;
            }

            String[] s = new String[4];
            s[0] = "Images/1";
            s[1] = "Images/2";
            s[2] = "Images/3";
            s[3] = "Images/4";

            Vector2[] post = new Vector2[60];

            int k = 0;

            for (int j = 0; j <= 5; j++)
            {
                for (int i = 0; i <= 9; i++)
                {
                    post[k] = new Vector2(minX, minY);
                    minX += 70;
                    k++;
                }
                minX = 50;
                minY += 50;
            }

            int[] temp1 = new int[4];
            int[] temp2 = new int[60];
            int u = 0;
            int[] temp3 = new int[780];

            int f=0;
            foreach(int n in UniqueRandom(0,779))
            {
                temp3[f] = n;
                f++;
            }

            foreach (int i in UniqueRandom(0, 3))
            {
                temp1[u] = i;
                u++;
            }

            u = 0;

            foreach (int i in UniqueRandom(0, 59))
            {
                temp2[u] = i;
                u++;
            }

            int m = 0;
            int q = 0;
            for (int i = 0; i <= goals.Length - 1; i++)
            {
                int x1 = temp1[q];
                int x2 = temp2[m];
                m++;
                q++;
                if (q == 4)
                {
                    q = 0;
                }                
                String directory = s[x1];
                
                if(directory.Equals("Images/1"))
                {
                    scores[i] = new Score(Content.Load<Texture2D>("Images/twentyfive"), new Vector2(temp3[i], 500),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);   
                }
                if (directory.Equals("Images/2"))
                {
                    scores[i] = new Score(Content.Load<Texture2D>("Images/fifty"), new Vector2(temp3[i], 600),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight); 
                }
                if (directory.Equals("Images/3"))
                {
                    scores[i] = new Score(Content.Load<Texture2D>("Images/hundred"), new Vector2(temp3[i], 700),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight); 
                }
                if (directory.Equals("Images/4"))
                {
                    scores[i] = null;
                }

                Vector2 vectorx = post[x2];
                goalTypes[i] = directory;
                goals[i] = new Goal(Content.Load<Texture2D>(directory), vectorx,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            }




            controller = new Controller(Content.Load<Texture2D>("Images/control"), new Vector2(370, 400),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        {
        }

        static IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
        {
            List<int> candidates = new List<int>();
            for (int i = minInclusive; i <= maxInclusive; i++)
            {
                candidates.Add(i);
            }
            Random rnd = new Random();
            while (candidates.Count > 0)
            {
                int index = rnd.Next(candidates.Count);
                yield return candidates[index];
                candidates.RemoveAt(index);
            }
        }









        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.Exit();

            KeyboardState keyboardState = Keyboard.GetState();




            storage.Add(gameBall.getPosition().Y);




            globalCounter++;

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                Vector2 a = controller.getPosition();
                int x = 0;
                if (controller.getPosition().X > 0)
                {
                    x = (int)a.X - 7;
                }
                else
                {
                    x = (int)a.X;
                }
                int y = (int)a.Y;
                controller.setPosition(x, y);
            }

            if (keyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                Vector2 a = controller.getPosition();
                int x = 0;
                if (controller.getPosition().X < 735)
                {
                    x = (int)a.X + 7;
                }
                else
                {
                    x = (int)a.X;
                }
                int y = (int)a.Y;
                controller.setPosition(x, y);
            }


            for (int i = 0; i <= goals.Length - 1; i++)
            {
                if (gameBall.getPosition().X >= goals[i].getXPosition() - 50
                    && gameBall.getPosition().X < goals[i].getXPosition() + goals[i].getTextureWidth()
                    && gameBall.getPosition().Y >= goals[i].getYPosition()
                    && gameBall.getPosition().Y < goals[i].getYPosition()+10)
                {
                    if (!goalTypes[i].Equals("Images/4"))
                    {
                        goals[i].setExists(false); 
                    }
                    if (!hit[i] && !goalTypes[i].Equals("Images/4"))
                    {
                        score[i] = true;    
                        goalCounter++;
                        hit[i] = true;
                    }
                    if (goalTypes[i].Equals("Images/4"))
                    {
                        if ((int)storage[globalCounter - 2] > gameBall.getPosition().Y)
                        {
                            gameBall.setBarrier(true);
                        }                        
                    }


                }


                if (gameBall.getPosition().X >= goals[i].getXPosition() - 50
                    && gameBall.getPosition().X < goals[i].getXPosition() + goals[i].getTextureWidth()
                    && gameBall.getPosition().Y < goals[i].getYPosition() && 
                    gameBall.getPosition().Y >= goals[i].getYPosition()-30)
                {
                    try
                    {
                        if (goalTypes[i].Equals("Images/4"))
                        {

                            if ((int)storage[globalCounter - 2] <= gameBall.getPosition().Y)
                            {
                                gameBall.setUpper(true);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                
                }


            }
            if (goalCounter == goals.Length - countBarriers(goalTypes))
            {
                win.Update(gameTime);
                over = true;
            }

            if (gameBall.getPosition().Y > 440&&!over)
            {
                loss.Update(gameTime);
            }
            for (int i = 0; i <= score.Length - 1; i++)
            {
                if (score[i])
                {
                    scores[i].Update(gameTime);
                }
            }
            gameBall.Update(gameTime, controller);
            base.Update(gameTime);
        }

        public static int countBarriers(String[] x)
        {
            int a = 0;
            for (int i = 0; i <= x.Length - 1; i++)
            {
                if (x[i].Equals("Images/4"))
                {
                    a++;
                }
            }
            return a;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            //background
            Win c1 = new Win(Content.Load<Texture2D>("Images/bk"), new Vector2(0, 0),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            c1.draw(spriteBatch);


            for (int i = 0; i <= score.Length - 1; i++)
            {
                if (score[i])
                {
                    scores[i].draw(spriteBatch);
                }
            }

            if (gameBall.getPosition().Y > 440&&!over)
            {
                loss.draw(spriteBatch);
            }


            if (goalCounter == goals.Length - countBarriers(goalTypes))
            {
                win.draw(spriteBatch);
                over = true;
            }
            gameBall.draw(spriteBatch);
            for (int i = 0; i <= goals.Length - 1; i++)
            {
                goals[i].draw(spriteBatch);
            }
            controller.draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
