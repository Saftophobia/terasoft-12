using System;
namespace NunitTesting
{
    class Factorial
    {
        public Factorial()
        {
        }
        public int GenerateFactorial(int number)
        {
            if (number == 0) return 1;
            if (number < 0) return -1;
            return number * GenerateFactorial(number - 1);
        }
        // had to add a main method since this is an empty project 
        public static void Main(String[] args)
        {
            //Console.WriteLine(new Factorial().GenerateFactorial(4));
        }
    }

}