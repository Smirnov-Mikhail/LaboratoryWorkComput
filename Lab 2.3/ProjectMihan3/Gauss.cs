namespace ProjectMihan3
{
    using System;

    public class Gauss
    {
        public double[] GaussOnlyDivision(double[,] matrix)
        {
            for (int k = 0; k < matrix.GetLength(0); k++)
            {
                double dioganalElement = matrix[k, k];
                for (int j = k; j < matrix.GetLength(1); j++)
                    matrix[k, j] /= dioganalElement;
                for (int i = k + 1; i < matrix.GetLength(0); i++)
                {
                    double temp = matrix[i, k];
                    for (int j = k + 1; j < matrix.GetLength(1); j++)
                        matrix[i, j] -= matrix[k, j] * temp;
                }
            }

            double[] x = new double[matrix.GetLength(0)];
            for (int i = matrix.GetLength(0) - 1; i >= 0; i--)
                x[i] = matrix[i, matrix.GetLength(1) - 1];

            for (int i = matrix.GetLength(0) - 1; i >= 0; i--)
            {
                double temp = matrix[i, matrix.GetLength(1) - 1];
                for (int j = matrix.GetLength(0) - 1; j > i; --j)
                    temp -= matrix[i, j] * x[j];

                x[i] = temp / matrix[i, i];
            }

            Console.WriteLine("Решение методом Гаусса:");
            for (int i = 0; i < matrix.GetLength(0); i++)
                Console.Write("{0} ", x[i]);

            Console.WriteLine();

            return x;
        }

        private void DeleteLineByElement(double[,] matrix, int numberOfLine)
        {
            for (int i = numberOfLine; i < matrix.GetLength(1); i++)
                matrix[numberOfLine, i] /= matrix[numberOfLine, numberOfLine];
        }

        public double[,] Union(double[,] matrixA, double[] b)
        {
            double[,] matrixResult = new double[3, 4];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    matrixResult[i, j] = matrixA[i, j];

            for (int i = 0; i < 3; i++)
                matrixResult[i, 3] = b[i];

            return matrixResult;
        }

        public void printMatrix(double[,] matrixA)
        {
            for (int i = 0; i < matrixA.GetLength(0); i++)
            {
                for (int j = 0; j < matrixA.GetLength(1); j++)
                    Console.Write("{0} ", matrixA[i, j]);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
