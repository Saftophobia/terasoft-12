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
        float previousVelo;
        float previousAcc;

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
        /// The Draw function is used to draw a line connecing the points (a1,a2) and (b1,b2). 
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>       
        /// <returns>void</returns>
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
        /// The drawLine function is used to draw a straight line connecting an initial point (point1) with a final point (point2).
        /// </summary>
        /// <param name="batch">An instance of the spriteBatch class.</param>
        /// <param name="blank">An instance of the Texture2D class.</param>
        /// <param name="width">The width of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="point1">The initial point.</param>
        /// <param name="point2">The final point.</param>        
        /// <returns>void</returns>
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
        /// where dt is (1/12) since the kinect is programmed to capture 
        /// 12 frames per second implying that the time space (dt) between 
        /// each depth frame and its successor is (1/12) seconds.
        /// The resulting velocity is multiplied by negative one to
        /// get the Player's velocity relative to the Player not to the
        /// kinect, since the List holding the Player's displacements
        /// from the kinect is relative to the kinect not the player.
        /// 
        /// The try and catch statement is used to add a 0 at the begining 
        /// of the List representing the player's velocity since the players
        /// would be at a fixed distance from the kinect at the exact instant
        /// when the race starts.
        /// </summary>
        /// <param name="DisplacementList"> A List representing the
        /// player's displacements during the race.</param>      
        /// <returns>List: returns a list representing the player's
        /// velocity.</returns>

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
        /// where dt is (1/12) since the kinect is programmed to capture 12 
        /// frames per second implying that the time space (dt) between each  
        /// depth frame and its successor is (1/12) seconds.
        /// The resulting acceleration is not multiplied by -1 since the
        /// incoming velocities represent the player's velocity relative
        /// to the player not the kinect.
        /// 
        /// The try and catch statement is used to add a 0 at the begining 
        /// of the List representing the player's velocity since the players 
        /// would have 0 velocity at the exact instant when the race starts. 
        /// </summary>
        /// <param name="VelocityList">A list representing the player's
        /// velocities during the race.</param>    
        /// <returns>List: returns a list representing the player's
        /// acceleration.</returns>

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
        /// The function drawGraphs calls the necessary functions to derive each player's velocity 
        /// and acceleration as well as the optimum values, in addition, the function also calls
        /// the necessary functions required to draw the curve.
        /// </summary>
        /// <param name="Player1Displacement">A list holding Player 1's displacements.</param>
        /// <param name="Player2Displacement">A list holding Player 2's displacements.</param>
        /// <param name="Commands">A list holding each command initiated during the race.</param>
        /// <param name="time">A list holding the time elapsed by each command.</param>
        /// <param name="g1">An instance of the game class.</param>       
        /// <returns>void</returns>
        public void drawGraphs(List<float> Player1Disp, List<float> Player2Disp, List<String> Commands, List<double> time,double player1disqtime,double player2disqtime,int gwidth,int gheight)
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
            //this.currentGame = g1;
            Player1Velocity = GetPlayerVelocity(Player1Displacement);
            Player2Velocity = GetPlayerVelocity(Player2Displacement);
            Player1Acceleration = GetPlayerAcceleration(Player1Velocity);
            Player2Acceleration = GetPlayerAcceleration(Player2Velocity);
            GetOptimum((double)player1disqtime,(double)player2disqtime);
            choose();
            setMaximum();
            setDestinations(gwidth,gheight);
            setAxis();
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function choose is used to pick only 17 evenly distributed seconds among the total race time
        /// in order to represent each graph's range versus the chosen seconds.
        /// </summary>
        /// <param></param>         
        /// <returns>void</returns>
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
        /// of both players and the optimum player during the race in order to set the maximum point on 
        /// each graph's y-axis according to these values.
        /// </summary>
        /// <param></param>        
        /// <returns>void</returns>
        public void setMaximum()
        {
            maxVelocity = 0;
            maxAcceleration = 0;
            List<float> temporaryList1 = new List<float>();
            List<float> temporaryList2 = new List<float>();
            for (int i = 0; i <= chosenOptD.Length - 1; i++)
            {
                temporaryList1.Clear();
                temporaryList2.Clear();
                temporaryList1.Add(chosenOptV[i]);
                temporaryList2.Add(chosenOptA[i]);
                temporaryList1.Add(chosenVelocity1[i]);
                temporaryList2.Add(chosenAcceleration1[i]);
                temporaryList1.Add(chosenVelocity2[i]);
                temporaryList2.Add(chosenAcceleration2[i]);
                for (int j = 0; j <= temporaryList1.Count - 1; j++)
                {
                    if (temporaryList1[j] < 0)//to get maximum negative value
                    {
                        temporaryList1[j] *= -1;
                    }
                    if (temporaryList2[j] < 0)
                    {
                        temporaryList2[j] *= -1;
                    }
                    if (temporaryList1[j] > maxVelocity)
                    {
                        maxVelocity = temporaryList1[j];
                    }
                    if (temporaryList2[j] > maxAcceleration)
                    {
                        maxAcceleration = temporaryList2[j];
                    }
                }
            }            
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function setDestinations is used to create 16 instances of the PerformanceGraph class for each graph giving
        /// it the initial and final points to be connected in order to represent each graph by 16 connected lines.
        /// </summary>
        /// <param name="graphics">An instance of the GraphicsDeviceManger class.</param>
        /// <returns>void</returns>
        public void setDestinations(int Width, int Height)
        {
            int counter1 = 0;
            float value = 0;
            double r = 0;
            for (int j = 0; j <= 8; j++)
            {
                PerformanceGraph[] current = new PerformanceGraph[16];               
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
                    current[i] = new PerformanceGraph(counter1, r3, counter1 + 16, r5, Width,
                        Height, color);
                    counter1 = counter1 + 16;
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
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function setAxis chooses 5 evenly distributed values among the total time to be represented on the x-axis
        /// as well as 5 evenly distributed values among the total displacement/velocity/acceleration to be represented on
        /// each graph's y-axis.
        /// </summary>
        /// <returns>void</returns>
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

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawRange loops over each instance of the PerformanceGraph class initialized in the function
        /// setDestinations and connects each initial point with each final point.
        /// </summary>
        /// <param name="spriteBatch">An instance of the SpriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <returns>void</returns>
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
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawLabels is used to add a label for each axis indicating whether each graph represents displacement or velocity or acceleration.
        /// </summary>
        /// <param name="spriteBatch">An instance of the SpriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void drawLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            spriteBatch.DrawString(font, "Displacement", new Vector2(5, 70), Color.Black);
            spriteBatch.DrawString(font, "Velocity", new Vector2(340, 70), Color.Black);
            spriteBatch.DrawString(font, "Acceleration", new Vector2(640, 70), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(270, 380), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(600, 380), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(930, 380), Color.Black);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawAxis is used to draw the X and Y axis for each graph.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
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

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawArrows is used to add an arrow at the end of each axis for each graph.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
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

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawXLabels is used to add the values to be displayed on the X-axis.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
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

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawYLabels is used to add the values to be displayed on the Y-axis.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void drawYLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //+ve part of the y-axis
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
            //-ve part of the y-axis
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
                    spriteBatch.DrawString(font2, -1 * current[j] + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawEnvironment calls the necessary functions to draw the X and Y axis with their labels for each graph on the screen
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void drawEnvironment(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            drawAxis(spriteBatch, GraphicsDevice, font, font2);
            drawLabels(spriteBatch, GraphicsDevice, font, font2);
            drawArrows(spriteBatch, GraphicsDevice, font, font2);
            drawXLabels(spriteBatch, GraphicsDevice, font, font2);
            drawYLabels(spriteBatch, GraphicsDevice, font, font2);
            //drawing the marks on the X-axis
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
            //drawing the marks on the Y-axis
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

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 22/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawDisqualification is used to add a mark on the point where a disqualified player was disqualified, the function
        /// first decides the mark's X-coordinate then uses it to derive its Y-coordinate before representing it on each graph. 
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class.</param>
        /// <param name="graphics">An instance of the GraphicsDeviceManager class.</param>
        /// <param name="P1Tex">A Texture2D representing the image "xRed.png".</param>
        /// <param name="P2Tex">A Texture2D representing the image "xBlue.png".</param>
        /// <returns>void</returns>
        public void drawDisqualification(SpriteBatch spriteBatch, int dwidth, int dheight, Texture2D P1Tex, Texture2D P2Tex, double player1disqtime,double player2disqtime)
        {
            if (player1disqtime > 0 || player2disqtime > 0)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Boolean t = false;
                    double n = 0;
                    switch (j)
                    {
                        case 0: n = player1disqtime; if (n >= 0) { t = true; }; break;
                        case 1: n = player2disqtime; if (n >= 0) { t = true; }; break;
                    }
                    if (t)
                    {
                        double time = totalTime;
                        int index = 8;
                        for (int i = 0; i <= chosenTimings.Length - 1; i++)
                        {
                            if (i < chosenTimings.Length - 1)
                            {
                                double d1 = ((double)chosenTimings[i] / (double)12);
                                double d2 = ((double)chosenTimings[i + 1] / (double)12);
                                if (n >= d1 && n < d2)
                                {
                                    double x = d1 + ((double)(d2 - d1) / (double)2);
                                    if (n < x)
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
                        int y1 = 0; int y2 = 0; int y3 = 0;
                        double r1 = (double)totalTime / (double)256;
                        double r2 = (double)(time) / (double)r1;
                        int r3 = 40 + (int)r2;
                        Texture2D texture = null;
                        switch (j)
                        {
                            case 0: y1 = P1DispGraph[index] - 10; y2 = P1VeloGraph[index] - 10; y3 = P1AccGraph[index] - 10; texture = P1Tex; break;
                            case 1: y1 = P2DispGraph[index] - 10; y2 = P2VeloGraph[index] - 10; y3 = P2AccGraph[index] - 10; texture = P2Tex; break;
                        }
                        xDP1 = new CountDown(texture, dwidth,dheight, r3, y1, 20, 20);
                        xDP1.Draw(spriteBatch);
                        r3 = 370 + (int)r2;
                        xVP1 = new CountDown(texture, dwidth, dheight, r3, y2, 20, 20);
                        xVP1.Draw(spriteBatch);
                        r3 = 700 + (int)r2;
                        xAP1 = new CountDown(texture, dwidth, dheight, r3, y3, 20, 20);
                        xAP1.Draw(spriteBatch);
                    }
                }
            }
        }
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantVelocity derives the optimum values for the "constantVelocity" command, by comparing the
        /// avarages of each player's velocity and choosing the higher average value as an optimum value, or taking a player's 
        /// average velocity if the other player was disqualified during the command's time slice or the average of both players'
        /// average velocities if both have been disqualified during the command's time slice.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="velocityTest1">A list representing Player 1's velocity during the race.</param>
        /// <param name="velocityTest2">A list representing Player 2's velocity during the race.</param>
        /// <returns>void</returns>
        public void OptimumConstantVelocity(int size)
        {
            float velocity = previousVelo;
            float x = previousDisp - velocity;
            for (int i = 0; i <= size - 1; i++)
            {
                if (x >= 0)
                {
                    OptimumVelocity.Add(velocity);
                    OptimumDisplacement.Add(x);
                }
                else
                {
                    OptimumVelocity.Add(0);
                    OptimumDisplacement.Add(0);
                }
                OptimumAcceleration.Add(0);
                x = x - velocity;
            }
            this.previousDisp = OptimumDisplacement[OptimumDisplacement.Count - 1];
            this.previousVelo = OptimumVelocity[OptimumVelocity.Count - 1];
            this.previousAcc = 0;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantAcceleration derives the optimum values for the "constantAcceleration" and "constantDeceleration" 
        /// commands, by comparing the avarages of each player's acceleration and choosing the higher average value as an optimum value, or 
        /// taking a player's average acceleration if the other player was disqualified during the command's time slice or the average of both players'
        /// accelerations if both have been disqualified during the command's time slice.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="accelerationTest1">A list representing Player 1's acceleration during the race.</param>
        /// <param name="accelerationTest2">A list representing Player 2's acceleration during the race.</param>
        /// <param name="currentCommand">A string representing the current command.</param>
        /// <returns>void</returns>
        public void OptimumConstantAcceleration(int size)
        {
            float acceleration = previousAcc;
            if (acceleration == 0)
            {
                acceleration = 41;
            }
            float accumulator = previousVelo;
            float z = previousVelo + acceleration;
            float x = previousDisp - z;
            for (int i = 0; i <= size - 1; i++)
            {
                if (x >= 0)
                {
                    OptimumAcceleration.Add(acceleration);
                    OptimumVelocity.Add(z);
                    OptimumDisplacement.Add(x);
                }
                else
                {
                    OptimumAcceleration.Add(0);
                    OptimumVelocity.Add(0);
                    OptimumDisplacement.Add(0);
                }
                z = z + acceleration;
                x = x - z;
            }
            this.previousAcc = OptimumAcceleration[OptimumAcceleration.Count - 1];
            this.previousVelo = OptimumVelocity[OptimumVelocity.Count - 1];
            this.previousDisp = OptimumDisplacement[OptimumDisplacement.Count - 1];
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumIncreasingAcceleration derives the optimum values for the "increasingAcceleration" command,
        /// by comparing between the average acceleration differences for each player during the race and choosing the higher 
        /// average value as an optimum value, or taking a player's average acceleration difference if the other player was disqualified 
        /// during the command's time slice or assigning fixed values as optimum values if both have been disqualified during the command's 
        /// time slice.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="accelerationTest1">A list representing Player 1's acceleration during the race.</param>
        /// <param name="accelerationTest2">A list representing Player 2's acceleration during the race/</param>
        /// <returns>void</returns>
        public void OptimumIncreasingAcceleration(int disq1, int disq2, int start, int end, List<float> accelerationTest1, List<float> accelerationTest2)
        {
            float totalStep1 = getTotalStep(accelerationTest1);
            float totalStep2 = getTotalStep(accelerationTest2);
            double a = (double)((double)totalStep1 / (double)accelerationTest1.Count);
            double b = (double)((double)totalStep2 / (double)accelerationTest2.Count);
            double value = 0;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                if (a > b)
                {
                    value = a;
                }
                else
                {
                    value = b;
                }
            }
            if (!LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                value = a;
            }
            if (LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                value = b;
            }
            if (LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                value = 41;
            }
            if (value == 0)
            {
                value = 41;
            }
            List<float> accelerationTrial = new List<float>();
            float accumulator = previousAcc;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z = accumulator + (float)value;
                accelerationTrial.Add(z);
                accumulator = z;
            }
            List<float> velocityTest = new List<float>();
            accumulator = previousVelo;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z = accumulator + accelerationTrial[i];
                velocityTest.Add(z);
                accumulator = z;
            }
            accumulator = previousDisp;
            float adder = (float)value;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z = accumulator - (float)velocityTest[i];
                if (z >= 0)
                {
                    OptimumAcceleration.Add((float)adder);
                    OptimumDisplacement.Add(z);
                    OptimumVelocity.Add(velocityTest[i]);
                }
                else
                {
                    OptimumAcceleration.Add(0);
                    OptimumDisplacement.Add(0);
                    OptimumVelocity.Add(0);
                }
                adder += (float)value;
                accumulator = z;
            }
            this.previousAcc = OptimumAcceleration[OptimumAcceleration.Count - 1];
            this.previousVelo = OptimumVelocity[OptimumVelocity.Count - 1];
            this.previousDisp = OptimumDisplacement[OptimumDisplacement.Count - 1];
        }


        public void OptimumDecreasingAcceleration(int disq1, int disq2, int start, int end, List<float> accelerationTest1, List<float> accelerationTest2)
        {
            float totalStep1 = getTotalStep(accelerationTest1);
            float totalStep2 = getTotalStep(accelerationTest2);
            double a = (double)((double)totalStep1 / (double)accelerationTest1.Count);
            double b = (double)((double)totalStep2 / (double)accelerationTest2.Count);
            double value = 0;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                if (a > b)
                {
                    value = a;
                }
                else
                {
                    value = b;
                }
            }
            if (!LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                value = a;
            }
            if (LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                value = b;
            }
            if (LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                value = 41;
            }
            if (value == 0)
            {
                value = 41;
            }
            List<float> accelerationTrial = new List<float>();
            float accumulator = previousAcc;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z = accumulator - (float)value;
                if (z >= 0)
                {
                    accelerationTrial.Add(z);
                }
                else
                {
                    accelerationTrial.Add(0);
                }
                accumulator = z;
            }
            List<float> velocityTest = new List<float>();
            accumulator = previousVelo;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z = accumulator + accelerationTrial[i];
                velocityTest.Add(z);
                accumulator = z;
            }
            accumulator = previousDisp;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z = accumulator - (float)velocityTest[i];
                if (z >= 0)
                {
                    OptimumAcceleration.Add(accelerationTrial[i]);
                    OptimumDisplacement.Add(z);
                    OptimumVelocity.Add(velocityTest[i]);
                }
                else
                {
                    OptimumAcceleration.Add(0);
                    OptimumDisplacement.Add(0);
                    OptimumVelocity.Add(0);
                }

                accumulator = z;
            }
            this.previousAcc = OptimumAcceleration[OptimumAcceleration.Count - 1];
            this.previousVelo = OptimumVelocity[OptimumVelocity.Count - 1];
            this.previousDisp = OptimumDisplacement[OptimumDisplacement.Count - 1];
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantDisplacement derives the optimum values for the "constantDisplacement" command, by adding
        /// 0 as an optimum acceleration and velocity value and fixing the displacement from the kinect considering the fixed displacement
        /// as an optimum displacement.
        /// </summary>
        /// <param name="size">The number of frames corresponding to the current command</param>
        /// <returns>void</returns>
        public void OptimumConstantDisplacement(int size)
        {
            for (int k = 0; k <= size - 1; k++)
            {
                OptimumDisplacement.Add(this.previousDisp);
                OptimumVelocity.Add(0);
                OptimumAcceleration.Add(0);
            }
            this.previousDisp = OptimumDisplacement[OptimumDisplacement.Count - 1];
            this.previousVelo = 0;
            this.previousAcc = 0;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function getTotalStep calculates the cummulative acceleration difference for the given acceleration list.
        /// </summary>
        /// <param name="l1">A list holding a player's acceleration values</param>
        /// <returns>float: The total calculated step</returns>
        public float getTotalStep(List<float> l1)
        {
            float totalStep = 0;
            for (int k = 0; k <= l1.Count - 1; k++)
            {
                float step = 0;
                try
                {
                    if (l1[k] > 0 && l1[k + 1] > 0)//to avoid 0
                    {
                        step = l1[k + 1] - l1[k];
                    }
                }
                catch (Exception e)
                {
                }
                totalStep += step;
            }
            if (totalStep < 0)
            {
                totalStep *= -1;
            }
            return totalStep;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 19-4-2012</para>
        /// <para>Date Modified: 23-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The GetOptimum funciton is used to derive the optimum accelerations/velocities/displacements
        /// during the race by calling the necessary functions.
        /// </summary>
        /// <param></param>
        /// <returns>void</returns>
        public void GetOptimum(double player1disq,double player2disq)
        {
            int disq1 = (int)(player1disq * 12);//index of disq frame
            int disq2 = (int)(player2disq * 12);//index of disq frame
            int start = 0;
            int end = 0;
            previousDisp = 4000;
            previousVelo = 0;
            previousAcc = 0;
            for (int i = 0; i <= TimeSpaces.Count - 1; i++)
            {
                List<float> tempList = new List<float>();
                end = start + (int)(TimeSpaces[i] * 12);
                List<float> velocityTest1 = new List<float>();
                List<float> velocityTest2 = new List<float>();
                List<float> accelerationTest1 = new List<float>();
                List<float> accelerationTest2 = new List<float>();
                for (int j = start; j <= end - 1; j++)
                {
                    velocityTest1.Add(Player1Velocity[j]);
                    velocityTest2.Add(Player2Velocity[j]);
                    accelerationTest1.Add(Player1Acceleration[j]);
                    accelerationTest2.Add(Player2Acceleration[j]);
                }
                if (CommandsList[i].Equals("constantVelocity"))
                {
                    int x = end - start;
                    OptimumConstantVelocity(x);
                }
                if (CommandsList[i].Equals("constantAcceleration"))
                {
                    int x = end - start;
                    OptimumConstantAcceleration(x);
                }
                if (CommandsList[i].Equals("increasingAcceleration"))
                {
                    OptimumIncreasingAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2);
                }
                if (CommandsList[i].Equals("constantDeceleration"))
                {
                    OptimumDecreasingAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2);
                }
                if (CommandsList[i].Equals("constantDisplacement"))
                {
                    int size = end - start;
                    OptimumConstantDisplacement(size);
                }
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
        /// lies between another two given values or not.
        /// </summary>
        /// <param name="value">The number to be checked.</param> 
        /// <param name="start">The lower bound.</param> 
        /// <param name="end">The upper bound.</param> 
        /// <returns>Boolean: true if the given number lies between
        /// the other two values.</returns>
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
        /// The function getMax checks the largest number in a list.
        /// </summary>
        /// <param name="x">The List to be checked.</param>   
        /// <returns>float: the maximum of the list.</returns>
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
        /// from a set of values in a list.
        /// </summary>
        /// <param name="list">A list holding a set of values.</param
        /// <returns>int: returns an integer representing the average 
        /// value of the list.</returns>
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
