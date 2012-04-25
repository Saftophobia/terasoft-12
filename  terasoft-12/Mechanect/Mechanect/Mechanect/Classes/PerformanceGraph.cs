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

namespace Mechanect.Classes
{
    class PerformanceGraph
    {
        int stagewidth, stageheight;
        int a1;
        int a2;
        int a3;
        int a4;
        int finishx;
        int finishy;
        Color curveColor;
        List<float> Player1Displacement;
        List<float> Player2Displacement;
        List<float> Player1Velocity;
        List<float> Player2Velocity;
        List<float> Player1Acceleration;
        List<float> Player2Acceleration;
        List<float> OptimumDisplacement = new List<float>();
        List<float> OptimumVelocity = new List<float>();
        List<float> OptimumAcceleration = new List<float>();
        List<String> CommandsList;
        List<double> TimeSpaces;
        Game currentGame;
        double totalTime;
        int[] chosenTimings;
        float[] chosendisp1 = new float[17];
        float[] chosendisp2 = new float[17];
        float[] chosenVelocity1 = new float[17];
        float[] chosenVelocity2 = new float[17];
        float[] chosenAcceleration1 = new float[17];
        float[] chosenAcceleration2 = new float[17];
        float[] chosenOptD = new float[17];
        float[] chosenOptV = new float[17];
        float[] chosenOptA = new float[17];
        PerformanceGraph[] disp1 = new PerformanceGraph[16];
        PerformanceGraph[] disp2 = new PerformanceGraph[16];
        PerformanceGraph[] velo1 = new PerformanceGraph[16];
        PerformanceGraph[] velo2 = new PerformanceGraph[16];
        PerformanceGraph[] acc1 = new PerformanceGraph[16];
        PerformanceGraph[] acc2 = new PerformanceGraph[16];
        PerformanceGraph[] optD = new PerformanceGraph[16];
        PerformanceGraph[] optV = new PerformanceGraph[16];
        PerformanceGraph[] optA = new PerformanceGraph[16];
        float maxVelocity;
        float maxAcceleration;
        List<int> P1DispGraph = new List<int>();
        List<int> P2DispGraph = new List<int>();
        List<int> P1VeloGraph = new List<int>();
        List<int> P2VeloGraph = new List<int>();
        List<int> P1AccGraph = new List<int>();
        List<int> P2AccGraph = new List<int>();
        double[] xaxis = new double[5];
        double[] yaxisDisplacement = new double[5];
        double[] yaxisVelocity = new double[5];
        double[] yaxisAcceleration = new double[5];
        CountDown xDP1;
        CountDown xDP2;
        CountDown xVP1;
        CountDown xVP2;
        CountDown xAP1;
        CountDown xAP2;
        float previousDisp;

        public PerformanceGraph(int start1, int start2, int finishx, int finishy, int a, int b, Color col)
        {
            a1 = start1;
            a2 = start2;
            a3 = finishx;
            a4 = finishy;
            curveColor = col;
            this.finishx = finishx;
            this.finishy = finishy;
            stagewidth = a;
            stageheight = b;
        }

