using System;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
 
namespace Mechanect
{
   public class Environment2 
    {
        public Prey Prey { get; set; }
        public Predator Predator { get; set; }
        public Aquarium Aquarium { get; set; }
        private readonly Random _rand;
        private double _velocity;
        public double Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }
        private double _angle;
        private int _tolerance;
        /// <summary>
        /// getterMethod for tolerance
        /// setterMethod for tolerance
        /// </summary>
        public int Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; } 
        }
    /// <summary>
    /// getAngle,returns the angle in degree.
    /// </summary>
        public double Angle
        {
            get { return _angle*(180/Math.PI); }
        }
        public Environment2(int tolerance) {
            _rand = new Random();
            _tolerance = tolerance;
            _velocity = 0;
            _angle = 0;
            GetSolvablePoints();
        }
        /// <summary>
        /// Constructor that assigns Predator,prey and aquarium positions,length and width
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="position">represents position for predator</param>
        /// <param name="prey">represents x and y coordinates for prey + length + width</param>
        /// <param name="aquriaum">represents x and y coordinates for aquarium + length + width</param>
       
        public Environment2(Vector2 position,Rectangle prey,Rectangle aquriaum)
        {    
              Predator = new Predator(position);
              Prey = new Prey(new Vector2(prey.X,prey.Y),prey.Width,prey.Height);
              Aquarium = new Aquarium(new Vector2(aquriaum.X, aquriaum.Y), aquriaum.Width, aquriaum.Height);
        }
        /// <summary>
        /// generates random angle between 20 and 70
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>return angle in form of int</returns>
        private int GetRandomAngle()
        {
            return (int)GetRandomNumber(20, 70);
        }  
        /// <summary>
        /// generates random velocity between 5 and 25
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns random velocity "int"</returns>
        private int GetRandomVelocity()
        {
            return (int)GetRandomNumber(5, 25);
        }
        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
       
        private void GetSolvablePoints()
        {
            var preyLocation = new Vector2();
            var predatorLocation = new Vector2();
            var aquariumLocation = new Vector2();
            predatorLocation.X = 0;
            predatorLocation.Y = _rand.Next(0, 20);
            double angleInDegree = GetRandomAngle();
            _angle = angleInDegree * (Math.PI / 180);
            _velocity = GetRandomVelocity();
            double totalTime = GetTotalTime(predatorLocation.Y);

            double timeSlice = totalTime / 3;
            double timePrey = GetRandomNumber(timeSlice - 10/100, (timeSlice * 2)-(timeSlice + timeSlice*Tolerance*2/100));
            double timeAquarium = GetRandomNumber(timePrey + (timePrey * Tolerance*2 / 100), totalTime);

            preyLocation.X = GetX(timePrey);
            preyLocation.Y = (float)((_velocity * Math.Sin(_angle) * timePrey) - (0.5 * 9.8 * Math.Pow(timePrey, 2)) + predatorLocation.Y);

            aquariumLocation.X = GetX(timeAquarium);
            aquariumLocation.Y = (float)((_velocity * Math.Sin(_angle) * timeAquarium) - (0.5 * 9.8 * Math.Pow(timeAquarium, 2)) + predatorLocation.Y);

            float minimumHeight = Math.Min(predatorLocation.Y, aquariumLocation.Y - aquariumLocation.Y*((float) _tolerance/100));
            predatorLocation.Y = predatorLocation.Y - minimumHeight;
            preyLocation.Y = preyLocation.Y - minimumHeight;
            aquariumLocation.Y = aquariumLocation.Y - minimumHeight;
            

            Predator = new Predator(predatorLocation);
            Aquarium = new Aquarium(aquariumLocation, aquariumLocation.X * ((float)_tolerance / 100),aquariumLocation.Y * ((float)_tolerance / 100));
            Prey = new Prey(preyLocation,Aquarium.Length * 2/5,Aquarium.Width* 2/5);
        }

        /// <summary>
        /// GetTotalTime method calculates the total time needed for aprojectile to come to an end.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="predatorLocationY">takes as input location y of the predator to be able to calculate time</param>
        /// <returns>returns total time needed for the projectile</returns>
        private double GetTotalTime(float predatorLocationY)
        {
            var secondqudrantInFormula = _velocity * Math.Sin(_angle);
            //solving quadratic formula for time
            double totalTime = (-secondqudrantInFormula + Math.Sqrt(Math.Pow(secondqudrantInFormula, 2) - (4 * 0.5 * -9.8 * predatorLocationY))) / (2 * 0.5 * -9.8);
            if (totalTime < 0)
                totalTime = (-secondqudrantInFormula - Math.Sqrt(Math.Pow(secondqudrantInFormula, 2) - (4 * 0.5 * -9.8 * predatorLocationY))) / (2 * 0.5 * -9.8);
            return totalTime;
        }
        /// <summary>
        /// GetX is method which return the horizontal displacment of a projectile at certain time. 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="time">time in which you want to know the value of x axis at</param>
        /// <returns>x position at certain time in float</returns>
        private float GetX(double time)
        {
           var xPosition = (float) (_velocity * (Math.Cos(_angle)) * time);
            if (xPosition >= 0)
                return xPosition;
            return xPosition * -1;
        }
        /// <summary>
        /// generate Random number between min and max
        /// </summary>
         ///   <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="min">min number to generate</param>
        /// <param name="max">max number to generate></param>
        /// <returns>returns random number in double </returns>
        private double GetRandomNumber(double min,double max)
        {
            return (max - min) * _rand.NextDouble() + min;
        }
    }
}

