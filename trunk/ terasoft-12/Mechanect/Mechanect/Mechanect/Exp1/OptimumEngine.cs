using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Exp1
{
    class OptimumEngine
    {
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantVelocity derives the optimum values for the "constantVelocity" command.
        /// </summary>
        /// <param name="size">The number of frames assigned to the current command.</param> 
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void OptimumConstantVelocity(int size, PerformanceGraph g)
        {
            float velocity = g.getPreviousV();
            float x = g.getPreviousD() - velocity;
            for (int i = 0; i <= size - 1; i++)
            {
                if (x >= 0)
                {
                    g.getOptV().Add(velocity);
                    g.getOptD().Add(x);
                }
                else
                {
                    g.getOptV().Add(0);
                    g.getOptD().Add(0);
                }
                g.getOptA().Add(0);
                x = x - velocity;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantAcceleration derives the optimum values for the "constantAcceleration" command.
        /// </summary>
        /// <param name="size">The number of frames assigned to the current command.</param>  
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void OptimumConstantAcceleration(int size, PerformanceGraph g)
        {
            float acceleration = g.getPreviousA();
            if (acceleration == 0)
            {
                acceleration = 5;
            }
            float accumulator = g.getPreviousV();
            float z = g.getPreviousV() + acceleration;
            float x = g.getPreviousD() - z;
            for (int i = 0; i <= size - 1; i++)
            {
                if (x >= 0)
                {
                    g.getOptA().Add(acceleration);
                    g.getOptV().Add(z);
                    g.getOptD().Add(x);
                }
                else
                {
                    g.getOptA().Add(0);
                    g.getOptV().Add(0);
                    g.getOptD().Add(0);
                }
                z = z + acceleration;
                x = x - z;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 17/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumIncreasingAcceleration derives the optimum values for the "increasingAcceleration" command.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="accelerationTest1">A list representing Player 1's acceleration during the race.</param>
        /// <param name="accelerationTest2">A list representing Player 2's acceleration during the race.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void OptimumIncreasingAcceleration(int start, int end, List<float> accelerationTest1, List<float> accelerationTest2, PerformanceGraph g)
        {
            double value = 0.6;
            List<float> accelerationTrial = new List<float>();
            List<float> velocityTest = new List<float>();
            float accumulatorAcc = g.getPreviousA();
            float accumulatorVel = g.getPreviousV();
            float accumulatorDis = g.getPreviousD();
            float adder = (float)value;
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z0 = accumulatorAcc + (float)value;
                accelerationTrial.Add(z0);
                float z1 = accumulatorVel + accelerationTrial[i];
                velocityTest.Add(z1);
                float z2 = accumulatorDis - (float)velocityTest[i];
                if (z2 >= 0)
                {
                    g.getOptA().Add((float)adder);
                    g.getOptD().Add(z2);
                    g.getOptV().Add(velocityTest[i]);
                }
                else
                {
                    g.getOptA().Add(0);
                    g.getOptD().Add(0);
                    g.getOptV().Add(0);
                }
                adder += (float)value;
                accumulatorAcc = z0;
                accumulatorVel = z1;
                accumulatorDis = z2;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 17/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumDeccreasingAcceleration derives the optimum values for the "decreasingAcceleration" command.
        /// </summary>
        /// <param name="disq1">Player 1's disqualification time.</param>
        /// <param name="disq2">Player 2's disqualification time.</param>
        /// <param name="start">The command's initial frame number.</param>
        /// <param name="end">The command's final frame number.</param>
        /// <param name="accelerationTest1">A list representing Player 1's acceleration during the race.</param>
        /// <param name="accelerationTest2">A list representing Player 2's acceleration during the race.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void OptimumDecreasingAcceleration(int start, int end, List<float> accelerationTest1, List<float> accelerationTest2, PerformanceGraph g)
        {
            double value = 0.1;
            List<float> accelerationTrial = new List<float>();
            List<float> velocityTest = new List<float>();
            float accumulatorAcc = g.getPreviousA();
            float accumulatorVel = g.getPreviousV();
            float accumulatorDis = g.getPreviousD();
            for (int i = 0; i <= accelerationTest1.Count - 1; i++)
            {
                float z0 = accumulatorAcc - (float)value;
                if (z0 >= 0)
                {
                    accelerationTrial.Add(z0);
                }
                else
                {
                    accelerationTrial.Add(0);
                }
                float z1 = accumulatorVel + accelerationTrial[i];
                velocityTest.Add(z1);
                float z2 = accumulatorDis - (float)velocityTest[i];
                if (z2 >= 0)
                {
                    g.getOptA().Add(accelerationTrial[i]);
                    g.getOptD().Add(z2);
                    g.getOptV().Add(velocityTest[i]);
                }
                else
                {
                    g.getOptA().Add(0);
                    g.getOptD().Add(0);
                    g.getOptV().Add(0);
                }
                accumulatorAcc = z0;
                accumulatorVel = z1;
                accumulatorDis = z2;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function OptimumConstantDisplacement derives the optimum values for the "constantDisplacement" command.
        /// </summary>
        /// <param name="size">The number of frames assigned to the current command.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void OptimumConstantDisplacement(int size, PerformanceGraph g)
        {
            for (int k = 0; k <= size - 1; k++)
            {
                g.getOptD().Add(g.getPreviousD());
                g.getOptV().Add(0);
                g.getOptA().Add(0);
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 19/4/2012</para>
        /// <para>Date Modified: 17/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The GetOptimum funciton is used to derive the optimum accelerations/velocities/displacements during the race by calling the necessary functions.
        /// </summary>
        /// <param name="player1disq">The instance when player 1 was disqualified.</param>
        /// <param name="player2disq">The instance when player 2 was disqualified.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void GetOptimum(double player1disq, double player2disq, PerformanceGraph g)
        {
            int start = 0;
            int end = 0;
            g.setPreviousD(g.getTrackLength());
            g.setPreviousV(0);
            g.setPreviousA(0);
            for (int i = 0; i <= g.getTimeSpaces().Count - 1; i++)
            {
                List<float> tempList = new List<float>();
                end = start + (int)(g.getTimeSpaces()[i] * 24);
                List<float> velocityTest1 = new List<float>();
                List<float> velocityTest2 = new List<float>();
                List<float> accelerationTest1 = new List<float>();
                List<float> accelerationTest2 = new List<float>();
                for (int j = start; j <= end; j++)
                {
                    try
                    {
                        velocityTest1.Add(g.getP1Vel()[j]);
                        velocityTest2.Add(g.getP2Vel()[j]);
                        accelerationTest1.Add(g.getP1Acc()[j]);
                        accelerationTest2.Add(g.getP2Acc()[j]);
                    }
                    catch (Exception e)
                    {
                    }
                }
                if (g.getCommands()[i].Equals("constantVelocity"))
                {
                    int x = end - start;
                    OptimumConstantVelocity(x, g);
                }
                if (g.getCommands()[i].Equals("constantAcceleration"))
                {
                    int x = end - start;
                    OptimumConstantAcceleration(x, g);
                }
                if (g.getCommands()[i].Equals("increasingAcceleration"))
                {
                    OptimumIncreasingAcceleration(start, end, accelerationTest1, accelerationTest2, g);
                }
                if (g.getCommands()[i].Equals("decreasingAcceleration"))
                {
                    OptimumDecreasingAcceleration(start, end, accelerationTest1, accelerationTest2, g);
                }
                if (g.getCommands()[i].Equals("constantDisplacement"))
                {
                    int size = end - start;
                    OptimumConstantDisplacement(size, g);
                }
                g.setPreviousA(g.getOptA()[g.getOptA().Count - 1]);
                g.setPreviousV(g.getOptV()[g.getOptV().Count - 1]);
                g.setPreviousD(g.getOptD()[g.getOptD().Count - 1]);
                velocityTest1.Clear();
                velocityTest2.Clear();
                accelerationTest1.Clear();
                accelerationTest2.Clear();
                start = end;
                tempList.Clear();
            }
            CompleteList(g);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 14/5/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function CompleteList is used to equalize the length of the optimum lists and the players' lists.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param
        /// <returns>void.</returns>
        public static void CompleteList(PerformanceGraph g)
        {
            if (g.getOptD().Count < g.getP1Disp().Count)
            {
                int difference = g.getP1Disp().Count - g.getOptD().Count;
                for (int i = 0; i <= difference - 1; i++)
                {
                    g.getOptD().Add(g.getPreviousD());
                }
            }
            if (g.getOptV().Count < g.getP1Vel().Count)
            {
                int difference = g.getP1Vel().Count - g.getOptV().Count;
                for (int i = 0; i <= difference - 1; i++)
                {
                    g.getOptV().Add(g.getPreviousV());
                }
            }
            if (g.getOptA().Count < g.getP1Acc().Count)
            {
                int difference = g.getP1Acc().Count - g.getOptA().Count;
                for (int i = 0; i <= difference - 1; i++)
                {
                    g.getOptA().Add(g.getPreviousA());
                }
            }
        }
    }
}
