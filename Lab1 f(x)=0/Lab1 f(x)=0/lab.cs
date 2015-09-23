namespace Lab1_f_x__0
{
    using System;
    using System.Collections.Generic;

    public class lab
    {
        double A = -15;
        double B = 15;
        double h = 0.001;
        double epsilon = 0.000001;
        List<Pair> pairs;

        public lab()
        {
            separation();
        }

        public class Pair
        {
            public Pair(double f, double s)
            {
                First = f;
                Second = s;
            }
            public double First { get; set; }
            public double Second { get; set; }
        }

        double f(double x)
        {
            //return x * x - 4;
            return x * x * x - Math.Cos(x + 0.5);
        }

        double fDer(double x)
        {
            //return 2 * x;
            return 3.0 * x * x + Math.Sin(x + 0.5);
        }

        private void separation()
        {
            List<Pair> list = new List<Pair>();
            for (double i = A; i <= B; i += h)
            {
                if (f(i) * f(i + h) < 0)
                    list.Add(new Pair(i, i + h));
            }

            pairs = list;
        }

        public void Print()
        {
            foreach (Pair i in pairs)
            {
                Console.Write("[{0};", i.First);
                Console.Write("{0}]", i.Second);
                Console.WriteLine();
            }
        }

        public void Bisection()
        {
            Console.WriteLine("Метод бисекции:");
            foreach (Pair i in pairs)
            {
                Console.WriteLine("Начальное приближение к корню: {0}", (i.First + i.Second) / 2);
                RecBisection(i.First, i.Second, i.First, 1);
            }
        }

        private void RecBisection(double left, double right, double previous, int count)
        {
            if (Math.Abs(((right + left) / 2) - previous) <= epsilon)
            {
                Console.WriteLine("Корень: {0}", (right + left) / 2);
                Console.WriteLine("Количество итераций: {0}", count);
                Console.WriteLine("Невязка: {0} \n", Math.Abs(f((right + left) / 2)));
                return;
            }
            else if (f(left) * f((right + left) / 2) < 0)
            {
                RecBisection(left, (right + left) / 2, (right + left) / 2, ++count);
            }
            else
            {
                RecBisection((right + left) / 2, right, (right + left) / 2, ++count);
            }
        }

        public void Newton()
        {
            Console.WriteLine("Метод Ньютона:");
            foreach (Pair i in pairs)
            {
                Console.WriteLine("Начальное приближение к корню: {0}", (i.First + i.Second) / 2);
                RecNewton((i.First + i.Second) / 2, 1);
            }
        }

        private void RecNewton(double previous, int count)
        {
            double current = previous - f(previous) / fDer(previous);

            if (Math.Abs(current - previous) <= epsilon)
            {
                Console.WriteLine("Корень: {0}", current);
                Console.WriteLine("Количество итераций: {0}", count);
                Console.WriteLine("Невязка: {0} \n", Math.Abs(f(current)));
                return;
            }
            else
            {
                RecNewton(current, ++count);
            }
        }

        public void ModNewton()
        {
            Console.WriteLine("Модифицированный метод Ньютона:");
            foreach (Pair i in pairs)
            {
                Console.WriteLine("Начальное приближение к корню: {0}", (i.First + i.Second) / 2);
                RecModNewton((i.First + i.Second) / 2, 1, (i.First + i.Second) / 2);
            }
        }

        private void RecModNewton(double previous, int count, double startValue)
        {
            double current = previous - f(previous) / fDer(startValue);

            if (Math.Abs(current - previous) <= epsilon)
            {
                Console.WriteLine("Корень: {0}", current);
                Console.WriteLine("Количество итераций: {0}", count);
                Console.WriteLine("Невязка: {0} \n", Math.Abs(f(current)));
                return;
            }
            else
            {
                RecModNewton(current, ++count, startValue);
            }
        }

        public void Chord()
        {
            Console.WriteLine("Метод хорд:");
            foreach (Pair i in pairs)
            {
                Console.WriteLine("Начальное приближение к корню: {0}"
                , i.First - (i.Second - i.First) * f(i.First) / (f(i.Second) - f(i.First)));
                RecChord(i.First, 1, i.Second);
            }
        }

        private void RecChord(double first, int count, double second)
        {
            double current = first - (second - first) * f(first) / (f(second) - f(first));

            if (Math.Abs(current - second) <= epsilon)
            {
                Console.WriteLine("Корень: {0}", current);
                Console.WriteLine("Количество итераций: {0}", count);
                Console.WriteLine("Невязка: {0} \n", Math.Abs(f(current)));
                return;
            }
            else
            {
                RecChord(second, ++count, current);
            }
        }
    }
}