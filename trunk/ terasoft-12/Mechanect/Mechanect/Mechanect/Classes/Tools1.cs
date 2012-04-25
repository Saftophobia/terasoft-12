using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace Mechanect.Classes
{
    class Tools1
    {

        /// <summary>
        /// Fisher–Yates shuffle algorithm "http://en.wikipedia.org/wiki/Fisher-Yates_shuffle"
        /// It shuffles the content of the List in random way
        /// </summary>
        /// <typeparam name="T">Type of the Data in the array</typeparam>
        /// <param name="somelist">provide a list to get shuffled</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>

        public static void shuffle<T>(IList<T> somelist)
        {

            Random random = new Random();
            int n = somelist.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = somelist[k];
                somelist[k] = somelist[n];
                somelist[n] = value;

            }

        }

        

        /// <summary>
        /// modified Fisher–Yates shuffle algorithm "http://en.wikipedia.org/wiki/Fisher-Yates_shuffle"
        /// It shuffles the content of the List in random way
        /// </summary>
        /// <typeparam name="T">Type of the Data in the array</typeparam>
        /// <param name="list">provide a list to get shuffled</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
        public static void RNGshuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return;
        }

        /// <summary>
        /// applies constraints to the generated list
        /// </summary>
        /// <typeparam name="T">Type of the Data in the array</typeparam>
        /// <param name="list">provide a list to get shuffled according to some constraints</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
         public static void commandshuffler<T>(IList<T> list)
        {
            RNGshuffle<T>(list);
            if (list[0].ToString() == "decreasingAcceleration" || list[0].ToString() == "constantVelocity" || list[0].ToString() == "constantDisplacement")
                commandshuffler<T>(list);
            if (list[1].ToString() == "decreasingAcceleration" || list[1].ToString() == "constantAcceleration"  || list[1].ToString() == "increasingAcceleration")
                commandshuffler<T>(list);
            if (list[1].ToString() == "constantDisplacement")
            {
                 if (list[2].ToString() == "decreasingAcceleration" || list[2].ToString() == "constantVelocity" || list[2].ToString() == "constantDisplacement")
                commandshuffler<T>(list);
            }
            return;
            }

        /// <summary>
        /// create a list with generated random seconds (3-6)
        /// </summary>
        /// <param name="size">desired size of the list to be created</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
         public static List<int> generaterandomnumbers(int size){
                Random rand = new Random();
                List<int> result = new List<int>();
                HashSet<int> check = new HashSet<int>();
                for (Int32 i = 0; i < size; i++) {
                    int curValue = rand.Next(3,6);
                    
                    result.Add(curValue);
                    }
                return result;
            }



        //Michel's Methods:


         /// <summary>
         /// this method should be called on each update and it will do the check on both players to see if they followed the commands
         /// </summary>
         /// <param name="timeInSeconds">the second the game is in</param>
         /// <param name="User11">the first User1</param>
         /// <param name="User12">the second User1</param>
         /// <param name="timeOfCommands">the time of each command throughout the whole race</param>
         /// <param name="CurrentCommands">the list of commands that should be satisfied for the whole race</param>
         /// <param name="tolerance">the tolerance level</param>
         /// <param name="spriteBatch">the sprite batch to draw the string of state in</param>
         /// <param name="spFont">the font to draw the string of state with</param>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static void CheckEachSecond(int timeInSeconds, User1 User11, User1 User12, List<int> timeOfCommands, List<String> currentCommands, float tolerance, SpriteBatch spriteBatch, SpriteFont spFont)
         {
             int pastSecondsFor1 = 4;
             for (int i = 0; i < User11.ActiveCommand; i++)
                 pastSecondsFor1 += timeOfCommands[i];

             int pastSecondsFor2 = 4;
             for (int i = 0; i < User11.ActiveCommand; i++)
                 pastSecondsFor2 += timeOfCommands[i];

             List<float> User11Displacement = new List<float>();
             List<float> User12Displacement = new List<float>();
             for (int i = (pastSecondsFor1 - 1) * 12; i < User11.Positions.Count; i++)
                 User11Displacement.Add(User11.Positions[i]);

             for (int i = (pastSecondsFor2 - 1) * 12; i < User12.Positions.Count; i++)
                 User12Displacement.Add(User12.Positions[i]);

             if (!CommandSatisfied(currentCommands[User11.ActiveCommand], User11Displacement, tolerance))
             {
                 User11.Disqualified = true;
                 User11.DisqualificationTime = timeInSeconds;
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, "User1 1 got Disqualified", new Vector2(50.0f, 50.0f), Color.Red);
                 spriteBatch.End();
             }
             if (!CommandSatisfied(currentCommands[User12.ActiveCommand], User12Displacement, tolerance))
             {
                 User12.Disqualified = true;
                 User12.DisqualificationTime = timeInSeconds;
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, "User1 2 got Disqualified", new Vector2(50.0f, 50.0f), Color.Blue);
                 spriteBatch.End();
             }
         }

         /// <summary>
         /// this method should be called at the beginning of the race to set the players' position
         /// </summary>
         /// <param name="User11">the first User1</param>
         /// <param name="User12">the second User1</param>
         /// <param name="spriteBatch">the sprite batch to draw the string result</param>
         /// <param name="spFont">the font to draw the string with</param>
         /// <param name="tolerance">the tolerane level</param>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 20/4/12 </para>
         /// </remarks>
         public static bool SetPositions(User1 User1, User1 User2, SpriteBatch spriteBatch, SpriteFont spFont, float tolerance)
         {
             Skeleton sk1 = User1.USER;
             Skeleton sk2 = User1.USER;
             bool result = true;

                 float z;
                 float z2;
                 bool isThePositionRight;
                 bool isThePositionRight2;
                 String User11State = "";
                 String User12State = "";
                 if (sk1 != null)
                 {
                     z = (float)sk1.Position.Z;
                     isThePositionRight = CheckPosition(z, tolerance);

                     User11State = "User1 1: Your position is: " + z.ToString() + " this is " + isThePositionRight + ", it should be 4.0 m \n";

                     if (!isThePositionRight)
                         result = false;
                 }
                 if (sk2 != null)
                 {
                     z2 = (float)sk2.Position.Z;
                     isThePositionRight2 = CheckPosition(z2, tolerance);

                     User12State = "User1 2: Your position is: " + z2.ToString() + " this is " + isThePositionRight2 + ", it should be 4.0 m \n";

                     if (!isThePositionRight2)
                         result = false;
                 }
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, User11State, new Vector2(50.0f, 50.0f), Color.Red);
                 spriteBatch.DrawString(spFont, User12State, new Vector2(50.0f, 50.0f), Color.Blue);
                 spriteBatch.End();
                 return result;
         }

         /// <summary>
         /// this method checks whether the User1 is standing in about 4m or not(with certain tolerance)
         /// </summary>
         /// <param name="UserPosition">the position that the User1 is currently standing at</param>
         /// <param name="tolerance">the tolerance level</param>
         /// <returns>if the position sent is in the range or not</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 20/4/12 </para>
         /// </remarks>
         public static bool CheckPosition(float UserPosition, float tolerance)
         {
             //initiate the minimum distance from the Kinect which means
             //the maximum distance between the two players which is actually the tolerance on a scale from 0-10 mm
             float min = 4.0f - (tolerance / 1000);
             //check if the actual position of the player is within the range or not
             if (UserPosition >= min && UserPosition <= 4.0f)
                 //if it is within the range then it will be true
                 return true;
             //if it is out of this range and/or the kinect range it will be false
             else return false;
         }

         /// <summary>
         /// this method checks whether the User1 satisfied the commands or not
         /// </summary>
         /// <param name="command">which is the name of the command that should be satisfied</param>
         /// <param name="positions">a list containing the positions of the User1</param>
         /// <param name="tolerance">the tolerance level</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool CommandSatisfied(String command, List<float> positions, float tolerance)
         {
             bool result = true;
             float currentTolerance = (10 - tolerance) / 100;

             if (command.Equals("constantVelocity"))
             {
                 result = ConstantVelocity(positions, currentTolerance);
             }
             else
             {
                 if (command.Equals("constantAcceleration"))
                 {
                     result = ConstantAcceleration(positions, currentTolerance);
                 }
                 else
                 {
                     if (command.Equals("constantDisplacement"))
                     {
                         result = ConstantDisplacement(positions, currentTolerance);
                     }
                     else
                     {
                         if (command.Equals("increasingAcceleration"))
                         {
                             result = IncreasingAcceleration(positions, currentTolerance);
                         }
                         else
                         {
                             if (command.Equals("decreasingAcceleration"))
                             {
                                 result = DecreasingAcceleration(positions, currentTolerance);
                             }
                         }
                     }
                 }
             }
             return result;
         }

         /// <summary>
         /// checks if the user applied the command of Constant Velocity or not
         /// </summary>
         /// <param name="positions">list of the user positions throughout the command</param>
         /// <param name="currentTolerance">the tolerance of the game</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool ConstantVelocity(List<float> positions, float currentTolerance)
         {
             bool result = true;
             float firstVelocity = positions[0] - positions[1];
             for (int i = 2; i < positions.Count; i++)
             {
                 float currentDisplacement = (positions[i - 1] - positions[i]);
                 if (!(currentDisplacement >= (firstVelocity - currentTolerance) && currentDisplacement <= (firstVelocity + currentTolerance)))
                 {
                     if (positions[positions.Count - 1] == 0.8)
                     {
                         result = true;
                         break;
                     }
                     else
                     {
                         result = false;
                         break;
                     }
                 }
             }
             return result;
         }
         
         /// <summary>
         /// checks if the user applied the command of Constant Acceleration or not
         /// </summary>
         /// <param name="positions">list of the user positions throughout the command</param>
         /// <param name="currentTolerance">the tolerance of the game</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool ConstantAcceleration(List<float> positions, float currentTolerance)
         {
             List<float> velocities = PerformanceGraph.GetPlayerVelocity(positions);


             bool result = true;
             float firstVelocity = velocities[1] - velocities[0];
             for (int i = 2; i < velocities.Count; i++)
             {
                 float currentVelocity = (velocities[i] - velocities[i - 1]);
                 if (!(currentVelocity >= (firstVelocity - currentTolerance) && currentVelocity <= (firstVelocity + currentTolerance)))
                 {
                     if (positions[positions.Count - 1] == 0.8)
                     {
                         result = true;
                         break;
                     }
                     else
                     {
                         result = false;
                         break;
                     }
                 }
             }
             return result;
         }

         /// <summary>
         /// checks if the user applied the command of Constant Displacement or not
         /// </summary>
         /// <param name="positions">list of the user positions throughout the command</param>
         /// <param name="currentTolerance">the tolerance of the game</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool ConstantDisplacement(List<float> positions, float currentTolerance)
         {
             bool result = true;
             float firstDisplacement = positions[0];
             for (int i = 1; i < positions.Count; i++)
             {
                 float currentDisplacement = positions[i];
                 if (!(currentDisplacement >= (firstDisplacement - currentTolerance) && currentDisplacement <= (firstDisplacement + currentTolerance)))
                 {
                     if (positions[positions.Count - 1] == 0.8)
                     {
                         result = true;
                         break;
                     }
                     else
                     {
                         result = false;
                         break;
                     }
                 }
             }
             return result;
         }

         /// <summary>
         /// checks if the user applied the command of Increasing Acceleration or not
         /// </summary>
         /// <param name="positions">list of the user positions throughout the command</param>
         /// <param name="currentTolerance">the tolerance of the game</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool IncreasingAcceleration(List<float> positions, float currentTolerance)
         {
             List<float> accelerations = PerformanceGraph.GetPlayerAcceleration(PerformanceGraph.GetPlayerVelocity(positions));


             bool result = true;
             float firstAcceleration = accelerations[1] - accelerations[0];
             for (int i = 2; i < accelerations.Count; i++)
             {
                 float currentAcceleration = (accelerations[i] - accelerations[i - 1]);
                 if (!(currentAcceleration >= (firstAcceleration - currentTolerance)))
                {
                    if (positions[positions.Count - 1] == 0.8)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                 }
             }
             return result;
         }

         /// <summary>
         /// checks if the user applied the command of Decreasing Acceleration or not
         /// </summary>
         /// <param name="positions">list of the user positions throughout the command</param>
         /// <param name="currentTolerance">the tolerance of the game</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool DecreasingAcceleration(List<float> positions, float currentTolerance)
         {
             List<float> accelerations = PerformanceGraph.GetPlayerAcceleration(PerformanceGraph.GetPlayerVelocity(positions));


             bool result = true;
             float firstAcceleration = accelerations[1] - accelerations[0];
             for (int i = 2; i < accelerations.Count; i++)
             {
                 float currentAcceleration = (accelerations[i] - accelerations[i - 1]);
                 if (!(currentAcceleration <= (firstAcceleration - currentTolerance)))
                 {
                     if (positions[positions.Count - 1] == 0.8)
                     {
                         result = true;
                         break;
                     }
                     else
                     {
                         result = false;
                         break;
                     }
                 }
             }

             return result;
         }

        }

    }

