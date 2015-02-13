﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    static class predicatesPOLIN
    {
        private static List<Operation> operation;

        private static int STINDEX = 0;

        private static List<int> STINDEXES = new List<int>();

        public static void polins() 
        {
            operation = new List<Operation>()
            {
                new Operation("!",4,1),
                new Operation("all",5,1),
                new Operation("exist",5,1),
                new Operation("^",3,2),
                new Operation("v",2,2),
                new Operation("xor",2,2),
                new Operation("->",1,2),
                new Operation("~",0,2),
                Operation.CloseScob,
                Operation.OpenScob,
                Operation.GetOp,
                Operation.Separator
            };
            Console.WriteLine(Poliz(Console.ReadLine())); 
        }
        private static bool isCorrect(string text)
        {
            if (!text[text.Length - 1].Equals('#'))
            {
                return false;
            }
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
            while (t[pos] != '#' && !operation.Any(o => o.isOperation(t, pos)))
            {
                res += t[pos];
                pos++;
            }
            return res;
        }

        private static string Poliz(string t)
        {
            int indexOfList = -1;
            if (!isCorrect(t)) 
                return "";
            t = new string(t.Where(c => c != ' ').ToArray());
            int pos = 0;
            string res = "";
            var st = new Stack<Operation>();
            while (t[pos] != '#')
            {
                var temp = findNum(t, pos);
                if (temp == "")
                {
                    if (st.Count == 0 || st.Peek() == Operation.OpenScob)
                    {
                        Operation o = getOperation(t, ref pos);
                        if (o == Operation.Separator)
                        {
                            STINDEXES[indexOfList]++;
                        }
                        st.Push(o);
                    }
                    else if (t[pos] == '(')
                        st.Push(getOperation(t, ref pos));
                    else if (t[pos] == ')')
                    {
                        Operation i;
                        while ((i = st.Pop()) != Operation.OpenScob)
                        {
                            if (i == Operation.GetOp)
                            {
                                st.Pop();
                                break;
                            }
                            if (i == Operation.Separator || i.Character == ",")
                            {
                                st.Pop();
                                STINDEXES[indexOfList]++;
                                continue;
                            }
                            STINDEXES[indexOfList]++;
                            i.Size = STINDEXES[indexOfList];
                            res += " " + i;
                        }
                        pos++;
                        indexOfList--;
                    }
                    else if (t[pos] == ',')
                    {
                        st.Push(Operation.Separator);
                        pos++;
                    }
                    else
                    {
                        var o = getOperation(t, ref pos);
                        while (st.Count != 0 && st.Peek() >= o)
                            res += " " + st.Pop();
                        st.Push(o);
                    }
                }
                else if (t[pos+temp.Length] == '(')
                {
                    Operation op = new Operation(temp);
                    st.Push(Operation.GetOp);
                    st.Push(op);
                    STINDEX = 1;
                    STINDEXES.Add(STINDEX);
                    indexOfList++;
                    pos = pos + temp.Length;
                }
                else
                {
                    res += (res.Length == 0 ? "" : " ") + temp;
                    pos += temp.Length;
                }
            }
            while (st.Count() != 0)
            {
                int count = 0;
                Operation o = st.Pop();
                o.Size = STINDEXES[STINDEXES.Count-count-1];
                if (o == Operation.GetOp || o == Operation.CloseScob || o == Operation.OpenScob || o.Character == ",")
                {
                    continue;
                }
                res += " " + o;
                count++;
            }
            return res;
        }

        private static Operation getOperation(string t, ref int pos)
        {
            int tPos = pos;
            var op = operation.First(o => o.isOperation(t, tPos));
            pos += op.Character.Length;
            return op;
        }
    }
}
