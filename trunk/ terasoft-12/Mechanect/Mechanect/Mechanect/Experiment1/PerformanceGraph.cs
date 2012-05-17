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


namespace Mechanect.Experiment1
{
    class PerformanceGraph
    {
        private int stageWidth, stageHeight;
        private Vector2 point1;
        private Vector2 point2;
        private Color curveColor;
        private List<float> player1Displacement=new List<float>();
        private List<float> player2Displacement = new List<float>();
        private List<float> player1Velocity;
        private List<float> player2Velocity;
        private List<float> player1Acceleration;
        private List<float> player2Acceleration;
        private List<float> optimumDisplacement = new List<float>();
        private List<float> optimumVelocity = new List<float>();
        private List<float> optimumAcceleration = new List<float>();
        private List<String> commandsList;
        private List<double> timeSpaces;
        private double totalTime;
        private int[] chosenTimings;
        private int samples;
        private int distance;
        private float[,] chosen;
        private PerformanceGraph[] disp1;
        private PerformanceGraph[] disp2;
        private PerformanceGraph[] velo1;
        private PerformanceGraph[] velo2;
        private PerformanceGraph[] acc1;
        private PerformanceGraph[] acc2;
        private PerformanceGraph[] optD;
        private PerformanceGraph[] optV;
        private PerformanceGraph[] optA;
        private float maxVelocity;
        private float maxAcceleration;
        private List<int> p1DispGraph = new List<int>();
        private List<int> p2DispGraph = new List<int>();
        private List<int> p1VeloGraph = new List<int>();
        private List<int> p2VeloGraph = new List<int>();
        private List<int> p1AccGraph = new List<int>();
        private List<int> p2AccGraph = new List<int>();
        private double[] xAxis = new double[5];
        private double[] yAxisDisplacement = new double[5];
        private double[] yAxisVelocity = new double[5];
        private double[] yAxisAcceleration = new double[5];
        private float previousDisp;
        private float previousVelo;
        private float previousAcc;
        private double player1Win;
        private double player2Win;
        private double player3Win;
        private int trackLength;

        public PerformanceGraph(int start1, int start2, int finishx, int finishy, int a, int b, Color col)
        {
            point1.X = start1;
            point1.Y = start2;
            point2.X = finishx;
            point2.Y = finishy;
            curveColor = col;
            stageWidth = a;
            stageHeight = b;
        }

        public PerformanceGraph()
        {

        }

