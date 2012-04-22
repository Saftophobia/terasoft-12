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
         private List<int> generaterandomnumbers(int size){
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
         /// <param name="user1">the first user</param>
         /// <param name="user2">the second user</param>
         /// <param name="timeOfCommands">the time of each command throughout the whole race</param>
         /// <param name="CurrentCommands">the list of commands that should be satisfied for the whole race</param>
         /// <param name="tolerance">the tolerance level</param>
         /// <param name="spriteBatch">the sprite batch to draw the string of state in</param>
         /// <param name="spFont">the font to draw the string of state with</param>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 21/4/12 </para>
         /// </remarks>
         public static void CheckEachSecond(int timeInSeconds, User user1, User user2, List<int> timeOfCommands, List<GameCommands> currentCommands, float tolerance, SpriteBatch spriteBatch, SpriteFont spFont)
         {
             int pastSecondsFor1 = 5;
             for (int i = 0; i < user1.ActiveCommand; i++)
                 pastSecondsFor1 += timeOfCommands[i];

             int pastSecondsFor2 = 5;
             for (int i = 0; i < user1.ActiveCommand; i++)
                 pastSecondsFor2 += timeOfCommands[i];

             List<int> user1Displacement = new List<int>();//change this back to float
             List<int> user2Displacement = new List<int>();//change this back to float
             for (int i = (pastSecondsFor1 - 1) * 24; i < user1.Positions.Count; i++)
                 user1Displacement.Add((int)user1.Positions[i]);//remove the type-cast

             for (int i = (pastSecondsFor2 - 1) * 24; i < user2.Positions.Count; i++)
                 user2Displacement.Add((int)user2.Positions[i]);//remove the type-cast

             if (!CommandSatisfied(currentCommands[user1.ActiveCommand].Name, user1Displacement, tolerance))
             {
                 user1.Disqualified = true;
                 user1.DisqualificationTime = timeInSeconds;
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, "user 1 got Disqualified", new Vector2(50.0f, 50.0f), Color.Red);
                 spriteBatch.End();
             }
             if (!CommandSatisfied(currentCommands[user2.ActiveCommand].Name, user2Displacement, tolerance))
             {
                 user2.Disqualified = true;
                 user2.DisqualificationTime = timeInSeconds;
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, "user 2 got Disqualified", new Vector2(50.0f, 50.0f), Color.Blue);
                 spriteBatch.End();
             }
         }

         /// <summary>
         /// this method should be called at the beginning of the race to set the players' position
         /// </summary>
         /// <param name="user1">the first user</param>
         /// <param name="user2">the second user</param>
         /// <param name="spriteBatch">the sprite batch to draw the string result</param>
         /// <param name="spFont">the font to draw the string with</param>
         /// <param name="tolerance">the tolerane level</param>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 20/4/12 </para>
         /// </remarks>
         public static void SetPositions(User user1, User user2, SpriteBatch spriteBatch, SpriteFont spFont, float tolerance)
         {
             Skeleton sk1 = user1.USER;
             Skeleton sk2 = user2.USER;

                 float z;
                 float z2;
                 bool isThePositionRight;
                 bool isThePositionRight2;
                 String user1State = "";
                 String user2State = "";
                 if (sk1 != null)
                 {
                     z = (float)sk1.Position.Z;
                     isThePositionRight = CheckPosition(z, tolerance);

                     user1State = "User 1: Your position is: " + z.ToString() + " this is " + isThePositionRight + ", it should be 4.0 m \n";
                 }
                 if (sk2 != null)
                 {
                     z2 = (float)sk2.Position.Z;
                     isThePositionRight2 = CheckPosition(z2, tolerance);

                     user2State = "User 2: Your position is: " + z2.ToString() + " this is " + isThePositionRight2 + ", it should be 4.0 m \n";
                 }
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, user1State, new Vector2(50.0f, 50.0f), Color.Red);
                 spriteBatch.DrawString(spFont, user2State, new Vector2(50.0f, 50.0f), Color.Blue);
                 spriteBatch.End();
         }

         /// <summary>
         /// this method checks whether the user is standing in about 4m or not(with certain tolerance)
         /// </summary>
         /// <param name="userPosition">the position that the user is currently standing at</param>
         /// <param name="tolerance">the tolerance level</param>
         /// <returns>if the position sent is in the range or not</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 20/4/12 </para>
         /// </remarks>
         public static bool CheckPosition(float userPosition, float tolerance)
         {
             //initiate the minimum distance from the Kinect which means
             //the maximum distance between the two players which is actually the tolerance on a scale from 0-10 mm
             float min = 4.0f - (tolerance / 1000);
             //check if the actual position of the player is within the range or not
             if (userPosition >= min && userPosition <= 4.0f)
                 //if it is within the range then it will be true
                 return true;
             //if it is out of this range and/or the kinect range it will be false
             else return false;
         }

         /// <summary>
         /// this method checks whether the user satisfied the commands or not
         /// </summary>
         /// <param name="command">which is the name of the command that should be satisfied</param>
         /// <param name="positions">a list containing the positions of the user</param>
         /// <param name="tolerance">the tolerance level</param>
         /// <returns>if the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 21/4/12 </para>
         /// </remarks>
         public static bool CommandSatisfied(String command, List<int> positions, float tolerance)//change this back to float
         {
             bool result = true;
             float currentTolerance = tolerance / 100;
             if (command.Equals("constantVelocity"))
             {
                 float firstVelocity = positions[0] - positions[1];
                 for (int i = 2; i < positions.Count; i++)
                 {
                     float currentDisplacement = (positions[i - 1] - positions[i]);
                     if (!(currentDisplacement >= (firstVelocity - currentTolerance) && currentDisplacement <= (firstVelocity + currentTolerance)))
                     {
                         result = false;
                         break;
                     }
                     else
                     {
                         if (positions[positions.Count - 1] == 0)
                         {
                             result = true;
                         }
                     }
                 }
             }
             else
             {
                 if (command.Equals("constantAcceleration"))
                 {
                     List<int> velocities = PerformanceGraph.GetPlayerVelocity(positions);//change this back to float
                     float firstVelocity = velocities[1] - velocities[0];
                     for (int i = 2; i < positions.Count; i++)
                     {
                         float currentVelocity = (velocities[i] - velocities[i - 1]);
                         if (!(currentVelocity >= (firstVelocity - currentTolerance) && currentVelocity <= (firstVelocity + currentTolerance)))
                         {
                             result = false;
                             break;
                         }
                         else
                         {
                             if (positions[positions.Count - 1] == 0)
                             {
                                 result = true;
                             }
                         }
                     }
                 }
                 else
                 {
                     if (command.Equals("constantDisplacement"))
                     {
                         float firstDisplacement = positions[0];
                         for (int i = 1; i < positions.Count; i++)
                         {
                             float currentDisplacement = positions[i];
                             if (!(currentDisplacement >= (firstDisplacement - currentTolerance) && currentDisplacement <= (firstDisplacement + currentTolerance)))
                             {
                                 result = false;
                                 break;
                             }
                             else
                             {
                                 if (positions[positions.Count - 1] == 0)
                                 {
                                     result = true;
                                 }
                             }
                         }
                     }
                     else
                     {
                         if (command.Equals("increasingAcceleration"))
                         {
                             List<int> accelerations = PerformanceGraph.GetPlayerAcceleration(PerformanceGraph.GetPlayerVelocity(positions));//change this back to float
                             float firstAcceleration = accelerations[1] - accelerations[0];
                             for (int i = 2; i < positions.Count; i++)
                             {
                                 float currentAcceleration = (accelerations[i] - accelerations[i - 1]);
                                 if (!(currentAcceleration >= (firstAcceleration - currentTolerance)))
                                 {
                                     result = false;
                                     break;
                                 }
                                 else
                                 {
                                     if (positions[positions.Count - 1] == 0)
                                     {
                                         result = true;
                                     }
                                 }
                             }
                         }
                         else
                         {
                             if (command.Equals("decreasingAcceleration"))
                             {
                                 List<int> accelerations = PerformanceGraph.GetPlayerAcceleration(PerformanceGraph.GetPlayerVelocity(positions));//change this back to float
                                 float firstAcceleration = accelerations[1] - accelerations[0];
                                 for (int i = 2; i < positions.Count; i++)
                                 {
                                     float currentAcceleration = (accelerations[i] - accelerations[i - 1]);
                                     if (!(currentAcceleration <= (firstAcceleration - currentTolerance)))
                                     {
                                         result = false;
                                         break;
                                     }
                                     else
                                     {
                                         if (positions[positions.Count - 1] == 0)
                                         {
                                             result = true;
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 }
             }
             return result;
         }

        }

    }