        public PerformanceGraph()
        {
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The Draw function is used to draw a line connecing the points (a1,a2) and (b1,b2) when called
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            drawLine(spriteBatch, blank, 2, curveColor, new Vector2(a1, a2), new Vector2(a3, a4));
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The drawLine function is used to draw a straight line connecting
        /// an initial point with a final point
        /// </summary>
        /// <param name="batch">An instance of the spriteBatch class</param>
        /// <param name="blank">An instance of the Texture2D class</param>
        /// <param name="width">The width of the line</param>
        /// <param name="color">The color of the line</param>
        /// <param name="point1">The initial point</param>
        /// <param name="point2">The final point</param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void drawLine(SpriteBatch batch, Texture2D blank,
              float width, Microsoft.Xna.Framework.Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 18/4/2012</para>
        /// <para>Date Modified 19/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The static method GetPlayerVelocity is used to generate
        /// a List representing the player's velocity during the race.
        /// 
        /// The velocity is calculated using the following equation:
        ///        
        /// Velocity= (Displacement.Final-Displacement.Initial)/dt
        ///
        /// where dt is (1/12) since the kinect captures 12 frames per
        /// second implying that the time space (dt) between each depth 
        /// frame and its successor is (1/12) seconds.
        /// The resulting velocity is multiplied by negative one to
        /// get the Player's velocity relative to the Player not to the
        /// kinect, since the List holding the Player's displacements
        /// from the kinect is relative to the kinect not the player.
        /// 
        /// The try and catch statement is used to add a 0 at the begining 
        /// of the List representing the player's velocity since the players
        /// would be at a fixed distance from the kinect at the exact instant
        /// when the race starts
        /// </summary>
        /// <param name="DisplacementList"> A List representing the
        /// player's displacements during the race</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>List: returns a list representing the player's
        /// velocity</returns>

        public static List<float> GetPlayerVelocity(List<float> DisplacementList)
        {
            int size = DisplacementList.Count;
            List<float> result = new List<float>();
            for (int i = 0; i <= size - 1; i++)
            {
                try
                {
                    double dt = ((double)1 / (double)12);
                    float currentVelocity = (float)((DisplacementList[i] - DisplacementList[i - 1]) / dt) * -1;
                    result.Add(currentVelocity);
                }
                catch (Exception e)
                {
                    result.Add(0);
                }
            }
            return result;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 18/4/2012</para>
        /// <para>Date Modified 19/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The static method GetPlayerAcceleration is used to generate
        /// a List representing the player's acceleration during the race.
        /// 
        /// The acceleration is calculated using the following equation:
        ///               
        /// Acceleration= (Velocity.Final-Velocity.Initial)/dt
        /// 
        /// where dt is (1/12) since the kinect captures 12 frames per
        /// second implying that the time space (dt) between each depth 
        /// frame and its successor is (1/12) seconds.
        /// The resulting acceleration is not multiplied by -1 since the
        /// incoming velocities represent the player's velocity relative
        /// to the player not the kinect.
        /// 
        /// The try and catch statement is used to add a 0 at the begining 
        /// of the List representing the player's velocity since the players 
        /// would have 0 velocity at the exact instant when the race starts. 
        /// </summary>
        /// <param name="VelocityList">A list representing the player's
        /// velocities during the race</param>         
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>List: returns a list representing the player's
        /// acceleration</returns>

        public static List<float> GetPlayerAcceleration(List<float> VelocityList)
        {
            int size = VelocityList.Count;
            List<float> result = new List<float>();
            for (int i = 0; i <= size - 1; i++)
            {
                try
                {
                    double dt = ((double)1 / (double)12);
                    float currentAcceleration = (float)((VelocityList[i] - VelocityList[i - 1]) / dt);
                    result.Add(currentAcceleration);
                }
                catch (Exception e)
                {
                    result.Add(0);
                }
            }
            return result;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 18/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawGraphs calls the methods GetPlayerVelocity, GetPlayerAcceleration
        /// GetOptimum in order to derive each player's velocity, acceleration and the optimum
        /// values, then calls the choose, setMaximum, setDestinations, setAxis functions in 
        /// order to display the graph on the screen
        /// </summary>
        /// <param name="Player1Displacement">A list holding Player 1's displacements</param>
        /// <param name="Player2Displacement">A list holding Player 2's displacements</param>
        /// <param name="Commands">A list holding each command initiated during the race</param>
        /// <param name="time">A list holding the time elapsed by each command</param>
        /// <param name="g1">An instance of the game class</param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void drawGraphs(List<float> Player1Disp, List<float> Player2Disp, List<String> Commands, List<double> time, Game g1)
        {
            Player1Displacement = new List<float>();
            Player2Displacement = new List<float>();
            double accumulator = 0;
            for (int i = 0; i <= time.Count - 1; i++)
            {
                accumulator += time[i];
            }
            totalTime = accumulator;
            int frames = (int)(totalTime * 12);
            for (int i = 0; i <= frames - 1; i++)
            {
                Player1Displacement.Add(0);
                Player2Displacement.Add(0);
            }
            for (int i = 0; i <= Player1Disp.Count - 1; i++)
            {
                Player1Displacement[i] = Player1Disp[i];
            }
            for (int i = 0; i <= Player2Disp.Count - 1; i++)
            {
                Player2Displacement[i] = Player2Disp[i];
            }
            this.CommandsList = Commands;
            this.TimeSpaces = time;
            this.currentGame = g1;
            Player1Velocity = GetPlayerVelocity(Player1Displacement);
            Player2Velocity = GetPlayerVelocity(Player2Displacement);
            Player1Acceleration = GetPlayerAcceleration(Player1Velocity);
            Player2Acceleration = GetPlayerAcceleration(Player2Velocity);
            GetOptimum();
            choose();
            setMaximum();
            setDestinations(g1.getGraphicsDeviceManager());
            setAxis();
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function choose is used to choose certain times according to the race time and
        /// chooses the values of displacement, velocity and acceleration for each player at the
        /// chosen times in order to represent them on the graph
        /// </summary>
        /// <param></param> 
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void choose()
        {
            chosenTimings = new int[17];
            int timeCounter = 0;
            for (int i = 0; i <= chosenTimings.Length - 1; i++)
            {
                chosenTimings[i] = (int)(12 * totalTime * ((double)timeCounter / (double)16));
                timeCounter++;
            }
            int u = 0;
            for (int i = 0; i <= chosendisp1.Length - 1; i++)
            {
                if (i > 0)
                {
                    u = chosenTimings[i] - 1;
                }
                else
                {
                    u = chosenTimings[i];
                }
                chosendisp1[i] = Player1Displacement[u];
                chosendisp2[i] = Player2Displacement[u];
                chosenVelocity1[i] = Player1Velocity[u];
                chosenVelocity2[i] = Player2Velocity[u];
                chosenAcceleration1[i] = Player1Acceleration[u];
                chosenAcceleration2[i] = Player2Acceleration[u];
                chosenOptD[i] = OptimumDisplacement[u];
                chosenOptV[i] = OptimumVelocity[u];
                chosenOptA[i] = OptimumAcceleration[u];

            }
        }





        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function setMaximum is used to derive the maximum velocity and the maximum acceleration
        /// of both players during the race in order to set the maximum point on each graph's y-axis according
        /// to these values
        /// </summary>
        /// <param></param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void setMaximum()
        {
            maxVelocity = 0;
            maxAcceleration = 0;

            for (int i = 0; i <= chosenOptD.Length - 1; i++)
            {
                float v = chosenOptV[i];
                float a = chosenOptA[i];
                if (v < 0)
                {
                    v *= -1;
                }
                if (a < 0)
                {
                    a *= -1;
                }
                if (v > maxVelocity)
                {
                    maxVelocity = v;
                }
                if (a > maxAcceleration)
                {
                    maxAcceleration = a;
                }
            }
            for (int i = 0; i <= chosenVelocity1.Length - 1; i++)
            {
                float v1 = chosenVelocity1[i];
                float a1 = chosenAcceleration1[i];
                if (v1 < 0)
                {
                    v1 = v1 * -1;
                }
                if (a1 < 0)
                {
                    a1 = a1 * -1;
                }
                if (v1 > maxVelocity)
                {
                    maxVelocity = v1;
                }
                if (a1 > maxAcceleration)
                {
                    maxAcceleration = a1;
                }
            }
            for (int i = 0; i <= chosenVelocity2.Length - 1; i++)
            {
                float v2 = chosenVelocity2[i];
                float a2 = chosenAcceleration2[i];
                if (v2 < 0)
                {
                    v2 = v2 * -1;
                }
                if (a2 < 0)
                {
                    a2 = a2 * -1;
                }
                if (v2 > maxVelocity)
                {
                    maxVelocity = v2;
                }
                if (a2 > maxAcceleration)
                {
                    maxAcceleration = a2;
                }
            }
        }


        public void setDestinations(GraphicsDeviceManager graphics)
        {
            int counter1 = 0;
            float value = 0;
            double r = 0;
            for (int j = 0; j <= 8; j++)
            {
                PerformanceGraph[] current = new PerformanceGraph[16];//change sampling from here                
                float[] temporary = new float[17];
                List<int> disqList = new List<int>();
                Color color = new Color();
                switch (j)
                {
                    case 0: counter1 = 50; value = 4000; current = disp1; color = Color.Red; temporary = chosendisp1; disqList = P1DispGraph; break;
                    case 1: counter1 = 50; value = 4000; current = disp2; color = Color.Blue; temporary = chosendisp2; disqList = P2DispGraph; break;
                    case 2: counter1 = 50; value = 4000; current = optD; color = Color.Yellow; temporary = chosenOptD; break;
                    case 3: counter1 = 380; value = maxVelocity; current = velo1; color = Color.Red; temporary = chosenVelocity1; disqList = P1VeloGraph; break;
                    case 4: counter1 = 380; value = maxVelocity; current = velo2; color = Color.Blue; temporary = chosenVelocity2; disqList = P2VeloGraph; break;
                    case 5: counter1 = 380; value = maxVelocity; current = optV; color = Color.Yellow; temporary = chosenOptV; break;
                    case 6: counter1 = 710; value = maxAcceleration; current = acc1; color = Color.Red; temporary = chosenAcceleration1; disqList = P1AccGraph; break;
                    case 7: counter1 = 710; value = maxAcceleration; current = acc2; color = Color.Blue; temporary = chosenAcceleration2; disqList = P2AccGraph; break;
                    case 8: counter1 = 710; value = maxAcceleration; current = optA; color = Color.Yellow; temporary = chosenOptA; break;
                }
                r = (double)value / (double)232;

                for (int i = 0; i <= 15; i++)
                {
                    int a1 = 118;
                    int a2 = 118;
                    if (j > 2)
                    {
                        if (temporary[i] < 0)
                        {
                            a1 = 134;
                        }
                        if (temporary[i + 1] < 0)
                        {
                            a2 = 134;
                        }
                    }
                    double r2 = 0; double r4 = 0;
                    if (j <= 2)
                    {
                        r2 = (double)(temporary[i]) / (double)r;
                        r4 = (double)(temporary[i + 1]) / (double)r;
                    }

                    if (j > 2 && j <= 5)
                    {
                        r2 = (double)(maxVelocity - 2 - temporary[i]) / (double)r;
                        r4 = (double)(maxVelocity - 2 - temporary[i + 1]) / (double)r;
                    }
                    if (j > 5 && j <= 8)
                    {
                        r2 = (double)(maxAcceleration - 2 - temporary[i]) / (double)r;
                        r4 = (double)(maxAcceleration - 2 - temporary[i + 1]) / (double)r;
                    }
                    int r3 = a1 + (int)r2;
                    int r5 = a2 + (int)r4;
                    current[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                        graphics.PreferredBackBufferHeight, color);
                    counter1 = counter1 + 15;
                    if (j != 2 && j != 5 && j != 8)
                    {
                        if (i == 0)
                        {
                            disqList.Add(r3);
                        }
                        disqList.Add(r5);
                    }
                }
            }
        }

        public void setAxis()
        {
            xaxis[0] = 0;
            double step = (double)totalTime / (double)4;
            for (int i = 1; i <= xaxis.Length - 1; i++)
            {
                xaxis[i] = xaxis[i - 1] + step;
            }
            int counter = 0;
            for (int i = 0; i <= 4; i++)
            {
                yaxisDisplacement[i] = counter;
                counter += 1000;
            }
            yaxisVelocity[0] = 0;
            step = (double)maxVelocity / (double)4;
            for (int i = 1; i <= yaxisVelocity.Length - 1; i++)
            {
                yaxisVelocity[i] = yaxisVelocity[i - 1] + step;
            }
            yaxisAcceleration[0] = 0;
            step = (double)maxAcceleration / (double)4;
            for (int i = 1; i <= yaxisAcceleration.Length - 1; i++)
            {
                yaxisAcceleration[i] = yaxisAcceleration[i - 1] + step;
            }
        }




        public void drawRange(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice) //to be called in the draw function
        {
            for (int i = 0; i <= disp1.Length - 1; i++)
            {
                disp1[i].Draw(spriteBatch, GraphicsDevice);
                disp2[i].Draw(spriteBatch, GraphicsDevice);
                velo1[i].Draw(spriteBatch, GraphicsDevice);
                velo2[i].Draw(spriteBatch, GraphicsDevice);
                acc1[i].Draw(spriteBatch, GraphicsDevice);
                acc2[i].Draw(spriteBatch, GraphicsDevice);
                optD[i].Draw(spriteBatch, GraphicsDevice);
                optV[i].Draw(spriteBatch, GraphicsDevice);
                optA[i].Draw(spriteBatch, GraphicsDevice);
            }
        }

        public void drawLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            spriteBatch.DrawString(font, "Displacement", new Vector2(5, 70), Color.Black);
            spriteBatch.DrawString(font, "Velocity", new Vector2(340, 70), Color.Black);
            spriteBatch.DrawString(font, "Acceleration", new Vector2(640, 70), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(270, 380), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(600, 380), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(930, 380), Color.Black);
        }

        public void drawAxis(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 100),
                new Vector2(50, 620));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 100),
                new Vector2(380, 620));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 100),
                new Vector2(710, 620));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 350),
                new Vector2(316, 350));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 350),
                new Vector2(646, 350));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 350),
                new Vector2(976, 350));
        }

        public void drawArrows(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            int counter = 309;
            for (int i = 0; i <= 2; i++)
            {
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 343),
                new Vector2(counter + 7, 350));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 357),
                new Vector2(counter + 7, 350));
                counter += 330;
            }
            counter = 40;
            for (int i = 0; i <= 2; i++)
            {
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 110),
                new Vector2(counter + 10, 100));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 110),
                new Vector2(counter + 8, 100));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 610),
                new Vector2(counter + 10, 620));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 610),
                new Vector2(counter + 8, 620));

                counter += 330;
            }
        }


        public void drawXLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);            
            int counter = 35;
            for (int i = 0; i <= 2; i++)
            {
                switch (i)
                {
                    case 1: counter = 365; break;
                    case 2: counter = 695; break;
                    default: counter = 35; break;
                }
                for (int j = 0; j <= 4; j++)
                {
                    spriteBatch.DrawString(font2, xaxis[j] + "", new Vector2(counter - 5, 358), Color.Black);
                    counter += 67;
                }
            }    
        }

        public void drawYLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //+ve y
            int y = 0;            
            double[] current = yaxisDisplacement; ;
            for (int i = 0; i <= 2; i++)
            {
                int counter = 110;
                switch (i)
                {
                    case 1: y = 320; current = yaxisVelocity; break;
                    case 2: y = 650; current = yaxisAcceleration; break;
                    default: y = 0; current = yaxisDisplacement; break;
                }
                for (int j = 4; j >= 1; j--)
                {
                    spriteBatch.DrawString(font2, current[j] + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
            //-ve y
            for (int i = 0; i <= 2; i++)
            {
                int counter = 400;
                switch (i)
                {
                    case 1: y = 320; current = yaxisVelocity; break;
                    case 2: y = 650; current = yaxisAcceleration; break;
                    default: y = 0; current = yaxisDisplacement; break;
                }
                for (int j = 1; j <= 4; j++)
                {
                    spriteBatch.DrawString(font2, -1*current[j] + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
        }

        public void drawEnvironment(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            drawAxis(spriteBatch, GraphicsDevice, font, font2);
            drawLabels(spriteBatch, GraphicsDevice, font, font2);
            drawArrows(spriteBatch, GraphicsDevice, font, font2);
            drawXLabels(spriteBatch, GraphicsDevice, font, font2);
            drawYLabels(spriteBatch, GraphicsDevice, font, font2);
            //drawing the marks on the xaxis
            int counter = 50;
            for (int i = 0; i <= 2; i++)
            {
                switch (i)
                {
                    case 1: counter = 380; break;
                    case 2: counter = 710; break;
                    default: counter = 50; break;
                }
                for (int j = 0; j <= 4; j++)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 345),
                        new Vector2(counter, 355));
                    counter += 64;
                }
            }
            //drawing the marks on the yaxis
            int y = 50;
            for (int i = 0; i <= 2; i++)
            {
                int counter2 = 118;
                switch (i)
                {
                    case 1: y = 380; break;
                    case 2: y = 710; break;
                    default: y = 50; break;
                }
                for (int j = 1; j <= 9; j++)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(y - 7, counter2),
                        new Vector2(y + 5, counter2));
                    counter2 += 58;
                }
            }
            
        }


        public void drawDisqualification(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, Texture2D P1Tex, Texture2D P2Tex)
        {
            double player1DisqualificationTime = currentGame.GetPlayer1Disq();
            double player2DisqualificationTime = currentGame.GetPlayer2Disq();

            if (player1DisqualificationTime > 0)
            {
                double time = totalTime;
                int index = 8;
                for (int i = 0; i <= chosenTimings.Length - 1; i++)
                {
                    if (i < chosenTimings.Length - 1)
                    {
                        double d1 = ((double)chosenTimings[i] / (double)12);
                        double d2 = ((double)chosenTimings[i + 1] / (double)12);
                        if (player1DisqualificationTime >= d1 && player1DisqualificationTime < d2)
                        {
                            double x = d1 + ((double)(d2 - d1) / (double)2);
                            if (player1DisqualificationTime < x)
                            {
                                time = (double)chosenTimings[i] / (double)12;
                                index = i;
                            }
                            else
                            {
                                time = (double)chosenTimings[i + 1] / (double)12;
                                index = i + 1;
                            }
                        }
                    }
                }
                int y = P1DispGraph[index] - 10;
                double r1 = (double)totalTime / (double)240;
                double r2 = (double)(time) / (double)r1;
                int r3 = 40 + (int)r2;
                xDP1 = new CountDown(P1Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);

                xDP1.Draw(spriteBatch);

                y = P1VeloGraph[index] - 10;
                r1 = (double)totalTime / (double)240;
                r2 = (double)(time) / (double)r1;
                r3 = 370 + (int)r2;
                xVP1 = new CountDown(P1Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);

                xVP1.Draw(spriteBatch);

                y = P1AccGraph[index] - 10;
                r1 = (double)totalTime / (double)240;
                r2 = (double)(time) / (double)r1;
                r3 = 700 + (int)r2;
                xAP1 = new CountDown(P1Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);

                xAP1.Draw(spriteBatch);


            }
            if (player2DisqualificationTime > 0)
            {
                double time = totalTime;
                int index = 8;
                for (int i = 0; i <= chosenTimings.Length - 1; i++)
                {
                    if (i < chosenTimings.Length - 1)
                    {
                        double d1 = ((double)chosenTimings[i] / (double)12);
                        double d2 = ((double)chosenTimings[i + 1] / (double)12);
                        if (player2DisqualificationTime >= d1 && player2DisqualificationTime < d2)
                        {
                            double x = d1 + ((double)(d2 - d1) / (double)2);
                            if (player2DisqualificationTime < x)
                            {
                                time = (double)chosenTimings[i] / (double)12;
                                index = i;
                            }
                            else
                            {
                                time = (double)chosenTimings[i + 1] / (double)12;
                                index = i + 1;
                            }
                        }
                    }
                }
                int y = P2DispGraph[index] - 10;
                double r1 = (double)totalTime / (double)240;
                double r2 = (double)(time) / (double)r1;
                int r3 = 40 + (int)r2;
                xDP2 = new CountDown(P2Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);

                xDP2.Draw(spriteBatch);

                y = P2VeloGraph[index] - 10;
                r1 = (double)totalTime / (double)240;
                r2 = (double)(time) / (double)r1;
                r3 = 370 + (int)r2;
                xVP2 = new CountDown(P2Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);

                xVP2.Draw(spriteBatch);

                y = P2AccGraph[index] - 10;
                r1 = (double)totalTime / (double)240;
                r2 = (double)(time) / (double)r1;
                r3 = 700 + (int)r2;
                xAP2 = new CountDown(P2Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);

                xAP2.Draw(spriteBatch);

            }
        }

        public Boolean checkWinner(int condition, List<float> Test1, List<float> Test2, int count)
        {
            Boolean winner = false;

            if (condition == 1 || condition == 4)
            {
                for (int k = 0; k <= count - 1; k++)
                {
                    if (Test1[k] == 0 || Test2[k] == 0) //case: winning
                    {
                        winner = true;
                    }
                }
            }
            else
            {
                List<float> secondary = Test1;
                if (condition == 3)
                {
                    secondary = Test2;
                }
                for (int k = 0; k <= count - 1; k++)
                {
                    if (secondary[k] == 0)
                    {
                        winner = true;
                    }
                }
            }
            return winner;
        }


        public List<float> fillList(int count, Boolean winner, double value)
        {
            List<float> tempList = new List<float>();
            for (int k = 0; k <= count - 1; k++)
            {
                if (winner)
                {
                    tempList.Add(0);
                }
                else
                {
                    tempList.Add((float)value);
                }
            }
            return tempList;
        }

        public void OptimumConstantVelocity(int disq1, int disq2, int start, int end, List<float> velocityTest1, List<float> velocityTest2,
            List<float> displacementTest1, List<float> displacementTest2, List<float> tempList)
        {
            int advantage = 1;
            double a = GetAverageList(velocityTest1);
            double b = GetAverageList(velocityTest2);
            double value = 0;
            Boolean winner = false;
            int condition = 0;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                if (a >= b)
                {
                    value = a; advantage = 1;
                }
                else
                {
                    value = b; advantage = 2;
                }
                condition = 1;
            }
            if (!LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1; value = a;
                condition = 2;
            }
            if (LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                advantage = 2; value = b;
                condition = 3;
            }
            if (LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1;
                value = (double)((double)(a + b) / (double)2);
                condition = 4;
            }
            winner = checkWinner(condition, displacementTest1, displacementTest2, velocityTest1.Count);
            tempList = fillList(velocityTest1.Count, winner, value);
            List<float> disp = new List<float>();
            List<float> temp1 = Player1Displacement;
            if (advantage == 2)
            {
                temp1 = Player2Displacement;
            }
            float initial = 0;
            for (int k = 0; k <= tempList.Count - 1; k++)
            {
                disp.Add(temp1[start] - initial);
                initial += tempList[0];
            }
            List<float> acc = GetPlayerAcceleration(tempList);
            for (int j = 0; j <= tempList.Count - 1; j++)
            {
                if (disp[j] >= 0)
                {
                    OptimumDisplacement.Add(disp[j]);
                    OptimumVelocity.Add(tempList[j]);
                    OptimumAcceleration.Add(acc[j]);
                }
                else
                {
                    OptimumDisplacement.Add(0);
                    OptimumVelocity.Add(0);
                    OptimumAcceleration.Add(0);
                }
            }
            this.previousDisp = OptimumDisplacement[tempList.Count - 1];
        }

        public void OptimumConstantAcceleration(int disq1, int disq2, int start, int end, List<float> accelerationTest1, List<float> accelerationTest2,
           List<float> displacementTest1, List<float> displacementTest2, List<float> tempList, String currentCommand)
        {
            int advantage = 1;
            double a = GetAverageList(accelerationTest1);
            double b = GetAverageList(accelerationTest2);
            double value = 0;
            Boolean winner = false;
            int condition = 0;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                if (a >= b)
                {
                    value = a; advantage = 1;
                }
                else
                {
                    value = b; advantage = 2;
                }
                condition = 1;
            }
            if (!LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1; condition = 2;
                value = getMax(accelerationTest1);
                if (currentCommand.Equals("constantAcceleration"))
                {
                    value = a;
                }
            }
            if (LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                advantage = 2; condition = 3;
                value = getMax(accelerationTest2);
                if (currentCommand.Equals("constantAcceleration"))
                {
                    value = b;
                }
            }
            if (LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1; condition = 4;
                value = -41;
                if (currentCommand.Equals("constantAcceleration"))
                {
                    value = (double)((double)(a + b) / (double)2);
                }
            }
            winner = checkWinner(condition, displacementTest1, displacementTest2, accelerationTest1.Count);
            tempList = fillList(accelerationTest1.Count, winner, value);
            List<float> vel = new List<float>();
            List<float> disp = new List<float>();
            List<float> temp1 = Player1Velocity;
            List<float> temp2 = Player1Displacement;
            if (advantage == 2)
            {
                temp1 = Player2Velocity;
                temp2 = Player2Displacement;
            }
            float initial = 0;
            for (int k = 0; k <= tempList.Count - 1; k++)
            {
                vel.Add(temp1[start] + initial);
                disp.Add(temp2[start] - initial);
                initial += tempList[0];
            }
            for (int j = 0; j <= tempList.Count - 1; j++)
            {
                if (disp[j] >= 0)
                {
                    OptimumDisplacement.Add(disp[j]);
                    OptimumVelocity.Add(vel[j]);
                    OptimumAcceleration.Add(tempList[j]);
                }
                else
                {
                    OptimumDisplacement.Add(0);
                    OptimumVelocity.Add(0);
                    OptimumAcceleration.Add(0);
                }
            }
            this.previousDisp = OptimumDisplacement[tempList.Count - 1];
        }



        public void OptimumIncreasingAcceleration(int disq1, int disq2, int start, int end, List<float> accelerationTest1, List<float> accelerationTest2,
            List<float> displacementTest1, List<float> displacementTest2, List<float> tempList)
        {
            int advantage = 1;

            if (!LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
            {
                float totalStep1 = 0;
                float totalStep2 = 0;

                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    float step1 = 0;
                    float step2 = 0;
                    try
                    {
                        step1 = accelerationTest1[k + 1] - accelerationTest1[k];
                        step2 = accelerationTest2[k + 1] - accelerationTest2[k];
                    }
                    catch (Exception e)
                    {

                    }
                    totalStep1 += step1;
                    totalStep2 += step2;
                }

                double average1 = (double)((double)totalStep1 / (double)accelerationTest1.Count);
                double average2 = (double)((double)totalStep2 / (double)accelerationTest2.Count);
                double value1 = accelerationTest1[0];
                double value2 = accelerationTest2[0];


                if (average1 == 0 && average2 == 0)
                {
                    average1 = 41;
                }


                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {

                    if (average1 >= average2)
                    {
                        advantage = 1;
                        if (displacementTest1[k] == 0)
                        {
                            tempList.Add(0);
                        }
                        else
                        {
                            tempList.Add((float)value1);
                        }
                        value1 += average1;

                    }
                    else
                    {
                        advantage = 2;
                        if (displacementTest2[k] == 0)
                        {
                            tempList.Add(0);
                        }
                        else
                        {
                            tempList.Add((float)value2);
                        }
                        value2 += average2;
                    }

                }
            }

            if (!LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
            {
                advantage = 1;
                float totalStep = 0;
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    float step = 0;
                    try
                    {
                        step = accelerationTest1[k + 1] - accelerationTest1[k];
                    }
                    catch (Exception e)
                    {

                    }
                    totalStep += step;
                }
                double average = (double)((double)totalStep / (double)accelerationTest1.Count);
                if (average == 0)
                {
                    average = 41;
                }
                double value = accelerationTest1[0];
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0)
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        tempList.Add((float)value);
                    }
                    value += average;
                }

            }

            if (LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
            {
                advantage = 2;
                float totalStep = 0;
                for (int k = 0; k <= accelerationTest2.Count - 1; k++)
                {
                    float step = 0;
                    try
                    {
                        step = accelerationTest2[k + 1] - accelerationTest2[k];
                    }
                    catch (Exception e)
                    {

                    }
                    totalStep += step;
                }
                double average = (double)((double)totalStep / (double)accelerationTest1.Count);
                if (average == 0)
                {
                    average = 41;
                }
                double value = accelerationTest2[0];
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    if (displacementTest2[k] == 0)
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        tempList.Add((float)value);
                    }
                    value += average;
                }
            }

            if (LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
            {
                float totalStep1 = 0;
                float totalStep2 = 0;
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    float step1 = 0;
                    float step2 = 0;
                    try
                    {

                        float b2 = accelerationTest2[k];
                        float b1 = accelerationTest1[k];
                        float a2 = accelerationTest2[k + 1];
                        float a1 = accelerationTest1[k + 1];


                        if (b2 < 0)
                        {
                            b2 = b2 * -1;
                        }
                        if (b1 < 0)
                        {
                            b1 = b1 * -1;
                        }
                        if (a2 < 0)
                        {
                            a2 = a2 * -1;
                        }
                        if (a1 < 0)
                        {
                            a1 = a1 * -1;
                        }

                        step1 = a1 - b1;
                        step2 = a2 - b2;
                        if (step1 < 0)
                        {
                            step1 *= -1;
                        }
                        if (step2 < 0)
                        {
                            step2 *= -1;
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    totalStep1 += step1;
                    totalStep2 += step2;
                }
                double average1 = (double)((double)totalStep1 / (double)accelerationTest1.Count);
                double average2 = (double)((double)totalStep2 / (double)accelerationTest2.Count);
                if (average1 == 0 && average2 == 0)
                {
                    average1 = 41;
                }
                double value1 = accelerationTest1[0];
                double value2 = accelerationTest2[0];
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    if (average1 >= average2)
                    {
                        advantage = 1;
                        if (displacementTest1[k] == 0)
                        {
                            tempList.Add(0);
                        }
                        else
                        {
                            tempList.Add((float)value1);
                        }
                        value1 += average1;
                    }
                    else
                    {
                        advantage = 2;
                        if (displacementTest2[k] == 0)
                        {
                            tempList.Add(0);
                        }
                        else
                        {
                            tempList.Add((float)value2);
                        }
                        value2 += average2;
                    }
                }

            }
            List<float> vel = new List<float>();
            if (advantage == 1)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    vel.Add(Player1Velocity[start] + initial);
                    initial += tempList[k];
                }
            }
            if (advantage == 2)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    vel.Add(Player2Velocity[start] + initial);
                    initial += tempList[k];
                }
            }

            List<float> disp = new List<float>();

            if (advantage == 1)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    disp.Add(Player1Displacement[start] - initial);
                    initial += tempList[k];
                }
            }
            if (advantage == 2)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    disp.Add(Player2Displacement[start] - initial);
                    initial += tempList[k];
                }
            }

            for (int j = 0; j <= tempList.Count - 1; j++)
            {
                if (disp[j] >= 0)
                {
                    OptimumDisplacement.Add(disp[j]);
                }
                else
                {
                    OptimumDisplacement.Add(0);
                }
                if (disp[j] >= 0)
                {
                    OptimumVelocity.Add(vel[j]);
                    OptimumAcceleration.Add(tempList[j]);
                }
                else
                {
                    OptimumVelocity.Add(0);
                    OptimumAcceleration.Add(0);
                }
            }

            previousDisp = OptimumDisplacement[tempList.Count - 1];
        }

        public void OptimumConstantDisplacement(int size)
        {
            for (int k = 0; k <= size - 1; k++)
            {
                OptimumDisplacement.Add(previousDisp);
                OptimumVelocity.Add(0);
                OptimumAcceleration.Add(0);
            }
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 19-4-2012</para>
        /// <para>Date Modified: 23-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The GetOptimum funciton is used to derive the optimum accelerations/velocities/displacements
        /// during the race by checking the current command and comparing each player's displacement/velocity/
        /// acceleration 's values in order to choose which list holds the best values to add it to the optimum
        /// list
        /// </summary>
        /// <param></param>        
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void GetOptimum()
        {
            int disq1 = (int)(currentGame.GetPlayer1Disq() * 12);//index of disq frame
            int disq2 = (int)(currentGame.GetPlayer2Disq() * 12);//index of disq frame
            int start = 0;
            int end = 0;
            previousDisp = 4000;
            for (int i = 0; i <= TimeSpaces.Count - 1; i++)
            {
                List<float> tempList = new List<float>();
                end = start + (int)(TimeSpaces[i] * 12);
                List<float> displacementTest1 = new List<float>();
                List<float> displacementTest2 = new List<float>();
                List<float> velocityTest1 = new List<float>();
                List<float> velocityTest2 = new List<float>();
                List<float> accelerationTest1 = new List<float>();
                List<float> accelerationTest2 = new List<float>();
                for (int j = start; j <= end - 1; j++)
                {
                    displacementTest1.Add(Player1Displacement[j]);
                    displacementTest2.Add(Player2Displacement[j]);
                    velocityTest1.Add(Player1Velocity[j]);
                    velocityTest2.Add(Player2Velocity[j]);
                    accelerationTest1.Add(Player1Acceleration[j]);
                    accelerationTest2.Add(Player2Acceleration[j]);
                }
                if (CommandsList[i].Equals("constantVelocity"))
                {
                    OptimumConstantVelocity(disq1, disq2, start, end, velocityTest1, velocityTest2, displacementTest1, displacementTest2, tempList);
                }
                if (CommandsList[i].Equals("constantAcceleration") || CommandsList[i].Equals("constantDeceleration"))
                {
                    String currentCommand = CommandsList[i];
                    OptimumConstantAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2, displacementTest1, displacementTest2, tempList, currentCommand);
                }
                if (CommandsList[i].Equals("increasingAcceleration"))
                {
                    OptimumIncreasingAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2, displacementTest1, displacementTest2, tempList);
                }
                if (CommandsList[i].Equals("constantDisplacement"))
                {
                    int size = accelerationTest1.Count;
                    OptimumConstantDisplacement(size);
                }
                displacementTest1.Clear();
                displacementTest2.Clear();
                velocityTest1.Clear();
                velocityTest2.Clear();
                accelerationTest1.Clear();
                accelerationTest2.Clear();
                start = end;
                tempList.Clear();
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function LiesInBetween checks whether a given number
        /// lies between another two given values or not
        /// </summary>
        /// <param name="value">The number to be checked</param> 
        /// <param name="start">The lower bound</param> 
        /// <param name="end">The upper bound</param> 
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>Boolean: true if the given number lies between
        /// the other two values</returns>
        public Boolean LiesInBetween(int value, int start, int end)
        {
            Boolean t = false;
            if (value <= end && value > start)
            {
                t = true;
            }
            if (value >= start && value < end)
            {
                t = true;
            }
            return t;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 24/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function getMax checks the largest number in a list
        /// </summary>
        /// <param name="x">The List to be checked</param>       
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>float: the maximum of the list</returns>
        public float getMax(List<float> x)
        {
            float max = 0;
            for (int i = 0; i <= x.Count - 1; i++)
            {
                if (x[i] > max)
                {
                    max = x[i];
                }
            }
            return max;
        }
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetAverageList is used to get the average value
        /// from a set of values in a list
        /// </summary>
        /// <param name="list">A list holding a set of values</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>int: returns an integer representing the average 
        /// value of the list</returns>
        public double GetAverageList(List<float> list)
        {
            double x = 0;

            for (int i = 0; i <= list.Count - 1; i++)
            {
                x += (double)list[i];
            }
            return (double)x / (double)(list.Count);
        }
    }
}
