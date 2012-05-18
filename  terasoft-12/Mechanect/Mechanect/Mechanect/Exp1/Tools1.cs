﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;

namespace Mechanect.Exp1
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
         /// Check if any of both players won the race or not.
         /// </summary>
         /// <param name="user1">The first player.</param>
         /// <param name="user2">The second player.</param>
         /// <returns>
         /// string: The status of both players, whether any of them won, 
         /// both of them won, no one won, or the race is still going (will be an empty string)
         /// </returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 17/5/12 </para>
         /// <para>DATE MODIFIED: 17/5/12 </para>
         /// </remarks>
         public static string GetWinner(User1 user1, User1 user2)
         {
             if ((user1.Positions[user1.Positions.Count - 1] >= 1000.0) && !(user1.Disqualified))
             {
                 user1.Winner = true;
                 return "Player 1 won the race";
             }
             else
             {
                 if ((user2.Positions[user2.Positions.Count - 1] >= 1000.0) && !(user2.Disqualified))
                 {
                     user2.Winner = true;
                     return "Player 1 won the race";
                 }
                 else
                     if ((user2.Positions[user2.Positions.Count - 1] >= 1000.0) && (user2.Disqualified)
                         && (user1.Positions[user1.Positions.Count - 1] >= 1000.0) && (user1.Disqualified))
                         return "No one won this race because you both got disqualified";
                     else
                         if ((user2.Positions[user2.Positions.Count - 1] >= 1000.0) && !(user2.Disqualified)
                            && (user1.Positions[user1.Positions.Count - 1] >= 1000.0) && !(user1.Disqualified))
                         {
                             user1.Winner = true;
                             user2.Winner = true;
                             return "It's a tie!!!";
                         }
                         else
                             return "";
             }
         }

         /// <summary>
         /// This method checks the disqualification of the two players once the command is over
         /// </summary>
         /// <param name="timeInSeconds">The time of the game.</param>
         /// <param name="user1">The first player.</param>
         /// <param name="user2">The second player.</param>
         /// <param name="timeOfCommands">The list specifying the time of each command.</param>
         /// <param name="currentCommands">The list of current game commands.</param>
         /// <param name="tolerance">The tolerance level.</param>
         /// <returns>void: Within the method itself it updates the value of the variable isDisqualified of the two players.</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 18/5/12 </para>
         /// <para>DATE MODIFIED: 18/5/12 </para>
         /// </remarks>
         public static void CheckTheCommand(int timeInSeconds, User1 user1, User1 user2, List<int> timeOfCommands, List<string> currentCommands, float tolerance)
         {
             //get the accumulative time of current command
             int accumulativeTime = 0;
             for (int i = 0; i < user1.ActiveCommand; i++)
                 accumulativeTime += timeOfCommands[i];

             if (accumulativeTime != timeInSeconds)
                 return;

             //get the start index of speed list
             int startIndexFor1 = 0;
             int startIndexFor2 = 0;
             for (int i = 0; i < user1.Velocitylist.Count; i++)
                 if ((accumulativeTime + 1) >= user1.Velocitylist[i][1])
                 {
                     startIndexFor1 = i;
                     break;
                 }

             for (int i = 0; i < user2.Velocitylist.Count; i++)
                 if ((accumulativeTime + 1) >= user2.Velocitylist[i][1])
                 {
                     startIndexFor2 = i;
                     break;
                 }

             //set the list of speeds
             List<float> speedsOf1 = new List<float>();
             List<float> speedsOf2 = new List<float>();
             if (startIndexFor1 < user1.Velocitylist.Count)
                 for (int i = startIndexFor1; i < user1.Velocitylist.Count; i++)
                     speedsOf1.Add(user1.Velocitylist[i][0]);

             if (startIndexFor2 < user2.Velocitylist.Count)
                 for (int i = startIndexFor2; i < user2.Velocitylist.Count; i++)
                     speedsOf2.Add(user2.Velocitylist[i][0]);

             //here the command is checked pver the two players to see if any of them got disqualified
             string s = "";
             if (!CommandSatisfied(currentCommands[user1.ActiveCommand], speedsOf1, tolerance))
             {
                 user1.Disqualified = true;
                 user1.DisqualificationTime = timeInSeconds;
                 s += "User 1 got Disqualified \n";
                 Console.Write("User1 1 got Disqualified");
             }
             if (!CommandSatisfied(currentCommands[user2.ActiveCommand], speedsOf2, tolerance))
             {
                 user2.Disqualified = true;
                 user2.DisqualificationTime = timeInSeconds;
                 s += "User 2 got Disqualified";
                 Console.Write("User 2 got Disqualified");
             }
         }

         /// <summary>
         /// This method should be called on each second, and it will do the check on both players to see if they followed the commands.
         /// </summary>
         /// <param name="timeInSeconds">The second the game is in.</param>
         /// <param name="user11">The first user.</param>
         /// <param name="user12">The second user.</param>
         /// <param name="timeOfCommands">The time of each command throughout the whole race.</param>
         /// <param name="CurrentCommands">The list of commands that should be satisfied for the whole race.</param>
         /// <param name="tolerance">The tolerance level.</param>
         /// <param name="spriteBatch">The sprite batch to draw the string of state in.</param>
         /// <param name="spFont">The font to draw the string of state with.</param>
         /// <returns>void: within the method itself it updates the value of the variable isDisqualified of the two users</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 14/5/12 </para>
         /// </remarks>
         public static void CheckEachSecond(int timeInSeconds, User1 user11, User1 user12, List<int> timeOfCommands, List<String> currentCommands, float tolerance, SpriteBatch spriteBatch, SpriteFont spFont)
         {
             //check if I have a very low number of frames (which doesn't allow me to check)
             if ((user11.Positions.Count <= 3) || (user12.Positions.Count <= 3))
                 return;

             //check the frame rate that the kinect could get
             int frameRate = user11.Positions.Count / timeInSeconds;

             //this part is to check to define the start of the positions to check the command on them
             int pastSecondsFor1 = 0;
             for (int i = 0; i < user11.ActiveCommand; i++)
                 pastSecondsFor1 += timeOfCommands[i];

             int pastSecondsFor2 = 0;
             for (int i = 0; i < user12.ActiveCommand; i++)
                 pastSecondsFor2 += timeOfCommands[i];

             if ((pastSecondsFor1 * frameRate) >= user11.Positions.Count)
             {
                 //pastSecondsFor1 = timeInSeconds * frameRate;
                 //pastSecondsFor2 = timeInSeconds * frameRate;
                 return;
             }

             //check to drop the first second of the command
             if (timeInSeconds == pastSecondsFor1)
                 return;

             List<float> user11Displacement = new List<float>();
             List<float> user12Displacement = new List<float>();
             for (int i = (pastSecondsFor1 + 1) * frameRate; i < user11.Positions.Count; i++)
                 user11Displacement.Add(user11.Positions[i]);

             for (int i = (pastSecondsFor1 + 1) * frameRate; i < user12.Positions.Count; i++)
                 user12Displacement.Add(user12.Positions[i]);

             //a condition in order not to get an OutOfBounds if the kinect sent wrong data
             if ((user11Displacement.Count < 2) || (user12Displacement.Count < 2))
                 return;

             //here the command is checked pver the two players to see if any of them got disqualified
             if (!CommandSatisfied(currentCommands[user11.ActiveCommand], user11Displacement, tolerance))
             {
                 user11.Disqualified = true;
                 user11.DisqualificationTime = timeInSeconds;
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, "User 1 got Disqualified", new Vector2(50.0f, 50.0f), Color.Red);
                 spriteBatch.End();
                 Console.Write("User1 1 got Disqualified");
             }
             if (!CommandSatisfied(currentCommands[user12.ActiveCommand], user12Displacement, tolerance))
             {
                 user12.Disqualified = true;
                 user12.DisqualificationTime = timeInSeconds;
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, "User 2 got Disqualified", new Vector2(50.0f, 50.0f), Color.Blue);
                 spriteBatch.End();
                 Console.Write("User 2 got Disqualified");
             }
         }

         /// <summary>
         /// This method should be called at the beginning of the race to set the players' positions.
         /// </summary>
         /// <param name="sk1">The skeleton of the first user.</param>
         /// <param name="sk2">The skeleton of the second user.</param>
         /// <param name="spriteBatch">The sprite batch to draw the string result.</param>
         /// <param name="spFont">The font to draw the string with.</param>
         /// <param name="tolerance">The tolerane level.</param>
         /// <returns>bool: If the position of both users is right then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 27/4/12 </para>
         /// </remarks>
         public static bool SetPositions(Skeleton sk1, Skeleton sk2, SpriteBatch spriteBatch, SpriteFont spFont, float tolerance)
         {

                 float z;
                 float z2;
                 bool isThePositionRight = false;
                 bool isThePositionRight2 = false;
                 String user11State = "";
                 String user12State = "";
                 if (sk1 != null)
                 {
                     z = (float)sk1.Position.Z;
                     isThePositionRight = CheckPosition(z, tolerance);

                     user11State = "User 1: Your position is: " + z.ToString() + " this is " + isThePositionRight + ", it should be 4.0 m \n";
                 }
                 if (sk2 != null)
                 {
                     z2 = (float)sk2.Position.Z;
                     isThePositionRight2 = CheckPosition(z2, tolerance);

                     user12State = "User 2: Your position is: " + z2.ToString() + " this is " + isThePositionRight2 + ", it should be 4.0 m \n";
                 }
             
                 spriteBatch.Begin();
                 spriteBatch.DrawString(spFont, user11State, new Vector2(50.0f, 50.0f), Color.Red);
                 spriteBatch.DrawString(spFont, user12State, new Vector2(50.0f, 150.0f), Color.Blue);
                 spriteBatch.End();
                 Console.Write(user11State + "\n" + user12State);
                 return isThePositionRight && isThePositionRight2;
         }

         /// <summary>
         /// This method checks whether the user is standing in about 4m or not (with certain tolerance).
         /// </summary>
         /// <param name="userPosition">The position that the User1 is currently standing at.</param>
         /// <param name="tolerance">The tolerance level.</param>
         /// <returns>bool: If the position sent is in the range or not.</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 26/4/12 </para>
         /// </remarks>
         public static bool CheckPosition(float userPosition, float tolerance)
         {
             //initiate the minimum distance from the Kinect which means
             //the maximum distance between the two players which is actually the tolerance on a scale from 0-10 mm
             float min = 4.0f - (tolerance / 1000);
             //check if the actual position of the player is within the range or not
             if ((userPosition >= min) && (userPosition <= 4.0f))
                 //if it is within the range then it will be true
                 return true;
             //if it is out of this range and/or the kinect range it will be false
             else return false;
         }

         /// <summary>
         /// This method checks whether the user satisfied the commands or not.
         /// </summary>
         /// <param name="command">Which is the name of the command that should be satisfied.</param>
         /// <param name="positions">A list containing the positions of the user.</param>
         /// <param name="tolerance">The tolerance level.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 26/4/12 </para>
         /// </remarks>
         public static bool CommandSatisfied(String command, List<float> positions, float tolerance)
         {
             bool result = true;
             float currentTolerance = tolerance;

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
         /// Checks if the user applied the command of Constant Velocity or not.
         /// </summary>
         /// <param name="positions">List of the user positions throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 19/5/12 </para>
         /// </remarks>
         public static bool ConstantVelocity(List<float> positions, float currentTolerance)
         {
             bool result = true;
             float firstVelocity = positions[0];
             for (int i = 1; i < positions.Count; i++)
             {
                 float currentVelocity = positions[i];
                 if (!((currentVelocity >= (firstVelocity - currentTolerance)) && (currentVelocity <= (firstVelocity + currentTolerance))))
                 {
                     return false;
                 }
             }
             return result;
         }
         
         /// <summary>
         /// Checks if the user applied the command of Constant Acceleration or not.
         /// </summary>
         /// <param name="positions">List of the user positions throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 23/4/12 </para>
         /// </remarks>
         public static bool ConstantAcceleration(List<float> positions, float currentTolerance)
         {
             currentTolerance = currentTolerance * 10;
             List<float> velocities = GraphEngine.GetPlayerVelocity(positions);

             bool result = true;
             float firstVelocity = velocities[1] - velocities[0];
             for (int i = 2; i < velocities.Count; i++)
             {
                 float currentVelocity = (velocities[i] - velocities[i - 1]);
                 if (!((currentVelocity >= (firstVelocity - currentTolerance)) && (currentVelocity <= (firstVelocity + currentTolerance))))
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
         /// Checks if the user applied the command of Constant Displacement or not.
         /// </summary>
         /// <param name="positions">List of the user positions throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 18/5/12 </para>
         /// </remarks>
         public static bool ConstantDisplacement(List<float> positions, float currentTolerance)
         {
             //bool result = true;
             //float firstDisplacement = positions[0];
             //for (int i = 1; i < positions.Count; i++)
             //{
             //    float currentDisplacement = positions[i];
             //    if (!((currentDisplacement >= (firstDisplacement - currentTolerance)) && (currentDisplacement <= (firstDisplacement + currentTolerance))))
             //    {
             //        if (positions[positions.Count - 1] == 0.8)
             //        {
             //            result = true;
             //            break;
             //        }
             //        else
             //        {
             //            result = false;
             //            break;
             //        }
             //    }
             //}
             //return result;
             return (positions.Count == 0);
         }

         /// <summary>
         /// Checks if the user applied the command of Increasing Acceleration or not.
         /// </summary>
         /// <param name="positions">List of the user positions throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 26/4/12 </para>
         /// </remarks>
         public static bool IncreasingAcceleration(List<float> positions, float currentTolerance)
         {
             currentTolerance = currentTolerance * 100;
             List<float> accelerations = GraphEngine.GetPlayerAcceleration(GraphEngine.GetPlayerVelocity(positions));

             bool result = true;
             for (int i = 1; i < accelerations.Count; i++)
             {
                 float currentAcceleration = accelerations[i];
                 if (!(currentAcceleration > (accelerations[i - 1] - currentTolerance)))
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
         /// Checks if the user applied the command of Decreasing Acceleration or not.
         /// </summary>
         /// <param name="positions">List of the user positions throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 26/4/12 </para>
         /// </remarks>
         public static bool DecreasingAcceleration(List<float> positions, float currentTolerance)
         {
             currentTolerance = currentTolerance * 100;
             List<float> accelerations = GraphEngine.GetPlayerAcceleration(GraphEngine.GetPlayerVelocity(positions));

             bool result = true;
             for (int i = 1; i < accelerations.Count; i++)
             {
                 float currentAcceleration = accelerations[i];
                 if (!(currentAcceleration < (accelerations[i - 1] + currentTolerance)))
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
        /// Gets Y position of the Knee every frame and returns the knee speed at the equivalent Frame
        /// </summary>
        /// <param name="kneeposition">Y coordinate of the knee position from kinect</param>
         /// <returns>knee speed at the equivalent Frame</returns>
         /// <remarks>
         /// <para>AUTHOR: Safty</para>
         /// <para>DATE WRITTEN: 15/5/12 </para>
         /// <para>DATE MODIFIED: 15/5/12 </para>
         /// </remarks>
         public static void getKneespeed(List<float> kneeposition, List<float> playerdisplacement)
         {

             if (kneeposition.Count() == 0 || kneeposition.Count() == 1)
             {
                 //do nothing
             }
             if (kneeposition.Count() == 2)
             {
                 

             }
             else
             {

             }


             return;
         }
         
        /// <summary>
        /// converts kneespeed list to displacement list
        /// </summary>
        /// <param name="kneespeed">Speed of the knee on specific frame</param>
        /// <returns>linear displacement which the avatar should move on screen</returns>
        /// <remarks>
        /// <para>AUTHOR: Safty</para>
        /// <para>DATE WRITTEN: 15/5/12 </para>
        /// <para>DATE MODIFIED: 15/5/12 </para>
        /// </remarks>
         public static List<float> TransitionalDisplacment(List<float> kneespeed)
         {
             return null;
         }














        }

    }

