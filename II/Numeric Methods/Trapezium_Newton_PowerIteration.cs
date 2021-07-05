using System;

namespace Trapezium_Newton_PowerIteration
{
    class Program
    {
        delegate double Unary_Function(double x);
        delegate double Binary_Function(double x, double y);
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // For Trapezium
            Console.WriteLine("Trapezium method for integral(3x^4 + x), x from 0 to 1.5");
            Unary_Function f1 = (x) => 3 * x * x * x * x + x;
            Console.WriteLine(String.Format("{0:f5}",Trapezium(f1, 0, 1.5, 81, 0.001)));
            // For Power Iteration
            double[,] powerIterationMatrix = { {1.4, 0.5, 0.6},
                                               {0.5, 1.4, 0.3 },
                                               {0.6, 0.3, 1.4 } };
            Console.WriteLine("\nPower Iteration method\nThe max eigenvalue is:");
            Console.WriteLine(String.Format("{0:f5}",
                        PowerIteration(powerIterationMatrix, new double[] { 1, 1, 1 }, 0.001)));
            // For Newton
            Binary_Function equationsSystem = (x, y) => Math.Tan(x * y + 0.1) - x * x ;
            equationsSystem += (x, y) => x * x + 2 * y * y - 1;

            Binary_Function eq1dx = (x, y) => y * (1 / Math.Cos(x * y + 0.1) * Math.Cos(x * y + 0.1)) - 2 * x;
            Binary_Function eq1dy = (x, y) => x * (1 / Math.Cos(x * y + 0.1) * Math.Cos(x * y + 0.1));
            Binary_Function eq2dx = (x, y) => 2 * x;
            Binary_Function eq2dy = (x, y) => 4 * y;

            Binary_Function[,] jacobiMatrix = { { eq1dx, eq1dy},
                                                 { eq2dx, eq2dy} };

            double[] newtonResult = Newton(equationsSystem, jacobiMatrix, new double[] { 1.25, 0 }, 0.001);

            Console.WriteLine("\nSolution by Newton ");
            for (int i = 0; i < newtonResult.GetLength(0); i++)
            {
                Console.WriteLine($"x[{i}] = {String.Format("{0:f6}", newtonResult[i])}");
            }
        }
        static double Trapezium(Unary_Function f, double start, double end, double m2, double eps)
        {
            double delta = Math.Sqrt((12 * eps) / ((end - start) * m2));
            double result = f(start)/2 + f(end)/2;
            start += delta;
            while (end > start)
            {
                result += f(start);
                start += delta;
            }
            result *= delta;

            return result;
        }

        static double [] Newton(Binary_Function equations, Binary_Function [,] jacobiMatrix, double [] initialValues, double eps)
        {
            int n = equations.GetInvocationList().Length;

            double[,] intermediateMatrix = new double[n, n];
            double[] z = new double[n];
            do
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                        intermediateMatrix[i, j] = jacobiMatrix[i, j](initialValues[0], initialValues[1]);
                }
				Delegate[] equationsList = equations.GetInvocationList();
				double [] b = new double[initialValues.GetLength(0)];
				int q = 0;
				foreach(Delegate f in equationsList)
				{
					if (f is Binary_Function)
					{
						b[q++] = (f as Binary_Function)(initialValues[0], initialValues[1]);
					}
				}
                z = Gauss(intermediateMatrix, b);
                for (int i = 0; i < initialValues.GetLength(0); i++)
                    initialValues[i] = initialValues[i] - z[i];
            } while (z[0] > eps);
            return initialValues;

        }
        static double[] Gauss (double[,] initMatrix, double[] constantTerms)
        {
            double[,] matrix = new double[initMatrix.GetLength(0), initMatrix.GetLength(0) + 1];
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = initMatrix[i, j];
            for (int j = 0; j < n; j++)
                matrix[j, n] = constantTerms[j];

            double [] x = new double[n];

            //Прямий хід
            for (int k = 0; k < n; k++)
            {
                double maxInRow = Math.Abs(matrix[k, k]);
                int rowForMax = k;
                for (int j = k + 1; j < n; j++)
                {
                    if (Math.Abs(matrix[k, k]) < Math.Abs(matrix[j, k]))
                    {
                        rowForMax = j;
                        maxInRow = matrix[j, k];
                    }
                }
                if (maxInRow == 0)
                {
                    return new double[0];
                }

                if (rowForMax != k)
                {
                    for (int j = 0; j < n + 1; j++)
                    {
                        double t = matrix[k, j];
                        matrix[k, j] = matrix[rowForMax, j];
                        matrix[rowForMax, j] = t;
                    }
                }
                double d = matrix[k, k];
                for (int j = k; j < n + 1; j++)
                    matrix[k, j] = matrix[k, j] / d;

                
                for (int i = k + 1; i < n; i++)
                {
                    double coef = matrix[i, k] / matrix[k, k];
                    for (int j = k; j < n + 1; j++)
                        matrix[i, j] -= coef * matrix[k, j];
                }
            }
            //Зворотній хід
            for (int i = n - 1; i >= 0; i--)
            {
                x[i] = 0;
                double t = matrix[i, n];
                for (int j = n; j > i; j--)
                    t -= matrix[i, j - 1] * x[j - 1];
                x[i] = t;
            }
            return x;
        }
    

        static double PowerIteration(double [,] matrix, double [] initialValues, double eps)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            if (initialValues.GetLength(0) != m)
                throw new Exception();

            double coef_prev = 1;
            double coef_cur = 1;
            double division_prev = 1;
            double division_cur = 1;
            do
            {

                division_prev = coef_cur / coef_prev;
                coef_prev = coef_cur;
                Array.Copy(MultiplyMatrixByVector(matrix, initialValues), initialValues, m);
                coef_cur = initialValues[0];
                division_cur = coef_cur / coef_prev;
            } while (Math.Abs(division_cur - division_prev) > eps);
            return division_cur;
        }

        static double[] MultiplyMatrixByVector(double [,] matrix, double [] vector)
        {
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            if (vector.GetLength(0) != m)
                throw new Exception();

            double[] result = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
            }

            return result;

        }
    }
}
