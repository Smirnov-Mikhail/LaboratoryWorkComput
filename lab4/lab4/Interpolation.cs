namespace lab4
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
            h = (b - a) / (m - 1);

            do
            {
                Console.WriteLine("Введите степень многочлена (n < {0}):", m);
                n = Convert.ToInt32(Console.ReadLine());
            } while (n >= m || n <= 0);
            
            Console.WriteLine("Таблица численного дифференцирования.");
            FillingTable(true, table); // заполняем таблицу x_k -> f(x_k)
            FillingTablePrOIZvodnAYA(table); // заполняем таблицу производных
            PrintProiZvodNayaTable();

            double x;
            bool result;
            Console.WriteLine("Решение задачи обратного интерполирования.");
            do
            {
                Console.WriteLine("Введите параметр задачи:");
                string str = Console.ReadLine();
                result = Double.TryParse(str, out x);

                if (result)
                {
                    FillingTable(true, table); // заполняем таблицу x_k -> f(x_k)
                    Sorting(x, table);
                    inverseTable = new double[m, m];
                    FillingTable(false, inverseTable); // заполняем таблицу f(x_k) -> x_k
                    Sorting(x, inverseTable);
                    //PrintTable(inverseTable);

                    Console.WriteLine("Результаты решения задачи обратного интерполирования.");
                    double calculate = CalculationOfValue(x, inverseTable, true);
                    Console.WriteLine("1 способ: {0}, модуль невязки: {1}", calculate, Math.Abs(f(calculate) - x));
                    calculate = CalculationOfValue(x, inverseTable, false);
                    Console.WriteLine("2 способ: {0}, модуль невязки: {1}", calculate, Math.Abs(f(calculate) - x));
                    Console.WriteLine();
                }
                else if (str != "exit")
                    result = true;

            } while (result);
        }

        /// <summary>
        /// 2 способ решения (методом бисекции).
        /// </summary>
        /// <param name="F"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private double WayTwo(double F, double left, double right, double[,] table, int count)
        {
            double ValueMid = Lagrange((right + left) / 2, table);
            double ValueLeft = Lagrange(left, table);
            if (Math.Abs(ValueMid - F) <= epsilon /*|| count == 6000*/)
                return (right + left) / 2;
            else if ((ValueLeft - F) * (ValueMid - F) < 0)
                return WayTwo(F, left, (right + left) / 2, table, ++count);
            else
                return WayTwo(F, (right + left) / 2, right, table, ++count);
        }

        /// <summary>
        /// Заполняем таблицу.
        /// </summary>
        public void FillingTable(bool choice, double[,] table)
        {
            for (int i = 0; i < m; i++)
            {
                double temp = a + i * (b - a) / (m - 1);
                table[i, 1 - Convert.ToInt32(choice)] = temp;
                table[i, Convert.ToInt32(choice)] = f(temp);
            }

            // код для метода Ньютона.
            //for (int j = 2; j < m; j++)
              //  for (int i = 0; i < m - j + 1; i++)
                //    table[i, j] = (table[i + 1, j - 1] - table[i, j - 1]) / (table[i + j - 1, 0] - table[i, 0]);
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
        /// Выводим на экран таблицу производных.
        /// </summary>
        public void PrintProiZvodNayaTable()
        {
            Console.WriteLine("  x    f(x)");
            for (int i = 0; i < m; i++)
                Console.WriteLine("{0:0.000} {1:0.000}", table[i, 0], table[i, 1]); // x_k и f(x_k).
            Console.WriteLine("______________________________________");
            Console.WriteLine("\tf'(x)\t\tпогрешность");
            for (int i = 0; i < m; i++)
                Console.WriteLine("{0} {1}", table[i, 2], Math.Abs(fPrOIZvodnAYA(table[i, 0]) - table[i, 2])); // f' и погрешность.
            Console.WriteLine("______________________________________");
            Console.WriteLine("\tf''(x)\t\tпогрешность");
            for (int i = 0; i < m; i++)
                Console.WriteLine("{0} {1}", table[i, 3], Math.Abs(fPrOIZvodnAYA2(table[i, 0]) - table[i, 3])); // f'' и погрешность.
            Console.WriteLine("______________________________________");
                //, table[i, 2], Math.Abs(fPrOIZvodnAYA(table[i, 0]) - table[i, 2]) // f' и погрешность.
                //, table[i, 3], Math.Abs(fPrOIZvodnAYA2(table[i, 0]) - table[i, 3])); // f'' и погрешность.
        }

        /// <summary>
        /// Проверяем наличие введённого значения в таблицы, если нет, 
        /// то считем занчение методами Лагранжа и Ньютона.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="choice"></param>
        /// <returns></returns>
        private double CalculationOfValue(double x, double[,] table, bool choice)
        {
            for (int i = 0; i < m; i++)
                if (table[i, 0] == x)
                    return table[i, 1];

            if (choice)
                return Lagrange(x, table);
            else
                return WayTwo(x, a, b, table, 1);
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
        /// Дополняем таблицу производными.
        /// </summary>
        private void FillingTablePrOIZvodnAYA(double[,] table)
        {
            table[0, 2] = Formula4(table[0, 0]);

            for (int i = 1; i < m - 1; i++)
            {
                table[i, 2] = Formula3(table[i, 0]);
                table[i, 3] = Formula6(table[i, 0]);
            }

            table[m - 1, 2] = Formula5(table[m - 1, 0]);
        }
        
        /// <summary>
        /// Формула для расчёта средних значений первой производной.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double Formula3(double a)
        {
            return (f(a + h) - f(a - h)) / (2 * h);
        }

        /// <summary>
        /// Формула для рачёта крайнего левого значения производной.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double Formula4(double a)
        {
            return (-3 * f(a) + 4 * f(a + h) - f(a + 2 * h)) / (2 * h);
        }

        /// <summary>
        /// Формула для расчёта крайнего правого значения производной.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double Formula5(double a)
        {
            return (3 * f(a) - 4 * f(a - h) + f(a - 2 * h)) / (2 * h);
        }

        /// <summary>
        /// Формула для расчёта центральных значений второй производной.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private double Formula6(double a)
        {
            return (f(a + h) - 2 * f(a) + f(a - h)) / (h * h);
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

        /// <summary>
        /// Производная исходной функции.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double fPrOIZvodnAYA(double x)
        {
            //return 3 * x * x;
            return 2 * Math.Exp(-2 * x);
        }

        /// <summary>
        /// Вторая производная исходной функции.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private double fPrOIZvodnAYA2(double x)
        {
            return -4 * Math.Exp(-2 * x);
        }

        private int m;
        private double a;
        private double b;
        private double[,] table;
        private double[,] inverseTable;
        private double h;
        private int n;
        private double epsilon = 0.00000001;
    }
}
