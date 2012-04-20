using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mechanect.Cameras;
using Mechanect.Common;

namespace Mechanect
{
    class Environment2
    {
    
       
        Prey prey;

        Predator predator;

        Aquarium aquarium;
        Random rand = new Random();

        int tolerance = 10;
        int velocity;

        double angleInDegree;
        double angle;
        double TotalTime;

        /// <summary>
        /// Defining the Textures that will contain the images and will represent the objects in the experiment
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: April, 20 </para>
        /// <para>DATE MODIFIED: April, 20  </para>
        /// </remarks>

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D backgroundTexture;
        Texture2D xyAxisTexture;
        Texture2D preyTexture;
        Texture2D bowlTexture;
        //list of models to be drawn
        List<CustomModel> models = new List<CustomModel>();
        Camera camera;
        //Variables that will change how the Gui will look
        Boolean preyEaten = false;
        Boolean grayScreen = false;

        

       
        /// <summary>
        /// generates random angle between 10 and 90
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        private double getRandomAngle()
        {
            return rand.Next(10, 90);
        }
        /// <summary>
        /// generates random velocity between 10 and 90
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        private int getRandomVelocity()
        {
            return rand.Next(10, 70);
        }

        /// <summary>
        /// getSolve : Generate solvable enviroment by setting solvable points for predator,Prey and Aquarium 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        public void getSolvablePoints()
        {
            double xPredator;
            double yPredator;

            double xPrey;
            double yPrey;

            double xAquarium;
            double yAquarium;

            xPredator = 0;
            yPredator = rand.Next(0,70);
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

            int randomTimeforAquarium = rand.Next(randomTimeForPrey +(randomTimeForPrey * 10/100), TimeSlice2 * 3);

       
            Double TimePrey = (Double)randomTimeForPrey / 10;

            Double TimeAquarium = (Double)randomTimeforAquarium / 10;

            xPrey = getX(TimePrey);
          
            yPrey = (velocity * Math.Sin(angle) * TimePrey) - (0.5 * 9.8 * Math.Pow(TimePrey, 2)) + yPredator;
 
            xAquarium = getX(TimeAquarium);
           
            yAquarium = (velocity * Math.Sin(angle) * TimeAquarium) - (0.5 * 9.8 * Math.Pow(TimeAquarium, 2)) + yPredator;

            // Sorry had to change Point to System.Windows.Point to solve a conflict

           setPredator(new Predator(new System.Windows.Point(xPredator,yPredator)));

           setPrey(new Prey(new System.Windows.Point(xPrey, yPrey), (int)xPrey * (tolerance / 100), (int)yPrey * (tolerance / 100)));

           setAquarium(new Aquarium(new System.Windows.Point(xAquarium, yAquarium), (int)xAquarium * (tolerance / 100), (int)yAquarium * ((tolerance / 2) / 100)));


          
      
        }

        /// <summary>
        /// getX is method which return the horizontal displacment of a projectile at certain time. 
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="caseH"></param>
        /// <param name="time"></param>
        /// <returns></returns>

        public Double getX(Double time)
        {

            return CheckPositive(velocity * (Math.Cos(angle)) * time);

        }


        /// <summary>
        /// CheckPostive is a method which check if number is positive or not.If positive it return it and if negative 
        /// it multiply it by -1 to be positive and return it.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="number"></param>
        /// <returns></returns>
        private double CheckPositive(Double number)
        {
            if (number >= 0)
                return number;
            else
                return number * -1;
        }
        /// <summary>
        /// sets the instance variable of prey to the prey given as parameter
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="prey"></param>
        private void setPrey(Prey prey)
        {
            this.prey = prey;
        }
        /// <summary>
        /// sets the instance variable of Predator to the predator given as parameter
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="predator"></param>
         private  void setPredator(Predator predator)
        {
            this.predator = predator;
        }
        /// <summary>
         /// set the instance variable of Aquarium to the Aquarium given as parameter
        /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Tamer Nabil </para>
         /// </remarks>
        /// <param name="aquarium"></param>

        private void setAquarium(Aquarium aquarium)
        {
            this.aquarium = aquarium;
        }
    
    
    }
}
