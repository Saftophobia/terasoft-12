using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
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
	List<int> Player1Displacement;
        List<int> Player2Displacement;
        List<int> Player1Velocity;
        List<int> Player2Velocity;
        List<int> Player1Acceleration;
        List<int> Player2Acceleration;
        List<String> CommandsList;
        //CommandsList is a list represening each command given during the race
        List<double> TimeSpaces;
        //TimeSpaces is a List representing the number of seconds elapsed by each command
        List<int> OptimumDisplacement;
        List<int> OptimumVelocity;
        List<int> OptimumAcceleration;
        Game currentGame;

        public PerformanceGraph()
        {
            
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

        public static List<int> GetPlayerVelocity(List<int> DisplacementList)
        {
            int size = DisplacementList.Count;
            List<int> result = new List<int>();
            for (int i = 0; i <= size - 1; i++)
            {
                try
                {
                    double dt = 0.0333333333333333333; //equivalent to 1/30
                    int currentVelocity = (int)((DisplacementList[i] - DisplacementList[i - 1]) / dt) * -1;
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

        public static List<int> GetPlayerAcceleration(List<int> VelocityList)
        {
            int size = VelocityList.Count;
            List<int> result = new List<int>();
            for (int i = 0; i <= size - 1; i++)
            {
                try
                {
                    double dt = 0.0333333333333333333; //equivalent to 1/30
                    int currentAcceleration = (int)((VelocityList[i] - VelocityList[i - 1]) / dt);
                    result.Add(currentAcceleration);
                }
                catch (Exception e)
                {
                    result.Add(0);
                }
            }
            return result;
        }

        public void GetOptimum()
        {
            int size = CommandsList.Count;
            List<int> displacementTest1 = new List<int>();
            List<int> displacementTest2 = new List<int>();
            List<int> velocityTest1 = new List<int>();
            List<int> velocityTest2 = new List<int>();
            List<int> accelerationTest1 = new List<int>();
            List<int> accelerationTest2 = new List<int>();
            int start = 0;

            for (int i = 0; i <= size - 1; i++)
            {
                int NumberOfFrames = (int)(30 * TimeSpaces[i]);
                displacementTest1.Clear();
                displacementTest2.Clear();
                velocityTest1.Clear();
                velocityTest2.Clear();
                accelerationTest1.Clear();
                accelerationTest2.Clear();


                int counter = 0;

                for (int j = start; j <= NumberOfFrames - 1; j++)
                {
                    displacementTest1.Add(Player1Displacement[j]);
                    displacementTest2.Add(Player2Displacement[j]);
                    velocityTest1.Add(Player1Velocity[j]);
                    velocityTest2.Add(Player2Velocity[j]);
                    accelerationTest1.Add(Player1Acceleration[j]);
                    accelerationTest2.Add(Player2Acceleration[j]);
                    counter++;
                }

                int disq1 = 0;//(int)(currentGame.GetPlayer1Disq() * 30);
                int disq2 = 0;// (int)(currentGame.GetPlayer2Disq() * 30);
                int end = start + counter;

                if (CommandsList[i].Equals("constantVelocity"))
                {
                    List<int> storage = new List<int>();
                    if (!LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
                    {
                        int a = GetAverageList(velocityTest1);
                        int b = GetAverageList(velocityTest2);
                        for (int j = 0; j <= velocityTest1.Count - 1; j++)
                        {
                            if (a >= b)
                            {
                                storage.Add(a);
                            }
                            else
                            {
                                storage.Add(b);
                            }
                        }
                    }

                    if (!LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
                    {
                        int average = GetAverageList(velocityTest1);
                        for (int j = 0; j <= velocityTest1.Count - 1; j++)
                        {
                            storage.Add(average);
                        }
                    }

                    if (LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
                    {
                        int average = GetAverageList(velocityTest2);
                        for (int j = 0; j <= velocityTest2.Count - 1; j++)
                        {
                            storage.Add(average);
                        }
                    }

                    if (LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
                    {
                        //Case: Non constant velocity
                        int average1 = GetAverageList(velocityTest1);
                        int average2 = GetAverageList(velocityTest2);
                        int value = (int)((average1 + average2) / 2);
                        //Case: No movement, the optimum velocity is assumed to be 1m/sec 
                        if (average1 == 0 && average2 == 0)
                        {
                            value = 1000;
                        }
                        for (int j = 0; j <= velocityTest1.Count - 1; j++)
                        {
                            storage.Add(value);
                        }
                    }
                    List<int> accelerationList = GetPlayerAcceleration(storage);
                    for (int k = 0; k <= accelerationList.Count - 1; k++)
                    {
                        OptimumAcceleration.Add(accelerationList[i]);
                    }
                    storage.Clear();
                }


                if (CommandsList[i].Equals("constantAcceleration") || CommandsList[i].Equals("constantDesceleration"))
                {
                    if (!LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
                    {
                        int a = GetAverageList(accelerationTest1);
                        int b = GetAverageList(accelerationTest2);
                        for (int j = 0; j <= accelerationTest1.Count - 1; j++)
                        {
                            if (a >= b)
                            {
                                OptimumAcceleration.Add(a);
                            }
                            else
                            {
                                OptimumAcceleration.Add(b);
                            }
                        }
                    }

                    if (!LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
                    {
                        int average = GetAverageList(accelerationTest1);
                        for (int j = 0; j <= accelerationTest1.Count - 1; j++)
                        {
                            OptimumAcceleration.Add(average);
                        }
                    }

                    if (LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
                    {
                        int average = GetAverageList(accelerationTest2);
                        for (int j = 0; j <= accelerationTest2.Count - 1; j++)
                        {
                            OptimumAcceleration.Add(average);
                        }
                    }

                    if (LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
                    {
                        //Case: Non constant acceleration
                        int average1 = GetAverageList(accelerationTest1);
                        int average2 = GetAverageList(accelerationTest2);

                        int value = (int)((average1 + average2) / 2);
                        //Case: No movement, the average acceleration calculated is 29700 assuming 1m/sec is
                        //the optimum velocity
                        if (average1 == 0 && average2 == 0)
                        {
                            if (CommandsList[i].Equals("constantAcceleration"))
                            {
                                value = 29700;
                            }
                            else
                            {
                                value = -29700;
                            }
                        }
                        for (int j = 0; j <= velocityTest1.Count - 1; j++)
                        {
                            OptimumAcceleration.Add(value);
                        }
                    }

                }


                //average step
                if (CommandsList[i].Equals("increasingAcceleration"))
                {
                    if (!LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
                    {
                        int totalStep1 = 0;
                        int totalStep2 = 0;

                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            int step1 = 0;
                            int step2 = 0;
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

                        int average1 = (int)(totalStep1 / accelerationTest1.Count);
                        int average2 = (int)(totalStep2 / accelerationTest2.Count);
                        int value1 = accelerationTest1[0];
                        int value2 = accelerationTest2[0];

                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            if (average1 >= average2)
                            {
                                OptimumAcceleration.Add(value1);
                                value1 += average1;
                            }
                            else
                            {
                                OptimumAcceleration.Add(value2);
                                value2 += average2;
                            }
                        }
                    }

                    if (!LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
                    {
                        int totalStep = 0;
                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            int step = 0;
                            try
                            {
                                step = accelerationTest1[k + 1] - accelerationTest1[k];
                            }
                            catch (Exception e)
                            {

                            }
                            totalStep += step;
                        }
                        int average = (int)(totalStep / accelerationTest1.Count);
                        int value = accelerationTest1[0];
                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            OptimumAcceleration.Add(value);
                            value += average;
                        }

                    }

                    if (LiesInBetween(disq1, start, end) && !LiesInBetween(disq2, start, end))
                    {
                        int totalStep = 0;
                        for (int k = 0; k <= accelerationTest2.Count - 1; k++)
                        {
                            int step = 0;
                            try
                            {
                                step = accelerationTest2[k + 1] - accelerationTest2[k];
                            }
                            catch (Exception e)
                            {

                            }
                            totalStep += step;
                        }
                        int average = (int)(totalStep / accelerationTest1.Count);
                        int value = accelerationTest2[0];
                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            OptimumAcceleration.Add(value);
                            value += average;
                        }
                    }
                    //abs(avg step)
                    if (LiesInBetween(disq1, start, end) && LiesInBetween(disq2, start, end))
                    {
                        int totalStep1 = 0;
                        int totalStep2 = 0;
                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            int step1 = 0;
                            int step2 = 0;
                            try
                            {

                                int b2 = accelerationTest2[k];
                                int b1 = accelerationTest1[k];
                                int a2 = accelerationTest2[k + 1];
                                int a1 = accelerationTest1[k + 1];


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
                        int average1 = (int)(totalStep1 / accelerationTest1.Count);
                        int average2 = (int)(totalStep2 / accelerationTest2.Count);
                        int value1 = accelerationTest1[0];
                        int value2 = accelerationTest2[0];
                        //Case: No movement, 29700 is the calculated average constantly increasing
                        //acceleration assuming the player's optimum velocity is 1m/sec
                        if (average1 == 0 && average2 == 0)
                        {
                            average1 = 29700;
                            average2 = 29700;
                        }
                        for (int k = 0; k <= accelerationTest1.Count - 1; k++)
                        {
                            if (average1 >= average2)
                            {
                                OptimumAcceleration.Add(value1);
                                value1 += average1;
                            }
                            else
                            {
                                OptimumAcceleration.Add(value2);
                                value2 += average2;
                            }
                        }

                    }
                }
                start = start + counter;
            }

            OptimumVelocity = Integral(OptimumAcceleration, 0);
            List<int> Temp = Integral(OptimumVelocity, 0);
            for (int i = 0; i <= Temp.Count - 1; i++)
            {
                int x = 4000 - Temp[i];
                OptimumDisplacement.Add(x);
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
            if(value<=end&&value>start)
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
        /// from a set of values in a list
        /// </summary>
        /// <param name="list">A list holding a set of values</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>int: returns an integer representing the average 
        /// value of the list</returns>
        public int GetAverageList(List<int> list)
        {
            int x = 0;
            for (int i = 0; i <= list.Count - 1; i++)
            {
                x += list[i];
            }
            int y = (int)(x / list.Count);
            return y;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 19/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// Integral is used to generate the player's velocity using the 
        /// player's acceleration or the player's displacement using the
        /// player's velocity, therefore it acts as an opposite to the 
        /// functions GetPlayerVelocity and GetPlayerAccleration
        /// 
        /// The function integral makes use of the following equations:
        ///            
        /// Velocity= (Displacement.Final-Displacement.Initial)/dt
        ///
        /// Acceleration= (Velocity.Final-Velocity.Initial)/dt
        ///                            
        /// to convert them to the following equations:
        /// 
        /// Displacement.Final=Displacement.Initial+(Velocity/30)
        /// 
        /// Velocity.Final=Velocity.Initial+(Acceleration/30)
        ///                            
        /// to derive the final displacement or the final velocity 
        /// given the initial displacement or velocity of the player.        
        /// </summary>
        /// <param name="list">A list representing the Velocities
        /// or the Accelerations of the player</param>
        /// <param name="initial">The initial displacement or velocity
        /// of the player</param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>List: returns a list representing the player's
        /// displacement or velocity</returns>
        public List<int> Integral(List<int> list, int initial)
        {
            List<int> wanted = new List<int>();
            int x = initial;
            for (int i = 0; i <= list.Count - 1; i++)
            {

                int z = x + (int)(list[i] / 30);
                wanted.Add(z);
                x = z;

            }
            return wanted;
        }

        public void drawLine(SpriteBatch batch, Texture2D blank,
              float width, Microsoft.Xna.Framework.Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        public void drawGraphs(List<int> Player1Displacement, List<int> Player2Displacement,
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
            OptimumAcceleration = new List<int>();
            OptimumVelocity = new List<int>();
            OptimumDisplacement = new List<int>();
            GetOptimum();
        }





        protected void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}
