namespace Lab5
{
    using System;
    using System.Collections.Generic;

    public class Integration
    {
        public void startWork()
        {
            Console.WriteLine("Введите количество значений функции:");
            m = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите концы отрезка [a; b]:");
            string[] num = Console.ReadLine().Split(' ');
            a = double.Parse(num[0]);
            b = double.Parse(num[1]);
            h = (b - a) / m;

            Console.WriteLine("Ответы для промежутка [{0}; {1}].", a, b);
	        Console.WriteLine("Точное значение J = {0}", J(a, b));
            Console.WriteLine();

            Console.WriteLine("Левые прямоугольники J(h) = {0}", LeftRectangle(a, b));
	        Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - LeftRectangle(a, b)));
            Console.WriteLine();

            Console.WriteLine("Правые прямоугольники J(h) = {0}", RightRectangle(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - RightRectangle(a, b)));
            Console.WriteLine();

            Console.WriteLine("Средние прямоугольники J(h) = {0}", MiddleRectangle(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - MiddleRectangle(a, b)));
            Console.WriteLine();

            Console.WriteLine("Трапеции J(h) = {0}", Trapezoid(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - Trapezoid(a, b)));
            Console.WriteLine();

            Console.WriteLine("Симпсон J(h) = {0}", Sympson(a, b));
            Console.WriteLine("|J - J(h)| = {0}", Math.Abs(J(a, b) - Sympson(a, b)));
            Console.WriteLine();
        }

        /// <summary>
        /// Неопределённый интеграл.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double J_Ub(double x)
        {
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

        private double w (double x)
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
            //return x * x * x;
            return 1 - Math.Exp(-2 * x);
        }

        private double a = 0;
        double b = 1;
        int m = 20;
        private double h;
    }
}
