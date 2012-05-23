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
    class GraphUI
    {
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The DrawLine function is used to draw a straight line connecting an initial point (point1) with a final point (point2).
        /// </summary>
        /// <param name="batch">An instance of the spriteBatch class.</param>
        /// <param name="blank">An instance of the Texture2D class.</param>
        /// <param name="width">The width of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="point1">The initial point.</param>
        /// <param name="point2">The final point.</param>        
        /// <returns>void</returns>
        public static void DrawLine(SpriteBatch batch, Texture2D blank, float width, Microsoft.Xna.Framework.Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The Draw function is used to draw a line connecing the points (a1,a2) and (b1,b2). 
        /// </summary>
        /// <param name="spriteBatch">An instance of the spriteBatch class.</param>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice class.</param>       
        /// <returns>void</returns>
        public static void Draw(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            DrawLine(spriteBatch, blank, 2, g.getCurveColor(), new Vector2(g.getPoint1().X, g.getPoint1().Y), new Vector2(g.getPoint2().X, g.getPoint2().Y));
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 22/5/2012</para>
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
            for (int n = 0; n <= 8; n++)
            {
                PerformanceGraph[] temp = new PerformanceGraph[10];
                switch (n)
                {
                    case 0: temp = g.getDisplacement1(); break;
                    case 1: temp = g.getDisplacement2(); break;
                    case 2: temp = g.getVelocity1(); break;
                    case 3: temp = g.getVelocity2(); break;
                    case 4: temp = g.getAcceleration1(); break;
                    case 5: temp = g.getAcceleration2(); break;
                    case 6: temp = g.getOptimumDisplacement(); break;
                    case 7: temp = g.getOptimumVelocity(); break;
                    case 8: temp = g.getOptimumAcceleration(); break;
                }
                for (int i = 0; i <= temp.Length - 2; i++)
                {
                    Draw(temp[i], spriteBatch, GraphicsDevice);
                }
            }
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/5/2012</para>
        /// <para>Date Modified 22/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function is used to determine the position where the texture/line/etc will be drawn.
        /// </summary>
        /// <param name="size">Width or Height of the screen.</param>
        /// <param name="value">Percentage to be multiplied by the size.</param>
        /// <returns>void</returns>
        public static int Percentage(int size, double value)
        {
            return (int)(size * (value / 100));
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
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
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
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 50),
                new Vector2(50, 570));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 50),
                new Vector2(380, 570));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 50),
                new Vector2(710, 570));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(50, 299),
                new Vector2(316, 299));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(380, 299),
                new Vector2(646, 299));
            DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(710, 299),
                new Vector2(976, 299));
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
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 293),
                new Vector2(counter + 7, 300));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 307),
                new Vector2(counter + 7, 300));
                counter += 330;
            }
            counter = 40;
            for (int i = 0; i <= 2; i++)
            {
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 60),
                new Vector2(counter + 10, 50));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 60),
                new Vector2(counter + 8, 50));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 560),
                new Vector2(counter + 10, 570));
                DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter + 10 + 8, 560),
                new Vector2(counter + 8, 570));
                counter += 330;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 25/4/2012</para>
        /// <para>Date Modified 22/5/2012</para>
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
        /// <para>Date Modified 22/5/2012</para>
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
            int y = 10;
            double[] current = g.YAxisDis(); ;
            for (int i = 0; i <= 2; i++)
            {
                int counter = 60;
                switch (i)
                {
                    case 1: y = 335; current = g.YAxisVel(); break;
                    case 2: y = 665; current = g.YAxisAcc(); break;
                    default: y = 10; current = g.YAxisDis(); break;
                }
                for (int j = 4; j >= 1; j--)
                {
                    spriteBatch.DrawString(font2, current[j].ToString("N2") + "", new Vector2(y, counter), Color.Black);
                    counter += 60;
                }
            }
            //-ve part of the y-axis
            for (int i = 0; i <= 2; i++)
            {
                int counter = 350;
                switch (i)
                {
                    case 1: y = 335; current = g.YAxisVel(); break;
                    case 2: y = 665; current = g.YAxisAcc(); break;
                    default: y = 10; current = g.YAxisDis(); break;
                }
                for (int j = 1; j <= 4; j++)
                {
                    double n = -1 * current[j];
                    spriteBatch.DrawString(font2, n.ToString("N2") + "", new Vector2(y, counter), Color.Black);
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
                    DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(counter, 295), new Vector2(counter, 305));
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
                    DrawLine(spriteBatch, blank, 2, Microsoft.Xna.Framework.Color.Black, new Vector2(y - 7, counter2), new Vector2(y + 5, counter2));
                    counter2 += 58;
                }
            }
            //drawing the legend
            DrawLine(spriteBatch, blank, 3, Microsoft.Xna.Framework.Color.Red, new Vector2(130, 580),
                new Vector2(180, 580));
            DrawLine(spriteBatch, blank, 3, Microsoft.Xna.Framework.Color.Blue, new Vector2(460, 580),
                new Vector2(510, 580));
            DrawLine(spriteBatch, blank, 3, Microsoft.Xna.Framework.Color.Yellow, new Vector2(790, 580),
                new Vector2(840, 580));
            spriteBatch.DrawString(font, "Player 1", new Vector2(185, 573), Color.Red);
            spriteBatch.DrawString(font, "Player 2", new Vector2(515, 573), Color.Blue);
            spriteBatch.DrawString(font, "Optimum", new Vector2(845, 573), Color.Yellow);
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 22/4/2012</para>
        /// <para>Date Modified 23/5/2012</para>
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
        public static void DrawDisqualification(PerformanceGraph g, GraphicsDevice GraphicsDevice, SpriteBatch spriteBatch, int dwidth, int dheight, Texture2D P1Tex, Texture2D P2Tex, double player1disqtime, double player2disqtime)
        {
            if (player1disqtime > 0 || player2disqtime > 0)
            {
                for (int j = 0; j <= 1; j++)
                {
                    Boolean t = false;
                    double n = 0;
                    List<double> timings = new List<double>();
                    switch (j)
                    {
                        case 0: n = player1disqtime; timings = g.GetCum1(); if (n >= 0) { t = true; }; break;
                        case 1: n = player2disqtime; timings = g.GetCum2(); if (n >= 0) { t = true; }; break;
                    }
                    if (t)
                    {
                        double time = g.getTotalTime();
                        int index = 8;
                        for (int i = 0; i <= timings.Count - 1; i++)
                        {
                            if (i < timings.Count - 1)
                            {
                                double d1 = timings[i];
                                double d2 = timings[i + 1];
                                if (n >= d1 && n < d2)
                                {
                                    double x = d1 + ((double)(d2 - d1) / (double)2);
                                    if (n < x)
                                    {
                                        time = timings[i];
                                        index = i;
                                    }
                                    else
                                    {
                                        time = timings[i+1];
                                        index = i + 1;
                                    }
                                }
                            }
                        }
                        int y1 = 0; int y2 = 0; int y3 = 0;
                        double r1 = ((double)g.getTotalTime() / (double)GraphUI.Percentage(dwidth, 25));
                        Texture2D texture = null;
                        switch (j)
                        {
                            case 0: y1 = g.getP1DispGraph()[index] - 8; y2 = g.getP1VelGraph()[index] - 8; y3 = g.getP1AccGraph()[index] - 8; texture = P1Tex; break;
                            case 1: y1 = g.getP2DispGraph()[index] - 8; y2 = g.getP2VelGraph()[index] - 8; y3 = g.getP2AccGraph()[index] - 8; texture = P2Tex; break;
                        }    
                        CountDown xDP = new CountDown();
                        CountDown xVP = new CountDown();
                        CountDown xAP = new CountDown();                    

                        int r3 = GraphUI.Percentage(dwidth, 4.88) +(int)(time / r1)-10;
                        xDP = new CountDown(texture, r3, y1, 20, 20);
                        xDP.Draw(spriteBatch);
                        r3 = GraphUI.Percentage(dwidth, 37.1) +(int)(time / r1)-10;
                        xVP = new CountDown(texture, r3, y2, 20, 20);
                        xVP.Draw(spriteBatch);
                        r3 = GraphUI.Percentage(dwidth, 69.3) + (int)(time / r1)-10; 
                        xAP = new CountDown(texture, r3, y3, 20, 20);
                        xAP.Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
