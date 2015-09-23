namespace Lab1_f_x__0
{
        using System;
        using System.Collections.Generic;
    
        class Program
        {
            static void Main(string[] args)
            {
                lab class1 = new lab();
                Console.WriteLine("Отрезки, содержащие по одному корню:");
                class1.Print();
                Console.WriteLine();

                class1.Bisection();
                Console.WriteLine();

                class1.Newton();
                Console.WriteLine();

                class1.ModNewton();
                Console.WriteLine();

                class1.Chord();

            }
        }
}
