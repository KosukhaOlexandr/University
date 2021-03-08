using System;
using System.Runtime;

namespace ConsoleApp1
{
    public delegate double Func(double x);
    public interface ISolStrategy
    {
        double FindSolution(Equation eq, double eps);
    }

    class Solution
    {
        private ISolStrategy _strategy;

        public Solution(ISolStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetSolStrategy(ISolStrategy strategy)
        {
            _strategy = strategy;
        }

        public double Solve(Equation eq, double eps)
        {
            if (eq != null)
            {
                double res = _strategy.FindSolution(eq, eps);
                return res;
            }
            throw new NotImplementedException();
        }

    }

    class DivideSolStrategy : ISolStrategy
    {
        public double FindSolution(Equation eq, double eps)
        {
            Console.WriteLine("Binsearch:\n");
            Console.WriteLine($"Eps: {eps}");
            double a = eq.start, b = eq.end;
            Console.WriteLine($"Start: {a}\nEnd: {b}\n");

            double x = (a + b) / 2;
            double fvalue = eq.function(x);
            Console.WriteLine("Initial approximation: " + String.Format("{0:f6}", x) +
                              " Function value: " + String.Format("{0:f6}", fvalue));
            int i = 1;
            while (Math.Abs(fvalue) - eps > 0)
            {
                if (fvalue > 0)
                {
                    b = x;
                }
                else
                {
                    a = x;
                }
                x = (a + b) / 2;
                // fvalue == eq.function(x) - це зроблено для уникання багатьох перераховувань функції
                fvalue = eq.function(x);
                Console.WriteLine($"Iteration {i}: " + String.Format("{0:f6}", x) +
                                   " Function value: " + String.Format("{0:f6}", fvalue));
                i++;
            }
            Console.WriteLine("Result: " + String.Format("{0:f6}", x) +
                              " Function value: " + String.Format("{0:f6}", fvalue));
            return x;
        }
    }

    class ModNewtonSolStrategy : ISolStrategy
    {
        public double FindSolution(Equation eq, double eps)
        {
            Console.WriteLine("\nNewton's Modified Method:\n");
            Console.WriteLine($"Eps: {eps}");
            Console.WriteLine($"Start: {eq.start}\nEnd: {eq.end}\n");

            double x = (eq.start + eq.end) / 2;
            double fvalue = eq.function(x);
            Console.WriteLine("Initial approximation: " + String.Format("{0:f8}", x) +
                              " Function value: " + String.Format("{0:f8}", fvalue));
            int i = 1;
            while (Math.Abs(fvalue) - eps > 0)
            {
                // fvalue == eq.function(x) - це зроблено для уникання багатьох перераховувань функції
                x -= (fvalue / eq.derivative(x));
                fvalue = eq.function(x);
                Console.WriteLine($"Iteration {i}: " + String.Format("{0:f8}", x) +
                                  " Function value: " + String.Format("{0:f8}", fvalue));
                i++;
            }
            Console.WriteLine("Result: " + String.Format("{0:f8}", x) +
                              " Function value: " + String.Format("{0:f8}", fvalue));
            return x;
        }
    }

    class SecantSolStrategy : ISolStrategy
    {
        public double FindSolution(Equation eq, double eps)
        {
            Console.WriteLine("\nSecant Method\n");
            Console.WriteLine($"Eps: {eps}");
            Console.WriteLine($"Start: {eq.start}\nEnd: {eq.end}\n");

            double x = (eq.start + eq.end) / 2;
            double fvalue = eq.function(x);
            Console.WriteLine("Initial approximation: x0 = " + String.Format("{0:f7}", x) +
                              " Function value: " + String.Format("{0:f7}", fvalue));

            double x_next = x - (fvalue / eq.derivative(x));
            fvalue = eq.function(x_next);
            Console.WriteLine("Second approximation: x1 = " + String.Format("{0:f7}", x_next) +
                              " Function value: " + String.Format("{0:f7}", fvalue));
            int i = 1;
            while (Math.Abs(fvalue) - eps > 0)
            {

                double t = x_next;
                // fvalue == eq.function(x_next) - це зроблено для уникання багатьох перераховувань функції
                x_next -= ((x_next - x) * fvalue) / (fvalue - eq.function(x));
                x = t;
                fvalue = eq.function(x_next);
                Console.WriteLine($"Iteration {i}: " + String.Format("{0:f7}", x_next) +
                                  " Function value: " + String.Format("{0:f7}", fvalue));
                i++;
            }
            Console.WriteLine("Result: " + String.Format("{0:f7}", x_next) +
                              " Function value: " + String.Format("{0:f7}", fvalue));
            return x_next;
        }
    }
    public class Equation
    {
        public Func function { get; set; }
        public Func derivative { get; set; }
        public double start { get; set; }
        public double end { get; set; }

        public Equation()
        { }

        public Equation(double start, double end, Func function)
        {
            this.start = start;
            this.end = end;
            this.function = function;
        }

        public Equation(double start, double end, Func function, Func derivative)
        {
            this.start = start;
            this.end = end;
            this.function = function;
            this.derivative = derivative;
        }
    }
    class Program
    {
        static double Function1(double x)
        {
            return x * x * x - 4 * x * x + x + 6;
        }

        static double Function2(double x)
        {
            return x * x * x - 7 * x - 6;
        }
        static double Function2Derivative(double x)
        {
            return 3 * x * x - 7;
        }

        static double Function3(double x)
        {
            return x * x * x - 6 * x * x + 5 * x + 12;
        }
        static double Function3Derivative(double x)
        {
            return 3 * x * x - 12 * x + 5;
        }
        static void Main(string[] args)
        {
            Solution solution = new Solution(new DivideSolStrategy());
            Equation equation1 = new Equation(-10, 0, Function1);
            double res = solution.Solve(equation1, 0.0001);

            Equation equation2 = new Equation();
            equation2.start = 1;
            equation2.end = 3;
            equation2.function = Function2;
            equation2.derivative = Function2Derivative;
            solution.SetSolStrategy(new ModNewtonSolStrategy());
            res = solution.Solve(equation2, 0.0001);

            Equation equation3 = new Equation(-6, 1, Function3, Function3Derivative);
            solution.SetSolStrategy(new SecantSolStrategy());
            res = solution.Solve(equation3, 0.0001);

        }
    }
}
