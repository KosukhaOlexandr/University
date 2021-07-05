using System;

namespace Seidel_Jacobi
{
    class Program
    {
        static double[] Jacobi(double [,] matrix, double [] initialPoint)
        {
            int n = matrix.GetLength(0);
            double[] xcur = new double[n];
            if (n != initialPoint.GetLength(0))
                throw new Exception();
            double[] x = new double[n];
            Array.Copy(initialPoint, x, n);

            double eps = 0.001;
            double epscur = eps + 1;
            while (epscur > eps)
            {
                for (int i = 0; i < n; i++)
                {
                    xcur[i] = matrix[i, n];
                    for (int k = 0; k < n; k++)
                    {
                        if (k != i)
                        {
                            xcur[i] -= matrix[i, k] * x[k];
                        }
                    }
                    xcur[i] /= matrix[i, i];
                }

                epscur = Math.Abs(xcur[0] - x[0]);
                for (int i = 0; i < n; i++)
                {
                    if (Math.Abs(xcur[i] - x[i]) > epscur)
                        epscur = Math.Abs(xcur[i] - x[i]);
                    x[i] = xcur[i];
                }
            }
            return x;
        }

        static double[] Seidel(double[,] matrix, double[] initialPoint)
        {
            int n = matrix.GetLength(0);           
            if (n != initialPoint.GetLength(0))
                throw new Exception();
            double[] x = new double[n];
            Array.Copy(initialPoint, x, n);

            double eps = 0.001;
            double epscur = eps + 1;
            while (epscur > eps)
            {
                double t = x[0];
                for (int i = 0; i < n; i++)
                {
                    x[i] = matrix[i, n];
                    for (int k = 0; k < n; k++)
                    {
                        if (k != i)
                        {
                            x[i] -= matrix[i, k] * x[k];
                        }
                    }
                    x[i] /= matrix[i, i];
                }

                epscur = Math.Abs(t - x[0]);
            }
            return x;
        }
        static void Main(string[] args)
        {
            double[,] jacobiMatrix = { { 6, 3, 1, 0, 15 },
                                       { 3, 5, 0, 2, 21 },
                                       { 1, 0, 3, 1, 14 },
                                       { 0, 2, 1, 5, 27 } };
            double[] x1 = new double[jacobiMatrix.GetLength(0)];
            Array.Copy(Jacobi(jacobiMatrix, new double [] { 0, 0, 0, 0 }), x1, jacobiMatrix.GetLength(0));

            Console.WriteLine("Solution by Jacobi");
            for (int i = 0; i < x1.GetLength(0); i++)
            {
                Console.WriteLine($"x[{i}] = {String.Format("{0:f6}", x1[i])}");
            }

            double[,] seidelMatrix = { { 5, 1, 1, 0, 10},
                                       { 1, 2, 0, 0, 5 },
                                       { 1, 0, 4, 2, 21 },
                                       { 0, 0, 2, 3, 18 } };
            double[] x2 = new double[seidelMatrix.GetLength(0)];
            Array.Copy(Seidel(seidelMatrix, new double[] { 0, 0, 0, 0 }), x2, seidelMatrix.GetLength(0));

            Console.WriteLine("Solution by Seidel");
            for (int i = 0; i < x2.GetLength(0); i++)
            {
                Console.WriteLine($"x[{i}] = {String.Format("{0:f6}", x2[i])}");
            }

        }
    }
}
