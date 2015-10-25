namespace lab2
{
    using System;
    using System.Collections.Generic;

    class Interpolation
    {
        /// <summary>
        /// Вводим с консоли необходимые значения и 
        /// выводим на экран таблицу сопостовления значения х и f(x).
        /// </summary>
        public void startWork()
        {
            Console.WriteLine("Введите количество значений функции:");
            m = Convert.ToInt32(Console.ReadLine());

            // Инициализируем таблицу.
            table = new double[m, m];

            Console.WriteLine("Введите концы отрезка [a; b]:");
            string[] num = Console.ReadLine().Split(' ');
            a = double.Parse(num[0]);
            b = double.Parse(num[1]);

            FillingTable(a);
            PrintTable();

            do
            {
                Console.WriteLine("Введите степень многочлена (n < {0}):", m);
                n = Convert.ToInt32(Console.ReadLine());
            } while (n >= m || n <= 0);

            double x;
            bool result;
            do
            {
                Console.WriteLine("Введите точку интерполирования:");
                string str = Console.ReadLine();
                result = Double.TryParse(str, out x);

                if (result)
                {
                    FillingTable(x);
                    if (n != m)
                        Sorting(x);
                    //PrintTable();

                    Console.WriteLine("\nЗначение многочлена по Лагранжу:");
                    double calculate = CalculationOfValue(x, true);
                    Console.WriteLine("{0}, фактическая погрешность: {1}", calculate, Math.Abs(f(x) - calculate));
                    calculate = CalculationOfValue(x, false);
                    Console.WriteLine("Значение многочлена по Нютону:");
                    Console.WriteLine("{0}, фактическая погрешность: {1}", calculate, Math.Abs(f(x) - calculate));
                    Console.WriteLine();
                }
                else if (str != "exit")
                    result = true;

            } while (result);
            
        }

        private void SwapColumn()
        {
            double temp = 0;
            for (int i = 0; i < m; i++)
            {
                temp = table[i, 0];
                table[i, 0] = table[i, 1];
                table[i, 1] = temp;
            }
        }

        /// <summary>
        /// Заполняем таблицу разделённых разностей.
        /// </summary>
        public void FillingTable(double x)
        {
            for (int i = 0; i < m; i++)
            {
                double temp = a + i * (b - a) / (m - 1);
                table[i, 0] = temp;
                table[i, 1] = f(temp);
            }

            Sorting(x);

            for (int j = 2; j < m; j++)
                for (int i = 0; i < m - j + 1; i++)
                    table[i, j] = (table[i + 1, j - 1] - table[i, j - 1]) / (table[i + j - 1, 0] - table[i, 0]);
        }

        /// <summary>
        /// Выводим на экран значения х и f(x) из таблицы разделённых разностей.
        /// </summary>
        public void PrintTable()
        {
            for (int i = 0; i < m; i++)
                Console.WriteLine("{0:0.000}  {1:0.000}", table[i, 0], table[i, 1]);
        }

        /// <summary>
        /// Проверяем наличие введённого значения в таблицы, если нет, 
        /// то считем занчение методами Лагранжа и Ньютона.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        private double CalculationOfValue(double x, bool choice)
        {
            for (int i = 0; i < m; i++)
                if (table[i, 0] == x)
                    return table[i, 1];

            if (choice)
                return Lagrange(x);
            else
                return Newton(x);
        }
        
        /// <summary>
        /// Вычисляем значение многочлена, используя метод Лагранжа.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double Lagrange(double x)
        {
            double result = 0;
            for (int j = 0; j < n; j++)
                result += table[j, 1] * Phi(x, j) / Phi(table[j, 0], j);

            return result;
        }

        /// <summary>
        /// Вычисляем значение многочлена, используя метод Ньютона.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double Newton(double x)
        {
            double result = table[0, 1];
            double multiplication = 1;

            for (int i = 1; i < n - 1; i++)
            {
                multiplication *= x - table[i - 1, 0];
                result += table[0, i + 1] * multiplication;
            }

            return result;
        }
        
        /// <summary>
        /// Функция Фи, которая является произведением х_i - х_k, где i != k.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        private double Phi(double x, int j)
        {
            double result = 1;
            for (int i = 0; i < n; i++)
                if (j != i)
                    result *= (x - table[i, 0]);

            return result;
        }

        /// <summary>
        /// Сортируем узлы, в зависимости от точки интерполирования.
        /// </summary>
        /// <param name="x"></param>
        private void Sorting(double x)
        {
            for (int i = 0; i < m; i++)
            {
                double min = b - a;
                int index = i;
                for (int j = i; j < m; j++)
                {
                    if (Math.Abs(table[j, 0] - x) < min)
                    {
                        index = j;
                        min = Math.Abs(table[j, 0] - x);
                    }
                }

                Swap(table, index, i);
            }
        }

        /// <summary>
        /// Меняем местами строки таблицы.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private void Swap(double[,] table, int index1, int index2)
        {
            double[] tempArray = new double[m];

            for (int i = 0, j = 0, k = index1; j < m; i++, j++)
                tempArray[i] = table[index1, j];

            for (int j = 0; j < m; j++)
                table[index1, j] = table[index2, j];

            for (int j = 0; j < m; j++)
                table[index2, j] = tempArray[j];
        }

        /// <summary>
        /// Исходная функция.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double f(double x)
        {
            //return x * x * x;
            return 1 - Math.Exp(-2 * x);
        }

        private int m;
        private double a;
        private double b;
        private double[,] table;
        private List<double> listOf_x = new List<double>();
        private List<double> listOfF_x = new List<double>();
        private int n;
    }
}
