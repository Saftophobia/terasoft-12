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
    class GraphUI
    {
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawRange loops over each instance of the PerformanceGraph class initialized in the function
        /// setDestinations and connects each initial point with each final point.
        /// </summary>
        /// <param name="spriteBatch">An instance of the SpriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void DrawRange(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, PerformanceGraph g)
        {
            for (int i = 0; i <= g.getDisplacement1().Length - 1; i++)
            {
                g.getDisplacement1()[i].Draw(spriteBatch, GraphicsDevice);
                g.getDisplacement2()[i].Draw(spriteBatch, GraphicsDevice);
                g.getVelocity1()[i].Draw(spriteBatch, GraphicsDevice);
                g.getVelocity2()[i].Draw(spriteBatch, GraphicsDevice);
                g.getAcceleration1()[i].Draw(spriteBatch, GraphicsDevice);
                g.getAcceleration2()[i].Draw(spriteBatch, GraphicsDevice);
                g.getOptimumDisplacement()[i].Draw(spriteBatch, GraphicsDevice);
                g.getOptimumVelocity()[i].Draw(spriteBatch, GraphicsDevice);
                g.getOptimumAcceleration()[i].Draw(spriteBatch, GraphicsDevice);
            }
        }
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawLabels is used to add a label for each axis indicating whether each graph represents displacement or velocity or acceleration.
        /// </summary>
        /// <param name="spriteBatch">An instance of the SpriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <returns>void</returns>
        public static void DrawLabels(SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font)
        {
            spriteBatch.DrawString(font, "Displacement", new Vector2(5, 20), Color.Black);
            spriteBatch.DrawString(font, "Velocity", new Vector2(340, 20), Color.Black);
            spriteBatch.DrawString(font, "Acceleration", new Vector2(640, 20), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(270, 325), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(600, 325), Color.Black);
            spriteBatch.DrawString(font, "Time", new Vector2(930, 325), Color.Black);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawAxis is used to draw the X and Y axis for each graph.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void DrawAxis(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 50),
                new Vector2(50, 570));
            g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 50),
                new Vector2(380, 570));
            g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 50),
                new Vector2(710, 570));
            g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 300),
                new Vector2(316, 300));
            g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 300),
                new Vector2(646, 300));
            g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 300),
                new Vector2(976, 300));
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawArrows is used to add an arrow at the end of each axis for each graph.
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void DrawArrows(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            int counter = 309;
            for (int i = 0; i <= 2; i++)
            {
                g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 293),
                new Vector2(counter + 7, 300));
                g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 307),
                new Vector2(counter + 7, 300));
                counter += 330;
            }
            counter = 40;
            for (int i = 0; i <= 2; i++)
            {
                g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 60),
                new Vector2(counter + 10, 50));
                g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 60),
                new Vector2(counter + 8, 50));
                g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 560),
                new Vector2(counter + 10, 570));
                g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 560),
                new Vector2(counter + 8, 570));
                counter += 330;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawXLabels is used to add the values to be displayed on the X-axis.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public static void DrawXLabels(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            int counter = 35;
            for (int i = 0; i <= 2; i++)
            {
                switch (i)
                {
                    case 1: counter = 365; break;
                    case 2: counter = 695; break;
                    default: counter = 35; break;
                }
                for (int j = 0; j <= 4; j++)
                {
                    string formatted = g.GetXAxis()[j].ToString("N2");
                    spriteBatch.DrawString(font2, formatted + "", new Vector2(counter - 5, 308), Color.Black);
                    counter += 67;
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawYLabels is used to add the values to be displayed on the Y-axis.
        /// </summary>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public static void DrawYLabels(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            //+ve part of the y-axis
            int y = 0;
            double[] current = g.YAxisDis(); ;
            for (int i = 0; i <= 2; i++)
            {
                int counter = 60;
                switch (i)
                {
                    case 1: y = 320; current = g.YAxisVel(); break;
                    case 2: y = 650; current = g.YAxisAcc(); break;
                    default: y = 0; current = g.YAxisDis(); break;
                }
                for (int j = 4; j >= 1; j--)
                {
                    spriteBatch.DrawString(font2, (int)current[j] + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
            //-ve part of the y-axis
            for (int i = 0; i <= 2; i++)
            {
                int counter = 350;
                switch (i)
                {
                    case 1: y = 320; current = g.YAxisVel(); break;
                    case 2: y = 650; current = g.YAxisAcc(); break;
                    default: y = 0; current = g.YAxisDis(); break;
                }
                for (int j = 1; j <= 4; j++)
                {
                    spriteBatch.DrawString(font2, -1 * (int)current[j] + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawEnvironment calls the necessary functions to draw the X and Y axis with their labels for each graph on the screen
        /// </summary>
        /// <param name="spriteBatch">An instance of the Spritebatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>
        /// <param name="font">The spritefont "Myfont1.spritefont".</param>
        /// <param name="font2">The spritefont "Myfont2.spritefont".</param>
        /// <returns>void</returns>
        public static void DrawEnvironment(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice, SpriteFont font, SpriteFont font2)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            DrawAxis(g, spriteBatch, GraphicsDevice);
            DrawLabels(spriteBatch, GraphicsDevice, font);
            DrawArrows(g, spriteBatch, GraphicsDevice);
            DrawXLabels(g, spriteBatch, GraphicsDevice, font2);
            DrawYLabels(g, spriteBatch, GraphicsDevice, font2);
            //drawing the marks on the X-axis
            int counter = 50;
            for (int i = 0; i <= 2; i++)
            {
                switch (i)
                {
                    case 1: counter = 380; break;
                    case 2: counter = 710; break;
                    default: counter = 50; break;
                }
                for (int j = 0; j <= 4; j++)
                {
                    g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 295),
                        new Vector2(counter, 305));
                    counter += 64;
                }
            }
            //drawing the marks on the Y-axis
            int y = 50;
            for (int i = 0; i <= 2; i++)
            {
                int counter2 = 68;
                switch (i)
                {
                    case 1: y = 380; break;
                    case 2: y = 710; break;
                    default: y = 50; break;
                }
                for (int j = 1; j <= 9; j++)
                {
                    g.DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(y - 7, counter2),
                        new Vector2(y + 5, counter2));
                    counter2 += 58;
                }
            }
            //drawing the legend
            g.DrawLine(spriteBatch, blank, 3, Microsoft.Xna.Framework.Color.Red, new Vector2(130, 580),
                new Vector2(180, 580));
            g.DrawLine(spriteBatch, blank, 3, Microsoft.Xna.Framework.Color.Blue, new Vector2(460, 580),
                new Vector2(510, 580));
            g.DrawLine(spriteBatch, blank, 3, Microsoft.Xna.Framework.Color.Yellow, new Vector2(790, 580),
                new Vector2(840, 580));
            spriteBatch.DrawString(font, "Player 1", new Vector2(185, 573), Color.Red);
            spriteBatch.DrawString(font, "Player 2", new Vector2(515, 573), Color.Blue);
            spriteBatch.DrawString(font, "Optimum", new Vector2(845, 573), Color.Yellow);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 14/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawDisqualification is used to add a mark on the point where a player got disqualified, the function
        /// first decides the mark's X-coordinate then uses it to derive its Y-coordinate before representing it on each graph. 
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class.</param>
        /// <param name="dwidth">The width of the screen.</param>
        /// <param name="dheight">The height of the screen.</param> 
        /// <param name="P1Tex">A Texture2D representing the image "xRed.png".</param>
        /// <param name="P2Tex">A Texture2D representing the image "xBlue.png".</param>
        /// <param name="player1disqtime">The instance when player 1 was disqualified.</param>
        /// <param name="player2disqtime">The instance when player 2 was disqualified</param>
        /// <returns>void</returns>
        public static void DrawDisqualification(PerformanceGraph g, SpriteBatch spriteBatch, int dwidth, int dheight, Texture2D P1Tex, Texture2D P2Tex, double player1disqtime, double player2disqtime)
        {
            if (player1disqtime > 0 || player2disqtime > 0)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Boolean t = false;
                    double n = 0;
                    switch (j)
                    {
                        case 0: n = player1disqtime; if (n >= 0) { t = true; }; break;
                        case 1: n = player2disqtime; if (n >= 0) { t = true; }; break;
                    }
                    if (t)
                    {
                        double time = g.getTotalTime();
                        int index = 8;
                        for (int i = 0; i <= g.getChosen().Length - 1; i++)
                        {
                            if (i < g.getChosen().Length - 1)
                            {
                                double d1 = ((double)g.getChosen()[i] / (double)12);
                                double d2 = ((double)g.getChosen()[i + 1] / (double)12);
                                if (n >= d1 && n < d2)
                                {
                                    double x = d1 + ((double)(d2 - d1) / (double)2);
                                    if (n < x)
                                    {
                                        time = (double)g.getChosen()[i] / (double)12;
                                        index = i;
                                    }
                                    else
                                    {
                                        time = (double)g.getChosen()[i + 1] / (double)12;
                                        index = i + 1;
                                    }
                                }
                            }
                        }
                        int y1 = 0; int y2 = 0; int y3 = 0;
                        double r1 = (double)g.getTotalTime() / (double)256;
                        double r2 = (double)(time) / (double)r1;
                        int r3 = 40 + (int)r2;
                        Texture2D texture = null;
                        switch (j)
                        {
                            case 0: y1 = g.getP1DispGraph()[index] - 8; y2 = g.getP1VelGraph()[index] - 8; y3 = g.getP1AccGraph()[index] - 8; texture = P1Tex; break;
                            case 1: y1 = g.getP2DispGraph()[index] - 8; y2 = g.getP2VelGraph()[index] - 8; y3 = g.getP2AccGraph()[index] - 8; texture = P2Tex; break;
                        }
                        CountDown xDP = new CountDown();
                        CountDown xVP = new CountDown();
                        CountDown xAP = new CountDown();
                        xDP = new CountDown(texture, dwidth, dheight, r3, y1, 20, 20);
                        xDP.Draw(spriteBatch);
                        r3 = 370 + (int)r2;
                        xVP = new CountDown(texture, dwidth, dheight, r3, y2, 20, 20);
                        xVP.Draw(spriteBatch);
                        r3 = 700 + (int)r2;
                        xAP = new CountDown(texture, dwidth, dheight, r3, y3, 20, 20);
                        xAP.Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
