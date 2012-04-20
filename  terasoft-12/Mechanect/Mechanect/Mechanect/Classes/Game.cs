using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;

namespace Mechanect.Classes
{
    class Game
    {  
        private User Winner;
        private List<String> CommandsTime;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<float> positionsList = new List<float>();
        int initialPosition;
        int tolerance = 500;

        List<GameCommands> currentCommands;

        User user1;
        User user2;

        /// <summary>
        /// a timer to check the number of seconds
        /// </summary>
        int timer = 0;

        /// <summary>
        /// the number of frames taken from the kinect
        /// </summary>
        int frame = 0;

        /// <summary>
        /// Text management
        /// </summary>
        SpriteFont spFont;

        public Game(Game1 game, User user1, User user2)
        {
            graphics = new GraphicsDeviceManager(game);
            this.user1 = user1;
            this.user2 = user2;
        }


        /// <summary>
        /// this method should be called on each update and it will do the check on both players to see if they followed the commands
        /// </summary>
        public void CheckEachSecond()
        {
            int second = timer % 60;
            if (second == 0)
            {
                List<float> user1Displacement = new List<float>(30);
                List<float> user2Displacement = new List<float>(30);
                for (int i = (timer / 2) - 30; i < timer / 2; i++)
                {
                    user1Displacement.Add(user1.Positions[i]);
                    user2Displacement.Add(user2.Positions[i]);
                }
                if (!CommandSatisfied(currentCommands[user1.ActiveCommand].Name, user1Displacement))
                {
                    user1.Disqualified = true;
                    user1.DisqualificationTime = second;
                    spriteBatch.Begin();
                    spriteBatch.DrawString(spFont, "user 1 got Disqualified", new Vector2(50.0f, 50.0f), Color.Red);
                    spriteBatch.End();
                }
                if (!CommandSatisfied(currentCommands[user2.ActiveCommand].Name, user2Displacement))
                {
                    user2.Disqualified = true;
                    user2.DisqualificationTime = second;
                    spriteBatch.Begin();
                    spriteBatch.DrawString(spFont, "user 2 got Disqualified", new Vector2(50.0f, 50.0f), Color.Red);
                    spriteBatch.End();
                }
            }

            timer++;
        }

        /// <summary>
        /// this method should be called at the beginning of the race to set the players' position
        /// </summary>
        public void SetPositions()
        {
            Skeleton sk1 = user1.USER;
            Skeleton sk2 = user2.USER;

            if (frame % 2 == 1)
            {
                float z;
                float z2;
                bool isThePositionRight;
                bool isThePositionRight2;
                String user1State = "";
                String user2State = "";
                if (sk1 != null)
                {
                    z = (float)sk1.Position.Z;
                    isThePositionRight = CheckPosition(z);

                    user1State = "User 1: Your position is: " + z.ToString() + " this is " + isThePositionRight + ", it should be 4.0 m \n";
                    frame += 1;
                }
                if (sk2 != null)
                {
                    z2 = (float)sk2.Position.Z;
                    isThePositionRight2 = CheckPosition(z2);

                    user2State = "User 2: Your position is: " + z2.ToString() + " this is " + isThePositionRight2 + ", it should be 4.0 m \n";
                    frame += 1;
                }
                spriteBatch.Begin();
                spriteBatch.DrawString(spFont, user1State, new Vector2(50.0f, 50.0f), Color.Red);
                spriteBatch.DrawString(spFont, user2State, new Vector2(50.0f, 50.0f), Color.Blue);
                spriteBatch.End();
            }
            else
            {
                frame += 1;
            }
        }

        /// <summary>
        /// this method checks whether the user is standing in about 4m or not(with certain tolerance)
        /// </summary>
        /// <param name="userPosition">the position that the user is currently standing at</param>
        /// <returns></returns>
        public bool CheckPosition(float userPosition)
        {
            //initiate the minimum distance from the Kinect which means
            //the maximum distance between the two players which is actually the tolerance on a scale from 0-10 mm
            float min = 4.0f - (tolerance / 100);
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
        /// <returns></returns>
        public bool CommandSatisfied(String command, List<float> positions)
        {
            bool result = true;
            if (command.Equals("Constant Speed"))
            {
                float firstDistance = positions[1] - positions[0];
                for (int i = 2; i < positions.Count; i++)
                {
                    float currentDisplacement = (positions[i] - positions[i - 1]);
                    if (!(currentDisplacement >= (firstDistance - tolerance) && currentDisplacement <= (firstDistance + tolerance)))
                        result = false;
                }
            }
            return result;
        }

        
    }
}
