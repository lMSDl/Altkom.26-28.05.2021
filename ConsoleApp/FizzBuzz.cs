using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class FizzBuzz
    {

        public string Work()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 1; i < 100; i++)
            {
                var value = GetValue(i);
                stringBuilder.AppendLine(value);
            }
            stringBuilder.Append(GetValue(100));

            return stringBuilder.ToString();
        }

        private static string GetValue(int i)
        {
            string value = "";
            if (i % 3 == 0)
                value += "Fizz";
            if (i % 5 == 0)
                value += "Buzz";
            if(string.IsNullOrEmpty(value))
                value = i.ToString();

            return value;
        }
    }
}
