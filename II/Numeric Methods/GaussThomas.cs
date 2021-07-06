using System;

namespace lab2
{
    class GaussThomas
    {
        static void Main(string[] args)
        {
            //Для Гауса
            /*double[,] gaussMatrix = {  { 4, 3, 1, 0},
                                       { -2, 2, 6, 1 },
                                       { 0, 5, 2, 3 },
                                       { 0, 1, 2, 7 } };
            double[] constantTerms = { 29, 38, 48, 56 };*/
            double[,] gaussMatrix = {  { -2.5, 1.25},
                                       { 2.5, 0 } };
            double[] constantTerms = { -1.4621653279145, 0.5625 };
            double[] x0 = new double[gaussMatrix.GetLength(0)];
            Array.Copy(Gauss(gaussMatrix, constantTerms, out bool hasSolution), x0, gaussMatrix.GetLength(0));

            if (hasSolution)
            {
                Console.WriteLine("Solution by Gauss");
                for (int i = 0; i < x0.GetLength(0); i++)
                {
                    Console.WriteLine($"x[{i}] = {String.Format("{0:f6}", x0[i])}");
                }
            }
            else
                Console.WriteLine("There is no solution");

            //Для Томаса(прогонки)
            double[,] thomasMatrix = { {1, 2, 2, 5 },
                                       {2, 2, 4, 22 },
                                       {0, 3, 3, 20 } };
            double[] x1 = new double[thomasMatrix.GetLength(0)];
            Array.Copy(Thomas(thomasMatrix), x1, thomasMatrix.GetLength(0));

            Console.WriteLine("Solution by Thomas");
            for (int i = 0; i < x1.GetLength(0); i++)
            {
                Console.WriteLine($"x[{i}] = {String.Format("{0:f6}", x1[i])}");
            }
        }
        static double[] Gauss(double[,] initMatrix, double[] constantTerms, out bool hasSolution)
        {
            double[,] matrix = new double[initMatrix.GetLength(0), initMatrix.GetLength(0) + 1];
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    matrix[i, j] = initMatrix[i, j];
            for (int j = 0; j < n; j++)
                matrix[j, n] = constantTerms[j];

            double[] x = new double[n];

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
                    hasSolution = false;
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
            hasSolution = true;
            return x;
        }
        //Метод прогонки
        static double[] Thomas(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[] alpha = new double[n - 1];
            double[] beta = new double[n];
            alpha[0] = matrix[0, 1] / matrix[0, 0];
            beta[0] = matrix[0, n] / matrix[0, 0];

            //Шукаємо коефіцієнти альфа та бета
            for (int i = 1; i < n - 1; i++)
            {
                alpha[i] = matrix[i, i + 1] / (matrix[i, i] - alpha[i - 1] * matrix[i, i - 1]);
            }
            for (int i = 1; i < n; i++)
            {
                beta[i] = (matrix[i, n] - matrix[i, i - 1] * beta[i - 1]) /
                          (matrix[i, i] - alpha[i - 1] * matrix[i, i - 1]);
            }
            
            //Знаходимо роз'язки x[i]
            double[] x = new double[n];
            x[n - 1] = beta[n - 1];
            for (int i = n - 2; i >= 0; i--)
            {
                x[i] = beta[i] - alpha[i] * x[i + 1];
            }
            return x;
        }
        
    }
}
