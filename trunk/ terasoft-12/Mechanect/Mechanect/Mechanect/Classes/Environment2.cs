using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mechanect.Classes;
using Mechanect.Common;
using Microsoft.Xna.Framework;
 

namespace Mechanect
{
    public class Environment2 
    {


        Prey prey;
        public Prey Prey
        {
            get
            {
                return prey;
            }
            set
            {
                prey = value;
            }
        }

        Predator predator;
        public Predator Predator
        {
            get
            {
                return predator;
            }
            set
            {
                predator = value;
            }
        }

        Aquarium aquarium;
        public Aquarium Aquarium
        {
            get
            {
                return aquarium;
            }
            set
            {
                aquarium = value;
            }
        }

        Random rand;

        
        int velocity;
        public int Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
     
        double angle;
        int tolerance;
        
    /// <summary>
    /// get in Angle,returns the angle in degree.
    /// </summary>
        public double Angle
        {
            
            
            get { return angle*(180/Math.PI); }
            set { angle = value; }
        }
       

        public Environment2(int tolerance) {
            rand = new Random();
            this.tolerance = tolerance;
            velocity = 0;
            angle = 0;
            GetSolvablePoints(tolerance);
        }

  
      
        /// <summary>
        /// generates random angle between 10 and 80
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>return angle in form of double</returns>
        private double GetRandomAngle()
        {
            return rand.Next(10, 80);
        }
       
        /// <summary>
        /// generates random velocity between 10 and 70
        /// </summary>
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns random velocity "int"</returns>
        private int GetRandomVelocity()
        {
            return rand.Next(10, 70);
        }
       
        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="tolerance"></param>
        
        private void GetSolvablePoints(int tolerance)
        {
            double TotalTime;
            double angleInDegree;
            Vector2 PPrey = new Vector2();
            Vector2 PPredator = new Vector2();
            Vector2 Paquarium = new Vector2();



            PPredator.X = 0;
            PPredator.Y = rand.Next(0, 70);
            angleInDegree = GetRandomAngle();
            angle = angleInDegree * (Math.PI / 180);
            velocity = GetRandomVelocity();



            double b = velocity * Math.Sin(angle);

            double a = 0.5 * -9.8;

            double c = PPredator.Y;

            TotalTime = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);

            if (TotalTime < 0)
            {
                TotalTime  = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
            }
            


            Double TimeSlice = TotalTime / 3;

            Double TimePrey = GetRandomNumber(TimeSlice, TimeSlice * 2);

            Double TimeAquarium = GetRandomNumber(TimePrey + (TimePrey * 10 / 100), TimeSlice * 3);

            PPrey.X = (float)GetX(TimePrey);

            PPrey.Y = (float)((velocity * Math.Sin(angle) * TimePrey) - (0.5 * 9.8 * Math.Pow(TimePrey, 2)) + PPredator.Y);

            Paquarium.X = (float)GetX(TimeAquarium);

            Paquarium.Y = (float)((velocity * Math.Sin(angle) * TimeAquarium) - (0.5 * 9.8 * Math.Pow(TimeAquarium, 2)) + PPredator.Y);

           

            Predator = new Predator(PPredator);

            Prey = new Prey(PPrey, (int)PPrey.X * (tolerance / 100), (int)PPrey.Y * (tolerance / 100));

            Aquarium = new Aquarium(Paquarium, (int)Paquarium.X* (tolerance / 100), (int)Paquarium.Y* (tolerance / 100));






        }

        
        /// <summary>
        /// GetX is method which return the horizontal displacment of a projectile at certain time. 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="time"></param>
        /// <returns>x position at certain time in double</returns>

        private Double GetX(Double time)
        {

            return CheckPositive(velocity * (Math.Cos(angle)) * time);

        }

       
        /// <summary>
        /// CheckPostive is a method which check if number is positive or not.If positive it return it and if negative 
        /// it multiply it by -1 to be positive and return it.
        /// </summary>  
        ///   <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="number"></param>
        /// <returns>returns positive number</returns>
        private double CheckPositive(Double number)
        {
            if (number >= 0)
                return number;
            else
                return number * -1;
        }

        /// <summary>
        /// generate Random number between min and max
        /// </summary>
         ///   <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>returns random number in double </returns>
        private double GetRandomNumber(double min,double max)
        {
            return (max - min) * rand.NextDouble() + min;
        }


    }
}

