﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        int stagewidth, stageheight;
        int a1;
        int a2;
        int a3;
        int a4;
        int finishx;
        int finishy;
        Boolean finish = false;
        Boolean directionUp = false;
        Boolean sameDirection = false;
        Color curveColor;
        List<int> Player1Displacement;
        List<int> Player2Displacement;        
        List<String> CommandsList;
        //CommandsList is a list represening each command given during the race
        List<double> TimeSpaces;
        //TimeSpaces is a List representing the number of seconds elapsed by each command        
        Game currentGame;
        double totalTime;//total race time
        int[] chosenTimings;
        int[] chosendisp1 = new int[9];//y-axis
        int[] chosendisp2 = new int[9];
        PerformanceGraph[] disp1 = new PerformanceGraph[8];
        PerformanceGraph[] disp2 = new PerformanceGraph[8];      
        List<int> P1DispGraph = new List<int>();
        List<int> P2DispGraph = new List<int>();
        double[] xaxis = new double[5];
        double[] yaxisDisplacement = new double[5];
        CountDown xDP1;
        CountDown xDP2;

        public PerformanceGraph(int start1, int start2, int finishx, int finishy, int a, int b, Color col)
        {
            a1 = start1;
            a2 = start2;
            a3 = start1;
            a4 = start2;
            curveColor = col;
            this.finishx = finishx;
            this.finishy = finishy;
            stagewidth = a;
            stageheight = b;
            if (finishy < start2)
            {
                directionUp = true;
            }
            if (finishy == start2)
            {
                sameDirection = true;
            }

        }

        public PerformanceGraph()
        {
        }



        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The Draw function is used to draw a line connecing the points (a1,a2) and (b1,b2) when called
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void Draw(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            //spriteBatch.Begin();
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            drawLine(spriteBatch, blank, 2, curveColor, new Vector2(a1, a2),
                new Vector2(a3, a4));
            //spriteBatch.End();
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The Update function is used to increment the final point (a3,a4) till the x and y
        /// co-ordinates of reach the final point reach the specified values
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void Update(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            if (!sameDirection)
            {
                if (directionUp)
                {
                    if (a3 <= finishx)
                    {
                        a3 = a3 + 3;
                    }
                    if (a4 >= finishy)
                    {
                        a4 = a4 - 4;
                    }
                    if (a4 < finishy && a3 > finishx)
                    {
                        finish = true;
                    }
                }
                else
                {
                    if (a3 <= finishx)
                    {
                        a3 = a3 + 3;
                    }
                    if (a4 <= finishy)
                    {
                        a4 = a4 + 4;
                    }
                    if (a4 > finishy && a3 > finishx)
                    {
                        finish = true;
                    }

                }
            }
            else
            {
                if (a3 <= finishx)
                {
                    a3 = a3 + 3;
                }
                if (a3 > finishx)
                {
                    finish = true;
                }
            }
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The getFinish function is used to get the boolean Finish from the
        /// PerformanceGraph class
        /// </summary>
        /// <param ></param>      
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>Boolean: a boolean stating whether the line has reached its final 
        /// destination or not</returns>
        public Boolean getFinish()
        {
            return finish;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The drawLine function is used to draw a straight line connecting
        /// an initial point with a final point
        /// </summary>
        /// <param name="batch">An instance of the spriteBatch class</param>
        /// <param name="blank">An instance of the Texture2D class</param>
        /// <param name="width">The width of the line</param>
        /// <param name="color">The color of the line</param>
        /// <param name="point1">The initial point</param>
        /// <param name="point2">The final point</param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void drawLine(SpriteBatch batch, Texture2D blank,
              float width, Microsoft.Xna.Framework.Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 18/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function drawGraphs calls the methods GetPlayerVelocity, GetPlayerAcceleration
        /// GetOptimum in order to derive each player's velocity, acceleration and the optimum
        /// values, then calls the choose, setMaximum, setDestinations, setAxis functions in 
        /// order to display the graph on the screen
        /// </summary>
        /// <param name="Player1Displacement">A list holding Player 1's displacements</param>
        /// <param name="Player2Displacement">A list holding Player 2's displacements</param>
        /// <param name="Commands">A list holding each command initiated during the race</param>
        /// <param name="time">A list holding the time elapsed by each command</param>
        /// <param name="g1">An instance of the game class</param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void drawGraphs(List<int> Player1Displacement, List<int> Player2Displacement,
           List<String> Commands, List<double> time, Game g1)
        {
            this.Player1Displacement = Player1Displacement;
            this.Player2Displacement = Player2Displacement;
            this.CommandsList = Commands;
            this.TimeSpaces = time;
            this.currentGame = g1;
           
            choose();
            
            setDestinations(g1.getGraphicsDeviceManager());
            setAxis();
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function choose is used to choose certain times according to the race time and
        /// chooses the values of displacement, velocity and acceleration for each player at the
        /// chosen times in order to represent them on the graph
        /// </summary>
        /// <param></param> 
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void choose()
        {
            double acc = 0;
            for (int i = 0; i <= TimeSpaces.Count - 1; i++)
            {
                acc += TimeSpaces[i];
            }
            totalTime = (int)acc;//to be changed
            chosenTimings = new int[9];
            int timeCounter = 0;
            for (int i = 0; i <= chosenTimings.Length - 1; i++)
            {
                chosenTimings[i] = (int)(30 * totalTime * ((double)timeCounter / (double)8));
                timeCounter++;
            }
            for (int i = 0; i <= chosendisp1.Length - 1; i++)
            {
                if (i > 0)
                {
                    chosendisp1[i] = Player1Displacement[chosenTimings[i] - 1];
                    chosendisp2[i] = Player2Displacement[chosenTimings[i] - 1];
                }
                else
                {
                    chosendisp1[i] = Player1Displacement[chosenTimings[i]];
                    chosendisp2[i] = Player2Displacement[chosenTimings[i]];
                }
            }
        }





        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function setMaximum is used to derive the maximum velocity and the maximum acceleration
        /// of both players during the race in order to set the maximum point on each graph's y-axis according
        /// to these values
        /// </summary>
        /// <param></param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        


        public void setDestinations(GraphicsDeviceManager graphics)
        {
            int counter1 = 0;
            for (int j = 0; j <= 1; j++)
            {
                if (j <= 1)
                {
                    counter1 = 50;
                }
                
                for (int i = 0; i <= 7; i++)
                {
                    int value = 0;
                    double r = 0;
                    if (j <= 1)
                    {
                        value = 4000;
                        r = (double)value / (double)232;
                        if (j == 0)
                        {
                            double r2 = (double)(chosendisp1[i]) / (double)r;
                            int r3 = 118 + (int)r2;
                            double r4 = (double)(chosendisp1[i + 1]) / (double)r;
                            int r5 = 118 + (int)r4;
                            disp1[i] = new PerformanceGraph(counter1, r3, counter1 + 30, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Blue);
                            counter1 = counter1 + 30;
                            if (i == 0)
                            {
                                P1DispGraph.Add(r3);
                            }
                            P1DispGraph.Add(r5);
                        }
                        if (j == 1)
                        {
                            double r2 = (double)(chosendisp2[i]) / (double)r;
                            int r3 = 118 + (int)r2;
                            double r4 = (double)(chosendisp2[i + 1]) / (double)r;
                            int r5 = 118 + (int)r4;
                            disp2[i] = new PerformanceGraph(counter1, r3, counter1 + 30, r5, graphics.PreferredBackBufferWidth,
                                graphics.PreferredBackBufferHeight, Color.Black);
                            counter1 = counter1 + 30;
                            if (i == 0)
                            {
                                P2DispGraph.Add(r3);
                            }
                            P2DispGraph.Add(r5);
                        }
                    }
                }
            }
        }

        public void setAxis()
        {
            xaxis[0] = 0;
            double step = (double)totalTime / (double)4;
            for (int i = 1; i <= xaxis.Length - 1; i++)
            {
                xaxis[i] = xaxis[i - 1] + step;
            }
            int counter = 0;
            for (int i = 0; i <= 4; i++)
            {
                yaxisDisplacement[i] = counter;
                counter += 1000;
            }
            
        }




        public void drawRange(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice) //to be called in the draw function
        {
            for (int i = 0; i <= disp1.Length - 1; i++)
            {
                int counter = 0;
                for (int j = 0; j <= i; j++)
                {
                    if (disp1[j].getFinish())
                    {
                        counter++;
                    }
                }
                if (counter == i)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        disp1[k].Draw(spriteBatch, GraphicsDevice);
                        disp2[k].Draw(spriteBatch, GraphicsDevice);
                    }
                }
            }
            for (int i = 0; i <= disp1.Length - 1; i++)
            {
                if (disp1[i].getFinish())
                {
                    disp1[i].Draw(spriteBatch, GraphicsDevice);
                    disp2[i].Draw(spriteBatch, GraphicsDevice);
                }
            }
        }

        public void drawAxis(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice
            , SpriteFont font, SpriteFont font2)//to be called in the draw function
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });

            //yaxis
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(50, 100),
                new Vector2(50, 620));
           

            //xaxis
            drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(50, 350),
                new Vector2(300, 350));
            

            //labels  
            spriteBatch.DrawString(font, "Displacement", new Vector2(5, 70), Color.Red);

            spriteBatch.DrawString(font, "Time", new Vector2(270, 380), Color.Red);

            int count = 35;
            int count2 = 300;
            int count3 = 290;
            int count4 = 40;
            int count5 = 45;
            for (int j = 0; j <1; j++)
            {
                for (int i = 0; i <= 4; i++)
                {
                    if (i == 0)
                    {
                        spriteBatch.DrawString(font, xaxis[i] + "", new Vector2(count - 5, 355), Color.Red);
                        count = count + 60;
                    }
                    if (i > 0)
                    {
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count + 15, 345),
                        new Vector2(count + 15, 355));
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count5, count3 + 8),
                        new Vector2(count5 + 10, count3 + 8));
                        spriteBatch.DrawString(font, xaxis[i] + "", new Vector2(count + 5, 355), Color.Red);
                        count = count + 60;
                    }
                    if (j == 0 && i > 0)
                    {
                        spriteBatch.DrawString(font2, yaxisDisplacement[i] + "", new Vector2(0, count3), Color.Red);
                        count3 = count3 - 60;
                    }

                }
                count3 = 290;
                //arrows
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count2, 345),
                new Vector2(count2 + 5, 350));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count2 - 1, 356),
                new Vector2(count2 + 5, 350));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count4 + 2, 105),
                new Vector2(count4 + 8, 99));
                drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(count4 + 16, 105),
                new Vector2(count4 + 9, 99));
                if (j == 0)
                {
                    count = 365;
                    count2 = 630;
                    count4 = 370;
                    count5 = 375;
                }
                if (j == 1)
                {
                    count = 695;
                    count2 = 960;
                    count4 = 700;
                    count5 = 705;
                }
            }

            double[] negativeDisp = new double[4];
            double[] negativeVel = new double[4];
            double[] negativeAcc = new double[4];

            int n = 1;
            for (int i = 0; i <= 3; i++)
            {
                negativeDisp[i] = yaxisDisplacement[n] * -1;
                n++;
            }

            count = 410;
            for (int i = 0; i <1; i++)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (i == 0)
                    {
                        spriteBatch.DrawString(font2, negativeDisp[j] + "", new Vector2(0, count), Color.Red);
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(45, count + 8),
                new Vector2(55, count + 8));
                    }
                    if (i == 1)
                    {
                        spriteBatch.DrawString(font2, negativeVel[j] + "", new Vector2(320, count), Color.Red);
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(375, count + 8),
                new Vector2(385, count + 8));
                    }
                    if (i == 2)
                    {
                        spriteBatch.DrawString(font2, negativeAcc[j] + "", new Vector2(650, count), Color.Red);
                        drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(705, count + 8),
                new Vector2(715, count + 8));
                    }
                    count += 60;
                }
                if (i == 0)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(42, 612),
                new Vector2(50, 620));
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(58, 612),
                new Vector2(50, 620));
                }
                if (i == 1)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(372, 612),
                new Vector2(380, 620));
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(388, 612),
                new Vector2(380, 620));
                }
                if (i == 2)
                {
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(702, 612),
                new Vector2(710, 620));
                    drawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Red, new Vector2(718, 612),
                new Vector2(710, 620));
                }
                count = 410;
            }
        }

        public void drawConnectors()
        {
            //to be called first
            //to be called first
            //to be called first
            //to be called first
            //to be called first
        }


        public void drawDisqualification(SpriteBatch spriteBatch, GraphicsDeviceManager graphics, int timer
            , Texture2D P1Tex, Texture2D P2Tex)
        {
            double player1DisqualificationTime = 1;
            double player2DisqualificationTime = 1;

            if (player1DisqualificationTime > 0)
            {
                double time = totalTime;
                int index = 8;
                for (int i = 0; i <= chosenTimings.Length - 1; i++)
                {
                    if (i < chosenTimings.Length - 1)
                    {
                        double d1 = ((double)chosenTimings[i] / (double)30);
                        double d2 = ((double)chosenTimings[i + 1] / (double)30);
                        if (player1DisqualificationTime >= d1 && player1DisqualificationTime < d2)
                        {
                            double x = d1 + ((double)(d2 - d1) / (double)2);
                            if (player1DisqualificationTime < x)
                            {
                                time = (double)chosenTimings[i] / (double)30;
                                index = i;
                            }
                            else
                            {
                                time = (double)chosenTimings[i + 1] / (double)30;
                                index = i + 1;
                            }
                        }
                    }
                }
                int y = P1DispGraph[index] - 10;
                double r1 = (double)totalTime / (double)240;
                double r2 = (double)(time) / (double)r1;
                int r3 = 40 + (int)r2;
                xDP1 = new CountDown(P1Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);
                if (timer >= 200)
                {
                    xDP1.Draw(spriteBatch);
                }
                

            }
            if (player2DisqualificationTime > 0)
            {
                double time = totalTime;
                int index = 8;
                for (int i = 0; i <= chosenTimings.Length - 1; i++)
                {
                    if (i < chosenTimings.Length - 1)
                    {
                        double d1 = ((double)chosenTimings[i] / (double)30);
                        double d2 = ((double)chosenTimings[i + 1] / (double)30);
                        if (player2DisqualificationTime >= d1 && player2DisqualificationTime < d2)
                        {
                            double x = d1 + ((double)(d2 - d1) / (double)2);
                            if (player2DisqualificationTime < x)
                            {
                                time = (double)chosenTimings[i] / (double)30;
                                index = i;
                            }
                            else
                            {
                                time = (double)chosenTimings[i + 1] / (double)30;
                                index = i + 1;
                            }
                        }
                    }
                }
                int y = P2DispGraph[index] - 10;
                double r1 = (double)totalTime / (double)240;
                double r2 = (double)(time) / (double)r1;
                int r3 = 40 + (int)r2;
                xDP2 = new CountDown(P2Tex, graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, r3, y, 20, 20);
                if (timer >= 220)
                {
                    xDP2.Draw(spriteBatch);
                }
                
            }
        }


        public void updateCurve(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)// to be called in the update function
        {
            for (int k = 0; k <= 1; k++)
            {
                PerformanceGraph[] array = new PerformanceGraph[8];

                switch (k)
                {
                    case 0: array = disp1; break;
                    case 1: array = disp2; break;
                }
                for (int i = 0; i <= array.Length - 1; i++)
                {
                    int counter = 0;
                    for (int j = 0; j <= i; j++)
                    {
                        if (array[j].getFinish())
                        {
                            counter++;
                        }
                    }
                    if (counter == i)
                    {
                        array[i].Update(spriteBatch, GraphicsDevice);
                    }
                }
            }
        }

        }
    }