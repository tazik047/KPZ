using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    class Program
    {
        private Dictionary<string, int> operation;

        private int STINEX = 0;

        static void Main(string[] args)
        {
            Console.WriteLine(Poliz(Console.ReadLine()));
        }

        private static bool isCorrect(string text)
        {
            int f = 0, s = 0, t = 0;
            foreach (var i in text)
            {
                if (i == '(')
                    t++;
                else if (i == ')')
                {
                    if (t == 0)
                            return false;
                        t--;
                }
            }
            return t == 0;
        }

        static string findNum(string t, int pos)
        {
            string res = "";
            while (pos != t.Length &&  char.IsLetterOrDigit(t[pos]))
            {
                res += t[pos];
                pos++;
            }
            return res;
        }

        private static int Prior(char c)
        {
            switch (c)
            {
                case '-':
                case '+':
                    return 1;
                case '*':
                case '/':
                    return 2;
                default:
                    return 0;
            }
        }

        private static string Poliz(string t)
        {
            if (!isCorrect(t))
                return "";
            t = new string(t.Where(c => c != ' ').ToArray());
            int pos = 0;
            string res = "";
            var st = new Stack<char>();
            while (t[pos]!='#')
            {
                var temp = findNum(t, pos);
                if (temp == "")
                {
                    if (st.Count == 0 || st.Peek()=='(')
                        st.Push(t[pos]);
                    else if(t[pos] == '(')
                        st.Push(t[pos]);
                    else if (t[pos] == ')')
                    {
                        char i;
                        while ((i = st.Pop()) != '(')
                            res += " " + i;

                    }
                    else
                    {
                        char c = st.Peek();
                        int p = Prior(c) - Prior(t[pos]);
                        if (p >= 0)
                            res += " " + st.Pop();
                        st.Push(t[pos]);
                    }
                    pos++;
                }
                else
                {
                    res += " " + temp;
                    pos += temp.Length;
                }
            }
            while (st.Count() != 0)
                res += " " + st.Pop();
            return res;
        }
    }
}
