namespace Lab_2._1
{
    using System;
    using System.Collections.Generic;

    public class Implementation
    {
        public void StartWork()
        {
            Console.WriteLine("Введите n:");
            while ((!Int32.TryParse(Console.ReadLine(), out n)) || n <= 0)
                Console.WriteLine("Введите n:");

            h = (b - a) / n;

            array_a = new double[n + 1];
            array_b = new double[n + 1];
            array_c = new double[n + 1];
            array_d = new double[n + 1];
            Console.WriteLine("Первый способ:");
            array_a[0] = 0;
            array_b[0] = h * alpha0 - alpha1;
            array_c[0] = alpha1;
            array_d[0] = h * A;
            array_a[n] = -betta1;
            array_b[n] = h * betta0 + betta1;
            array_c[n] = 0;
            array_d[n] = h * B;
            for (int i = 1; i < n; i++)
            {
                array_a[i] = p_xi(a + i * h) - 0.5 * q_xi(a + i * h) * h;
                array_b[i] = - 2.0 * p_xi(a + i * h) + r_xi(a + i * h) * h * h;
                array_c[i] = p_xi(a + i * h) + 0.5 * q_xi(a + i * h) * h;
                array_d[i] = f_xi(a + i * h) * h * h;
            }

            Progonka();

            array_x = new double[n + 1];
            ReverseProgonka();

            array_nev = new double[n + 1];
            Nevyazka();
            Console.WriteLine();
            Console.WriteLine("Вектор неизвестных        Невязка");
            for (int i = 0; i <= n; i++)
                Console.WriteLine("{0,-20} {1}", array_x[i], array_nev[i]);

            Console.WriteLine();
            Console.WriteLine("Точное решение:");
            for (double i = a; i <= b; i += h)
                Console.WriteLine("{0}", -2.0 * Math.Sqrt(2.0 * i + 1));


            Console.WriteLine(); Console.WriteLine();
            Console.WriteLine("Второй способ:");
            
            for (int i = 1; i < n; i++)
            {
                array_a[i] = p_xi(a + i * h) - 0.5 * q_xi(a + i * h) * h;
                array_b[i] = -2.0 * p_xi(a + i * h) + r_xi(a + i * h) * h * h;
                array_c[i] = p_xi(a + i * h) + 0.5 * q_xi(a + i * h) * h;
                array_d[i] = f_xi(a + i * h) * h * h;
            }
            array_a[0] = 0;
            array_b[0] = 2 * h * alpha0 + alpha1 * ((array_a[1] / array_c[1]) - 3);
            array_c[0] = alpha1 * ((array_b[1] / array_c[1]) + 4);
            array_d[0] = 2 * h * A + alpha1 * (array_d[1] / array_c[1]);
            array_a[n] = -betta1 * ((array_b[n - 1] / array_a[n - 1]) + 4);
            array_b[n] = 2 * h * betta0 + betta1 * (3 - (array_c[n - 1] / array_a[n - 1]));
            array_c[n] = 0;
            array_d[n] = 2 * h * B - betta1 * (array_d[n - 1] / array_a[n - 1]);

            Progonka();

            array_x = new double[n + 1];
            ReverseProgonka();

            Nevyazka();
            Console.WriteLine();
            Console.WriteLine("Вектор неизвестных        Невязка");
            for (int i = 0; i <= n; i++)
                Console.WriteLine("{0,-20} {1}", array_x[i], array_nev[i]);
        }

        private void Progonka()
        {
            array_m = new double[n + 1];
            array_k = new double[n + 1];
            array_m[0] = -array_c[0] / array_b[0];
            array_k[0] = array_d[0] / array_b[0];
            Console.WriteLine("Прогоночные коэффициенты:");
            Console.WriteLine("        m                    k");
            Console.WriteLine("{0,-20} {1}", array_m[0], array_k[0]);
            for (int i = 0; i < n; i++)
            {
                array_m[i + 1] = (-array_c[i]) / (array_a[i] * array_m[i] + array_b[i]);
                array_k[i + 1] = (array_d[i] - array_a[i] * array_k[i]) / (array_a[i] * array_m[i] + array_b[i]);
                Console.WriteLine("{0,-20} {1}", array_m[i], array_k[i]);
            }
        }

        private void ReverseProgonka()
        {
            array_x[n] = (array_d[n] - array_a[n] * array_k[n]) / (array_a[n] * array_m[n] + array_b[n]);
            for (int i = n; i > 0; i--)
                array_x[i - 1] = array_m[i] * array_x[i] + array_k[i];
        }

        private void Nevyazka()
        {
            for (int i = 0; i <= n; i++)
            {
                if (i > 0 && i < n)
                {
                    array_nev[i] = (array_a[i] * array_x[i - 1] + array_b[i] * array_x[i]
                                    + array_c[i] * array_x[i + 1]) - array_d[i];
                }
                else
                {
                    if (i == 0)
                        array_nev[i] = array_b[i] * array_x[i] + array_c[i] * array_x[i + 1] - array_d[i];
                    else
                        array_nev[i] = array_a[i] * array_x[i - 1] + array_b[i] * array_x[i] - array_d[i];
                }
             }
        }

        private double p_xi(double xi)
        {
            return 1;
        }

        private double q_xi(double xi)
        {
            return  0;
        }

        private double r_xi(double xi)
        {
            return -8.0 / ((1.0 + 2.0 * xi) * (1.0 + 2.0 * xi));
        }

        private double f_xi(double xi)
        {
            return  36 / (2.0 * Math.Pow(2.0 * xi + 1, 1.5));
        }

        private Int32 n;
        private double h;
        private double alpha0 = 2.0;
        private double alpha1 = -2.0;
        private double A = 0.0;
        private double betta0 = 0.0;
        private double betta1 = 1.0;
        private double B = -2.0/Math.Sqrt(3);
        private double[] array_x;
        private double[] array_a;
        private double[] array_b;
        private double[] array_c;
        private double[] array_d;
        private double[] array_m;
        private double[] array_k;
        private double[] array_nev;
        private const double a = 0;
        private const double b = 1;
    }
}
