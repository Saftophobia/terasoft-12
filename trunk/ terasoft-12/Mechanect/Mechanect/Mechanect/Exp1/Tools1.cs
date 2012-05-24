using System;
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
         /// <para>DATE MODIFIED: 20/5/12 </para>
         /// </remarks>
         public static void GetWinner(User1 user1, User1 user2, float raceLength)
         {
             user1.Winner = (user1.Positions.Sum() >= raceLength) ;
             user2.Winner = (user2.Positions.Sum() >= raceLength) ;
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
         /// <para>DATE MODIFIED: 22/5/12 </para>
         /// </remarks>
         public static void CheckTheCommand(float timeInSeconds, User1 user1, User1 user2, List<int> timeOfCommands, List<string> currentCommands, float tolerance)
         {
             float accumulativeTime = 0;
             for (int i = 0; i <= user1.ActiveCommand; i++)
                 accumulativeTime += timeOfCommands[i];

             if (!((accumulativeTime >= timeInSeconds - 0.01) && (accumulativeTime <= timeInSeconds + 0.01)))
                 return;

             float startCommandTime = 0;
             for (int i = 0; i < user1.ActiveCommand; i++)
                 startCommandTime += timeOfCommands[i];

             //get the start index of speed list
             UI.UILib.SayText(currentCommands[user1.ActiveCommand + 1]);
             int startIndexFor1 = GetStartIndex(user1, startCommandTime);
             int startIndexFor2 = GetStartIndex(user2, startCommandTime);

             //set the list of speeds
             List<float> speedsOf1 = GetSpeedsList(user1, startIndexFor1);
             List<float> speedsOf2 = GetSpeedsList(user2, startIndexFor2);
             List<float[]> velocitiesWithTimeOf1 = GetVelocitiesWithTime(user1, startIndexFor1);
             List<float[]> velocitiesWithTimeOf2 = GetVelocitiesWithTime(user2, startIndexFor2);

             //here the command is checked pver the two players to see if any of them got disqualified
             string s = "";
             if (!CommandSatisfied(currentCommands[user1.ActiveCommand], speedsOf1, tolerance, velocitiesWithTimeOf1))
             {
                 user1.Disqualified = true;
                 user1.DisqualificationTime = (int) timeInSeconds;
                 s += "User 1 got Disqualified \n";
                 Console.Write("User1 1 got Disqualified");
             }
             if (!CommandSatisfied(currentCommands[user2.ActiveCommand], speedsOf2, tolerance, velocitiesWithTimeOf2))
             {
                 user2.Disqualified = true;
                 user2.DisqualificationTime = (int) timeInSeconds;
                 s += "User 2 got Disqualified";
                 Console.Write("User 2 got Disqualified");
             }
         }

        /// <summary>
        /// Gets the list containing the velocities caught during this command.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="startIndex">The index of the array VelocityList to start from.</param>
        /// <returns>List<float>: The list containing the velocities</returns>
        /// <remarks>
        /// <para>AUTHOR: Michel Nader </para>
        /// <para>DATE WRITTEN: 24/5/12 </para>
        /// <para>DATE MODIFIED: 24/5/12 </para>
        /// </remarks>
         static List<float> GetSpeedsList(User1 user, int startIndex)
         {
             List<float> speeds = new List<float>();
             if ((startIndex < user.Velocitylist.Count) && (startIndex > -1))
                 for (int i = startIndex; i < user.Velocitylist.Count; i++)
                 {
                     speeds.Add(user.Velocitylist[i][0]);
                 }
             return speeds;
         }

         /// <summary>
         /// Gets the list containing the velocities caught during this command with their respective time.
         /// </summary>
         /// <param name="user">The user.</param>
         /// <param name="startIndex">The index of the array VelocityList to start from.</param>
         /// <returns>List<float>: The list containing the velocities and their respective time.</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 24/5/12 </para>
         /// <para>DATE MODIFIED: 24/5/12 </para>
         /// </remarks>
         static List<float[]> GetVelocitiesWithTime(User1 user, int startIndex)
         {
             List<float[]> velocitiesWithTime = new List<float[]>();
             if ((startIndex < user.Velocitylist.Count) && (startIndex > -1))
                 for (int i = startIndex; i < user.Velocitylist.Count; i++)
                 {
                     velocitiesWithTime.Add(user.Velocitylist[i]);
                 }
             return velocitiesWithTime;
         }

        /// <summary>
        /// Gets the start index of the array of velocities.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="startCommandTime">The when the command started.</param>
         /// <returns>int: The index of the array velicityList</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 24/5/12 </para>
         /// <para>DATE MODIFIED: 24/5/12 </para>
         /// </remarks>
         static int GetStartIndex(User1 user, float startCommandTime)
         {
             for (int i = 0; i < user.Velocitylist.Count; i++)
                 if ((startCommandTime + 1) <= user.Velocitylist[i][1])
                     return i;
             return -1;
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
         /// <param name="velocities">A list containing the velocities of the user.</param>
         /// <param name="currentTolerance">The tolerance level.</param>
         /// <param name="velocityListWithTime">A list containing the velocities of the user with its respective time.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/4/12 </para>
         /// <para>DATE MODIFIED: 26/4/12 </para>
         /// </remarks>
         public static bool CommandSatisfied(string command, List<float> velocities, float currentTolerance, List<float[]> velocityListWithTime)
         {
             if (command.Equals("constantVelocity"))
                 return ConstantVelocity(velocities, currentTolerance);
             else
                 if (command.Equals("constantAcceleration"))
                     return ConstantAcceleration(velocityListWithTime, currentTolerance);
                 else
                     if (command.Equals("constantDisplacement"))
                         return ConstantDisplacement(velocities);
                     else
                         if (command.Equals("increasingAcceleration"))
                             return IncreasingAcceleration(velocityListWithTime, currentTolerance);
                         else
                             if (command.Equals("decreasingAcceleration"))
                                 return DecreasingAcceleration(velocityListWithTime, currentTolerance);
             return true;
         }

        /// <summary>
        /// This method calculates the absolute acceleration value of the joint.
        /// </summary>
        /// <param name="velocities">The list of velocities and time of each velocity.</param>
        /// <returns>List<float>: The acceleration of the joint.</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 19/5/12 </para>
         /// <para>DATE MODIFIED: 24/5/12 </para>
         /// </remarks>
         public static List<float> GetAcceleration(List<float[]> velocities)
         {
             List<float> acc = new List<float>();
             if (velocities.Count > 2)
                 for (int i = 1; i < velocities.Count; i++)
                     acc.Add(Math.Abs(velocities[i][0] - velocities[i - 1][0]) / (velocities[i][1] - velocities[i - 1][1]));
             return acc;
         }

         /// <summary>
         /// Checks if the user applied the command of Constant Velocity or not.
         /// </summary>
         /// <param name="velocities">List of the user velocities throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false"</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 24/5/12 </para>
         /// </remarks>
         public static bool ConstantVelocity(List<float> velocities, float currentTolerance)
         {
             return CheckForConstants(velocities, currentTolerance);
         }
         
         /// <summary>
         /// Checks if the user applied the command of Constant Acceleration or not.
         /// </summary>
         /// <param name="velocitiesWithTime">List of the user velocities throughout the command with its respective time.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 24/5/12 </para>
         /// </remarks>
         public static bool ConstantAcceleration(List<float[]> velocitiesWithTime, float currentTolerance)
         {

             List<float> accelerations = GetAcceleration(velocitiesWithTime);
             return CheckForConstants(accelerations, currentTolerance);
         }

         static bool CheckForConstants(List<float> constantList, float tolerance)
         {
             for (int i = 1; i < constantList.Count; i++)
                 if (!((constantList[i] >= (constantList[i - 1] - tolerance)) &&
                     (constantList[i] <= (constantList[i - 1] + tolerance))))
                     return false;
             return true;
         }

         /// <summary>
         /// Checks if the user applied the command of Constant Displacement or not.
         /// </summary>
         /// <param name="velocities">List of the user velocities throughout the command.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 20/5/12 </para>
         /// </remarks>
         public static bool ConstantDisplacement(List<float> velocities)
         {
             return (velocities.Count <= 1);
         }

         /// <summary>
         /// Checks if the user applied the command of Increasing Acceleration or not.
         /// </summary>
         /// <param name="velocitiesWithTime">List of the user velocities throughout the command with its respective time.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 22/5/12 </para>
         /// </remarks>
         public static bool IncreasingAcceleration(List<float[]> velocitiesWithTime, float currentTolerance)
         {
             List<float> accelerations = GetAcceleration(velocitiesWithTime);

             for (int i = 1; i < accelerations.Count; i++)
                 if ((accelerations[i] <= (accelerations[i - 1] - currentTolerance))
                    return false;
             return (accelerations.Count != 0);
         }

         /// <summary>
         /// Checks if the user applied the command of Decreasing Acceleration or not.
         /// </summary>
         /// <param name="velocitiesWithTime">List of the user velocities throughout the command with its respective time.</param>
         /// <param name="currentTolerance">The tolerance of the game.</param>
         /// <returns>bool: If the command sent is satified then "true" else "false".</returns>
         /// <remarks>
         /// <para>AUTHOR: Michel Nader </para>
         /// <para>DATE WRITTEN: 23/4/12 </para>
         /// <para>DATE MODIFIED: 19/5/12 </para>
         /// </remarks>
         public static bool DecreasingAcceleration(List<float[]> velocitiesWithTime, float currentTolerance)
         {
             List<float> accelerations = GetAcceleration(velocitiesWithTime);

             for (int i = 1; i < accelerations.Count; i++)
                 if (accelerations[i] >= (accelerations[i - 1] + currentTolerance))
                     return false;
             return true;
         }

       

        }

    }

