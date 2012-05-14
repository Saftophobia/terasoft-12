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
        private Vector2 point1;
        private Vector2 point2;
        private Color curveColor;
        private List<float> player1Displacement;
        private List<float> player2Displacement;
        private List<float> player1Velocity;
        private List<float> player2Velocity;
        private List<float> player1Acceleration;
        private List<float> player2Acceleration;
        private List<float> optimumDisplacement;
        private List<float> optimumVelocity;
        private List<float> optimumAcceleration;
        private List<String> commandsList;
        private List<double> timeSpaces;
        private double totalTime;
        private int[] chosenTimings;
        private int samples;
        private int distance;
        private float[,] chosen;
        private PerformanceGraph[] disp1;
        private PerformanceGraph[] disp2;
        private PerformanceGraph[] velo1;
        private PerformanceGraph[] velo2;
        private PerformanceGraph[] acc1;
        private PerformanceGraph[] acc2;
        private PerformanceGraph[] optD;
        private PerformanceGraph[] optV;
        private PerformanceGraph[] optA;
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
        private float previousDisp;
        private float previousVelo;
        private float previousAcc;
        private double player1Win;
        private double player2Win;
        private double player3Win;
        private int trackLength;

        public PerformanceGraph(int start1, int start2, int finishx, int finishy, int a, int b, Color col)
        {
            point1.X = start1;
            point1.Y = start2;
            point2.X = finishx;
            point2.Y = finishy;
            curveColor = col;
            stageWidth = a;
            stageHeight = b;
        }

        public PerformanceGraph()
        {

        }
        
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Initialize determines the optimal number of points to be represented on the graph.
        /// </summary>
        /// <param name ="g">An instance of the PerformanceGraph.</param>        
        /// <returns>void</returns>
        public static void Initialize(PerformanceGraph g)
        {
            if (g.getTotalTime() <= 1)
            {
                g.setSamples(8);
            }
            if (g.getTotalTime() > 1 && g.getTotalTime() <= 2)
            {
                 g.setSamples(16);
            }
            if (g.getTotalTime() > 2 && g.getTotalTime() <= 5)
            {
                 g.setSamples(32);
            }
            if (g.getTotalTime() > 5 && g.getTotalTime() <= 10)
            {
                 g.setSamples(64);
            }
            if (g.getTotalTime() > 10 && g.getTotalTime() <= 21)
            {
                 g.setSamples(128);
            }
            if (g.getTotalTime() > 21)
            {
                 g.setSamples(256);
            }
            int x = g.getSamples();
            switch (x)
            {
                case 256: g.setDistance(1); break;
                case 128: g.setDistance(2); break;
                case 64: g.setDistance(4); break;
                case 32: g.setDistance(8); break;
                case 16: g.setDistance(16); break;
                case 8: g.setDistance(32); break;
                case 4: g.setDistance(64); break;
            }            
            g.setChosen(9, x + 1);
            g.setDisp1(x);
            g.setDisp2(x);
            g.setVel1(x);
            g.setVel2(x);
            g.setAcc1(x);
            g.setAcc2(x);
            g.setOptimumD(x);
            g.setOptimumV(x);
            g.setOptimumA(x);
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
            DrawLine(spriteBatch, blank, 2, curveColor, new Vector2(point1.X, point1.Y), new Vector2(point2.X, point2.Y));
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
        public void DrawGraphs(PerformanceGraph g, List<float> D1, List<float> D2, List<String> Commands, List<double> time, double player1disqtime, double player2disqtime, int gwidth, int gheight,int length)
        {
            this.trackLength = length;
            player1Displacement = new List<float>();
            player2Displacement = new List<float>();
            optimumDisplacement = new List<float>();
            optimumVelocity = new List<float>();
            optimumAcceleration = new List<float>();
            FixDifference(D1, D2, g);
            this.commandsList = Commands;
            this.timeSpaces = time;
            player1Velocity = GetPlayerVelocity(player1Displacement);
            player2Velocity = GetPlayerVelocity(player2Displacement);
            player1Acceleration = GetPlayerAcceleration(player1Velocity);
            player2Acceleration = GetPlayerAcceleration(player2Velocity);
            OptimumEngine.GetOptimum((double)player1disqtime, (double)player2disqtime,g);
            Discard(g);
            SetNewTime(time, Commands,g);
            GetWinning(g);
            CalculateTotalTime(g);
            Initialize(g);
            Choose(g);
            SetMaximum();
            SetDestinations(gwidth, gheight);
            SetAxis();
        }  

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function FixDifference aims to avoid any exceptions resulted from capturing more or less frames than the expected number of frames per second.
        /// </summary>
        /// <param name="D1">A list representing Player 1's displacement.</param>
        /// <param name="D2">A list representing Player 2's displacement</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void.</returns>
        public static void FixDifference(List<float> D1, List<float> D2, PerformanceGraph g)
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
                g.getP1Disp().Add(D1[i]);
                g.getP1Disp().Add(D2[i]);
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Discard drops the frames received after the instance when all the players including the optimum player have reached the finish line.
        /// </summary>
        /// <returns>void.</returns>
        public static void Discard(PerformanceGraph g)
        {
            Boolean found = false;
            List<float> temp1 = new List<float>();
            List<float> temp2 = new List<float>();
            List<float> temp3 = new List<float>();
            for (int i = 0; i <= g.getP1Disp().Count - 1; i++)
            {
                if (!found)
                {
                    temp1.Add(g.getP1Disp()[i]);
                    temp2.Add(g.getP2Disp()[i]);
                    temp3.Add(g.getOptD()[i]);
                    if (g.getP1Disp()[i] == 0 && g.getP2Disp()[i] == 0 && g.getOptD()[i] == 0)
                    {
                        found = true;
                    }
                }
            }
            g.setP1Disp(temp1);
            g.setP2Disp(temp2);
            g.setOpdD(temp3);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetNewTime sets the race time to be equal to the total time elapsed by the players to reach the finish line by discarding any additional Commands.
        /// </summary>
        /// <param name="time">A list representing each command's time slice</param>
        /// <param name="Commands">A list representing each command given during the race</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void.</returns>
        public static void SetNewTime(List<double> time, List<string> Commands, PerformanceGraph g)
        {
            int newSize = g.getP1Disp().Count;
            double newTime = ((double)newSize / (double)12);
            Boolean t = false;
            int count = 0;
            double acc = 0;
            g.clearTimeSpaces();
            g.clearCommands();
            while (!t)
            {
                if (acc + time[count] <= newTime)
                {
                    g.getTimeSpaces().Add(time[count]);
                    g.getCommands().Add(Commands[count]);
                }
                else
                {
                    double idealNumber = newTime - acc;
                    g.getTimeSpaces().Add(idealNumber);
                    g.getCommands().Add(Commands[count]);
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
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void.</returns>
        public static void GetWinning(PerformanceGraph g)
        {
            Boolean found1 = false;
            Boolean found2 = false;
            Boolean found3 = false;
            for (int i = 0; i <= g.getP1Disp().Count - 1; i++)
            {
                if (!found1)
                {
                    if (g.getP1Disp()[i] == 0)
                    {
                        g.setWin1((double)i / (double)12);
                        found1 = true;
                    }
                }
                if (!found2)
                {
                    if (g.getP2Disp()[i] == 0)
                    {
                        g.setWin2((double)i / (double)12);
                        found2 = true;
                    }
                }
                if (!found3)
                {
                    if (g.getOptD()[i] == 0)
                    {
                        g.setWin3((double)i / (double)12);
                        found3 = true;
                    }
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function CalculateTotalTime calculates the total race time.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void.</returns>
        public static void CalculateTotalTime(PerformanceGraph g)
        {
            double accumulator = 0;
            for (int i = 0; i <= g.getTimeSpaces().Count - 1; i++)
            {
                accumulator += g.getTimeSpaces()[i];
            }
            g.setTotalTime(accumulator);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Choose is used to choose a certain number of samples from the Lists in order to represent them on the graph.
        /// </summary>
        /// <param name ="g">An instance of the PerformanceGraph.</param>         
        /// <returns>void</returns>
        public static void Choose(PerformanceGraph g)
        {
            g.setChosenTimings(g.getSamples() + 1);
            int timeCounter = 0; 
            for (int i = 0; i <= g.getChosenTimings().Length - 1; i++)
            {
                g.getChosenTimings()[i] = (int)(12 * g.getTotalTime() * ((double)timeCounter / (double)g.getSamples()));
                timeCounter++;
            }
            int u = 0;
            for (int i = 0; i <= g.getSamples(); i++)
            {
                if (i > 0 && g.getChosenTimings()[i] - 1 >= 0)
                {
                    u = g.getChosenTimings()[i] - 1;
                }
                else
                {
                    u = g.getChosenTimings()[i];
                }
                g.setChosenGraph(0, i, g.getP1Disp()[u]);
                g.setChosenGraph(1, i, g.getP2Disp()[u]);
                g.setChosenGraph(2, i, g.getP1Vel()[u]);
                g.setChosenGraph(3, i, g.getP2Vel()[u]);
                g.setChosenGraph(4, i, g.getP1Acc()[u]);
                g.setChosenGraph(5, i, g.getP2Acc()[u]);
                g.setChosenGraph(6, i, g.getOptD()[u]);
                g.setChosenGraph(7, i, g.getOptV()[u]);
                g.setChosenGraph(8, i, g.getOptA()[u]);
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetMaximum is used to derive the maximum velocity and the maximum acceleration of both players and the optimum player during 
        /// the race.
        /// </summary>
        /// <param></param>        
        /// <returns>void</returns>
        public void SetMaximum()
        {
            maxVelocity = 0;
            maxAcceleration = 0;
            int p1WinningFrame = (int)(12 * player1Win);
            int p2WinningFrame = (int)(12 * player2Win);
            int p3WinningFrame = (int)(12 * player3Win);
            for (int i = 0; i <= samples; i++)
            {
                float[] velocity = new float[3];
                float[] acceleration = new float[3];
                velocity[0] = chosen[2, i];
                velocity[1] = chosen[3, i];
                velocity[2] = chosen[7, i];
                acceleration[0] = chosen[4, i];
                acceleration[1] = chosen[5, i];
                acceleration[2] = chosen[8, i];
                for (int j = 0; j <= 2; j++)
                {
                    if (velocity[j] < 0)
                    {
                        velocity[j] *= -1;
                    }
                    if (acceleration[j] < 0)
                    {
                        acceleration[j] *= -1;
                    }
                    if (velocity[j] > maxVelocity)
                    {
                        if (j == 0 && chosenTimings[i] <= p1WinningFrame)
                        {
                            maxVelocity = velocity[j];
                        }
                        if (j == 1 && chosenTimings[i] <= p2WinningFrame)
                        {
                            maxVelocity = velocity[j];
                        }
                        if (j == 2 && chosenTimings[i] <= p3WinningFrame)
                        {
                            maxVelocity = velocity[j];
                        }
                    }
                    if (acceleration[j] > maxAcceleration)
                    {
                        if (j == 0 && chosenTimings[i] <= p1WinningFrame)
                        {
                            maxAcceleration = acceleration[j];
                        }
                        if (j == 1 && chosenTimings[i] <= p2WinningFrame)
                        {
                            maxAcceleration = acceleration[j];
                        }
                        if (j == 2 && chosenTimings[i] <= p3WinningFrame)
                        {
                            maxAcceleration = acceleration[j];
                        }
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
        /// The function GetChosenArray returnes the required array given its index.
        /// </summary>
        /// <param name="x">The index of the required array</param>        
        /// <returns>void</returns>
        public float[] GetChosenArray(int x)
        {
            float[] temporary = new float[samples + 1];
            for (int i = 0; i <= temporary.Length - 1; i++)
            {
                temporary[i] = chosen[x, i];
            }
            return temporary;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 13/5/2012</para>
        /// <para>Date Modified 13/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetTotalLinesNeeded returnes the optimal number of lines to be drawn on the graph.
        /// </summary>
        /// <param name="player">The required player's number.</param>        
        /// <returns>void</returns>
        public int GetTotalLinesNeeded(int player)
        {
            int x = samples;
            double accumulator = 0;
            double temp = 0;
            switch (player)
            {
                case 1: temp = player1Win; break;
                case 2: temp = player2Win; break;
                case 3: temp = player3Win; break;
            }
            Boolean found = false;
            for (int i = 0; i <= samples - 1; i++)
            {
                accumulator += ((double)distance / (double)256) * ((double)(totalTime));
                if (!found)
                {
                    if (accumulator >= temp)
                    {
                        x = i;
                        found = true;
                    }
                }
            }
            return x;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetDestinations is used to create instances of the PerformanceGraph class for each graph giving
        /// it the initial and final points to be connected in order to represent each graph by connecting the points.
        /// </summary>
        /// <param name="Width">The width of the screen.</param>
        /// <param name="Height">The height of the screen.</param>
        /// <returns>void</returns>
        public void SetDestinations(int Width, int Height)
        {
            int Lines1 = GetTotalLinesNeeded(1) - 1;
            int Lines2 = GetTotalLinesNeeded(2) - 1;
            int Lines3 = GetTotalLinesNeeded(3) - 1;
            int counter1 = 0;
            float value = 0;
            double r = 0;
            for (int j = 0; j <= 8; j++)
            {
                PerformanceGraph[] current = new PerformanceGraph[16];
                float[] temporary = new float[17];
                List<int> disqList = new List<int>();
                int index = 0;
                Color color = new Color();
                int player = 0;
                switch (j)
                {
                    case 0: player = 1; counter1 = 50; value = trackLength; current = disp1; color = Color.Red; index = 0; disqList = p1DispGraph; break;
                    case 1: player = 2; counter1 = 50; value = trackLength; current = disp2; color = Color.Blue; index = 1; disqList = p2DispGraph; break;
                    case 2: player = 3; counter1 = 50; value = trackLength; current = optD; color = Color.Yellow; index = 6; break;
                    case 3: player = 1; counter1 = 380; value = maxVelocity; current = velo1; color = Color.Red; index = 2; disqList = p1VeloGraph; break;
                    case 4: player = 2; counter1 = 380; value = maxVelocity; current = velo2; color = Color.Blue; index = 3; disqList = p2VeloGraph; break;
                    case 5: player = 3; counter1 = 380; value = maxVelocity; current = optV; color = Color.Yellow; index = 7; break;
                    case 6: player = 1; counter1 = 710; value = maxAcceleration; current = acc1; color = Color.Red; index = 4; disqList = p1AccGraph; break;
                    case 7: player = 2; counter1 = 710; value = maxAcceleration; current = acc2; color = Color.Blue; index = 5; disqList = p2AccGraph; break;
                    case 8: player = 3; counter1 = 710; value = maxAcceleration; current = optA; color = Color.Yellow; index = 8; break;
                }
                temporary = GetChosenArray(index);
                r = (double)value / (double)232;
                for (int i = 0; i <= samples - 1; i++)
                {
                    int a1 = 68;
                    int a2 = 68;
                    if (j > 2)
                    {
                        if (temporary[i] < 0)
                        {
                            a1 = 84;
                        }
                        if (temporary[i + 1] < 0)
                        {
                            a2 = 84;
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
                    current[i] = new PerformanceGraph(counter1, r3 - 1, counter1 + distance, r5 - 1, Width,
                        Height, color);
                    if (((player == 1 && i > Lines1) || (player == 2 && i > Lines2) || (player == 3 && i > Lines3)) && j > 2)
                    {
                        current[i] = new PerformanceGraph(0, 0, 0, 0, 0,
                        Height, color);
                    }
                    counter1 = counter1 + distance;
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
        
        public List<float> getP1Disp()
        {
            return player1Displacement;
        }
        public List<float> getP2Disp()
        {
            return player2Displacement;
        }
        public double getTotalTime()
        {
            return totalTime;
        }
        public void setTotalTime(double value)
        {
            totalTime = value;
        }
        public int[] getChosen()
        {
            return chosenTimings;
        }
        public List<int> getP1DispGraph()
        {
            return p1DispGraph;
        }
        public List<int> getP2DispGraph()
        {
            return p2DispGraph;
        }
        public List<int> getP1VelGraph()
        {
            return p1VeloGraph;
        }
        public List<int> getP2VelGraph()
        {
            return p2VeloGraph;
        }
        public List<int> getP1AccGraph()
        {
            return p1AccGraph;
        }
        public List<int> getP2AccGraph()
        {
            return p2AccGraph;
        }
        public PerformanceGraph[] getDisplacement1()
        {
            return disp1;
        }
        public PerformanceGraph[] getDisplacement2()
        {
            return disp2;
        }
        public PerformanceGraph[] getVelocity1()
        {
            return velo1;
        }
        public PerformanceGraph[] getVelocity2()
        {
            return velo2;
        }
        public PerformanceGraph[] getAcceleration1()
        {
            return acc1;
        }
        public PerformanceGraph[] getAcceleration2()
        {
            return acc2;
        }
        public PerformanceGraph[] getOptimumDisplacement()
        {
            return optD;
        }
        public PerformanceGraph[] getOptimumVelocity()
        {
            return optV;
        }
        public PerformanceGraph[] getOptimumAcceleration()
        {
            return optA;
        }
        public double[] GetXAxis()
        {
            return xAxis;
        }
        public double[] YAxisDis()
        {
            return yAxisDisplacement;
        }
        public double[] YAxisVel()
        {
            return yAxisVelocity;
        }
        public double[] YAxisAcc()
        {
            return yAxisAcceleration;
        }
        public float getPreviousD()
        {
            return previousDisp;
        }
        public float getPreviousV()
        {
            return previousVelo;
        }
        public float getPreviousA()
        {
            return previousAcc;
        }
        public List<float> getOptD()
        {
            return optimumDisplacement;
        }
        public List<float> getOptV()
        {
            return optimumVelocity;
        }
        public List<float> getOptA()
        {
            return optimumAcceleration;
        }
        public void setPreviousD(float value)
        {
            this.previousDisp = value;
        }
        public void setPreviousV(float value)
        {
            this.previousVelo = value;
        }
        public void setPreviousA(float value)
        {
            this.previousAcc = value;
        }
        public List<string> getCommands()
        {
            return commandsList;
        }
        public List<double> getTimeSpaces()
        {
            return timeSpaces;
        }
        public List<float> getP1Vel()
        {
            return player1Velocity;
        }
        public List<float> getP2Vel()
        {
            return player2Velocity;
        }
        public List<float> getP1Acc()
        {
            return player1Acceleration;
        }
        public List<float> getP2Acc()
        {
            return player2Acceleration;
        }
        public int getTrackLength()
        {
            return trackLength;
        }
        public void setP1Disp(List<float> L)
        {
            this.player1Displacement = L;
        }
        public void setP2Disp(List<float> L)
        {
            this.player2Displacement = L;
        }
        public void setP1Vel(List<float> L)
        {
            this.player1Velocity = L;
        }
        public void setP2Vel(List<float> L)
        {
            this.player2Velocity = L;
        }
        public void setP1Acc(List<float> L)
        {
            this.player1Acceleration = L;
        }
        public void setP2Acc(List<float> L)
        {
            this.player2Acceleration = L;
        }
        public void setOpdD(List<float> L)
        {
            this.optimumDisplacement = L;
        }
        public void clearTimeSpaces()
        {
            timeSpaces = new List<double>();
        }
        public void clearCommands()
        {
            commandsList = new List<string>();
        }
        public void setWin1(double d)
        {
            player1Win = d;
        }
        public void setWin2(double d)
        {
            player2Win = d;
        }
        public void setWin3(double d)
        {
            player3Win = d;
        }
        public int getSamples()
        {
            return samples;
        }
        public void setSamples(int x)
        {
            samples = x;
        }
        public void setDistance(int x)
        {
            distance = x;
        }
        public int getDistance()
        {
            return distance;
        }
        public void setChosen(int a, int b)
        {
            chosen = new float[a, b];
        }
        public void setDisp1(int x)
        {
            disp1 = new PerformanceGraph[x];
        }
        public void setDisp2(int x)
        {
            disp2 = new PerformanceGraph[x];
        }
        public void setVel1(int x)
        {
            velo1 = new PerformanceGraph[x];
        }
        public void setVel2(int x)
        {
            velo2 = new PerformanceGraph[x];
        }
        public void setAcc1(int x)
        {
            acc1 = new PerformanceGraph[x];
        }
        public void setAcc2(int x)
        {
            acc2 = new PerformanceGraph[x];
        }
        public void setOptimumD(int x)
        {
            optD = new PerformanceGraph[x];
        }
        public void setOptimumV(int x)
        {
            optV = new PerformanceGraph[x];
        }
        public void setOptimumA(int x)
        {
            optA = new PerformanceGraph[x];
        }
        public void setChosenTimings(int x)
        {
            chosenTimings = new int[x];
        }
        public int[] getChosenTimings()
        {
            return chosenTimings;
        }
        public void setChosenGraph(int a, int b, float f)
        {
            chosen[a, b] = f;
        }
    }
}
