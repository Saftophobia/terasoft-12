using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Exp2;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace TestsLib.Tamer
{
     [TestFixture]
    class SolvableTest
    {

        [Test]
         public void TestSolvable()
        {
            for(int i =0;i<500;i++)
            {
                Environment2 env = new Environment2();

                double AngleInDegree = env.Angle;
                double AngleInRadian = AngleInDegree * (Math.PI / 180);
                double Velocityexp = env.Velocity;

                Prey prey = env.Prey;
                Aquarium aquarium = env.Aquarium;
                Predator predator = env.Predator;

                //checking null
                Assert.AreNotEqual(prey.Location.X,null);
                Assert.AreNotEqual(prey.Location.Y, null);
                Assert.AreNotEqual(aquarium.Location.X, null);
                Assert.AreNotEqual(aquarium.Location.Y, null);
                Assert.AreNotEqual(predator.Location.X, null);
                Assert.AreNotEqual(predator.Location.Y, null);

                //checking solvable
                /*
                float predatorLocationX = predator.Location.X;
                float predatorLocationY = (float) 6.475;

                float preyX = (float) 33.01;
                float preyY = (float) 19.825;

                float aquariumX = (float) 51;
                float aquariumY = (float) 0.72499;
                */

                float predatorLocationX = predator.Location.X;
                float predatorLocationY = predator.Location.Y;

                float preyX = prey.Location.X;
                float preyY = prey.Location.Y;

                float aquariumX = aquarium.Location.X;
                float aquariumY = aquarium.Location.Y;
                float TimeToReachPrey = (float) (preyX/(Velocityexp*Math.Cos(AngleInRadian)));

               

                float HeightOfPrey = (float) (Math.Round((Velocityexp*Math.Sin(AngleInRadian)*TimeToReachPrey) +
                                              (0.5*-9.807*Math.Pow(TimeToReachPrey, 2))+predatorLocationY,2));

                float TimeToReachAquri = (float) (aquariumX / (Velocityexp * Math.Cos(AngleInRadian)));

              
                float HeightOfAquri = (float)(Math.Round((Velocityexp * Math.Sin(AngleInRadian) *TimeToReachAquri) +
                                              (0.5 * -9.807 * Math.Pow(TimeToReachAquri, 2))+predatorLocationY,2));

                float differnce;
                float differnce2;
                double precent1;
                double precent2;

                
                if (HeightOfAquri >= aquariumY)
                {
                    differnce = HeightOfAquri - aquariumY;
                  //  precent1 = (differnce / HeightOfAquri) * 100;
                }
                else
                {
                    differnce = aquariumY - HeightOfAquri;
                  //  precent1 = (differnce / aquariumY) * 100;
                }





                if (HeightOfPrey >= preyY)
                {
                    differnce2 = HeightOfPrey - preyY;
                   // precent2 = (differnce2/HeightOfPrey)*100;

                }
                else
                {
                    differnce2 = preyY - HeightOfPrey;
                  //  precent2 = (differnce2/preyY)*100;
                }

                Assert.LessOrEqual(Velocityexp,25);
                Assert.GreaterOrEqual(Velocityexp, 5);
                Assert.LessOrEqual(AngleInDegree,70);
                Assert.GreaterOrEqual(AngleInDegree,20);
                Assert.LessOrEqual(differnce, 2);
                Assert.LessOrEqual(differnce2,2);
              //  Assert.AreEqual(HeightOfAquri,aquariumY);
              //  Assert.AreEqual(HeightOfPrey,preyY);
               // Assert.LessOrEqual(precent1, 10);
               // Assert.LessOrEqual(precent2, 10);

            }
        }


    }
}
