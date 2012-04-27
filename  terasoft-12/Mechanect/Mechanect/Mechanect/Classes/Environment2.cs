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


       private Prey prey;
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

        private Predator predator;
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

        private Aquarium aquarium;
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

        private Random rand;


        private double velocity;
        public double Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private double angle;
        private int tolerance;
        /// <summary>
        /// getterMethod for tolerance
        /// setterMethod for tolerance
        /// </summary>
        public int Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
          
        }
        
    /// <summary>
    /// get in Angle,returns the angle in degree.
    /// </summary>
        public double Angle
        {
            
            
            get { return angle*(180/Math.PI); }
            
        }
       

        public Environment2(int tolerance) {
            rand = new Random();
            this.tolerance = tolerance;
            velocity = 0;
            angle = 0;
            GetSolvablePoints();
        }

  
      
        /// <summary>
        /// generates random angle between 20 and 70
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>return angle in form of double</returns>
        private double GetRandomAngle()
        {
            return (int)(10*GetRandomNumber(20, 70)) / 10f;
        }
       
        /// <summary>
        /// generates random velocity between 5 and 25
        /// </summary>
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns random velocity "double"</returns>
        private double GetRandomVelocity()
        {
            return (int)(10 * GetRandomNumber(5, 25)) / 10f;
        }
       
        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
       
        private void GetSolvablePoints()
        {
            double TotalTime;
            double angleInDegree;
            Vector2 preyLocation = new Vector2();
            Vector2 predatorLocation = new Vector2();
            Vector2 aquariumLocation = new Vector2();



            predatorLocation.X = 0;
            predatorLocation.Y = rand.Next(0, 20);
            angleInDegree = GetRandomAngle();
            angle = angleInDegree * (Math.PI / 180);
            velocity = GetRandomVelocity();



            double b = velocity * Math.Sin(angle);

            double a = 0.5 * -9.8;

            double c = predatorLocation.Y;

            TotalTime = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);

            if (TotalTime < 0)
            {
                TotalTime  = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
            }
            

            Double TimeSlice = TotalTime / 3;

            Double TimePrey = GetRandomNumber(TimeSlice, TimeSlice * 2);

            Double TimeAquarium = GetRandomNumber(TimePrey + (TimePrey * 10 / 100), TimeSlice * 3);

            preyLocation.X = (float)GetX(TimePrey);

            preyLocation.Y = (float)((velocity * Math.Sin(angle) * TimePrey) - (0.5 * 9.8 * Math.Pow(TimePrey, 2)) + predatorLocation.Y);

            aquariumLocation.X = (float)GetX(TimeAquarium);

            aquariumLocation.Y = (float)((velocity * Math.Sin(angle) * TimeAquarium) - (0.5 * 9.8 * Math.Pow(TimeAquarium, 2)) + predatorLocation.Y);

           

            Predator = new Predator(predatorLocation);

            Prey = new Prey(preyLocation, (float)(preyLocation.X * ((float)tolerance / 100)), (float)preyLocation.Y * ((float)tolerance / 100));

            Aquarium = new Aquarium(aquariumLocation, (float)aquariumLocation.X * ((float)tolerance / 100), (float)aquariumLocation.Y * ((float)tolerance / 100));






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

