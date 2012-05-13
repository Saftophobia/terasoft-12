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
using Mechanect.Classes;



namespace Mechanect
{
    class PerformanceGraph
    {
        private int stageWidth, stageHeight;
        private int xPoint1;
        private int yPoint1;
        private int xPoint2;
        private int yPoint2;
        private Color curveColor;
        private int samples;
        private int distance;  
        private List<float> player1Displacement;
        private List<float> player2Displacement;
        private List<float> player1Velocity;
        private List<float> player2Velocity;
        private List<float> player1Acceleration;
        private List<float> player2Acceleration;
        private List<float> optimumDisplacement = new List<float>();
        private List<float> optimumVelocity = new List<float>();
        private List<float> optimumAcceleration = new List<float>();
        private List<String> commandsList;
        private List<double> timeSpaces;
        private double totalTime;
        private int[] chosenTimings;
        private float[] chosenDisp1 = new float[17];
        private float[] chosenDisp2 = new float[17];
        private float[] chosenVelocity1 = new float[17];
        private float[] chosenVelocity2 = new float[17];
        private float[] chosenAcceleration1 = new float[17];
        private float[] chosenAcceleration2 = new float[17];
        private float[] chosenOptD = new float[17];
        private float[] chosenOptV = new float[17];
        private float[] chosenOptA = new float[17];
        private PerformanceGraph[] disp1 = new PerformanceGraph[16];
        private PerformanceGraph[] disp2 = new PerformanceGraph[16];
        private PerformanceGraph[] velo1 = new PerformanceGraph[16];
        private PerformanceGraph[] velo2 = new PerformanceGraph[16];
        private PerformanceGraph[] acc1 = new PerformanceGraph[16];
        private PerformanceGraph[] acc2 = new PerformanceGraph[16];
        private PerformanceGraph[] optD = new PerformanceGraph[16];
        private PerformanceGraph[] optV = new PerformanceGraph[16];
        private PerformanceGraph[] optA = new PerformanceGraph[16];
        private float maxVelocity;
        private float maxAcceleration;
        private List<int> p1DispGraph = new List<int>();
        private List<int> p2DispGraph = new List<int>();
        private List<int> p1VeloGraph = new List<int>();
        private List<int> p2VeloGraph = new List<int>();
        private List<int> p1AccGraph = new List<int>();
        private List<int> p2AccGraph = new List<int>();
        private double[] xAxis = new double[5];
        private double[] yAxisDisplacement = new double[5];
        private double[] yAxisVelocity = new double[5];
        private double[] yAxisAcceleration = new double[5];
        private CountDown xDP1;
        private CountDown xVP1;
        private CountDown xAP1;
        private float previousDisp;
        private float previousVelo;
        private float previousAcc;
        private double player1Win;
        private double player2Win;
        private double player3Win;
        private float[,] chosen;

        public PerformanceGraph(int start1, int start2, int finishx, int finishy, int a, int b, Color col)
        {
            xPoint1 = start1;
            yPoint1 = start2;
            xPoint2 = finishx;
            yPoint2 = finishy;
            curveColor = col;
            stageWidth = a;
            stageHeight = b;
        }

