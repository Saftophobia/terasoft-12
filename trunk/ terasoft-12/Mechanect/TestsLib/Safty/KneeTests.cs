using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using Mechanect;
using Mechanect.Exp1;

namespace TestsLib.Safty
{
    [TestFixture]
    public class KneeTests
    {
        Mechanect.Exp1.User1 user1;
        Mechanect.Exp1.User1 user2;
        float speed;
        float[] speedlist;
        bool calculatespeedbool;
        float[] min;
        float[] max;
        float avatarconst;
        
        float timer = 0;
        List<float[]> velocitytime = new List<float[]>();

        [SetUp]
        public void init()
        {
            user1 = new Mechanect.Exp1.User1();
            speed = 5000;
            speedlist = new float[2];
            calculatespeedbool = false;
            min = new float[2];
            max = new float[2];
            avatarconst = 0.5f;
            setvelocitytime();


        }

        [Test]
        public void getkneelefttest()
        {


            for(int i = 0; i < velocitytime.Count();i ++)
            {
                user1.Kneepos.Add(velocitytime[i][0]);
                float timer = velocitytime[i][1];
                calculateleftknee();

            }
            Assert.IsTrue(user1.Velocitylist.Count() == 1);

        }

        public void setvelocitytime()
        {
            float[] a = {-0.6f, 0.9f };
            float[] b = { -0.5f, 1.68f };
            float[] c = { -0.4f, 2.5f };
            float[] d = { -0.3f, 2.866666f };
            float[] e = { -0.2f, 3.28f };
            float[] f = { -0.4f, 3.68f };
            float[] g = { -0.5f, 4.133f };
            float[] h = { -0.6f, 4.88f };
            float[] i = { -0.4f, 5.2f };
            float[] j = { -0.3f, 5.4f };
            float[] k = { -0.2f, 5.8f };

            velocitytime.Add(a);
            velocitytime.Add(b);
            velocitytime.Add(c);
            velocitytime.Add(d);
            velocitytime.Add(e);
            velocitytime.Add(f);
            velocitytime.Add(g);
            velocitytime.Add(h);
            velocitytime.Add(i);
            velocitytime.Add(j);
            velocitytime.Add(k);



           

            

        }

        public void calculateleftknee()
        {
            if (user1.Kneepos.Count() != 0)
                {
                    if (user1.Kneepos.Count() == 1)
                    {
                        min[0] = user1.Kneepos[0]; //set the first input to be minimum
                        min[1] = timer;
                    }
                    else
                    {
                        if (user1.Kneepos[user1.Kneepos.Count() - 1] == user1.Kneepos[user1.Kneepos.Count() - 2]) // if the next input inlist equal the one b4 .. discard the one b4
                        {

                            if (user1.Kneepos[user1.Kneepos.Count() - 1] == max[0])
                            {
                                max[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                                max[1] = timer;
                            }
                            if (user1.Kneepos[user1.Kneepos.Count() - 1] == min[0])
                            {
                                min[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                                min[1] = timer;
                            }
                            user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);
                        }
                        else
                        {
                            if (user1.Kneepos[user1.Kneepos.Count() - 1] > user1.Kneepos[user1.Kneepos.Count() - 2]) // if the next input greater than the one b4 .. set it to max
                            {
                                calculatespeedbool = true;
                                max[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                                max[1] = timer;
                                user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);
                            }
                            else // the next input is smaller than the one b4 .. 
                            {
                                if (calculatespeedbool)
                                {
                                    if ((max[0] - min[0]) >= 0.3)
                                    {
                                        speed = Math.Abs(((max[0] - min[0]) / (max[1] - min[1])));
                                        if (speed <= 4)
                                        {
                                            float[] speedlist = new float[2];
                                            speedlist[0] = speed;
                                            speedlist[1] = timer;//calculate the speed of the oscillation from min to max
                                            user1.Velocitylist.Add(speedlist);
                                            user1.Positions.Add(speed * this.avatarconst);
                                            calculatespeedbool = false;
                                        }
                                        else
                                        {
                                            calculatespeedbool = false;
                                        }
                                    }
                                    else
                                    {
                                        calculatespeedbool = false;
                                    }
                                }
                                min[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                                min[1] = timer;
                                user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);
                            }
                        }
                    }
                }
            
    
        }


    }
}
