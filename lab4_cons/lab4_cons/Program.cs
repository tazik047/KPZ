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
        static string TextName = "test.dat";

        static void Main(string[] args)
        {
            string s = Console.ReadLine();
            while (s != "q")
            {
                File.WriteAllText(TextName, s);
                Token t;
                Scanner scanner = new Scanner(TextName);
                while ((t = scanner.Scan()).kind != 0)
                {
                    Console.WriteLine("\tToken:{0}, Lexeme {1}", t.kind, t.val);
                }
                scanner = new Scanner(TextName);
                Parser parser = new Parser(scanner);
                parser.Parse();
                Console.WriteLine(parser.errors.count + " errors detected");
                s = Console.ReadLine();
            }
        }
    }
}