        public PerformanceGraph()
        {

        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Initialize determines the optimal number of points to be represented on the graph.
        /// </summary>
        /// <param></param>        
        /// <returns>void</returns>
        public void Initialize()
        {
            if (totalTime <= 1)
            {
                samples = 8;
            }
            if (totalTime > 1 && totalTime <= 2)
            {
                samples = 16;
            }
            if (totalTime > 2 && totalTime <= 5)
            {
                samples = 32;
            }
            if (totalTime > 5 && totalTime <= 10)
            {
                samples = 64;
            }
            if (totalTime > 10 && totalTime <= 21)
            {
                samples = 128;
            }
            if (totalTime > 21)
            {
                samples = 256;
            }
            switch (samples)
            {
                case 256: distance = 1; break;
                case 128: distance = 2; break;
                case 64: distance = 4; break;
                case 32: distance = 8; break;
                case 16: distance = 16; break;
                case 8: distance = 32; break;
                case 4: distance = 64; break;
            }
            chosen = new float[9, samples + 1];
            disp1 = new PerformanceGraph[samples];
            disp2 = new PerformanceGraph[samples];
            velo1 = new PerformanceGraph[samples];
            velo2 = new PerformanceGraph[samples];
            acc1 = new PerformanceGraph[samples];
            acc2 = new PerformanceGraph[samples];
            optD = new PerformanceGraph[samples];
            optV = new PerformanceGraph[samples];
            optA = new PerformanceGraph[samples];
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
            DrawLine(spriteBatch, blank, 2, curveColor, new Vector2(xPoint1, yPoint1), new Vector2(xPoint2, yPoint2));
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The DrawLine function is used to draw a straight line connecting an initial point (point1) with a final point (point2).
        /// </summary>
        /// <param name="batch">An instance of the spriteBatch class.</param>
        /// <param name="blank">An instance of the Texture2D class.</param>
        /// <param name="width">The width of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="point1">The initial point.</param>
        /// <param name="point2">The final point.</param>        
        /// <returns>void</returns>
        public void DrawLine(SpriteBatch batch, Texture2D blank,
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
                    float currentVelocity = ((float)(DisplacementList[i] - DisplacementList[i - 1]) / (float)(dt)) * -1;
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
        /// The function DrawGraphs calls the necessary functions to derive each player's velocity 
        /// and acceleration as well as the optimum values, in addition, the function also calls
        /// the necessary functions required to draw the curve.
        /// </summary>
        /// <param name="D1">A list holding Player 1's displacements.</param>
        /// <param name="D2">A list holding Player 2's displacements.</param>
        /// <param name="Commands">A list holding each command initiated during the race.</param>
        /// <param name="time">A list holding the time elapsed by each command.</param>
        /// <param name="player1disqtime">The instance when the first player was disqualified.</param>    
        /// <param name="player2disqtime">The instance when the second player was disqualified.</param>  
        /// <param name="gwidth">The width of the screen.</param>
        /// <param name="gheight">The height of the screen.</param>
        /// <returns>void</returns>
        public void DrawGraphs(List<float> D1, List<float> D2, List<String> Commands, List<double> time, double player1disqtime, double player2disqtime, int gwidth, int gheight)
        {
            player1Displacement = new List<float>();
            player2Displacement = new List<float>();
            optimumDisplacement = new List<float>();
            optimumVelocity = new List<float>();
            optimumAcceleration = new List<float>();
            FixDifference(D1, D2);
            this.commandsList = Commands;
            this.timeSpaces = time;
            player1Velocity = GetPlayerVelocity(player1Displacement);
            player2Velocity = GetPlayerVelocity(player2Displacement);
            player1Acceleration = GetPlayerAcceleration(player1Velocity);
            player2Acceleration = GetPlayerAcceleration(player2Velocity);
            GetOptimum((double)player1disqtime, (double)player2disqtime);
            Discard();
            SetNewTime(time, Commands);
            GetWinning();
            CalculateTotalTime();
            Initialize();
            Choose();
            SetMaximum();
            SetDestinations(gwidth, gheight);
            SetAxis();
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function FixDifference aims to avoid any exceptions resulted from
        /// capturing more or less frames than the expected number of frames per second.
        /// </summary>
        /// <param name="D1">A list representing Player 1's displacement.</param>
        /// <param name="D2">A list representing Player 2's displacement</param>
        /// <returns>void.</returns>
        public void FixDifference(List<float> D1, List<float> D2)
        {
            int difference = 0;
            int size = D1.Count;
            if (D1.Count > D2.Count)
            {
                difference = D1.Count - D2.Count;
                size = D1.Count - difference;
            }
            if (D2.Count > D1.Count)
            {
                difference = D2.Count - D1.Count;
                size = D2.Count - difference;
            }
            for (int i = 0; i <= size - 1; i++)
            {
                player1Displacement.Add(D1[i]);
                player2Displacement.Add(D2[i]);
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Discard drops the frames received after the instance when all the players including
        /// the optimum player have reached the finish line.
        /// </summary>
        /// <returns>void.</returns>
        public void Discard()
        {
            Boolean found = false;
            List<float> temp1 = new List<float>();
            List<float> temp2 = new List<float>();
            List<float> temp3 = new List<float>();
            for (int i = 0; i <= player1Displacement.Count - 1; i++)
            {
                if (!found)
                {
                    temp1.Add(player1Displacement[i]);
                    temp2.Add(player2Displacement[i]);
                    temp3.Add(optimumDisplacement[i]);
                    if (player1Displacement[i] == 0 && player2Displacement[i] == 0 && optimumDisplacement[i] == 0)
                    {
                        found = true;
                    }
                }
            }
            player1Displacement = temp1;
            player2Displacement = temp2;
            optimumDisplacement = temp3;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetNewTime sets the race time to be equal to the total time elapsed by the players to reach the finish line by discarding any
        /// additional Commands.
        /// </summary>
        /// <param name="time">A list representing each command's time slice</param>
        /// <param name="Commands">A list representing each command given during the race</param>
        /// <returns>void.</returns>
        public void SetNewTime(List<double> time, List<string> Commands)
        {
            timeSpaces = new List<double>();
            commandsList = new List<string>();
            int newSize = player1Displacement.Count;
            double newTime = ((double)newSize / (double)12);
            Boolean t = false;
            int count = 0;
            double acc = 0;
            this.timeSpaces = new List<double>();
            this.commandsList = new List<string>();
            while (!t)
            {
                if (acc + time[count] <= newTime)
                {
                    this.timeSpaces.Add(time[count]);
                    commandsList.Add(Commands[count]);
                }
                else
                {
                    double idealNumber = newTime - acc;
                    this.timeSpaces.Add(idealNumber);
                    commandsList.Add(Commands[count]);
                }
                acc += time[count];
                count++;
                if (acc >= newTime)
                {
                    t = true;
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetWinning determines the times at which each player reaches the finish line.
        /// </summary>
        /// <returns>void.</returns>
        public void GetWinning()
        {
            Boolean found1 = false;
            Boolean found2 = false;
            Boolean found3 = false;
            for (int i = 0; i <= player1Displacement.Count - 1; i++)
            {
                if (!found1)
                {
                    if (player1Displacement[i] == 0)
                    {
                        player1Win = (double)i / (double)12;
                        found1 = true;
                    }
                }
                if (!found2)
                {
                    if (player2Displacement[i] == 0)
                    {
                        player2Win = (double)i / (double)12;
                        found2 = true;
                    }
                }
                if (!found3)
                {
                    if (optimumDisplacement[i] == 0)
                    {
                        player3Win = (double)i / (double)12;
                        found3 = true;
                    }
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function CalculateTotalTime calculates the total race time.
        /// </summary>
        /// <returns>void.</returns>
        public void CalculateTotalTime()
        {
            double accumulator = 0;
            for (int i = 0; i <= timeSpaces.Count - 1; i++)
            {
                accumulator += timeSpaces[i];
            }
            totalTime = accumulator;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Choose is used to pick only 17 evenly distributed seconds among the total race time
        /// in order to represent each graph's range versus the chosen seconds.
        /// </summary>
        /// <param></param>         
        /// <returns>void</returns>
        public void Choose()
        {
            chosenTimings = new int[17];
            int timeCounter = 0;
            for (int i = 0; i <= chosenTimings.Length - 1; i++)
            {
                chosenTimings[i] = (int)(12 * totalTime * ((double)timeCounter / (double)16));
                timeCounter++;
            }
            int u = 0;
            for (int i = 0; i <= chosenDisp1.Length - 1; i++)
            {
                if (i > 0)
                {
                    u = chosenTimings[i] - 1;
                }
                else
                {
                    u = chosenTimings[i];
                }
                chosenDisp1[i] = player1Displacement[u];
                chosenDisp2[i] = player2Displacement[u];
                chosenVelocity1[i] = player1Velocity[u];
                chosenVelocity2[i] = player2Velocity[u];
                chosenAcceleration1[i] = player1Acceleration[u];
                chosenAcceleration2[i] = player2Acceleration[u];
                chosenOptD[i] = optimumDisplacement[u];
                chosenOptV[i] = optimumVelocity[u];
                chosenOptA[i] = optimumAcceleration[u];
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetMaximum is used to derive the maximum velocity and the maximum acceleration
        /// of both players and the optimum player during the race in order to set the maximum point on 
        /// each graph's y-axis according to these values.
        /// </summary>
        /// <param></param>        
        /// <returns>void</returns>
        public void SetMaximum()
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
        /// The function SetDestinations is used to create 16 instances of the PerformanceGraph class for each graph giving
        /// it the initial and final points to be connected in order to represent each graph by 16 connected lines.
        /// </summary>
        /// <param name="Width">The width of the screen.</param>
        /// <param name="Height">The height of the screen.</param>
        /// <returns>void</returns>
        public void SetDestinations(int Width, int Height)
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
                    case 0: counter1 = 50; value = 4000; current = disp1; color = Color.Red; temporary = chosenDisp1; disqList = p1DispGraph; break;
                    case 1: counter1 = 50; value = 4000; current = disp2; color = Color.Blue; temporary = chosenDisp2; disqList = p2DispGraph; break;
                    case 2: counter1 = 50; value = 4000; current = optD; color = Color.Yellow; temporary = chosenOptD; break;
                    case 3: counter1 = 380; value = maxVelocity; current = velo1; color = Color.Red; temporary = chosenVelocity1; disqList = p1VeloGraph; break;
                    case 4: counter1 = 380; value = maxVelocity; current = velo2; color = Color.Blue; temporary = chosenVelocity2; disqList = p2VeloGraph; break;
                    case 5: counter1 = 380; value = maxVelocity; current = optV; color = Color.Yellow; temporary = chosenOptV; break;
                    case 6: counter1 = 710; value = maxAcceleration; current = acc1; color = Color.Red; temporary = chosenAcceleration1; disqList = p1AccGraph; break;
                    case 7: counter1 = 710; value = maxAcceleration; current = acc2; color = Color.Blue; temporary = chosenAcceleration2; disqList = p2AccGraph; break;
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
        /// The function SetAxis chooses 5 evenly distributed values among the total time to be represented on the x-axis
        /// as well as 5 evenly distributed values among the total displacement/velocity/acceleration to be represented on
        /// each graph's y-axis.
        /// </summary>
        /// <returns>void</returns>
        public void SetAxis()
        {
            xAxis[0] = 0;
            double step = (double)totalTime / (double)4;
            for (int i = 1; i <= xAxis.Length - 1; i++)
            {
                xAxis[i] = xAxis[i - 1] + step;
            }
            int counter = 0;
            for (int i = 0; i <= 4; i++)
            {
                yAxisDisplacement[i] = counter;
                counter += 1000;
            }
            yAxisVelocity[0] = 0;
            step = (double)maxVelocity / (double)4;
            for (int i = 1; i <= yAxisVelocity.Length - 1; i++)
            {
                yAxisVelocity[i] = yAxisVelocity[i - 1] + step;
            }
            yAxisAcceleration[0] = 0;
            step = (double)maxAcceleration / (double)4;
            for (int i = 1; i <= yAxisAcceleration.Length - 1; i++)
            {
                yAxisAcceleration[i] = yAxisAcceleration[i - 1] + step;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawRange loops over each instance of the PerformanceGraph class initialized in the function
        /// setDestinations and connects each initial point with each final point.
        /// </summary>
        /// <param name="spriteBatch">An instance of the SpriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <returns>void</returns>
        public void DrawRange(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
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
        /// The function DrawLabels is used to add a label for each axis indicating whether each graph represents displacement or velocity or acceleration.
        /// </summary>
        /// <param name="spriteBatch">An instance of the SpriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void DrawLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
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
        /// The function DrawAxis is used to draw the X and Y axis for each graph.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void DrawAxis(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 100),
                new Vector2(50, 620));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 100),
                new Vector2(380, 620));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 100),
                new Vector2(710, 620));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 350),
                new Vector2(316, 350));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 350),
                new Vector2(646, 350));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 350),
                new Vector2(976, 350));
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawArrows is used to add an arrow at the end of each axis for each graph.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void DrawArrows(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            int counter = 309;
            for (int i = 0; i <= 2; i++)
            {
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 343),
                new Vector2(counter + 7, 350));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 357),
                new Vector2(counter + 7, 350));
                counter += 330;
            }
            counter = 40;
            for (int i = 0; i <= 2; i++)
            {
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 110),
                new Vector2(counter + 10, 100));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 110),
                new Vector2(counter + 8, 100));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 610),
                new Vector2(counter + 10, 620));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 610),
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
        /// The function DrawXLabels is used to add the values to be displayed on the X-axis.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void DrawXLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
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
                    spriteBatch.DrawString(font2, xAxis[j] + "", new Vector2(counter - 5, 358), Color.Black);
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
        /// The function DrawYLabels is used to add the values to be displayed on the Y-axis.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void DrawYLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //+ve part of the y-axis
            int y = 0;
            double[] current = yAxisDisplacement; ;
            for (int i = 0; i <= 2; i++)
            {
                int counter = 110;
                switch (i)
                {
                    case 1: y = 320; current = yAxisVelocity; break;
                    case 2: y = 650; current = yAxisAcceleration; break;
                    default: y = 0; current = yAxisDisplacement; break;
                }
                for (int j = 4; j >= 1; j--)
                {
                    spriteBatch.DrawString(font2, (int)current[j] + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
            //-ve part of the y-axis
            for (int i = 0; i <= 2; i++)
            {
                int counter = 400;
                switch (i)
                {
                    case 1: y = 320; current = yAxisVelocity; break;
                    case 2: y = 650; current = yAxisAcceleration; break;
                    default: y = 0; current = yAxisDisplacement; break;
                }
                for (int j = 1; j <= 4; j++)
                {
                    spriteBatch.DrawString(font2, -1 * (int)current[j] + "", new Vector2(y, counter), Color.Black);
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
        /// The function DrawEnvironment calls the necessary functions to draw the X and Y axis with their labels for each graph on the screen
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public void DrawEnvironment(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            DrawAxis(spriteBatch, GraphicsDevice, font, font2);
            DrawLabels(spriteBatch, GraphicsDevice, font, font2);
            DrawArrows(spriteBatch, GraphicsDevice, font, font2);
            DrawXLabels(spriteBatch, GraphicsDevice, font, font2);
            DrawYLabels(spriteBatch, GraphicsDevice, font, font2);
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
                    DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 345),
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
                    DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(y - 7, counter2),
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
        /// The function DrawDisqualification is used to add a mark on the point where a disqualified player was disqualified, the function
        /// first decides the mark's X-coordinate then uses it to derive its Y-coordinate before representing it on each graph. 
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class.</param>
        /// <param name="dwidth">The width of the screen.</param>
        /// <param name="dheight">The height of the screen.</param> 
        /// <param name="P1Tex">A Texture2D representing the image "xRed.png".</param>
        /// <param name="P2Tex">A Texture2D representing the image "xBlue.png".</param>
        /// <param name="player1disqtime">The instance when player 1 was disqualified.</param>
        /// <param name="player2disqtime">The instance when player 2 was disqualified</param>
        /// <returns>void</returns>
        public void DrawDisqualification(SpriteBatch spriteBatch, int dwidth, int dheight, Texture2D P1Tex, Texture2D P2Tex, double player1disqtime, double player2disqtime)
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
                            case 0: y1 = p1DispGraph[index] - 10; y2 = p1VeloGraph[index] - 10; y3 = p1AccGraph[index] - 10; texture = P1Tex; break;
                            case 1: y1 = p2DispGraph[index] - 10; y2 = p2VeloGraph[index] - 10; y3 = p2AccGraph[index] - 10; texture = P2Tex; break;
                        }
                        xDP1 = new CountDown(texture, dwidth, dheight, r3, y1, 20, 20);
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
        /// The function OptimumConstantVelocity derives the optimum values for the "constantVelocity" command, by fixing the optimum player's
        /// velocity and deriving the optimum displacement and acceleration.
        /// </summary>
        /// <param name="size">The number of frames assigned to the current command.</param>        
        /// <returns>void</returns>
        public void OptimumConstantVelocity(int size)
        {
            float velocity = previousVelo;
            float x = previousDisp - velocity;
            for (int i = 0; i <= size - 1; i++)
            {
                if (x >= 0)
                {
                    optimumVelocity.Add(velocity);
                    optimumDisplacement.Add(x);
                }
                else
                {
                    optimumVelocity.Add(0);
                    optimumDisplacement.Add(0);
                }
                optimumAcceleration.Add(0);
                x = x - velocity;
            }
            this.previousDisp = optimumDisplacement[optimumDisplacement.Count - 1];
            this.previousVelo = optimumVelocity[optimumVelocity.Count - 1];
            this.previousAcc = 0;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantAcceleration derives the optimum values for the "constantAcceleration" command
        /// by either fixing the optimum player's acceleration or giving the optimum player an acceleration value and
        /// fixing the value then deriving the optimum displacement and velocity values.
        /// </summary>
        /// <param name="size">The number of frames assigned to the current command.</param>        
        /// <returns>void</returns>
        public void OptimumConstantAcceleration(int size)
        {
            float acceleration = previousAcc;
            if (acceleration == 0)
            {
                acceleration = 160;
            }
            float accumulator = previousVelo;
            float z = previousVelo + acceleration;
            float x = previousDisp - z;
            for (int i = 0; i <= size - 1; i++)
            {
                if (x >= 0)
                {
                    optimumAcceleration.Add(acceleration);
                    optimumVelocity.Add(z);
                    optimumDisplacement.Add(x);
                }
                else
                {
                    optimumAcceleration.Add(0);
                    optimumVelocity.Add(0);
                    optimumDisplacement.Add(0);
                }
                z = z + acceleration;
                x = x - z;
            }
            this.previousAcc = optimumAcceleration[optimumAcceleration.Count - 1];
            this.previousVelo = optimumVelocity[optimumVelocity.Count - 1];
            this.previousDisp = optimumDisplacement[optimumDisplacement.Count - 1];
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumIncreasingAcceleration derives the optimum values for the "increasingAcceleration" command,
        /// by comparing between the average acceleration differences for each player during the race and choosing the higher 
        /// average value as an optimum value to be added to the optimum player's acceleration, or taking a player's average acceleration difference if the other player was disqualified 
        /// during the command's time slice or assigning fixed values as optimum values if both have been disqualified during the command's 
        /// time slice.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="accelerationTest1">A list representing Player 1's acceleration during the race.</param>
        /// <param name="accelerationTest2">A list representing Player 2's acceleration during the race.</param>
        /// <returns>void</returns>
        public void OptimumIncreasingAcceleration(int disq1, int disq2, int start, int end, List<float> accelerationTest1, List<float> accelerationTest2)
        {
            float totalStep1 = GetTotalStep(accelerationTest1);
            float totalStep2 = GetTotalStep(accelerationTest2);
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
                value = 160;
            }
            if (value == 0)
            {
                value = 160;
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
                    optimumAcceleration.Add((float)adder);
                    optimumDisplacement.Add(z);
                    optimumVelocity.Add(velocityTest[i]);
                }
                else
                {
                    optimumAcceleration.Add(0);
                    optimumDisplacement.Add(0);
                    optimumVelocity.Add(0);
                }
                adder += (float)value;
                accumulator = z;
            }
            this.previousAcc = optimumAcceleration[optimumAcceleration.Count - 1];
            this.previousVelo = optimumVelocity[optimumVelocity.Count - 1];
            this.previousDisp = optimumDisplacement[optimumDisplacement.Count - 1];
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumDeccreasingAcceleration derives the optimum values for the "decreasingAcceleration" command,
        /// by comparing between the average acceleration differences for each player during the race and choosing the smaller 
        /// average value as an optimum value to be subtracted from the optimum player's acceleration, or taking a player's average acceleration difference if the other player was disqualified 
        /// during the command's time slice or assigning fixed values as optimum values if both have been disqualified during the command's 
        /// time slice.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="accelerationTest1">A list representing Player 1's acceleration during the race.</param>
        /// <param name="accelerationTest2">A list representing Player 2's acceleration during the race.</param>
        /// <returns>void</returns>
        public void OptimumDecreasingAcceleration(int disq1, int disq2, int start, int end, List<float> accelerationTest1, List<float> accelerationTest2)
        {
            float totalStep1 = GetTotalStep(accelerationTest1);
            float totalStep2 = GetTotalStep(accelerationTest2);
            double a = (double)((double)totalStep1 / (double)accelerationTest1.Count);
            double b = (double)((double)totalStep2 / (double)accelerationTest2.Count);
            double value = 0;
            if (!LiesInBetween(disq1, start, end - 1) && !LiesInBetween(disq2, start, end - 1))
            {
                if (a < b)
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
                    optimumAcceleration.Add(accelerationTrial[i]);
                    optimumDisplacement.Add(z);
                    optimumVelocity.Add(velocityTest[i]);
                }
                else
                {
                    optimumAcceleration.Add(0);
                    optimumDisplacement.Add(0);
                    optimumVelocity.Add(0);
                }

                accumulator = z;
            }
            this.previousAcc = optimumAcceleration[optimumAcceleration.Count - 1];
            this.previousVelo = optimumVelocity[optimumVelocity.Count - 1];
            this.previousDisp = optimumDisplacement[optimumDisplacement.Count - 1];
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantDisplacement derives the optimum values for the "constantDisplacement" command, by adding
        /// 0 as an optimum value for acceleration and velocity and fixing the displacement from the kinect considering the fixed displacement
        /// as an optimum displacement.
        /// </summary>
        /// <param name="size">The number of frames assigned to the current command.</param>
        /// <returns>void</returns>
        public void OptimumConstantDisplacement(int size)
        {
            for (int k = 0; k <= size - 1; k++)
            {
                optimumDisplacement.Add(this.previousDisp);
                optimumVelocity.Add(0);
                optimumAcceleration.Add(0);
            }
            this.previousDisp = optimumDisplacement[optimumDisplacement.Count - 1];
            this.previousVelo = 0;
            this.previousAcc = 0;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 26/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetTotalStep calculates the cummulative acceleration difference for the given acceleration list.
        /// </summary>
        /// <param name="l1">A list holding a player's acceleration values</param>
        /// <returns>float: The total calculated step</returns>
        public float GetTotalStep(List<float> l1)
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
        /// The GetOptimum funciton is used to derive the optimum accelerations/velocities/displacements during the race by calling the necessary functions.
        /// </summary>
        /// <param name="player1disq">The instance when player 1 was disqualified.</param>
        /// <param name="player2disq">The instance when player 2 was disqualified.</param>
        /// <returns>void</returns>
        public void GetOptimum(double player1disq, double player2disq)
        {
            int disq1 = (int)(player1disq * 12);//index of disq frame
            int disq2 = (int)(player2disq * 12);//index of disq frame
            int start = 0;
            int end = 0;
            previousDisp = 4000;
            previousVelo = 0;
            previousAcc = 0;
            for (int i = 0; i <= timeSpaces.Count - 1; i++)
            {
                List<float> tempList = new List<float>();
                end = start + (int)(timeSpaces[i] * 12);
                List<float> velocityTest1 = new List<float>();
                List<float> velocityTest2 = new List<float>();
                List<float> accelerationTest1 = new List<float>();
                List<float> accelerationTest2 = new List<float>();
                for (int j = start; j <= end - 1; j++)
                {
                    velocityTest1.Add(player1Velocity[j]);
                    velocityTest2.Add(player2Velocity[j]);
                    accelerationTest1.Add(player1Acceleration[j]);
                    accelerationTest2.Add(player2Acceleration[j]);
                }
                if (commandsList[i].Equals("constantVelocity"))
                {
                    int x = end - start;
                    OptimumConstantVelocity(x);
                }
                if (commandsList[i].Equals("constantAcceleration"))
                {
                    int x = end - start;
                    OptimumConstantAcceleration(x);
                }
                if (commandsList[i].Equals("increasingAcceleration"))
                {
                    OptimumIncreasingAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2);
                }
                if (commandsList[i].Equals("decreasingAcceleration"))
                {
                    OptimumDecreasingAcceleration(disq1, disq2, start, end, accelerationTest1, accelerationTest2);
                }
                if (commandsList[i].Equals("constantDisplacement"))
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
