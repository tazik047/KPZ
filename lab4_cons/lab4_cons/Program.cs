using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4_cons
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            while (s != "q")
            {
                File.WriteAllText("test.dat", s);
                Scanner scanner = new Scanner("test.dat");
                Parser parser = new Parser(scanner);
                parser.Parse();
                Console.Write(parser.errors.count + " errors detected" + Environment.NewLine);
                s = Console.ReadLine();
            }
        }
    }
}
