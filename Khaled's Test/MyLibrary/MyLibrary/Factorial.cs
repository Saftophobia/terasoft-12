using System;
namespace MyLibrary
{
    public class Factorial
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
    }

}