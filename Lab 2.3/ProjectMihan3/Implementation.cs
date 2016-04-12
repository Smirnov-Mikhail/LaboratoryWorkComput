namespace ProjectMihan3
{
    using System;

    public class Implementation
    {
        public void StartWork()
        {
            UpdateStarteTableAndVector();
            Gauss gauss = new Gauss();
            Console.WriteLine("Исходная матрица A:");
            gauss.printMatrix(matrixA);

            Console.WriteLine("Исходный вектор b:");
            for (int i = 0; i < b.Length; i++)
                Console.Write("{0} ", b[i]);
            Console.WriteLine("\n");

            double[,] matrixAb = gauss.Union(matrixA, b);
            double[] gaussSolution = gauss.GaussOnlyDivision(matrixAb);

            UpdateStarteTableAndVector();

            double min = FindMin(matrixA);
            double max = FindMax(matrixA);
            double alpha = 2.0f / (min + max);
            Console.WriteLine("\nОптимальный параметр альфа: {0}\n", alpha);
            double[,] B_alpha = CalculateB_alpha(matrixA, alpha);
            double[] c_alpha = b;
            MultyArrayByNumber(c_alpha, alpha);

            Console.WriteLine("Матрицa B_alpha:");
            gauss.printMatrix(B_alpha);

            double NormB_alpha = calculateMatrixNorm(B_alpha);
            Console.WriteLine("Норма B_alpha: {0}\n", NormB_alpha);

            double[] x_0 = {0, 0, 0};
            double[] x_1 = c_alpha;
            double vectorNorm = calculateVectorNorm(x_1);

            Console.WriteLine("Априорная оценка: {0}\n", CalculatePriori(NormB_alpha, vectorNorm, 1));

            UpdateStarteTableAndVector();
            int k_iter = 0;
            double valueForAposter = NormB_alpha / (1.0f - NormB_alpha);
            double valueForPriori = vectorNorm / (1.0f - NormB_alpha);
            while (Difference(x_0, x_1) > epsilon)
            {
                k_iter++;//Difference
                Console.WriteLine("Номер итерации: {0}", k_iter);
                Console.WriteLine("Норма вектора невязки: {0}", calculateVectorNorm(SubOfVectors(MultyMatrixByVector(matrixA, x_1), b)));
                Console.WriteLine("Фактическая погрешность: {0}", calculateVectorNorm(SubOfVectors(x_1, gaussSolution)));
                Console.WriteLine("Априорная оценка: {0}", ((double)Math.Pow(((double)NormB_alpha), k_iter)) * valueForPriori);
                Console.WriteLine("Апостериорная оценка: {0}\n", valueForAposter * calculateVectorNorm(SubOfVectors(x_0, x_1)));
                double[] temp = x_0;
                for (int i = 0; i < x_0.Length; i++)
                    x_0[i] = x_1[i];
                x_1 = SumOfVectors(MultyMatrixByVector(B_alpha, temp), c_alpha);      
            }

            Console.WriteLine("\nРешение методом итераций:");
            for (int i = 0; i < x_1.Length; i++)
                Console.Write("{0} ", x_1[i]);
            Console.WriteLine("\nИтогое количество итераций: {0}", k_iter);

            
            Console.WriteLine("\nМетод Зейделя:");
            double[] zeydel = MethodOfZeydel(matrixA, b);
            for (int i = 0; i < zeydel.Length; i++)
                Console.Write("{0} ", zeydel[i]);
            Console.WriteLine("");
            Console.WriteLine("Фактическая погрешность: {0}", calculateVectorNorm(SubOfVectors(MultyMatrixByVector(matrixA, zeydel), b)));
        }

        private double[] MethodOfZeydel(double[,] matrixA, double[] b)
        {
            double[] x_0 = { 0, 0, 0 };
            double[] x_1 = { 0, 0, 0 };
            int k_iter = 0;
            do
            {
                k_iter++;
                double[] temp = x_0;
                for (int i = 0; i < x_0.Length; i++)
                    x_0[i] = x_1[i];

                for (int i = 0; i < x_0.Length; i++)
                    x_1[i] = (1.0f / matrixA[i, i]) * (b[i] - functionForZeydel(matrixA, x_1, 0, i, i) - functionForZeydel(matrixA, temp, i + 1, temp.Length, i));

            } while (Difference(x_0, x_1) > epsilon);
            Console.WriteLine("Steps: {0}", k_iter);
            Console.WriteLine("Норма вектора невязки: {0}", calculateVectorNorm(SubOfVectors(MultyMatrixByVector(matrixA, x_1), b)));

            return x_1;
        }

        private double functionForZeydel(double[,] matrixA, double[] x, int a, int b, int i)
        {
            double result = 0;
            for (int j = a; j < b; j++)
                result += matrixA[i, j] * x[j];

            return result;
        }

        private double calculateMatrixNorm(double[,] matrix)
        {
            double maxNorm = 0;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                    temp += Math.Abs(matrix[i, j]);

                if (temp > maxNorm)
                    maxNorm = temp;
            }

            return maxNorm;
        }

        private double calculateVectorNorm(double[] vector)
        {
            double maxNorm = Math.Abs(vector[0]);
            for (int j = 1; j < vector.Length; j++)
                if (maxNorm > Math.Abs(vector[j]))
                    maxNorm = Math.Abs(vector[j]);

            return maxNorm;
        }

        private double FindMax(double[,] matrix)
        {
            double max = 0.0f;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double currentValue = matrix[i, i];

                for (int j = 0; j < matrix.GetLength(0); j++)
                    if (i != j)
                    currentValue += Math.Abs(matrix[i, j]);

                if (currentValue > max)
                    max = currentValue;
            }

            return max;
        }

        private double FindMin(double[,] matrix)
        {
            double min = 0.0f;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double currentValue = matrix[i, i];

                for (int j = 0; j < matrix.GetLength(0); j++)
                    if (i != j)
                    currentValue -= Math.Abs(matrix[i, j]);

                if (currentValue > min)
                    min = currentValue;
            }

            return min;
        }

        private void MultyMatrixByNumber(double[,] matrix, double number)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] *= number;
        }

        private double[] MultyMatrixByVector(double[,] matrix, double[] vector)
        {
            double[] result = new double[matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < vector.Length; j++)
                    result[i] += matrix[i, j] * vector[j];

            return result;
        }

        private void MultyArrayByNumber(double[] array, double number)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] *= number;
        }

        private double[] SumOfVectors(double[] vector1, double[] vector2)
        {
            double[] result = new double[vector1.Length];
            for (int i = 0; i < vector1.Length; i++)
                result[i] = vector1[i] + vector2[i];

            return result;
        }

        private double[] SubOfVectors(double[] vector1, double[] vector2)
        {
            double[] result = new double[vector1.Length];
            for (int i = 0; i < vector1.Length; i++)
                result[i] = vector1[i] - vector2[i];

            return result;
        }

        private double Difference(double[] arrayk, double[] arrayk1)
        {
	        double sum = 0.0f;
            for (int i = 0; i < arrayk.Length; i++)
                sum += Math.Abs(arrayk[i] - arrayk1[i]);

            return sum;
        }

        private double CalculatePriori(double NormB_alpha, double vectorNorm, int start)
        {
            int k = 1;
            double priori = NormB_alpha * vectorNorm / (1 - NormB_alpha);
            double valueForPriori = vectorNorm / (1.0 - NormB_alpha);
            while (priori > epsilon)
            {
                priori = ((double)Math.Pow(((double)NormB_alpha), start)) * valueForPriori;
                k++;
                start++;
            }

            return k;
        }

        private double CalculateAposter(double NormB_alpha, double vectorNorm, int start)
        {
            int k = 1;
            double priori = NormB_alpha * vectorNorm / (1 - NormB_alpha);
            while (priori > epsilon)
            {
                priori = ((double)Math.Pow(((double)NormB_alpha), start)) * vectorNorm / (1.0 - NormB_alpha);
                k++;
                start++;
            }

            return k;
        }

        private double[,] CalculateB_alpha(double[,] matrix, double number)
        {
            double[,] B_alpha = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    B_alpha[i, j] = -(matrix[i, j] * number);
                    if (i == j)
                    B_alpha[i, j]++;
                }

            return B_alpha;
        }

        private void UpdateStarteTableAndVector()
        {
            matrixA = new double[3, 3] { { 2.22322, 0.29464, 0.13790 }, { 0.29464, 3.20274, 0.49672 }, { 0.13790, 0.49672, 4.99831 } };
            b = new double[] { 2.19978, 6.73919, 6.58942 };
        }

        private double[,] matrixA;
        private double[] b;
        private double epsilon = 0.000001;
    }
}
