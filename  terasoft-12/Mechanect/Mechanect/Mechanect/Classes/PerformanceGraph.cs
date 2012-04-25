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
            //spriteBatch.Begin();
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            drawLine(spriteBatch, blank, 2, curveColor, new Vector2(a1, a2),
                new Vector2(a3, a4));
            //spriteBatch.End();
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
            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
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
        /// where dt is (1/30) since the kinect captures 30 frames per
        /// second implying that the time space (dt) between each depth 
        /// frame and its successor is (1/30) seconds.
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
                    double dt = ((double)1 / (double)24);
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
        /// where dt is (1/30) since the kinect captures 30 frames per
        /// second implying that the time space (dt) between each depth 
        /// frame and its successor is (1/30) seconds.
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
                    double dt = ((double)1 / (double)24);
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
        public void drawGraphs(List<float> Player1Displacement, List<float> Player2Displacement,
           List<String> Commands, List<double> time, Game g1)
        {

            this.Player1Displacement = Player1Displacement;
            this.Player2Displacement = Player2Displacement;
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
            double acc = 0;
            for (int i = 0; i <= TimeSpaces.Count - 1; i++)
            {
                acc += TimeSpaces[i];
            }
            totalTime = (int)acc;
            chosenTimings = new int[17];
            int timeCounter = 0;
            for (int i = 0; i <= chosenTimings.Length - 1; i++)
            {
                chosenTimings[i] = (int)(24 * totalTime * ((double)timeCounter / (double)16));
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
            for (int j = 0; j <= 5; j++)
            {
                if (j <= 1)
                {
                    counter1 = 50;
                }
                if (j > 1 && j <= 3)
                {
                    counter1 = 380;
                }
                if (j > 3)
                {
                    counter1 = 710;
                }
                for (int i = 0; i <= 15; i++)
                {
                    float value = 0;
                    double r = 0;
                    if (j <= 1)
                    {
                        value = 4000;
                        r = (double)value / (double)232;
                        if (j == 0)
                        {
                            double r2 = (double)(chosendisp1[i]) / (double)r;
                            int r3 = 118 + (int)r2;
                            double r4 = (double)(chosendisp1[i + 1]) / (double)r;
                            int r5 = 118 + (int)r4;
                            disp1[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Blue);
                            counter1 = counter1 + 15;
                            if (i == 0)
                            {
                                P1DispGraph.Add(r3);
                            }
                            P1DispGraph.Add(r5);
                        }
                        if (j == 1)
                        {
                            double r2 = (double)(chosendisp2[i]) / (double)r;
                            int r3 = 118 + (int)r2;
                            double r4 = (double)(chosendisp2[i + 1]) / (double)r;
                            int r5 = 118 + (int)r4;
                            disp2[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Black);
                            counter1 = counter1 + 15;
                            if (i == 0)
                            {
                                P2DispGraph.Add(r3);
                            }
                            P2DispGraph.Add(r5);
                        }
                    }
                    if (j > 1 && j <= 3)
                    {
                        value = maxVelocity;
                        r = (double)value / (double)232;
                        if (j == 2)
                        {
                            int a1 = 118;
                            int a2 = 118;
                            if (chosenVelocity1[i] < 0)
                            {
                                a1 = 134;
                            }
                            if (chosenVelocity1[i + 1] < 0)
                            {
                                a2 = 134;
                            }
                            double r2 = (double)(maxVelocity - 2 - chosenVelocity1[i]) / (double)r;
                            int r3 = a1 + (int)r2;
                            double r4 = (double)(maxVelocity - 2 - chosenVelocity1[i + 1]) / (double)r;
                            int r5 = a2 + (int)r4;
                            velo1[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Blue);
                            counter1 = counter1 + 15;
                            if (i == 0)
                            {
                                P1VeloGraph.Add(r3);
                            }
                            P1VeloGraph.Add(r5);
                        }
                        if (j == 3)
                        {
                            int a1 = 118;
                            int a2 = 118;
                            if (chosenVelocity2[i] < 0)
                            {
                                a1 = 134;
                            }
                            if (chosenVelocity2[i + 1] < 0)
                            {
                                a2 = 134;
                            }
                            double r2 = (double)(maxVelocity - 2 - chosenVelocity2[i]) / (double)r;
                            int r3 = a1 + (int)r2;
                            double r4 = (double)(maxVelocity - 2 - chosenVelocity2[i + 1]) / (double)r;
                            int r5 = a2 + (int)r4;
                            velo2[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Black);
                            counter1 = counter1 + 15;
                            if (i == 0)
                            {
                                P2VeloGraph.Add(r3);
                            }
                            P2VeloGraph.Add(r5);
                        }
                    }
                    if (j > 3)
                    {
                        value = maxAcceleration;
                        r = (double)value / (double)232;
                        if (j == 4)
                        {
                            int a1 = 118;
                            int a2 = 118;
                            if (chosenAcceleration1[i] < 0)
                            {
                                a1 = 134;
                            }
                            if (chosenAcceleration1[i + 1] < 0)
                            {
                                a2 = 134;
                            }
                            double r2 = (double)(maxAcceleration - 2 - chosenAcceleration1[i]) / (double)r;
                            int r3 = a1 + (int)r2;
                            double r4 = (double)(maxAcceleration - 2 - chosenAcceleration1[i + 1]) / (double)r;
                            int r5 = a2 + (int)r4;
                            acc1[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Blue);
                            counter1 = counter1 + 15;
                            if (i == 0)
                            {
                                P1AccGraph.Add(r3);
                            }
                            P1AccGraph.Add(r5);
                        }
                        if (j == 5)
                        {
                            int a1 = 118;
                            int a2 = 118;
                            if (chosenAcceleration2[i] < 0)
                            {
                                a1 = 134;
                            }
                            if (chosenAcceleration2[i + 1] < 0)
                            {
                                a2 = 134;
                            }
                            double r2 = (double)(maxAcceleration - 2 - chosenAcceleration2[i]) / (double)r;
                            int r3 = a1 + (int)r2;
                            double r4 = (double)(maxAcceleration - 2 - chosenAcceleration2[i + 1]) / (double)r;
                            int r5 = a2 + (int)r4;
                            acc2[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Black);
                            counter1 = counter1 + 15;
                            if (i == 0)
                            {
                                P2AccGraph.Add(r3);
                            }
                            P2AccGraph.Add(r5);
                        }
                    }
                }
            }

            for (int j = 0; j <= 2; j++)
            {
                if (j == 0)
                {
                    counter1 = 50;
                }
                if (j == 1)
                {
                    counter1 = 380;
                }
                if (j == 2)
                {
                    counter1 = 710;
                }
                for (int i = 0; i <= 15; i++)
                {
                    float value = 0;
                    double r = 0;
                    if (j == 0)
                    {
                        value = 4000;
                        r = (double)value / (double)232;
                        double r2 = (double)(chosenOptD[i]) / (double)r;
                        int r3 = 118 + (int)r2;
                        double r4 = (double)(chosenOptD[i + 1]) / (double)r;
                        int r5 = 118 + (int)r4;
                        optD[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                            graphics.PreferredBackBufferHeight, Color.Yellow);
                        counter1 = counter1 + 15;
                    }
                    if (j == 1)
                    {
                        value = maxVelocity;
                        r = (double)value / (double)232;
                        int a1 = 118;
                        int a2 = 118;
                        if (chosenOptV[i] < 0)
                        {
                            a1 = 134;
                        }
                        if (chosenOptV[i + 1] < 0)
                        {
                            a2 = 134;
                        }
                        double r2 = (double)(maxVelocity - 2 - chosenOptV[i]) / (double)r;
                        int r3 = a1 + (int)r2;
                        double r4 = (double)(maxVelocity - 2 - chosenOptV[i + 1]) / (double)r;
                        int r5 = a2 + (int)r4;
                        optV[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                            graphics.PreferredBackBufferHeight, Color.Yellow);
                        counter1 = counter1 + 15;
                    }
                    if (j == 2)
                    {
                        value = maxAcceleration;
                        r = (double)value / (double)232;
                        int a1 = 118;
                        int a2 = 118;
                        if (chosenOptA[i] < 0)
                        {
                            a1 = 134;
                        }
                        if (chosenOptA[i + 1] < 0)
                        {
                            a2 = 134;
                        }
                        double r2 = (double)(maxAcceleration - 2 - chosenOptA[i]) / (double)r;
                        int r3 = a1 + (int)r2;
                        double r4 = (double)(maxAcceleration - 2 - chosenOptA[i + 1]) / (double)r;
                        int r5 = a2 + (int)r4;
                        optA[i] = new PerformanceGraph(counter1, r3, counter1 + 15, r5, graphics.PreferredBackBufferWidth,
                            graphics.PreferredBackBufferHeight, Color.Yellow);
                        counter1 = counter1 + 15;
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

        public void drawAxis(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice
            , SpriteFont font, SpriteFont font2)//to be called in the draw function
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            //yaxis
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(50, 100),
                new Vector2(50, 620));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(380, 100),
                new Vector2(380, 620));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(710, 100),
                new Vector2(710, 620));

            //xaxis
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(50, 350),
                new Vector2(300, 350));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(380, 350),
                new Vector2(630, 350));
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(710, 350),
                new Vector2(960, 350));

            //labels  
            spriteBatch.DrawString(font, "Displacement", new Vector2(5, 70), Color.Red);
            spriteBatch.DrawString(font, "Velocity", new Vector2(340, 70), Color.Red);
            spriteBatch.DrawString(font, "Acceleration", new Vector2(640, 70), Color.Red);

            spriteBatch.DrawString(font, "Time", new Vector2(270, 380), Color.Red);
            spriteBatch.DrawString(font, "Time", new Vector2(600, 380), Color.Red);
            spriteBatch.DrawString(font, "Time", new Vector2(930, 380), Color.Red);

            int count = 35;
            int count2 = 300;
            int count3 = 290;
            int count4 = 40;
            int count5 = 45;
            for (int j = 0; j <= 2; j++)
            {
                for (int i = 0; i <= 4; i++)
                {
                    if (i == 0)
                    {
                        spriteBatch.DrawString(font, xaxis[i] + "", new Vector2(count - 5, 355), Color.Red);
                        count = count + 60;
                    }
                    if (i > 0)
                    {
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count + 15, 345),
                        new Vector2(count + 15, 355));
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count5, count3 + 8),
                        new Vector2(count5 + 10, count3 + 8));
                        spriteBatch.DrawString(font, xaxis[i] + "", new Vector2(count + 5, 355), Color.Red);
                        count = count + 60;
                    }
                    if (j == 0 && i > 0)
                    {
                        spriteBatch.DrawString(font2, yaxisDisplacement[i] + "", new Vector2(0, count3), Color.Red);
                        count3 = count3 - 60;
                    }
                    if (j == 1 && i > 0)
                    {
                        spriteBatch.DrawString(font2, yaxisVelocity[i] + "", new Vector2(320, count3), Color.Red);
                        count3 = count3 - 60;
                    }
                    if (j == 2 && i > 0)
                    {
                        spriteBatch.DrawString(font2, yaxisAcceleration[i] + "", new Vector2(650, count3), Color.Red);
                        count3 = count3 - 60;
                    }

                }
                count3 = 290;
                //arrows
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count2, 345),
                new Vector2(count2 + 5, 350));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count2 - 1, 356),
                new Vector2(count2 + 5, 350));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count4 + 2, 105),
                new Vector2(count4 + 8, 99));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count4 + 16, 105),
                new Vector2(count4 + 9, 99));
                if (j == 0)
                {
                    count = 365;
                    count2 = 630;
                    count4 = 370;
                    count5 = 375;
                }
                if (j == 1)
                {
                    count = 695;
                    count2 = 960;
                    count4 = 700;
                    count5 = 705;
                }
            }

            double[] negativeDisp = new double[4];
            double[] negativeVel = new double[4];
            double[] negativeAcc = new double[4];

            int n = 1;
            for (int i = 0; i <= 3; i++)
            {
                negativeDisp[i] = yaxisDisplacement[n] * -1;
                negativeVel[i] = yaxisVelocity[n] * -1;
                negativeAcc[i] = yaxisAcceleration[n] * -1;
                n++;
            }

            count = 410;
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i == 0)
                    {
                        spriteBatch.DrawString(font2, negativeDisp[j] + "", new Vector2(0, count), Color.Red);
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(45, count + 8),
                new Vector2(55, count + 8));
                    }
                    if (i == 1)
                    {
                        spriteBatch.DrawString(font2, negativeVel[j] + "", new Vector2(320, count), Color.Red);
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(375, count + 8),
                new Vector2(385, count + 8));
                    }
                    if (i == 2)
                    {
                        spriteBatch.DrawString(font2, negativeAcc[j] + "", new Vector2(650, count), Color.Red);
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(705, count + 8),
                new Vector2(715, count + 8));
                    }
                    count += 60;
                }
                if (i == 0)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(42, 612),
                new Vector2(50, 620));
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(58, 612),
                new Vector2(50, 620));
                }
                if (i == 1)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(372, 612),
                new Vector2(380, 620));
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(388, 612),
                new Vector2(380, 620));
                }
                if (i == 2)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(702, 612),
                new Vector2(710, 620));
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(718, 612),
                new Vector2(710, 620));
                }
                count = 410;
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
                        double d1 = ((double)chosenTimings[i] / (double)24);
                        double d2 = ((double)chosenTimings[i + 1] / (double)24);
                        if (player1DisqualificationTime >= d1 && player1DisqualificationTime < d2)
                        {
                            double x = d1 + ((double)(d2 - d1) / (double)2);
                            if (player1DisqualificationTime < x)
                            {
                                time = (double)chosenTimings[i] / (double)24;
                                index = i;
                            }
                            else
                            {
                                time = (double)chosenTimings[i + 1] / (double)24;
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
                        double d1 = ((double)chosenTimings[i] / (double)24);
                        double d2 = ((double)chosenTimings[i + 1] / (double)24);
                        if (player2DisqualificationTime >= d1 && player2DisqualificationTime < d2)
                        {
                            double x = d1 + ((double)(d2 - d1) / (double)2);
                            if (player2DisqualificationTime < x)
                            {
                                time = (double)chosenTimings[i] / (double)24;
                                index = i;
                            }
                            else
                            {
                                time = (double)chosenTimings[i + 1] / (double)24;
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


        public void OptimumConstantVelocity(int disq1, int disq2, int start, int end, List<float>velocityTest1, List<float>velocityTest2,
            List<float> displacementTest1, List<float> displacementTest2, List<float> tempList, float previousDisp)
        {
            int advantage = 1;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                double a = GetAverageList(velocityTest1);
                double b = GetAverageList(velocityTest2);
                for (int k = 0; k <= velocityTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0 || displacementTest2[k] == 0) //case: winning
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        if (a >= b)
                        {
                            advantage = 1;
                            tempList.Add((float)a);
                        }
                        else
                        {
                            advantage = 2;
                            tempList.Add((float)b);
                        }
                    }
                }
            }
            if (!LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1;
                double average = GetAverageList(velocityTest1);
                for (int k = 0; k <= velocityTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0) //case: winning
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        tempList.Add((float)average);
                    }
                }
            }
            if (LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                advantage = 2;
                double average = GetAverageList(velocityTest2);
                for (int k = 0; k <= velocityTest1.Count - 1; k++)
                {
                    if (displacementTest2[k] == 0) //case: winning
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        tempList.Add((float)average);
                    }
                }
            }

            if (LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1;
                //Case: Non constant velocity
                double average1 = GetAverageList(velocityTest1);
                double average2 = GetAverageList(velocityTest2);
                double value = (double)((double)(average1 + average2) / (double)2);

                for (int k = 0; k <= velocityTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0 || displacementTest2[k] == 0) //case: winning
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        tempList.Add((float)value);
                    }
                }
            }

            List<float> disp = new List<float>();

            if (advantage == 1)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    disp.Add(Player1Displacement[start] - initial);
                    initial += tempList[0];
                }
            }
            if (advantage == 2)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    disp.Add(Player2Displacement[start] - initial);
                    initial += tempList[0];
                }
            }
            List<float> acc = GetPlayerAcceleration(tempList);

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
                    OptimumVelocity.Add(tempList[j]);
                    OptimumAcceleration.Add(acc[j]);
                }
                else
                {
                    OptimumVelocity.Add(0);
                    OptimumAcceleration.Add(0);
                }
            }
            previousDisp = OptimumDisplacement[tempList.Count - 1];
        }

        public void OptimumConstantAcceleration(int disq1, int disq2, int start, int end, List<float>accelerationTest1, List<float>accelerationTest2,
            List<float> displacementTest1, List<float> displacementTest2, List<float> tempList, String currentCommand, float previousDisp)
        {
            int advantage = 1;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                //with constant deceleration if both were not disqualified then the 
                //faster is more optimum
                double a = GetAverageList(accelerationTest1);
                double b = GetAverageList(accelerationTest2);
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0 || displacementTest2[k] == 0) //case: winning
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        if (a >= b)
                        {
                            tempList.Add((float)a);
                            advantage = 1;
                        }
                        else
                        {
                            tempList.Add((float)b);
                            advantage = 2;
                        }
                    }
                }
            }
            if (!LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                double average = GetAverageList(accelerationTest1);
                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0)
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        if (currentCommand.Equals("constantAcceleration"))
                        {
                            tempList.Add((float)average);
                        }
                        else
                        {
                            tempList.Add(getMax(accelerationTest1));
                        }
                        advantage = 1;
                    }
                }
            }
            if (LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {

                double average = GetAverageList(accelerationTest2);
                for (int k = 0; k <= accelerationTest2.Count - 1; k++)
                {
                    if (displacementTest2[k] == 0)
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        if (currentCommand.Equals("constantAcceleration"))
                        {
                            tempList.Add((float)average);
                        }
                        else
                        {
                            tempList.Add(getMax(accelerationTest2));
                        }
                        advantage = 2;
                    }
                }

            }

            if (LiesInBetween(disq1, start, end - 1) && LiesInBetween(disq2, start, end - 1))
            {
                advantage = 1;
                double average1 = GetAverageList(accelerationTest1);
                double average2 = GetAverageList(accelerationTest2);
                double value = (double)((double)(average1 + average2) / (double)2);

                for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                {
                    if (displacementTest1[k] == 0 || displacementTest2[k] == 0) //case: winning
                    {
                        tempList.Add(0);
                    }
                    else
                    {
                        if (currentCommand.Equals("constantAcceleration"))
                        {
                            tempList.Add((float)value);
                        }
                        else
                        {
                            tempList.Add(-41);
                        }
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
                    initial += tempList[0];
                }
            }
            if (advantage == 2)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    vel.Add(Player2Velocity[start] + initial);
                    initial += tempList[0];
                }
            }

            List<float> disp = new List<float>();

            if (advantage == 1)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    disp.Add(Player1Displacement[start] - initial);
                    initial += tempList[0];
                }
            }
            if (advantage == 2)
            {
                float initial = 0;
                for (int k = 0; k <= tempList.Count - 1; k++)
                {
                    disp.Add(Player2Displacement[start] - initial);
                    initial += tempList[0];
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



        public void OptimumIncreasingAcceleration(int disq1,int disq2, int start, int end, List<float>accelerationTest1, List<float>accelerationTest2,
            List<float> displacementTest1, List<float> displacementTest2, List<float> tempList, float previousDisp)
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

        public void OptimumConstantDisplacement(List<float>accelerationTest1, float previousDisp)
        {
            for (int k = 0; k <= accelerationTest1.Count - 1; k++)
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
            int disq1 = (int)(currentGame.GetPlayer1Disq() * 24);//index of disq frame
            int disq2 = (int)(currentGame.GetPlayer2Disq() * 24);//index of disq frame
            int start = 0;
            int end = 0;
            float previousDisp = 4000;
            for (int i = 0; i <= TimeSpaces.Count - 1; i++)
            {
                List<float> tempList = new List<float>();
                end = start + (int)(TimeSpaces[i] * 24);
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
                    OptimumConstantVelocity(disq1, disq2, start, end, velocityTest1, velocityTest2, displacementTest1, displacementTest2, tempList, previousDisp);
                }
                if (CommandsList[i].Equals("constantAcceleration") || CommandsList[i].Equals("constantDeceleration"))
                {
                    String currentCommand = CommandsList[i];
                    OptimumConstantAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2, displacementTest1, displacementTest2, tempList, currentCommand, previousDisp);
                }
                if (CommandsList[i].Equals("increasingAcceleration"))
                {
                    OptimumIncreasingAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2, displacementTest1, displacementTest2, tempList, previousDisp);
                }
                if (CommandsList[i].Equals("constantDisplacement"))
                {
                    OptimumConstantDisplacement(accelerationTest1, previousDisp);
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
