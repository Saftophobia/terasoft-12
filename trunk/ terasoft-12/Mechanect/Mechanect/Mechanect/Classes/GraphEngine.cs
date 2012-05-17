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

namespace Mechanect.Classes
{
    class GraphEngine
    {
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 18/4/2012</para>
        /// <para>Date Modified 19/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The static method GetPlayerVelocity is used to generate a List representing the player's velocity during the race. 
        /// </summary>
        /// <param name="DisplacementList"> A List representing the player's displacements during the race.</param>      
        /// <returns>List: returns a list representing the player's velocity.</returns>
        public static List<float> GetPlayerVelocity(List<float> DisplacementList)
        {
            int size = DisplacementList.Count;
            List<float> result = new List<float>();
            for (int i = 0; i <= size - 1; i++)
            {
                try
                {
                    double dt = ((double)1 / (double)24);
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
        /// The static method GetPlayerAcceleration is used to generate a List representing the player's acceleration during the race. 
        /// </summary>
        /// <param name="VelocityList">A list representing the player's velocities during the race.</param>    
        /// <returns>List: returns a list representing the player's acceleration.</returns>
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
        /// The function DrawGraphs calls the necessary functions to derive each player's velocity and acceleration as well as the optimum values, in addition, the function also calls the necessary functions required to draw the curve.
        /// </summary>
        /// <param name="D1">A list holding Player 1's displacements.</param>
        /// <param name="D2">A list holding Player 2's displacements.</param>
        /// <param name="Commands">A list holding each command initiated during the race.</param>
        /// <param name="time">A list holding the time elapsed by each command.</param>
        /// <param name="player1disqtime">The instance when the first player was disqualified.</param>    
        /// <param name="player2disqtime">The instance when the second player was disqualified.</param>  
        /// <param name="gwidth">The width of the screen.</param>
        /// <param name="gheight">The height of the screen.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void DrawGraphs(PerformanceGraph g, List<float> D1, List<float> D2, List<String> Commands, List<double> time, double player1disqtime, double player2disqtime, int gwidth, int gheight, int length)
        {
            g.setTrackLength(length);
            FixDifference(D1, D2, g);
            g.setCommandsList(Commands);
            g.setTimeSlices(time);
            g.setVel1(GetPlayerVelocity(g.getP1Disp()));
            g.setVel2(GetPlayerVelocity(g.getP2Disp()));
            g.setAcc1(GetPlayerAcceleration(g.getP1Vel()));
            g.setAcc2(GetPlayerAcceleration(g.getP2Vel()));
            OptimumEngine.GetOptimum((double)player1disqtime, (double)player2disqtime,g);
            Discard(g);
            SetNewTime(time, Commands, g);
            GetWinning(g);
            CalculateTotalTime(g);
            Initialize(g);
            Choose(g);
            SetMaximum(g);
            SetDestinations(g, gwidth, gheight);
            SetAxis(g);
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
                g.getP2Disp().Add(D2[i]);
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
            double newTime = ((double)newSize / (double)24);
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
                        g.setWin1((double)i / (double)24);
                        found1 = true;
                    }
                }
                if (!found2)
                {
                    if (g.getP2Disp()[i] == 0)
                    {
                        g.setWin2((double)i / (double)24);
                        found2 = true;
                    }
                }
                if (!found3)
                {
                    if (g.getOptD()[i] == 0)
                    {
                        g.setWin3((double)i / (double)24);
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
        /// <para>Date Modified 14/5/2012</para>
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
                g.getChosenTimings()[i] = (int)(24 * g.getTotalTime() * ((double)timeCounter / (double)g.getSamples()));
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
        /// The function SetMaximum is used to derive the maximum velocity and the maximum acceleration of both players and the optimum player during the race.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>        
        /// <returns>void</returns>
        public static void SetMaximum(PerformanceGraph g)
        {
            g.setMaxVelocity(0);
            g.setMaxAcceleration(0);
            int p1WinningFrame = (int)(24 * g.getWin1());
            int p2WinningFrame = (int)(24 * g.getWin2());
            int p3WinningFrame = (int)(24 * g.getWin3());
            for (int i = 0; i <= g.getSamples(); i++)
            {
                float[] velocity = new float[3];
                float[] acceleration = new float[3];
                velocity[0] = g.getChosenValue(2, i);
                velocity[1] = g.getChosenValue(3, i);
                velocity[2] = g.getChosenValue(7, i);
                acceleration[0] = g.getChosenValue(4, i);
                acceleration[1] = g.getChosenValue(5, i);
                acceleration[2] = g.getChosenValue(8, i);
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
                    if (velocity[j] > g.getMaxVelocity())
                    {
                        if ((j == 0 && g.getChosenTimings()[i] <= p1WinningFrame) || (j == 1 && g.getChosenTimings()[i] <= p2WinningFrame) || (j == 2 && g.getChosenTimings()[i] <= p3WinningFrame))
                        {
                            g.setMaxVelocity(velocity[j]);
                        }
                    }
                    if (acceleration[j] > g.getMaxAcceleration())
                    {
                        if ((j == 0 && g.getChosenTimings()[i] <= p1WinningFrame) || (j == 1 && g.getChosenTimings()[i] <= p2WinningFrame) || (j == 2 && g.getChosenTimings()[i] <= p3WinningFrame))
                        {
                            g.setMaxAcceleration(acceleration[j]);
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
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <param name="x">The index of the required array</param>        
        /// <returns>void</returns>
        public static float[] GetChosenArray(PerformanceGraph g, int x)
        {
            float[] temporary = new float[g.getSamples() + 1];
            for (int i = 0; i <= temporary.Length - 1; i++)
            {
                temporary[i] = g.getChosenValue(x, i);
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
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <param name="player">The required player's number.</param>        
        /// <returns>void</returns>
        public static int GetTotalLinesNeeded(PerformanceGraph g, int player)
        {
            int x = g.getSamples();
            double accumulator = 0;
            double temp = 0;
            switch (player)
            {
                case 1: temp = g.getWin1(); break;
                case 2: temp = g.getWin2(); break;
                case 3: temp = g.getWin3(); break;
            }
            Boolean found = false;
            for (int i = 0; i <= g.getSamples() - 1; i++)
            {
                accumulator += ((double)g.getDistance() / (double)256) * ((double)(g.getTotalTime()));
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
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void SetDestinations(PerformanceGraph g, int Width, int Height)
        {
            int Lines1 = GetTotalLinesNeeded(g, 1) - 1;
            int Lines2 = GetTotalLinesNeeded(g, 2) - 1;
            int Lines3 = GetTotalLinesNeeded(g, 3) - 1;
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
                    case 0: player = 1; counter1 = 50; value = g.getTrackLength(); current = g.getDisplacement1(); color = Color.Red; index = 0; disqList = g.getP1DispGraph(); break;
                    case 1: player = 2; counter1 = 50; value = g.getTrackLength(); current = g.getDisplacement2(); color = Color.Blue; index = 1; disqList = g.getP2DispGraph(); break;
                    case 2: player = 3; counter1 = 50; value = g.getTrackLength(); current = g.getOptimumDisplacement(); color = Color.Yellow; index = 6; break;
                    case 3: player = 1; counter1 = 380; value = g.getMaxVelocity(); current = g.getVelocity1(); color = Color.Red; index = 2; disqList = g.getP1VelGraph(); break;
                    case 4: player = 2; counter1 = 380; value = g.getMaxVelocity(); current = g.getVelocity2(); color = Color.Blue; index = 3; disqList = g.getP2VelGraph(); break;
                    case 5: player = 3; counter1 = 380; value = g.getMaxVelocity(); current = g.getOptimumVelocity(); color = Color.Yellow; index = 7; break;
                    case 6: player = 1; counter1 = 710; value = g.getMaxAcceleration(); current = g.getAcceleration1(); color = Color.Red; index = 4; disqList = g.getP1AccGraph(); break;
                    case 7: player = 2; counter1 = 710; value = g.getMaxAcceleration(); current = g.getAcceleration2(); color = Color.Blue; index = 5; disqList = g.getP2AccGraph(); break;
                    case 8: player = 3; counter1 = 710; value = g.getMaxAcceleration(); current = g.getOptimumAcceleration(); color = Color.Yellow; index = 8; break;
                }
                temporary = GetChosenArray(g, index);
                r = (double)value / (double)232;
                for (int i = 0; i <= g.getSamples() - 1; i++)
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
                        r2 = (double)(g.getMaxVelocity() - 2 - temporary[i]) / (double)r;
                        r4 = (double)(g.getMaxVelocity() - 2 - temporary[i + 1]) / (double)r;
                    }
                    if (j > 5 && j <= 8)
                    {
                        r2 = (double)(g.getMaxAcceleration() - 2 - temporary[i]) / (double)r;
                        r4 = (double)(g.getMaxAcceleration() - 2 - temporary[i + 1]) / (double)r;
                    }
                    int r3 = a1 + (int)r2;
                    int r5 = a2 + (int)r4;
                    current[i] = new PerformanceGraph(counter1, r3 - 1, counter1 + g.getDistance(), r5 - 1, Width,
                        Height, color);
                    if (((player == 1 && i > Lines1) || (player == 2 && i > Lines2) || (player == 3 && i > Lines3)) && j > 2)
                    {
                        current[i] = new PerformanceGraph(0, 0, 0, 0, 0,
                        Height, color);
                    }
                    counter1 = counter1 + g.getDistance();
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
        /// <para>Date Modified 17/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetAxis chooses 5 evenly distributed values among the total time to be represented on the x-axis
        /// as well as 5 evenly distributed values among the total displacement/velocity/acceleration to be represented on
        /// each graph's y-axis.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void SetAxis(PerformanceGraph g)
        {
            g.setXAxis(0, 0);
            double step = (double)g.getTotalTime() / (double)4;
            for (int i = 1; i <= g.GetXAxis().Length - 1; i++)
            {
                g.setXAxis(i, g.GetXAxis()[i - 1] + step);
            }
            int counter = 0;
            int stepping = (int)(g.getTrackLength() / 4);
            for (int i = 0; i <= 4; i++)
            {
                g.setYAxisDisp(i, (int)(counter / 1000));
                counter += stepping;
            }
            g.setYAxisVel(0, 0);
            step = (int)(((double)g.getMaxVelocity() / (double)4) / 1000);
            for (int i = 1; i <= g.YAxisVel().Length - 1; i++)
            {
                g.setYAxisVel(i, g.YAxisVel()[i - 1] + step);
            }
            g.setYAxisAcc(0, 0);
            step = (int)(((double)g.getMaxAcceleration() / (double)4) / 1000);
            for (int i = 1; i <= g.YAxisAcc().Length - 1; i++)
            {
                g.setYAxisAcc(i, g.YAxisAcc()[i - 1] + step);
            }
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
    }
}
