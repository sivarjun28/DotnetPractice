using System;
using System.Security.Cryptography.X509Certificates;

namespace Exercise01
{
    class ArrayUtils
    {
        //implementation find max
        static int FindMax(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty");
            }
            int max = array[0];
            foreach (int num in array)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }

        //implementation find min

        static int FindMin(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or less than 0");
            }
            int min = array[0];
            foreach (int num in array)
            {
                if (num < min)
                {
                    min = num;
                }

            }
            return min;
        }

        // implementation find Average
        static double FindAverage(int[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new ArgumentException("Array cannot be null or empty");
            }
            int sum = array.Sum();
            int count = array.Length;
            double average = (double)sum / count;
            return average;

        }

        public static int[] RemoveDuplicates(int[] array)
        {
            HashSet<int> hs = new HashSet<int>();

            foreach (int item in array)
            {
                hs.Add(item);
            }

            return hs.ToArray();
        }

        //implemetation of Reverse of Array
        public static void Reverse(int[] array)
        {
            int left = 0, right = array.Length - 1;
            while (left < right)
            {
                int temp = array[left];
                array[left] = array[right];
                array[right] = temp;
                left++;
                right--;
            }
        }

        //implementation of rotate

        public static void Rotate(int[] array)
        {
            if (array == null || array.Length <= 1)
                return;
            int last = array[array.Length - 1];
            for (int i = array.Length - 1; i > 0; i--)
            {
                array[i] = array[i - 1];
            }
            array[0] = last;
        }
        static void Main(string[] args)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7 };
            System.Console.WriteLine($"Array: {string.Join(", ", numbers)}");
            System.Console.WriteLine($"Max: {FindMax(numbers)}");
            System.Console.WriteLine($"Min: {FindMin(numbers)}");
            System.Console.WriteLine($"Average: {FindAverage(numbers)}");
            int[] unique = RemoveDuplicates(numbers);
            System.Console.WriteLine($"Unique: {string.Join(", ", unique)}");
            Reverse(numbers);
            Console.WriteLine($"Reversed: {string.Join(", ", numbers)}");
            Rotate(numbers);
            Console.WriteLine($"Rotate to Right: {string.Join(", ", numbers)}");



        }
    }
}
