namespace Lab_6
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Integration
    {
        public void StartWork()
        {
            do
            {
                Console.WriteLine("Введите концы отрезка [a; b]:");
                string[] num = Console.ReadLine().Split(' ');
                a = double.Parse(num[0]);
                b = double.Parse(num[1]);
            } while (a >= b);
            Console.WriteLine();

            h = (b - a) / m;
            moments = Moments();

            for (int i = 0; i < 2 * N; ++i)
                Console.WriteLine("m{0} = {1}", i, moments[i]);
            Console.WriteLine();

            FindOmega(moments);
            double[] roots = FindOmegaRoots();
            Console.WriteLine();

            for (int i = 0; i < roots.Length; ++i)
                Console.Write("x{0} = {1} ", i, roots[i]);
            Console.WriteLine();
            Console.WriteLine();

            double[] ai = FindAi(roots);
            for (int i = 0; i < ai.Length; i++)
                Console.Write("A{0} = {1} ", i, ai[i]);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Проверка.");
            Console.WriteLine("m5 = {0}", moments[5]);
            Console.WriteLine("A1*x1^5 + A2*x2^5 + A3*x3^5 = {0}"
                            , (ai[0] * Math.Pow(roots[0], 5) + ai[1] * Math.Pow(roots[1], 5) + ai[2] * Math.Pow(roots[2], 5)));
            Console.WriteLine();

            Console.WriteLine("A1*f(x1) + A2*f(x2) + A3*f(x3) = {0}"
                            , (ai[0] * f(roots[0]) + ai[1] * f(roots[1]) + ai[2] * f(roots[2])));
        }

        private double[] Moments()
        {
            double[] moments = new double[2 * N];
            for (int i = 0; i < 2 * N; i++)
                moments[i] = MR(i);

            return moments;
	    }

        private double MR(int pow)
        {
            double xk = a;
            double res = 0;
            while(xk < b)
            {
                res += Math.Pow((xk + h / 2), pow) * w((xk + h / 2));
                xk += h;
            }
            res *= h;
            return res;
        }

        private void FindOmega(double[] moments) 
        {
            double[,] temp = {{moments[2], moments[1], moments[0]}
						    , {moments[3], moments[2], moments[1]}
						    , {moments[4], moments[3], moments[2]}};
	        double det = CalculateDet3(temp);

            double [,] temp1 = {{-moments[3], moments[1], moments[0]}
						    , {-moments[4], moments[2], moments[1]}
						    , {-moments[5], moments[3], moments[2]}};
	        double det1 = CalculateDet3(temp1);

            double[,] temp2 = {{moments[2], -moments[3], moments[0]}
						    , {moments[3], -moments[4], moments[1]}
						    , {moments[4], -moments[5], moments[2]}};
	        double det2 = CalculateDet3(temp2);

            double[,] temp3 = {{moments[2], moments[1], -moments[3]}
						    , {moments[3], moments[2], -moments[4]}
						    , {moments[4], moments[3], -moments[5]}};
	        double det3 = CalculateDet3(temp3);

            a0 = det3 / det;
            a1 = det2 / det;
            a2 = det1 / det;
	        Console.WriteLine("c = {0}, d = {1}, p = {2}", a2, a1, a0);
        }

        private double[] FindAi(double[] roots) 
        {
            double[,] temp = {{1, 1, 1}
		                    , {roots[0], roots[1], roots[2]}
		                    , {roots[0] * roots[0], roots[1] * roots[1], roots[2] * roots[2]}};
	        double det = CalculateDet3(temp);

            double[,] temp1 = {{moments[0], 1, 1}
		                    , {moments[1], roots[1], roots[2]}
		                    , {moments[2], roots[1] * roots[1], roots[2] * roots[2]}};
	        double det1 = CalculateDet3(temp1);

            double[,] temp2 = {{1, moments[0], 1}
		                    , {roots[0], moments[1], roots[2]}
		                    , {roots[0] * roots[0], moments[2], roots[2] * roots[2]}};
	        double det2 = CalculateDet3(temp2);

            double[,] temp3 = {{1, 1, moments[0]}
		                    , {roots[0], roots[1], moments[1]}
		                    , {roots[0] * roots[0], roots[1] * roots[1], moments[2]}};
	        double det3 = CalculateDet3(temp3);
	        
            double[] result = { det1 / det, det2 / det, det3 / det };
            return result;
        }

        private double Omega(double x)
        {
            return x * x * x + a2 * x * x + a1 * x + a0;
        }

        private double[] FindOmegaRoots() 
        {
	        double[] result = new double[3];
	        double current = a;
            int index = 0;
	        while (current < b)
            {
                if (Omega(current) * Omega(current + h) < 0)
                {
			        result[index] = Binsearch(current, current + h);
                    index++;
		        }
		        current += h;
	        }

	        return result;
        }

        private double CalculateDet3(double[,] matrix)
        {
            return (  matrix[0,0] * matrix[1,1] * matrix[2,2]
                    + matrix[0,1] * matrix[1,2] * matrix[2,0]
                    + matrix[0,2] * matrix[1,0] * matrix[2,1]
                    - matrix[0,2] * matrix[1,1] * matrix[2,0]
                    - matrix[0,1] * matrix[1,0] * matrix[2,2]
                    - matrix[0,0] * matrix[1,2] * matrix[2,1]);
        }

        private double Binsearch(double start, double end) 
        {
	        if (end - start < epsilon)
		        return start;

	        double mid = (start + end) / 2;
            if (Omega(start) * Omega(mid) < 0)
		        return Binsearch(start, mid);
	        else 
            {
                if (Omega(start) == 0)
			        return start;
                else if (Omega(end) == 0)
			        return end;

		        return Binsearch(mid, end);
	        }
        }

        private double f(double x)
        {
            return Math.Sin(x);
        }

        private double w(double x)
        {
            return Math.Cos(x);
        }

        private double a = 0;
        private double b = 1;
        private int m = 100;
        private int N = 3;
        private double h;
        private double[] moments;
        private double a0;
        private double a1;
        private double a2;
        private double epsilon = 0.0000001;
    }
}
