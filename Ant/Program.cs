using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmsLibrary;

namespace Ant
{
    class Program
    {
        static void Main(string[] args)
        {
            AlgorithmsLibrary.AntColonyOptimization ACO = new AlgorithmsLibrary.AntColonyOptimization();
            ACO.Start();
            Console.WriteLine("END");
            Console.ReadKey();
        }

    }
}
