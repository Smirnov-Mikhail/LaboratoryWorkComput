namespace Lab3
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
            table = new double[m, m + 1];

            Console.WriteLine("Введите концы отрезка [a; b]:");
            string[] num = Console.ReadLine().Split(' ');
            a = double.Parse(num[0]);
            b = double.Parse(num[1]);
            h = (b - a) / (m - 1);

            do
            {
                Console.WriteLine("Введите степень многочлена (n < {0}):", m);
                n = Convert.ToInt32(Console.ReadLine());
            } while (n >= m || n <= 0);

            FillingTable(table, true); // заполняем таблицу x_k -> f(x_k)
            PrintTable(table);

            double x;
            bool result;
            do
            {
                string str;
                do
                {
                    Console.WriteLine("Введите точку интерполирования в диапазоне [{0}; {1}] или [{2}; {3}] или [{4}; {5}]:"
                        , table[0, 0], table[1, 0], table[m / 2, 0], table[m / 2 + 1, 0], table[m - 2, 0], table[m - 1, 0]);
                    str = Console.ReadLine();
                    result = Double.TryParse(str, out x);
                } while (!((table[0, 0] <= x && x <= table[1, 0]) || (table[m / 2, 0] <= x && x <= table[m / 2 + 1, 0]) || (table[m - 2, 0] <= x && x <= table[m - 1, 0])));

                if (result)
                {
                    double calc = CalculationOfValue(table, x);
                    Console.WriteLine("Значение многочлена {0}:", calc);
                    Console.WriteLine("Фактическая погрешность: {0}", Math.Abs(calc - f(x)));
                }
                else if (str != "exit")
                    result = true;

            } while (result);
        }

        /// <summary>
        /// Заполняем таблицу.
        /// </summary>
        private void FillingTable(double[,] table, bool choice)
        {
            for (int i = 0; i < m; i++)
            {
                double temp = a + i * (b - a) / (m - 1);
                table[i, 1 - Convert.ToInt32(choice)] = temp;
                table[i, Convert.ToInt32(choice)] = f(temp);
            }

            for (int j = 2; j < m + 1; j++)
                for (int i = 0; i < m + 1 - j; i++)
                    table[i, j] = table[i + 1, j - 1] - table[i, j - 1];
        }

        /// <summary>
        /// Проверяем наличие введённого значения в таблицы, если нет, 
        /// то вызываем метод, который займётся расчётами.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        private double CalculationOfValue(double[,] table, double x)
        {
            for (int i = 0; i < m; i++)
                if (table[i, 0] == x)
                    return table[i, 1];

            return ChooseWay(table, x);
        }

        /// <summary>
        /// Проверяем в какой промежуток попадает значение х.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="x"></param>
        private double ChooseWay(double[,] table, double x)
        {
            double delta = ((n + 1) / 2 - 1) * h;
            if ((x >= table[0, 0]) && (x <= table[1, 0])) 
                return ValueInBegin(table, x);
            else
            {
                if ((x >= a + delta) && (x <= b - delta)) 
                    return ValueInMiddle(table, x);
                else
                {
                    if ((x >= table[m - 2, 0]) && (x <= table[m - 1, 0]))
                        return ValueInEnd(table, x);
                    else
                    {
                        Console.WriteLine("Невозможно интерполировать число {0} по равностоящим узлам.", x);
                        return 0;
                    }
                }
            }
        }

        /// <summary>
        /// Если значение х в начале таблицы.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="x"></param>
        private double ValueInBegin(double[,] table, double x)
        {
	        double result = table[0, 1];
	        double temp = (x - table[0, 0]) / h;
	        double multiplication = temp;

	        for (int i = 2; i < n + 1; i++)
	        {
		        result += table[0, i] * multiplication;
		        multiplication *= (temp + 1 - i) / i;
	        }

            return result;
        }

        private int FindZ(double[,] table, double x)
        {
            for (int i = 0; i < m; i++)
                if (x < table[i, 0]) 
                    return i - 1;

            return 1;
        }

        /// <summary>
        /// Если значение х в середине таблицы.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="x"></param>
        private double ValueInMiddle(double[,] table, double x)
        {
	        int z0 = FindZ(table, x);

            double fact = 1;
	        double result = table[z0, 1];
	        double temp = (x - table[z0, 0]) / h;
	        double multiplication = temp;

	        for (int i = 1; i <= n; i++)    
	        {
		        result += table[z0 - (int)((i) / 2) , i + 1] * multiplication / fact;
                fact *= i + 1;
                multiplication *= i % 2 == 0 ? temp + ((i + 1) / 2) : temp - ((i + 1) / 2);
		        //multiplication *= (temp + Math.Pow(-1, i) * ( (int)((i + 1) / 2) )) / (i + 1);
	        }

            return result;
        }

        /// <summary>
        /// Если значение х в конце таблицы.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="x"></param>
        private double ValueInEnd(double[,] table, double x)
        {
            double result = table[m - 1, 1];
            double temp = (x - table[m - 1, 0]) / h;
            double multiplication = temp;

            for (int i = 2; i < n + 1; i++)
            {
                result += table[m - i, i] * multiplication;
                multiplication *= (temp + i - 1) / i;
            }

            return result;
        }

        /// <summary>
        /// Выводим на экран значения х и f(x) из таблицы.
        /// </summary>
        public void PrintTable(double[,] table)
        {
            for (int i = 0; i < m; i++)
                Console.WriteLine("{0:0.000}  {1:0.000}", table[i, 0], table[i, 1]);
        }

        /// <summary>
        /// Вычисляем значение многочлена, используя метод Лагранжа.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        private double Lagrange(double x, double[,] table)
        {
            double result = 0;
            for (int j = 0; j < n; j++)
                result += table[j, 1] * Phi(x, j) / Phi(table[j, 0], j);

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
        private void Sorting(double x, double[,] table)
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

                SwapRow(table, index, i);
            }
        }

        /// <summary>
        /// Меняем местами строки таблицы.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private void SwapRow(double[,] table, int index1, int index2)
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
        /// Меняем местами столбцы таблицы.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        private void SwapColumn(double[,] table)
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
        /// Исходная функция.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double f(double x)
        {
            return 1 - Math.Exp(-2 * x);
        }

        /// <summary>
        /// Производная исходной функции.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double fPrOIZvodnAYA(double x)
        {
            //return 3 * x * x;
            return 1 + 2 * Math.Exp(-2 * x);
        }

        private int m;
        private double a;
        private double b;
        private double[,] table;
        private double h;
        private int n;
    }
}
