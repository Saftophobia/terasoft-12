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
    public class GraphUI
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
        /// <para>Date Written 24/5/2012</para>
        /// <para>Date Modified 24/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawCurves is used to draw the curves for each graph.
        /// </summary>
        /// <param name="GraphicsDevice">An instance of the GraphicsDevice.</param>
        /// <param name="spriteBatch">An instance of the SpriteBatch.</param>
        /// <param name="g">An instance of the PerformanceGraph.</param>
        /// <returns>void</returns>
        public static void DrawCurves(PerformanceGraph g, SpriteBatch spriteBatch, GraphicsDevice GraphicsDevice)
        {
            int counter1 = 0;
            float value = 0;
            for (int j = 0; j <= 8; j++)
            {
                Color color = new Color();
                List<float> temporary = new List<float>();
                int width = GraphicsDevice.Viewport.Width;
                int height = GraphicsDevice.Viewport.Height;
                switch (j)
                {
                    case 0: temporary = g.getP1Disp(); counter1 = GraphUI.Percentage(width, 4.88); value = g.getTrackLength(); color = Color.Red; break;
                    case 1: temporary = g.getP2Disp(); counter1 = GraphUI.Percentage(width, 4.88); value = g.getTrackLength(); color = Color.Blue; break;
                    case 2: temporary = g.getOptD(); counter1 = GraphUI.Percentage(width, 4.88); value = g.getTrackLength(); color = Color.Yellow; break;
                    case 3: temporary = g.getP1Vel(); counter1 = GraphUI.Percentage(width, 37.1); value = g.getMaxVelocity(); color = Color.Red; break;
                    case 4: temporary = g.getP2Vel(); counter1 = GraphUI.Percentage(width, 37.1); value = g.getMaxVelocity(); color = Color.Blue; break;
                    case 5: temporary = g.getOptV(); counter1 = GraphUI.Percentage(width, 37.1); value = g.getMaxVelocity(); color = Color.Yellow; break;
                    case 6: temporary = g.getP1Acc(); counter1 = GraphUI.Percentage(width, 69.33); value = g.getMaxAcceleration(); color = Color.Red; break;
                    case 7: temporary = g.getP2Acc(); counter1 = GraphUI.Percentage(width, 69.33); value = g.getMaxAcceleration(); color = Color.Blue; break;
                    case 8: temporary = g.getOptA(); counter1 = GraphUI.Percentage(width, 69.33); value = g.getMaxAcceleration(); color = Color.Yellow; break;
                }
                for (int i = 0; i <= temporary.Count - 2; i++)
                {
                    double r2 = GraphEngine.GetGraphPosition(g, temporary[i], height, value);
                    double r4 = GraphEngine.GetGraphPosition(g, temporary[i + 1], height, value);
                    int r3 = GraphEngine.HandleNegative(j, temporary[i], height) + (int)r2;
                    int r5 = GraphEngine.HandleNegative(j, temporary[i + 1], height) + (int)r4;
                    int distance = 0;
                    double x = ((double)g.getTotalTime() / (double)GraphUI.Percentage(width, 25));
                    switch (j)
                    {
                        case 2:
                        case 5:
                        case 8: distance = (int)(1 / x); break;
                        case 0:
                        case 3:
                        case 6: distance = (int)((g.GetTimings1()[i + 1]) / x); break;
                        default: distance = (int)((g.GetTimings2()[i + 1]) / x); break;
                    }
                    GraphUI.Draw(new PerformanceGraph(counter1, r3 - 1, counter1 + distance, r5 - 1, width, height, color), spriteBatch, GraphicsDevice);
                    counter1 = counter1 + distance;
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
        /// <para>Date Modified 24/5/2012</para>
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
                        double time = GraphEngine.GetNearestDisqualificationTime(n, timings);
                        double r1 = ((double)g.getTotalTime() / (double)GraphUI.Percentage(dwidth, 25));
                        Texture2D texture = null;
                        List<float> displacement = new List<float>();
                        List<float> velocity = new List<float>();
                        List<float> acceleration = new List<float>();
                        switch (j)
                        {
                            case 0: displacement = g.getP1Disp(); velocity = g.getP1Vel(); acceleration = g.getP1Acc(); texture = P1Tex; break;
                            case 1: displacement = g.getP2Disp(); velocity = g.getP2Vel(); acceleration = g.getP2Acc(); texture = P2Tex; break;
                        }
                        float foundDisplacement = GraphEngine.GetValue(timings, time, displacement);
                        float foundVelocity = GraphEngine.GetValue(timings, time, velocity);
                        float foundAcceleration = GraphEngine.GetValue(timings, time, acceleration);
                        int y1 = GraphEngine.HandleNegative(j, foundDisplacement, dheight) + (int)(GraphEngine.GetGraphPosition(g, foundDisplacement, dheight, g.getTrackLength())) - 8;
                        int y2 = GraphEngine.HandleNegative(j, foundVelocity, dheight) + (int)(GraphEngine.GetGraphPosition(g, foundVelocity, dheight, g.getMaxVelocity())) - 8;
                        int y3 = GraphEngine.HandleNegative(j, foundAcceleration, dheight) + (int)(GraphEngine.GetGraphPosition(g, foundAcceleration, dheight, g.getMaxAcceleration())) - 8;
                        int r3 = GraphUI.Percentage(dwidth, 4.88) + (int)(time / r1) - 10;
                        new CountDown(texture, r3, y1, 20, 20).Draw(spriteBatch);
                        r3 = GraphUI.Percentage(dwidth, 37.1) + (int)(time / r1) - 10;
                        new CountDown(texture, r3, y2, 20, 20).Draw(spriteBatch);
                        r3 = GraphUI.Percentage(dwidth, 69.3) + (int)(time / r1) - 10;
                        new CountDown(texture, r3, y3, 20, 20).Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
