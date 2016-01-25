namespace Lab_7
{
    using System;
    using System.Collections.Generic;

    public class Classno
    {
        public void startWork()
        {
            double x0 = 0;
            double y0 = 1;
            double xi = x0;
            double yi = y0;

                    Console.WriteLine("Введите N:");
            while ((!Int32.TryParse(Console.ReadLine(), out N)) || N <= 0)
                Console.WriteLine("Введите N:");

            table = new double[25, 8];

            Console.WriteLine("Введите h:");
            while ((!Double.TryParse(Console.ReadLine(), out h)) || h < 0)
                Console.WriteLine("Введите h:");

            Console.WriteLine("Таблица значений точного решения:");
            Console.WriteLine("  xi             y(xi)");
            for (int i = -2; i <= N; ++i)
                Console.WriteLine("{0,4}           {1}", x0 + i * h, y(x0 + i * h));

            Console.WriteLine();
            Console.WriteLine("Тейлор:");
            Console.WriteLine("  xi          y(xi)               |y(x) - yT(x)|");
            for (int i = -2; i <= 10; ++i)
            {
                table[i + 2, 0] = x0 + i * h;
                table[i + 2, 1] = yT(x0 + i * h);
                table[i + 2, 2] = h * f(yT(x0 + i * h));//*/
                Console.WriteLine("{0,4}       {1:0.0000000000}        {2:0.0000000000}", x0 + i * h, yT(x0 + i * h), Math.Abs(y(x0 + i * h) - yT(x0 + i * h)));
            }//*/

            Console.WriteLine();
            Console.WriteLine("Адамс:");
            Console.WriteLine("  xi          y(xi)               |y(x) - yA(x)|");
            //calculateKR(1, 8);
            /*for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                    Console.Write("{0:0.0000} ", table[i, j]);
                Console.WriteLine();
            }//*/
            /*for (int i = -2; i <= 2; i++)
            {
                table[i + 2, 0] = x0 + i * h;
                table[i + 2, 1] = y(x0 + i * h);
                table[i + 2, 2] = h * f(y(x0 + i * h));
            }//*/
            for (int i = 5; i < N + 3; i++)
            {
                table[i, 0] = x0 + (i - 2) * h;
                table[i, 1] = table[i - 1, 1] + (1.0 / 720.0) * (1901 * table[i - 1, 2] - 2774 * table[i - 2, 2] 
                    + 2616 * table[i - 3, 2] - 1274 * table[i - 4, 2] + 251 * table[i - 5, 2]);
                table[i, 2] = h * f(table[i, 1]);
                Console.WriteLine("{0,4}       {1:0.0000000000}        {2:0.0000000000}", table[i, 0], table[i, 1], Math.Abs(y(table[i, 0]) - table[i, 1]));
            }/*
            for (int i = 10; i <= 24; ++i)
            {
                if (i % 2 == 0)
                {
                    table[i, 0] = table[i - 2, 0] + i * h;
                    table[i, 1] = table[i - 2, 1] + table[i - 2, 2] + 0.5 * table[i - 3, 3] + 5.0 / 12.0 * table[i - 4, 4] + 3.0 / 8.0 * table[i - 5, 5] + 251.0 / 720.0 * table[i - 6, 6];
                    table[i, 2] = h * f(table[i, 1]);
                    Console.WriteLine("{0,4}       {1:0.0000000000}        {2:0.0000000000}", x0 + ((i - 4) / 2) * h, table[i, 1], Math.Abs(y(x0 + ((i - 4) / 2) * h) - table[i, 1]));
                }

                if (i < 21)
                    calculateKR(i - 8, i);
                    //for (Int32 j = 3; j < 7; j++)
                        //table[i - 1, j] = table[i + 1, j - 1] - table[i - 1, j - 1];
            }//*/

            Console.WriteLine();
            Console.WriteLine("Рунге-Кутт:");
            Console.WriteLine("  xi          y(xi)               |y(x) - yRK(x)|");
            xi = x0;
            yi = y0;
            double k1, k2, k3, k4;
            for (int i = 1; i <= N; ++i)
            {
                k1 = h * f(yi);
                k2 = h * f(yi + k1 / 2.0);
                k3 = h * f(yi + k2 / 2.0);
                k4 = h * f(yi + k3);
                yi += (k1 + 2 * k2 + 2 * k3 + k4) / 6.0;
                Console.WriteLine("{0,4}       {1:0.0000000000,30}        {2:0.0000000000}", x0 + i * h, yi, Math.Abs(y(x0 + i * h) - yi));
            }
            //*/

            Console.WriteLine();
            Console.WriteLine("Эйлер:");
            Console.WriteLine("  xi          y(xi)               |y(x) - yE(x)|");
            xi = x0;
            yi = y0;
            for (int i = 1;  i <= N; ++i)
            {
                yi += h * f(yi);
                Console.WriteLine("{0,4}       {1:0.0000000000}        {2:0.0000000000}", x0 + i * h, yi, Math.Abs(y(x0 + i * h) - yi));
            }

            Console.WriteLine();
            Console.WriteLine("Усовершенствованный Эйлер:");
            Console.WriteLine("  xi          y(xi)               |y(x) - yE2(x)|");
            xi = x0;
            yi = y0;
            for (int i = 1; i <= N; ++i)
            {
                yi += h * f(yi + (h / 2.0) * f(yi));
                Console.WriteLine("{0,4}       {1:0.0000000000}        {2:0.0000000000}", x0 + i * h, yi, Math.Abs(y(x0 + i * h) - yi));
            }

            Console.WriteLine();
            Console.WriteLine("Эйлер-Коши:");
            Console.WriteLine("  xi          y(xi)               |y(x) - yEK(x)|");
            xi = x0;
            yi = y0;
            for (int i = 1; i <= N; ++i)
            {
                yi += h / 2 * (f(yi) + f(yi + h * f(yi)));
                Console.WriteLine("{0,4}       {1:0.0000000000}        {2:0.0000000000}", x0 + i * h, yi, Math.Abs(y(x0 + i * h) - yi));
            }
        }

        private void calculateTable(double h)
        {
            for (Int32 i = 10; i < 25; i++)
            {
                if (i % 2 == 0)
                {
                    table[i, 0] = table[i - 2, 0] + i * h;
                    table[i, 1] = table[i - 2, 1] + table[i - 2, 2] + 0.5 * table[i - 3, 3] + 5.0 / 12.0 * table[i - 4, 4] + 3.0 / 8.0 * table[i - 5, 5] + 251.0 / 720.0 * table[i - 6, 6];
                    table[i, 2] = h * f(table[i, 1]);
                }

                if (i < 21)
                    for (Int32 j = 3; j < 7; j++)
                        table[i - 1, j] = table[i + 1, j - 1] - table[i - 1, j - 1];
            }
        }

        void calculateTable(int a, int b) 
        {
	        for (Int32 i = a; i < b; ++i)
            {
                for (Int32 j = 3; j < 7; j++)
                    table[i, j] = table[i + 1, j - 1] - table[i - 1, j - 1];
            }
        }

        private double y(double x)
        {
            return 2.0 / (Math.Exp(2 * x) + 1);
        }

        private double yT(double x)
        {
            return (1.0 - x + Math.Pow(x, 3) / 3.0 - 2.0 / 15.0 * Math.Pow(x, 5) + 17.0 / 315.0 * Math.Pow(x, 7));
        }

        private double f(double x)
        {
            return -2 * x + x * x;
        }

        private double h;
        private Int32 N;
        private double[,] table;
    }
}
