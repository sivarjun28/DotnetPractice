using System;
using System.Data;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Exercise05
{
    internal class StringVsStringBuilder
    {
        static void Main(string[] args)
        {
            const int iterations = 10000;
            //String concatination

            Stopwatch sw1 = Stopwatch.StartNew();
            string res1 = "";
            for(int i =0; i<iterations; i++)
            {
                res1 += i.ToString();
            }
            sw1.Stop();
            System.Console.WriteLine($"String Concatinations {sw1.ElapsedMilliseconds}ms");
            System.Console.WriteLine($"Result Length: {res1.Length}");

            //string Builder Concatination
            Stopwatch sw2 = Stopwatch.StartNew();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < iterations; i++)
            {
                sb.Append(i);
            }
            string res2 = sb.ToString();
            sw2.Stop();
            System.Console.WriteLine($"String Concatinations {sw2.ElapsedMilliseconds}ms");
            System.Console.WriteLine($"Result Length: {res1.Length}");

            //speed check
            double speedUp = (double)sw1.ElapsedMilliseconds/sw2.ElapsedMilliseconds;
            System.Console.WriteLine($"\nString Builder is {speedUp:F2}x Faster");

        }
    }
}
