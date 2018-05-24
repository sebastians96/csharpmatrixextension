using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatrixextension
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix<int> i = new Matrix<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, 3, 4);
            Matrix<int> j = new Matrix<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 3, 3);
            //Matrix<double> d = l + m;
            //try
            //{
            //    (j * i).Write();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}
            Matrix<double> m = new Matrix<double>(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 }, 2, 3);
            Matrix<double> n = new Matrix<double>(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 }, 2, 3);
            Matrix<double> l = new Matrix<double>(new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0 }, 3, 3);
            Matrix<double> x = new Matrix<double>(new double[] { 1.0, 2.0, 3.0, 4.0 }, 2, 2);
            Matrix<double> y = new Matrix<double>(new double[] { 1.0, -1.0, 2.0, 3.0, 0, -4.0, 2.0, 3.0, 5.0 }, 3, 3);
            Matrix<double> p = new Matrix<double>(new double[] { 0.0, 3.0, 4.0, 5.0, 3.0, 0.0, 5.0, 6.0, 4.0, 5.0, 0.0, 7.0, 5.0, 6.0, 7.0, 0.0 }, 4, 4);
            m.Write();
            Console.WriteLine();
            l.Write();
            Console.WriteLine();
            try
            {
                l.Power(2).Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine();
            try
            {
                (m * l).Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            bool tmp = m != n;
            Console.WriteLine(tmp);

            p.Write();
            p = p.Inverse();
            p.Write();
            //n.Transpose();
            //Console.WriteLine(p.Determinant());
            //l = Matrix<double>.Diagonal(3);
            //foreach(double d in l.Column(1))
            //{
            //    Console.Write("{0:N2} ",d);
            //}
            //l.Write();
            // double a = l.PickValue(2, 2);
            // Console.WriteLine(a.ToString());
            Console.ReadKey();
            //while (true)
            //{
            //    Console.Clear();
            //    String input = Console.ReadLine();
            //    Scanner skaner = new Scanner();
            //    String[] tokens = skaner.tokenize(input);
            //    foreach (String s in tokens)
            //    {
            //        Console.WriteLine(s);
            //    }
            //    Console.ReadKey();
            //}
        }
    }
}