        public Vector2 getPoint1()
        {
            return point1;
        }
        public Vector2 getPoint2()
        {
            return point2;
        }
        public Color getCurveColor()
        {
            return curveColor;
        }
        public void setYAxisDisp(int x, double y)
        {
            yAxisDisplacement[x] = y;
        }
        public void setYAxisVel(int x, double y)
        {
            yAxisVelocity[x] = y;
        }
        public void setYAxisAcc(int x, double y)
        {
            yAxisAcceleration[x] = y;
        }
        public void setXAxis(int x, double y)
        {
            xAxis[x] = y;
        }
        public List<float> getP1Disp()
        {
            return player1Displacement;
        }
        public List<float> getP2Disp()
        {
            return player2Displacement;
        }
        public double getTotalTime()
        {
            return totalTime;
        }
        public void setTotalTime(double value)
        {
            totalTime = value;
        }
        public int[] getChosen()
        {
            return chosenTimings;
        }
        public List<int> getP1DispGraph()
        {
            return p1DispGraph;
        }
        public List<int> getP2DispGraph()
        {
            return p2DispGraph;
        }
        public List<int> getP1VelGraph()
        {
            return p1VeloGraph;
        }
        public List<int> getP2VelGraph()
        {
            return p2VeloGraph;
        }
        public List<int> getP1AccGraph()
        {
            return p1AccGraph;
        }
        public List<int> getP2AccGraph()
        {
            return p2AccGraph;
        }
        public PerformanceGraph[] getDisplacement1()
        {
            return disp1;
        }
        public PerformanceGraph[] getDisplacement2()
        {
            return disp2;
        }
        public PerformanceGraph[] getVelocity1()
        {
            return velo1;
        }
        public PerformanceGraph[] getVelocity2()
        {
            return velo2;
        }
        public PerformanceGraph[] getAcceleration1()
        {
            return acc1;
        }
        public PerformanceGraph[] getAcceleration2()
        {
            return acc2;
        }
        public PerformanceGraph[] getOptimumDisplacement()
        {
            return optD;
        }
        public PerformanceGraph[] getOptimumVelocity()
        {
            return optV;
        }
        public PerformanceGraph[] getOptimumAcceleration()
        {
            return optA;
        }
        public double[] GetXAxis()
        {
            return xAxis;
        }
        public double[] YAxisDis()
        {
            return yAxisDisplacement;
        }
        public double[] YAxisVel()
        {
            return yAxisVelocity;
        }
        public double[] YAxisAcc()
        {
            return yAxisAcceleration;
        }
        public float getPreviousD()
        {
            return previousDisp;
        }
        public float getPreviousV()
        {
            return previousVelo;
        }
        public float getPreviousA()
        {
            return previousAcc;
        }
        public List<float> getOptD()
        {
            return optimumDisplacement;
        }
        public List<float> getOptV()
        {
            return optimumVelocity;
        }
        public List<float> getOptA()
        {
            return optimumAcceleration;
        }
        public void setPreviousD(float value)
        {
            this.previousDisp = value;
        }
        public void setPreviousV(float value)
        {
            this.previousVelo = value;
        }
        public void setPreviousA(float value)
        {
            this.previousAcc = value;
        }
        public List<string> getCommands()
        {
            return commandsList;
        }
        public List<double> getTimeSpaces()
        {
            return timeSpaces;
        }
        public List<float> getP1Vel()
        {
            return player1Velocity;
        }
        public List<float> getP2Vel()
        {
            return player2Velocity;
        }
        public List<float> getP1Acc()
        {
            return player1Acceleration;
        }
        public List<float> getP2Acc()
        {
            return player2Acceleration;
        }
        public int getTrackLength()
        {
            return trackLength;
        }
        public void setP1Disp(List<float> L)
        {
            this.player1Displacement = L;
        }
        public void setP2Disp(List<float> L)
        {
            this.player2Displacement = L;
        }
        public void setP1Vel(List<float> L)
        {
            this.player1Velocity = L;
        }
        public void setP2Vel(List<float> L)
        {
            this.player2Velocity = L;
        }
        public void setP1Acc(List<float> L)
        {
            this.player1Acceleration = L;
        }
        public void setP2Acc(List<float> L)
        {
            this.player2Acceleration = L;
        }
        public void setOpdD(List<float> L)
        {
            this.optimumDisplacement = L;
        }
        public void clearTimeSpaces()
        {
            timeSpaces = new List<double>();
        }
        public void clearCommands()
        {
            commandsList = new List<string>();
        }
        public void setWin1(double d)
        {
            player1Win = d;
        }
        public void setWin2(double d)
        {
            player2Win = d;
        }
        public void setWin3(double d)
        {
            player3Win = d;
        }
        public int getSamples()
        {
            return samples;
        }
        public void setSamples(int x)
        {
            samples = x;
        }
        public void setDistance(int x)
        {
            distance = x;
        }
        public int getDistance()
        {
            return distance;
        }
        public void setChosen(int a, int b)
        {
            chosen = new float[a, b];
        }
        public void setDisp1(int x)
        {
            disp1 = new PerformanceGraph[x];
        }
        public void setDisp2(int x)
        {
            disp2 = new PerformanceGraph[x];
        }
        public void setVel1(int x)
        {
            velo1 = new PerformanceGraph[x];
        }
        public void setVel2(int x)
        {
            velo2 = new PerformanceGraph[x];
        }
        public void setAcc1(int x)
        {
            acc1 = new PerformanceGraph[x];
        }
        public void setAcc2(int x)
        {
            acc2 = new PerformanceGraph[x];
        }
        public void setOptimumD(int x)
        {
            optD = new PerformanceGraph[x];
        }
        public void setOptimumV(int x)
        {
            optV = new PerformanceGraph[x];
        }
        public void setOptimumA(int x)
        {
            optA = new PerformanceGraph[x];
        }
        public void setChosenTimings(int x)
        {
            chosenTimings = new int[x];
        }
        public int[] getChosenTimings()
        {
            return chosenTimings;
        }
        public void setChosenGraph(int a, int b, float f)
        {
            chosen[a, b] = f;
        }
        public void setMaxVelocity(float x)
        {
            maxVelocity = x;
        }
        public void setMaxAcceleration(float x)
        {
            maxAcceleration = x;
        }
        public float getMaxAcceleration()
        {
            return maxAcceleration;
        }
        public float getMaxVelocity()
        {
            return maxVelocity;
        }
        public double getWin1()
        {
            return player1Win;
        }
        public double getWin2()
        {
            return player2Win;
        }
        public double getWin3()
        {
            return player3Win;
        }
        public float getChosenValue(int a, int b)
        {
            return chosen[a, b];
        }
        public void setVel1(List<float> x)
        {
            player1Velocity = x;
        }
        public void setVel2(List<float> x)
        {
            player2Velocity = x;
        }
        public void setAcc1(List<float> x)
        {
            player1Acceleration = x;
        }
        public void setAcc2(List<float> x)
        {
            player2Acceleration = x;
        }
        public void setTrackLength(int x)
        {
            trackLength = x;
        }
        public void setCommandsList(List<string> d)
        {
            commandsList = d;
        }
        public void setTimeSlices(List<double> d)
        {
            timeSpaces = d;
        }
    }
}
