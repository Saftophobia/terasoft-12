using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Mechanect.Classes
{
    class Tools1
    {

        /// <summary>
        /// Fisher–Yates shuffle algorithm "http://en.wikipedia.org/wiki/Fisher-Yates_shuffle"
        /// It shuffles the content of the List in random way
        /// </summary>
        /// <typeparam name="T">Type of the Data in the array</typeparam>
        /// <param name="somelist">provide a list to get shuffled</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>

        public static void shuffle<T>(IList<T> somelist)
        {

            Random random = new Random();
            int n = somelist.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = somelist[k];
                somelist[k] = somelist[n];
                somelist[n] = value;

            }

        }

        

        /// <summary>
        /// modified Fisher–Yates shuffle algorithm "http://en.wikipedia.org/wiki/Fisher-Yates_shuffle"
        /// It shuffles the content of the List in random way
        /// </summary>
        /// <typeparam name="T">Type of the Data in the array</typeparam>
        /// <param name="list">provide a list to get shuffled</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
        public static void RNGshuffle<T>(IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return;
        }

        /// <summary>
        /// applies constraints to the generated list
        /// </summary>
        /// <typeparam name="T">Type of the Data in the array</typeparam>
        /// <param name="list">provide a list to get shuffled according to some constraints</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
         public static void commandshuffler<T>(IList<T> list)
        {
            RNGshuffle<T>(list);
            if (list[0].ToString() == "decreasingAcceleration" || list[0].ToString() == "constantVelocity" || list[0].ToString() == "constantDisplacement")
                commandshuffler<T>(list);
            if (list[1].ToString() == "decreasingAcceleration" || list[1].ToString() == "constantAcceleration"  || list[1].ToString() == "increasingAcceleration")
                commandshuffler<T>(list);
            if (list[1].ToString() == "constantDisplacement")
            {
                 if (list[2].ToString() == "decreasingAcceleration" || list[2].ToString() == "constantVelocity" || list[2].ToString() == "constantDisplacement")
                commandshuffler<T>(list);
            }
            return;
            }

        /// <summary>
        /// create a list with generated random seconds (3-6)
        /// </summary>
        /// <param name="size">desired size of the list to be created</param>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
         private List<int> generaterandomnumbers(int size){
                Random rand = new Random();
                List<int> result = new List<int>();
                HashSet<int> check = new HashSet<int>();
                for (Int32 i = 0; i < size; i++) {
                    int curValue = rand.Next(3,6);
                    
                    result.Add(curValue);
                    }
                return result;
            }

           
        }

    }

