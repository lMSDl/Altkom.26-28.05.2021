using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Calculator
    {
        public EventHandler<CalculationEventArgs> CalculationEvent;

        public Task<float> CalcAsync(string expression) // a * b, a + b
        {
            var split = expression.Split(' ');
            if (split.Length == 3)
            {
                if (split.Where(x => x == "|").Count() == 2)
                {
                    if (float.TryParse(split[1], out var value))
                        return Task.FromResult(Abs(value));
                }
                else
                {
                    if (float.TryParse(split[0], out var a))
                    {
                        if (float.TryParse(split[2], out var b))
                        {
                            switch (split[1])
                            {
                                case "+":
                                    return Task.FromResult(Add(a, b));
                                case "-":
                                    return Task.FromResult(Substract(a, b));
                                case "*":
                                    return Task.FromResult(Multiply(a, b));
                                case "/":
                                    return Task.FromResult(Divide(a, b));
                            }
                        }
                    }
                }
            }

            throw new ArgumentException("Bad expression", nameof(expression));
        }

        public float Add(float a, float b)
        {
            CheckParam(a, nameof(a));
            CheckParam(b, nameof(b));

            var result = a + b;
            CalculationEvent?.Invoke(this, new CalculationEventArgs { Expression = $"{a}+{b}", Result = result });
            return result;
        }

        public float Substract(float a, float b)
        {
            CheckParam(a, nameof(a));
            CheckParam(b, nameof(b));

            var result = a - b;
            CalculationEvent?.Invoke(this, new CalculationEventArgs { Expression = $"{a}-{b}", Result = result });
            return result;
        }

        public float Multiply(float a, float b)
        {
            CheckParam(a, nameof(a));
            CheckParam(b, nameof(b));

            var result = a * b;
            CalculationEvent?.Invoke(this, new CalculationEventArgs { Expression = $"{a}*{b}", Result = result });
            return result;
        }

        public float Divide(float a, float b)
        {
            CheckParam(a, nameof(a));
            CheckParam(b, nameof(b));
            if (b == 0)
                throw new ArgumentException("Argument can't be 0", nameof(b));

            var result = a / b;
            CalculationEvent?.Invoke(this, new CalculationEventArgs { Expression = $"{a}/{b}", Result = result });
            return result;
        }

        public float Abs(float a)
        {
            CheckParam(a, nameof(a));

            var result = Math.Abs(a);
            CalculationEvent?.Invoke(this, new CalculationEventArgs { Expression = $"|{a}|", Result = result });
            return result;
        }

        private static void CheckParam(float value, string name)
        {
            if (float.IsNaN(value))
                throw new ArgumentException("Value can't be NaN", name);
        }

        public class CalculationEventArgs
        {
               public string Expression { get; set; }
               public float Result { get; set; }
        }
    }
}
