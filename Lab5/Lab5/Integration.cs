namespace Lab5
{
    using System;
    using System.Collections.Generic;

    public class Integration
    {
        public void startWork()
        {
            Console.WriteLine("Введите m:");
            m = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите [a; b]:");
            string[] num = Console.ReadLine().Split(' ');
            a = double.Parse(num[0]);
            b = double.Parse(num[1]);
            h = (b - a) / m;

            Console.WriteLine("Левые прямоугольники\nJ(h) = {0}", LeftRectangle(a, b));
	        Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - LeftRectangle(a, b)));
            Console.WriteLine();

            Console.WriteLine("Правые прямоугольники\nJ(h) = {0}", RightRectangle(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - RightRectangle(a, b)));
            Console.WriteLine();

            Console.WriteLine("Средние прямоугольники\nJ(h) = {0}", MiddleRectangle(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - MiddleRectangle(a, b)));
            Console.WriteLine();

            Console.WriteLine("Трапеция\nJ(h) = {0}", Trapezoid(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - Trapezoid(a, b)));
            Console.WriteLine();

            Console.WriteLine("Симпсон\nJ(h) = {0}", Sympson(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - Sympson(a, b)));
            Console.WriteLine();

            /*Console.WriteLine("Левые и правые прямоугольники:");
            Console.WriteLine("|Rm(f)| <= {0}", (1.0 / 2.0) / m * Math.Abs(fPr1(a)));

            Console.WriteLine("Средние прямоугольники:");
            Console.WriteLine("|Rm(f)| <= {0}", (1.0 / 24.0) / (m * m) * Math.Abs(fPr2(a)));

            Console.WriteLine("Трапеции:");
            Console.WriteLine("|Rm(f)| <= {0}", (1.0 / 12.0) / (m * m) * Math.Abs(fPr2(a)));

            Console.WriteLine("Симпсон:");
            Console.WriteLine("|Rm(f)| <= {0}", (1.0 / 2880.0) / (m * m * m * m) * Math.Abs(fPr4(a)));*/
        }

        /// <summary>
        /// Неопределённый интеграл.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double J_Ub(double x)
        {
            //return x * x * x / 3 + x;
            return x + 0.5 * Math.Exp(-2 * x);
        }

        /// <summary>
        /// Определённый интеграл на [A , B].
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        double J(double A, double B)
        {
            return J_Ub(B) - J_Ub(A);
        }

        private double w(double x)
        {
	        return 1;
        }

        private double LeftRectangle(double A, double B)
        {
	        double res = 0;

	        for (int i = 0; i < m; i++)
		        res += h * f(A + h * i);	

	        return res;
        }

        private double RightRectangle(double A, double B)
        {
	        double res = 0;

	        for (int i = 0; i < m; i++)
		        res += h * f(A + h * (i + 1));	

	        return res;
        }

        private double MiddleRectangle(double A, double B)
        {
	        double res = 0;

	        for (int i = 0; i < m; i++)
		        res += h * f(A + h / 2 + h * i);	

	        return res;
        }

        private double Trapezoid(double A, double B)
        {
	        double res = 0;

	        for (int i = 0; i < m; i++)
		        res +=  h / 2 * (f(A + h * i) + f(A + h * (i + 1)));	

	        return res;
        }

        private double Sympson(double A, double B)
        {
	        double res = 0;

	        for (int i = 0; i < m; i++)
		        res += h / 6 * ( f(A + h * i) + f(A + h * (i + 1)) + 4 * f(A + h/2 + h * i) );	

	        return res;
        }

        private double f(double x)
        {
            //return x * x + 1;
            return 1 - Math.Exp(-2 * x);
        }

        private double fPr1(double x)
        {
            return 2 * Math.Exp(-2 * x);
        }

        private double fPr2(double x)
        {
            return -4 * Math.Exp(-2 * x);
        }

        private double fPr4(double x)
        {
            return -16 * Math.Exp(-2 * x);
        }

        private double a = 0;
        double b = 1;
        int m = 20;
        private double h;
    }
}
