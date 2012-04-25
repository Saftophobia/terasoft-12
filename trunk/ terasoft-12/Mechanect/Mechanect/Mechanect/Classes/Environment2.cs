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

        Random rand = new Random();

        
        int velocity = 0;
        public int Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        double angleInDegree;
        double angle = 0;
        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        double TotalTime;

        public Environment2() { }

  
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// generates random angle between 10 and 90
        /// </summary>
        /// <returns>return double</returns>
        private double getRandomAngle()
        {
            return rand.Next(10, 90);
        }
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// generates random velocity between 10 and 70
        /// </summary>
        /// <returns>return int</returns>
        private int getRandomVelocity()
        {
            return rand.Next(10, 70);
        }
         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <param name="tolerance"></param>
         
        public void getSolvablePoints(int tolerance)
        {
            double xPredator;
            double yPredator;

            double xPrey;
            double yPrey;

            double xAquarium;
            double yAquarium;

            xPredator = 0;
            yPredator = rand.Next(0, 70);
            angleInDegree = getRandomAngle();
            angle = angleInDegree * (Math.PI / 180);
            velocity = getRandomVelocity();



            double b = velocity * Math.Sin(angle);

            double a = 0.5 * -9.8;

            double c = yPredator;

            double Timeneeded = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);

            double Timeneeded2 = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);

            if (Timeneeded > 0)
            {
                TotalTime = Timeneeded;
            }
            else
            {
                TotalTime = Timeneeded2;
            }


            Double TimeSlice = TotalTime / 3;

            int TimeSlice2 = (int)(TimeSlice * 10);

            int randomTimeForPrey = rand.Next(TimeSlice2, TimeSlice2 * 2);

            int randomTimeforAquarium = rand.Next(randomTimeForPrey + (randomTimeForPrey * 10 / 100), TimeSlice2 * 3);


            Double TimePrey = (Double)randomTimeForPrey / 10;

            Double TimeAquarium = (Double)randomTimeforAquarium / 10;

            xPrey = getX(TimePrey);

            yPrey = (velocity * Math.Sin(angle) * TimePrey) - (0.5 * 9.8 * Math.Pow(TimePrey, 2)) + yPredator;

            xAquarium = getX(TimeAquarium);

            yAquarium = (velocity * Math.Sin(angle) * TimeAquarium) - (0.5 * 9.8 * Math.Pow(TimeAquarium, 2)) + yPredator;

            // Sorry had to change Point to System.Windows.Point to solve a conflict

            Predator = new Predator(new Vector2((float)xPredator, (float)yPredator));

            Prey = new Prey(new Vector2((float)xPrey, (float)yPrey), (int)xPrey * (tolerance / 100), (int)yPrey * (tolerance / 100));

            Aquarium = new Aquarium(new Vector2((float)xAquarium, (float)yAquarium), (int)xAquarium * (tolerance / 100), (int)yAquarium * (tolerance / 100));




        }

         /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// getX is method which return the horizontal displacment of a projectile at certain time. 
        /// </summary>
        /// <param name="time"></param>
        /// <returns>returns x position in double</returns>

        public Double getX(Double time)
        {

            return CheckPositive(velocity * (Math.Cos(angle)) * time);

        }

        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <summary>
        /// CheckPostive is a method which check if number is positive or not.If positive it return it and if negative 
        /// it multiply it by -1 to be positive and return it.
        /// </summary>  
        /// <param name="number"></param>
        /// <returns>return positive double number</returns>
        private double CheckPositive(Double number)
        {
            if (number >= 0)
                return number;
            else
                return number * -1;
        }


    }
}
