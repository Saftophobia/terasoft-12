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

namespace Mechanect.Exp1
{
    class GraphEngine
    {
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/5/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Differentiate is used to differentiate a given list.
        /// </summary>
        /// <param name="VelocityList">The list to be differentiated.</param>
        /// <param name="timings">The initial value.</param>
        /// <returns>List: The list representing the differentiated list.</returns>
        public static List<float> Differentiate(List<float> VelocityList, List<double> timings)
        {
            int size = VelocityList.Count;
            List<float> result = new List<float>();
            for (int i = 0; i <= size - 1; i++)
            {
                try
                {
                    float currentAcceleration = (float)((VelocityList[i] - VelocityList[i - 1]) / timings[i]);
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
        /// <para>Date Written 22/5/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetTimings is used to detect the instances of time when the velocities were captured.
        /// </summary>
        /// <param name="time">The list containing the cummulative timings.</param>
        /// <returns>List: The list representing required instances of time.</returns>
        public static List<double> GetTimings(List<double> time)
        {
            double initial = 0;
            List<double> res = new List<double>();
            for (int i = 0; i <= time.Count - 1; i++)
            {
                res.Add(time[i] - initial);
                initial = time[i];
            }
            return res;
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 18/4/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawGraphs calls the necessary functions to derive each player's velocity and acceleration as well as the optimum values, in addition, the function also calls the necessary functions required to draw the curve.
        /// </summary>
        /// <param name="vel1">A list holding Player 1's Velocities.</param>
        /// <param name="D2">A list holding Player 2's Velocities.</param>
        /// <param name="Commands">A list holding each command initiated during the race.</param>
        /// <param name="time">A list holding the time elapsed by each command.</param>
        /// <param name="player1disqtime">The instance when the first player was disqualified.</param>    
        /// <param name="player2disqtime">The instance when the second player was disqualified.</param>  
        /// <param name="gwidth">The width of the screen.</param>
        /// <param name="gheight">The height of the screen.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <param name="totaltime">The total race time.</param>
        /// <param name="timings1">A list representing the moments where the velocities were captured for player 1.</param>
        /// <param name="timings2">A list representing the moments where the velocities were captured for player 2.</param>
        /// <returns>void</returns>
        public static void DrawGraphs(PerformanceGraph g, List<float> vel1, List<double> timings1, List<float> vel2, List<double> timings2, List<String> Commands, List<double> time, double player1disqtime, double player2disqtime, int gwidth, int gheight, int length)
        {
            g.setTrackLength(length);
            g.setVel1(vel1);
            g.setVel2(vel2);
            g.setAcc1(Differentiate(g.getP1Vel(), timings1));
            g.setAcc2(Differentiate(g.getP2Vel(), timings2));
            g.setP1Disp(Integral(g,vel1, timings1));
            g.setP2Disp(Integral(g,vel2, timings2));
            g.SetTimings1(timings1);
            g.SetTimings2(timings2);
            g.setTrackLength(length);
            g.setCommandsList(Commands);
            g.setTimeSlices(time);
            OptimumEngine.GetOptimum(1,g);
            g.setTotalTime(GetTotalTime(g));
            ReflexOptimum(g);
            SetMaximum(g);
            g.setDisp1(g.GetTimings1().Count);
            g.setDisp2(g.GetTimings2().Count);
            g.setVel1(g.GetTimings1().Count);
            g.setVel2(g.GetTimings2().Count);
            g.setAcc1(g.GetTimings1().Count);
            g.setAcc2(g.GetTimings2().Count);
            g.setOptimumD(g.getOptD().Count);
            g.setOptimumV(g.getOptV().Count);
            g.setOptimumA(g.getOptA().Count);
            SetDestinations(g, gwidth, gheight);
            SetAxis(g);
        }



        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 23/5/2012</para>
        /// <para>Date Modified 23/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetTotalTime is used to deduce the total race time.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>List: The list representing the differentiated list.</returns>
        public static double GetTotalTime(PerformanceGraph g)
        {
            double first = g.GetCum1()[g.GetCum1().Count - 1];
            double second = g.GetCum2()[g.GetCum2().Count - 1];
            double third = g.getOptD().Count;
            if (first >= second && first >= third)
            {
                return first;
            }
            if (second >= first && second >= third)
            {
                return second;
            }
            else
            {
                return third;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/5/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function ReflexOptimum is to get the optimum displacements relative to the user.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void.</returns>
        public static void ReflexOptimum(PerformanceGraph g)
        {
            List<float> temp = new List<float>();
            for (int i = 0; i <= g.getOptD().Count - 1; i++)
            {
                temp.Add(g.getOptD()[i]);
            }
            g.getOptD().Clear();
            for (int i = 0; i <= temp.Count - 1; i++)
            {
                g.getOptD().Add(g.getTrackLength() - temp[i]);
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function SetMaximum is used to derive the maximum velocity and the maximum acceleration of both players and the optimum player during the race.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>        
        /// <returns>void</returns>
        public static void SetMaximum(PerformanceGraph g)
        {
            float maxVel = 0; float maxAcc = 0;
            for (int j = 0; j <= 2; j++)
            {
                List<float> temp1 = new List<float>();
                List<float> temp2 = new List<float>();
                switch (j)
                {
                    case 0: temp1 = g.getP1Vel(); temp2 = g.getP1Acc(); break;
                    case 1: temp1 = g.getP2Vel(); temp2 = g.getP2Acc(); break;
                    case 2: temp1 = g.getOptV(); temp2 = g.getOptA(); break;
                }
                for (int i = 0; i <= temp1.Count - 1; i++)
                {
                    if (temp1[i] > maxVel)
                    {
                        maxVel = temp1[i];
                    }
                    if (temp2[i] > maxAcc)
                    {
                        maxAcc = temp2[i];
                    }
                }
            }
            g.setMaxAcceleration(maxAcc);
            g.setMaxVelocity(maxVel);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 22/5/2012</para>
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
            int counter1 = 0;
            float value = 0;
            double r = 0;
            for (int j = 0; j <= 8; j++)
            {
                PerformanceGraph[] current = new PerformanceGraph[16];
                List<int> disqList = new List<int>();
                Color color = new Color();
                List<float> temporary = new List<float>();
                switch (j)
                {
                    case 0: temporary = g.getP1Disp(); counter1 = GraphUI.Percentage(Width, 4.88); value = g.getTrackLength(); current = g.getDisplacement1(); color = Color.Red; disqList = g.getP1DispGraph(); break;
                    case 1: temporary = g.getP2Disp(); counter1 = GraphUI.Percentage(Width, 4.88); value = g.getTrackLength(); current = g.getDisplacement2(); color = Color.Blue; disqList = g.getP2DispGraph(); break;
                    case 2: temporary = g.getOptD(); counter1 = GraphUI.Percentage(Width, 4.88); value = g.getTrackLength(); current = g.getOptimumDisplacement(); color = Color.Yellow; break;
                    case 3: temporary = g.getP1Vel(); counter1 = GraphUI.Percentage(Width, 37.1); value = g.getMaxVelocity(); current = g.getVelocity1(); color = Color.Red; disqList = g.getP1VelGraph(); break;
                    case 4: temporary = g.getP2Vel(); counter1 = GraphUI.Percentage(Width, 37.1); value = g.getMaxVelocity(); current = g.getVelocity2(); color = Color.Blue; disqList = g.getP2VelGraph(); break;
                    case 5: temporary = g.getOptV(); counter1 = GraphUI.Percentage(Width, 37.1); value = g.getMaxVelocity(); current = g.getOptimumVelocity(); color = Color.Yellow; break;
                    case 6: temporary = g.getP1Acc(); counter1 = GraphUI.Percentage(Width, 69.33); value = g.getMaxAcceleration(); current = g.getAcceleration1(); color = Color.Red; disqList = g.getP1AccGraph(); break;
                    case 7: temporary = g.getP2Acc(); counter1 = GraphUI.Percentage(Width, 69.33); value = g.getMaxAcceleration(); current = g.getAcceleration2(); color = Color.Blue; disqList = g.getP2AccGraph(); break;
                    case 8: temporary = g.getOptA(); counter1 = GraphUI.Percentage(Width, 69.33); value = g.getMaxAcceleration(); current = g.getOptimumAcceleration(); color = Color.Yellow; break;
                }
                r = (double)value / (double)GraphUI.Percentage(Height, 35.69);
                for (int i = 0; i <= temporary.Count - 2; i++)
                {
                    int a1 = GraphUI.Percentage(Height, 10.46);
                    int a2 = GraphUI.Percentage(Height, 10.46);
                    if (j > 2)
                    {
                        if (temporary[i] < 0)
                        {
                            a1 = GraphUI.Percentage(Height, 12.92); 
                        }
                        if (temporary[i + 1] < 0)
                        {
                            a2 = GraphUI.Percentage(Height, 12.92); 
                        }                        
                    }
                    double r2 = 0; double r4 = 0;
                    switch (j)
                    {
                        case 0:
                        case 1:
                        case 2: r2 = (double)(g.getTrackLength() - temporary[i]) / (double)r; r4 = (double)(g.getTrackLength() - temporary[i + 1]) / (double)r; break;
                        case 3:
                        case 4:
                        case 5: r2 = (double)(g.getMaxVelocity() - temporary[i]) / (double)r; r4 = (double)(g.getMaxVelocity() - temporary[i + 1]) / (double)r; break;
                        default: r2 = (double)(g.getMaxAcceleration() - temporary[i]) / (double)r; r4 = (double)(g.getMaxAcceleration() - temporary[i + 1]) / (double)r; break;
                    }
                    int r3 = a1 + (int)r2;
                    int r5 = a2 + (int)r4;
                    int distance = 0;
                    double x = ((double)g.getTotalTime() / (double)GraphUI.Percentage(Width,25));
                    switch (j)
                    {
                       case 2:
                       case 5:
                       case 8: distance = (int)(1/ x); break;
                       case 0:
                       case 3:
                       case 6: distance = (int)((g.GetTimings1()[i+1]) / x); break;
                       default: distance = (int)((g.GetTimings2()[i+1]) / x); break;
                    }                    
                    current[i] = new PerformanceGraph(counter1, r3 - 1, counter1 + distance, r5 - 1, Width, Height, color);
                    counter1 = counter1 + distance;
                    if (j != 2 && j != 5 && j != 8)
                    {
                        if (i == 0)
                        {
                            disqList.Add(r3-1);
                        }
                        disqList.Add(r5-1);
                    }
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 22/5/2012</para>
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
                g.setYAxisDisp(i, (int)(counter));
                counter += stepping;
            }
            g.setYAxisVel(0, 0);
            for (int i = 1; i <= g.YAxisVel().Length - 1; i++)
            {
                g.setYAxisVel(i, g.YAxisVel()[i - 1] + ((double)g.getMaxVelocity() / (double)4));
            }
            g.setYAxisAcc(0, 0);
            for (int i = 1; i <= g.YAxisAcc().Length - 1; i++)
            {
                g.setYAxisAcc(i, g.YAxisAcc()[i - 1] + (((double)g.getMaxAcceleration() / (double)4)));
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/5/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Integral is to get the Displacements out of the Velocities.
        /// </summary>
        /// <param name="x">A list representing the Velocities of the player.</param>
        /// <param name="time">The time differences.</param>
        /// <param name="g">An instance of the Performance graph.</param>
        /// <returns>List: A list representing the Displacements of the player.</returns>
        public static List<float> Integral(PerformanceGraph g, List<float> x, List<double> time)
        {
            List<float> res = new List<float>();
            float acc = 0;
            for (int i = 0; i <= x.Count - 1; i++)
            {
                float a = (float)((x[i] * time[i]) + acc);
                acc = a;
                if (a <= g.getTrackLength())
                {
                    res.Add(a);
                }
                else
                {
                    res.Add(g.getTrackLength());
                }
            }
            return res;
        }
    }
}
